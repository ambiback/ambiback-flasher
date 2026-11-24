using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmbiBack_USB_Flasher
{
    public enum BUILDENVS
    {
        V104 = 0,
        V106 = 1,
        V200 = 2,
    }

    /// <summary>Detected build environment with toolchain versions.</summary>
    public struct BuildEnvInfo
    {
        public string Name;
        /// <summary>Legacy: xtensa-esp32-elf-gcc version. New: empty.</summary>
        public string TensaTools;
        /// <summary>New layout: esp-x32 version (e.g. 2511). Legacy: empty.</summary>
        public string EspX32;
        public string EsptoolPy;
        /// <summary>Base path for tools (buildtools/{env} or Arduino15/packages/esp32).</summary>
        public string ToolsBasePath;
        /// <summary>True = xtensa-esp32-elf-gcc (1.x, 2.x). False = esp-x32 (3.x+).</summary>
        public bool UseLegacyToolchain;
    }

    public partial class frmMain : Form
    {
        public string szBinDir { get { return Properties.Settings.Default.szBinDir; } set { Properties.Settings.Default.szBinDir = value; } }
        public string szSourceDir { get { return Properties.Settings.Default.szSourceDir; } set { Properties.Settings.Default.szSourceDir = value; } }
        public string szSourceFile { get { return Properties.Settings.Default.szSourceFile; } set { Properties.Settings.Default.szSourceFile = value; } }
        public string szExtraIncludeDir { get { return Properties.Settings.Default.szExtraIncludeDir; } set { Properties.Settings.Default.szExtraIncludeDir = value; } }
        public string szDebugOutputHelper = "";  // Runtime buffer, not persisted

        /// <summary>App root path (firmware, buildenv, buildtools, esptool.exe). Uses Settings.szStartupPath if set, else szBuildDir.</summary>
        public string AppRootPath { get { return string.IsNullOrWhiteSpace(Properties.Settings.Default.szStartupPath) ? Properties.Settings.Default.szBuildDir : Properties.Settings.Default.szStartupPath.Trim(); } }

        /// <summary>Detected build environments from build/buildenv. Populated on load.</summary>
        private List<BuildEnvInfo> _buildEnvs = new List<BuildEnvInfo>();

        /// <summary>Get the currently selected build env info. Returns null if none selected or invalid.</summary>
        private BuildEnvInfo? GetCurrentBuildEnv()
        {
            string sel = cbBuildEnv?.Text;
            if (string.IsNullOrEmpty(sel)) return null;
            var env = _buildEnvs.FirstOrDefault(e => e.Name == sel);
            return string.IsNullOrEmpty(env.Name) ? (BuildEnvInfo?)null : env;
        }

        SerialPort serialPort = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized) return;

            const int margin = 12;
            const int progressHeight = 18;
            int progressBarY = ClientSize.Height - margin - progressHeight;

            // gbDebug: fill space between Device Control (y=410) and progress bar
            int gbDebugHeight = Math.Max(100, progressBarY - 410 - margin);
            gbDebug.Height = gbDebugHeight;

            // Progress bar: fixed at bottom (no anchor - we position manually)
            progressBarBuild.Location = new System.Drawing.Point(margin, progressBarY);
            progressBarBuild.Width = Math.Max(100, ClientSize.Width - margin * 2);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Globals.MainForm = this;
            frmMain_Resize(sender, e);
            string szDevice = Properties.Settings.Default.szDevice;

            PopulateBuildEnvCombo();
            UpdateComPorts();
            cbDevice.Enabled = false;
            cbDevice.DataSource = CDeviceHelper.aDeviceTypes.Clone();
            cbDevice.DisplayMember = "DisplayLabel";
            cbDevice.ValueMember = "DeviceType";
            cbDevice.Enabled = true;

            // Reset value back
            cbDevice.Text = szDevice;
            cbDevice_SelectedIndexChanged(sender, e);
            Console.WriteLine(Properties.Settings.Default.szDevice);
        }

        /// <summary>Detect build environments from build/buildenv and populate cbBuildEnv.</summary>
        private void PopulateBuildEnvCombo()
        {
            _buildEnvs.Clear();
            cbBuildEnv.Items.Clear();

            string root = AppRootPath;
            if (string.IsNullOrWhiteSpace(root))
            {
                AppendDebugText("[Build ENV] AppRootPath is empty, skipping tool detection.\r\n", Color.Gray);
                return;
            }
            string buildenvDir = Path.Combine(root, "buildenv");
            string buildtoolsDir = Path.Combine(root, "buildtools");

            AppendDebugText("[Build ENV] Scanning: " + buildenvDir + "\r\n", Color.Gray);

            if (!Directory.Exists(buildenvDir))
            {
                AppendDebugText("[Build ENV] buildenv directory not found.\r\n", Color.Gray);
                return;
            }

            var subdirs = Directory.GetDirectories(buildenvDir)
                .Select(d => Path.GetFileName(d))
                .Where(n => !string.IsNullOrEmpty(n) && !n.StartsWith("."))
                .OrderBy(n => n)
                .ToList();

            AppendDebugText("[Build ENV] Found " + subdirs.Count + " env(s): " + string.Join(", ", subdirs) + "\r\n", Color.Gray);

            string arduino15Packages = Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA") ?? "", "Arduino15", "packages", "esp32");
            string arduino15Tools = Path.Combine(arduino15Packages, "tools");

            foreach (string name in subdirs)
            {
                string tensa = null;
                string espX32 = null;
                string esptool = null;
                string toolsBasePath = null;
                bool useLegacy = false;

                string localToolsBase = Path.Combine(buildtoolsDir, name, "tools");
                string xtensaDir = Path.Combine(localToolsBase, "xtensa-esp32-elf-gcc");
                string espX32Dir = Path.Combine(localToolsBase, "esp-x32");
                string esptoolDir = Path.Combine(localToolsBase, "esptool_py");

                // Prefer new layout (esp-x32) over legacy (xtensa-esp32-elf-gcc)
                if (Directory.Exists(espX32Dir))
                {
                    var versions = Directory.GetDirectories(espX32Dir).Select(Path.GetFileName).Where(n => !string.IsNullOrEmpty(n)).ToList();
                    if (versions.Count > 0)
                    {
                        espX32 = versions.OrderBy(v => v).First();
                        toolsBasePath = Path.Combine(buildtoolsDir, name);
                    }
                }
                if (espX32 == null && Directory.Exists(xtensaDir))
                {
                    var versions = Directory.GetDirectories(xtensaDir).Select(Path.GetFileName).Where(n => !string.IsNullOrEmpty(n)).ToList();
                    if (versions.Count > 0)
                    {
                        tensa = versions.OrderBy(v => v).First();
                        toolsBasePath = Path.Combine(buildtoolsDir, name);
                        useLegacy = true;
                    }
                }
                if (Directory.Exists(esptoolDir) && toolsBasePath != null)
                {
                    var versions = Directory.GetDirectories(esptoolDir).Select(Path.GetFileName).Where(n => !string.IsNullOrEmpty(n)).ToList();
                    if (versions.Count > 0)
                        esptool = versions.OrderBy(v => v).First();
                }

                // Fallback: Arduino15 packages (for newer libs when local buildtools lacks tools)
                if (toolsBasePath == null && Directory.Exists(arduino15Tools))
                {
                    string ax32 = Path.Combine(arduino15Tools, "esp-x32");
                    string aesp = Path.Combine(arduino15Tools, "esptool_py");
                    if (Directory.Exists(ax32))
                    {
                        var versions = Directory.GetDirectories(ax32).Select(Path.GetFileName).Where(n => !string.IsNullOrEmpty(n)).ToList();
                        if (versions.Count > 0)
                        {
                            espX32 = versions.OrderBy(v => v).First();
                            toolsBasePath = arduino15Packages;
                            AppendDebugText("[Build ENV]   " + name + " -> using Arduino15 fallback (esp-x32: " + espX32 + ")\r\n", Color.DarkCyan);
                        }
                    }
                    if (esptool == null && Directory.Exists(aesp))
                    {
                        var versions = Directory.GetDirectories(aesp).Select(Path.GetFileName).Where(n => !string.IsNullOrEmpty(n)).ToList();
                        if (versions.Count > 0)
                            esptool = versions.OrderBy(v => v).First();
                    }
                }

                string toolchainStr = useLegacy ? ("xtensa: " + (tensa ?? "(not found)")) : ("esp-x32: " + (espX32 ?? "(not found)"));
                string esptoolStr = esptool ?? "(not found)";
                string sourceStr = toolsBasePath != null && toolsBasePath.Contains("Arduino15") ? " [Arduino15]" : "";
                AppendDebugText("[Build ENV]   " + name + " -> " + toolchainStr + ", esptool_py: " + esptoolStr + sourceStr + "\r\n", Color.Gray);

                _buildEnvs.Add(new BuildEnvInfo
                {
                    Name = name,
                    TensaTools = tensa ?? "",
                    EspX32 = espX32 ?? "",
                    EsptoolPy = esptool ?? "",
                    ToolsBasePath = toolsBasePath ?? "",
                    UseLegacyToolchain = useLegacy
                });
                cbBuildEnv.Items.Add(name);
            }

            string saved = Properties.Settings.Default.szBuildEnv;
            if (_buildEnvs.Any(e => e.Name == saved))
            {
                cbBuildEnv.Text = saved;
                AppendDebugText("[Build ENV] Selected: " + saved + "\r\n", Color.Gray);
            }
            else if (cbBuildEnv.Items.Count > 0)
            {
                cbBuildEnv.SelectedIndex = 0;
                AppendDebugText("[Build ENV] Selected (default): " + cbBuildEnv.Text + "\r\n", Color.Gray);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        /// <summary>Append text to debug output with optional color. Use null for default (black).</summary>
        private void AppendDebugText(string text, Color? color = null)
        {
            txtDebug.SelectionStart = txtDebug.TextLength;
            txtDebug.SelectionLength = 0;
            txtDebug.SelectionColor = color ?? txtDebug.ForeColor;
            txtDebug.AppendText(text);
            txtDebug.SelectionColor = txtDebug.ForeColor;
            ScrollDebugToEnd();
        }

        /// <summary>Scroll the debug output to show the latest content.</summary>
        private void ScrollDebugToEnd()
        {
            txtDebug.SelectionStart = txtDebug.TextLength;
            txtDebug.ScrollToCaret();
        }

        private void UpdateComPorts()
        {
            string[] aPorts = SerialPort.GetPortNames();
            bool bRefill = false;

            if (cbComPort.Items.Count == 0)
                bRefill = true;
            else
            {
                foreach (string szCbPort in cbComPort.Items)
                {
                    if (!aPorts.Contains(szCbPort)) {
                        bRefill = true;
                        break;
                    }
                }

                if (!bRefill) {
                    foreach (string szPort in aPorts)
                    {
                        if (!cbComPort.Items.Contains(szPort))
                            cbComPort.Items.Add(szPort);
                    }
                }
            }

            if (bRefill) {
                cbComPort.Items.Clear();
                foreach (string szPort in aPorts)
                    cbComPort.Items.Add(szPort);
            }
        }
        private void cbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cbDevice.Enabled)
                return;

            cbFirmwareVersion.Items.Clear();

            try
            {
                CDeviceType selectedType = (CDeviceType)cbDevice.Items[cbDevice.SelectedIndex];
                string[] szFirmwareVersions = Directory.GetDirectories(AppRootPath + "\\firmware\\" + selectedType.DeviceType + "\\");
                if (szFirmwareVersions.Length > 0)
                {
                    foreach (string szFirmwareVersion in szFirmwareVersions)
                    {
                        cbFirmwareVersion.Items.Add(szFirmwareVersion.Substring(szFirmwareVersion.LastIndexOf('\\') + 1));
                    }
                    cbFirmwareVersion.SelectedIndex = (cbFirmwareVersion.Items.Count > 0 ? cbFirmwareVersion.Items.Count - 1 : 0);
                }

                // Find latest Firmware version from source
                string szGlobalIncludesFile = szSourceDir + "\\globalincludes.h";
                if (File.Exists(szGlobalIncludesFile)) {
                    string szGlobalIncludesCode = File.ReadAllText(szGlobalIncludesFile);
                    string szVersion = Globals.GetSubstringFromLine(szGlobalIncludesCode, "#define AMBIBACK_VERSION \"", "\"", false);
                    //                    if (!cbFirmwareVersion.Items.Contains(szVersion)) {
                    //                    }
                    txtVersionCode.Text = szVersion.Trim();
                }

            }
            catch (Exception ex)
            {
                Globals.HandleException("cbDevice_SelectedIndexChanged", ex, false, false);
            }
        }
        private void cmdMonitor_Click(object sender, EventArgs e)
        {
            if (cmdMonitor.Text == "Monitor")
            {
                try
                {
                    txtDebug.Clear();
                    this.OpenSerial();

                    cmdGetInfo.Enabled = true;
                    cmdSendLedTestData.Enabled = true;
                    cmdSetWifi.Enabled = true;
                    cmdResetWifi.Enabled = true;
                    cmdReboot.Enabled = true;
                    cmdFactoryDefaults.Enabled = true;
                    cmdSetRelay.Enabled = true;
                    cmdModeCam.Enabled = true;
                    cmdModeDemo.Enabled = true;
                    cmdModeOff.Enabled = true;

                    txtConnectedDeviceVersion.Text = "";
                    txtConnectedDeviceType.Text = "";
                    txtConnectedDeviceID.Text = "";
                    txtConnectedDeviceMAC.Text = "";
                    txtConnectedDeviceName.Text = "";
                    txtConnectedDeviceIP.Text = "";

                    // Trigger Info Click
                    cmdGetInfo_Click(sender, e);
                }
                catch (Exception ex)
                {
                    Globals.HandleException("cmdMonitor_Click", ex, false, false);
                }
            } else {
                this.CloseSerial();
                cmdGetInfo.Enabled = false;
                cmdSendLedTestData.Enabled = false;
                cmdSetWifi.Enabled = false;
                cmdResetWifi.Enabled = false;
                cmdReboot.Enabled = false;
                cmdFactoryDefaults.Enabled = false;
                cmdSetRelay.Enabled = false;
                cmdModeCam.Enabled = false;
                cmdModeDemo.Enabled = false;
                cmdModeOff.Enabled = false;
            }
        }
        private void OpenSerial()
        {
            txtDebug.Clear();
            this.CloseSerial();
            try
            {
                this.serialPort = new SerialPort();
                serialPort.PortName = cbComPort.Text;
                serialPort.BaudRate = Int32.Parse(cbBaudrate.Text);
                serialPort.DataReceived += SerialPort_DataReceived;
                serialPort.ErrorReceived += SerialPort_ErrorReceived;
                serialPort.Open();
                cmdMonitor.Text = "Stop";
            }
            catch (Exception ex)
            {
                Globals.HandleException("OpenSerial", ex, true, false);
            }
        }
        private void HandleSerialOutput(string szOutput)
        {
            txtDebug.AppendText(szOutput);
            ScrollDebugToEnd();

            try
            {
                if (szOutput.Contains("%info: {") || szDebugOutputHelper.Length > 0) {
                    szDebugOutputHelper += szOutput;

                    if (szOutput.IndexOf('}') > 0)
                    {
                        // HAndle Info output
                        JSONNode jsonResults = JSON.Parse(szDebugOutputHelper.Substring(szDebugOutputHelper.IndexOf('{'), szDebugOutputHelper.IndexOf('}') - szDebugOutputHelper.IndexOf('{')));
                        if (jsonResults.Count > 0)
                        {
                            txtConnectedDeviceType.Text = jsonResults["DeviceType"];
                            txtConnectedDeviceVersion.Text = jsonResults["DeviceVersion"];
                            txtConnectedDeviceID.Text = jsonResults["DeviceID"];
                            txtConnectedDeviceMAC.Text = jsonResults["DeviceMAC"];
                            txtConnectedDeviceName.Text = jsonResults["DeviceName"];
                            txtConnectedDeviceIP.Text = jsonResults["DeviceIP"];

                            if (txtConnectedDeviceType.Text == "AmbiBack_Led_M5A1_Controller" ||
                                txtConnectedDeviceType.Text == "AmbiBack_Pc_M5AL_Controller" ||
                                txtConnectedDeviceType.Text == "AmbiBack_Pc_M5AX_Controller" ||
                                txtConnectedDeviceType.Text == "AmbiBack_Pc_Controller") {

                                // Copy to RelayID
                                txtRelayID.Text = txtConnectedDeviceMAC.Text;
                            }
                        }

                        szDebugOutputHelper = "";
                    }
                } 
            }
            catch (Exception ex)
            {
                Globals.HandleException("HandleSerialOutput" + szOutput, ex, false, false);
            }
        }
        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                AppendDebugText("!ERROR: " + (e.EventType.ToString() ?? string.Empty) + "\r\n", Color.Red);
            }));
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                HandleSerialOutput(indata.Replace("\n", "\r\n") ?? string.Empty);
            }));
        }
        private void CloseSerial()
        {
            cmdMonitor.Text = "Monitor";
            if (serialPort != null)
                serialPort.Close();
            serialPort = null;
        }
        private void cmdFlash_Click(object sender, EventArgs e)
        {
            this.CloseSerial();
            txtDebug.Clear();
            cmdFlash.Enabled = false;

            // Start the child process.
            Process pFlash = new Process();
            pFlash.EnableRaisingEvents = true;
            pFlash.StartInfo.CreateNoWindow = true;
            pFlash.StartInfo.UseShellExecute = false;
            pFlash.StartInfo.RedirectStandardOutput = true;
            pFlash.StartInfo.RedirectStandardError = true;
            //            pFlash.StartInfo.RedirectStandardInput = true;

            try
            {
                CDeviceType selectedType = (CDeviceType)cbDevice.Items[cbDevice.SelectedIndex];

                // Show the build information of the selected image, if the build wrote one
                string szBuildInfoFile = AppRootPath + "\\firmware\\" + selectedType.DeviceType + "\\" + cbFirmwareVersion.Text + "\\buildinfo.txt";
                if (File.Exists(szBuildInfoFile))
                    AppendDebugText("Build info:\r\n" + File.ReadAllText(szBuildInfoFile) + "\r\n", Color.DarkCyan);

                pFlash.StartInfo.FileName = AppRootPath + "\\esptool.exe";
                pFlash.StartInfo.Arguments = "--chip esp32 --port " + cbComPort.Text + " --baud " + selectedType.UploadSpeed + " --before default_reset --after hard_reset write_flash -z --flash_mode dio --flash_freq 80m --flash_size detect " +
                                            "0xe000 \"" + AppRootPath + "\\firmware\\boot_app0.bin\" " +
                                            "0x1000 \"" + AppRootPath + "\\firmware\\bootloader_qio_80m.bin\" " +
                                            "0x10000 \"" + AppRootPath + "\\firmware\\" + selectedType.DeviceType + "\\" + cbFirmwareVersion.Text + "\\AmbiBack.Controller.ino.bin\" " +
                                            "0x8000 \"" + AppRootPath + "\\firmware\\" + selectedType.DeviceType + "\\" + cbFirmwareVersion.Text + "\\AmbiBack.Controller.ino.partitions.bin\" ";

                txtDebug.AppendText("Firmware flash Cmd: \r\n" + pFlash.StartInfo.FileName + " " + pFlash.StartInfo.Arguments + "\r\n");
                ScrollDebugToEnd();

                // Redirect the output stream of the child process.
                pFlash.ErrorDataReceived += ErrorOutputHandler;
                pFlash.OutputDataReceived += StandardOutputHandler;
                // Start the process.
                pFlash.Start();

                // Start the asynchronous read
                pFlash.BeginOutputReadLine();
                pFlash.BeginErrorReadLine();

                while (!pFlash.HasExited)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1);
                }
                pFlash.WaitForExit();

                Application.DoEvents();
                txtDebug.AppendText("\r\n\r\nFirmware flash FINISHED\r\n");
                ScrollDebugToEnd();

                // Success
                cmdFlash.Enabled = true;

                // Open Serial Connection next
                this.cmdMonitor_Click(sender, e);
                // this.OpenSerial();
            }
            catch (Exception ex)
            {
                string szErrorText = "Command: " + pFlash.StartInfo.FileName +
                                        "Arguments: " + pFlash.StartInfo.Arguments +
                                        "Exception occured in cmdFlash" +
                                        "\r\nException Type: " + ex.GetType().ToString() +
                                        "\r\nDetails: \r\n\r\n" + ex.ToString();
                AppendDebugText(szErrorText, Color.Red);
            }
        }
        void StandardOutputHandler(object sender, DataReceivedEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                try
                {
                    if (e.Data != null && e.Data.Length > 0) {
                        // Parse Progress lines from arduino-builder - format: ===info ||| Progress {0} ||| [93.75]
                        // Match number in square brackets when line contains "Progress"
                        string numStr = null;
                        if (e.Data.IndexOf("Progress", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            var m = Regex.Match(e.Data, @"\[\s*([\d.,]+)\s*\]");
                            if (m.Success) numStr = m.Groups[1].Value.Replace(',', '.');
                        }
                        if (numStr != null && double.TryParse(numStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double pct))
                        {
                            int val = (int)Math.Min(100, Math.Max(0, pct));
                            try { if (progressBarBuild != null && progressBarBuild.Visible) progressBarBuild.Value = val; } catch { }
                            return;
                        }

                        string lineToAppend = null;
                        if (e.Data.Contains("|||"))
                        {
                            string[] aSeparators = new string[] { "|||" };
                            string[] dataSplitted = e.Data.Split(aSeparators, StringSplitOptions.RemoveEmptyEntries);
                            
                            if (dataSplitted.Length > 2) {
                                string[] aDataSeparators = new string[] { " " };
                                string[] dataParams = (dataSplitted[2].Length > 3 ? dataSplitted[2].Substring(2, dataSplitted[2].Length-3).Split(aDataSeparators, StringSplitOptions.RemoveEmptyEntries) : null);
                                if (dataParams != null)
                                {
                                    if (dataSplitted[1].Contains("{3}")) {
                                        lineToAppend = "Output: " + String.Format(dataSplitted[1], 
                                            Uri.UnescapeDataString(dataParams[0]), 
                                            (dataParams.Length > 0 ? Uri.UnescapeDataString(dataParams[1]) : string.Empty),
                                            (dataParams.Length > 0 ? Uri.UnescapeDataString(dataParams[2]) : string.Empty), string.Empty);
                                    }else if (dataSplitted[1].Contains("{2}")){
                                        lineToAppend = "Output: " + String.Format(dataSplitted[1], 
                                            Uri.UnescapeDataString(dataParams[0]), 
                                            (dataParams.Length > 0 ? Uri.UnescapeDataString(dataParams[1]) : string.Empty),
                                            (dataParams.Length > 0 ? Uri.UnescapeDataString(dataParams[2]) : string.Empty));
                                    } else if (dataSplitted[1].Contains("{1}")) {
                                        lineToAppend = "Output: " + String.Format(dataSplitted[1], 
                                            Uri.UnescapeDataString(dataParams[0]), 
                                            (dataParams.Length > 0 ? Uri.UnescapeDataString(dataParams[1]) : string.Empty));
                                    } else 
                                        lineToAppend = "Output: " + String.Format(dataSplitted[1], 
                                            Uri.UnescapeDataString(dataParams[0]));
                                } else
                                    lineToAppend = "Output: " + (e.Data ?? string.Empty);
                            }
                            else
                                lineToAppend = "Output: " + (e.Data ?? string.Empty);
                        }
                        else 
                            lineToAppend = "Output: " + (e.Data ?? string.Empty);
                        if (lineToAppend != null)
                        {
                            lineToAppend = lineToAppend.TrimEnd('\r', '\n');
                            txtDebug.AppendText(lineToAppend + "\r\n");
                            ScrollDebugToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Globals.HandleException("StandardOutputHandler", ex, false, false);
                }
            }));
        }
        void ErrorOutputHandler(object sender, DataReceivedEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                if (e.Data != null && e.Data.Length > 0)
                {
                    // Parse Progress lines - format: ===info ||| Progress {0} ||| [93.75]
                    string numStr = null;
                    if (e.Data.IndexOf("Progress", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        var m = Regex.Match(e.Data, @"\[\s*([\d.,]+)\s*\]");
                        if (m.Success) numStr = m.Groups[1].Value.Replace(',', '.');
                    }
                    if (numStr != null && double.TryParse(numStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double pct))
                    {
                        int val = (int)Math.Min(100, Math.Max(0, pct));
                        try { if (progressBarBuild != null && progressBarBuild.Visible) progressBarBuild.Value = val; } catch { }
                        return;
                    }
                    AppendDebugText("ERROR: " + (e.Data ?? string.Empty).TrimEnd('\r', '\n') + "\r\n", Color.Red);
                }
            }));
        }
        private void timerComPort_Tick(object sender, EventArgs e)
        {
            UpdateComPorts();
        }
        private void cmdGetInfo_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                this.serialPort.Write("%INFO:");
            }
        }
        private void cmdSetWifi_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                // Payload helpers 
                MemoryStream msPacket = new MemoryStream();
                BinaryWriter bwPacket = new BinaryWriter(msPacket);

                // Write Byte array
                bwPacket.Write("%WIFI:");
                bwPacket.Write(txtWifiSSID.Text);
                bwPacket.Write((byte)0x00);
                bwPacket.Write(txtWifiKey.Text);
                bwPacket.Write((byte)0x00);

                // Send byte area to serial
                byte[] bData = msPacket.ToArray();
                this.serialPort.Write(bData, 0, bData.Length);
            }
        }
        private void cmdSendLedTestData_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                // Payload helpers 
                MemoryStream msPacket = new MemoryStream();
                BinaryWriter bwPacket = new BinaryWriter(msPacket);
                byte bLeds = (byte)0xFF; //  0x7F; 
                // Write 4 Byte Detection Header
                bwPacket.Write("%LEDDATA:");
                bwPacket.Write((byte)0x00);

                // Write 4 Byte Package Header
                bwPacket.Write(bLeds); // Count of LEDs (Length)
                bwPacket.Write((byte)0x00); // StartIndex
                bwPacket.Write((byte)0x00); // Îf set to 0x01 we use CheckSum Check
                bwPacket.Write((byte)0x00); // Set to Checksum later!

                // add pixels to array!
                for (byte bPixelCurrent = 0; bPixelCurrent < bLeds; bPixelCurrent++)
                {
                    bwPacket.Write((byte)0x00);    // not used byte
                    bwPacket.Write(bPixelCurrent); // RED
                    bwPacket.Write(bPixelCurrent); // GREEN
                    bwPacket.Write(bPixelCurrent); // BLUE
                }

                // Convert into byte array
                byte[] bData = msPacket.ToArray();

                // Calc Checksum! 
                bData[Globals.TCP_HEADLEN + 10] = Globals.crc_8(bData, bData.Length, Globals.TCP_HEADLEN + 10);

                // Send byte area to serial
                this.serialPort.Write(bData, 0, bData.Length);
            }
        }
        private void cbBaudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iBaudrate = Int32.Parse(cbBaudrate.Text);
            if (serialPort != null && serialPort.IsOpen && serialPort.BaudRate != iBaudrate)
            {
                if (iBaudrate == 921600 || iBaudrate == 1500000)
                    this.serialPort.Write("%STARTLEDDATA:");
                else if (iBaudrate == 115200)
                    this.serialPort.Write("%ENDLEDDATA:");
                // reopen serial
                this.OpenSerial();
            }
        }

        /// <summary>Build ID for AMBIBACK_BUILD_ID: date, time and the short git commit of the source folder
        /// (with "-dirty" if it has local changes). Without git only date and time.</summary>
        private string GetBuildId()
        {
            string szId = DateTime.Now.ToString("yyyyMMdd-HHmm", CultureInfo.InvariantCulture);
            try
            {
                string szCommit = RunGit("rev-parse --short HEAD");
                if (!string.IsNullOrEmpty(szCommit))
                {
                    szId += "-" + szCommit;
                    if (!string.IsNullOrEmpty(RunGit("status --porcelain --untracked-files=no")))
                        szId += "-dirty";
                }
            }
            catch (Exception)
            {
                // git is optional
            }
            return szId;
        }

        private string RunGit(string szArgs)
        {
            using (Process pGit = new Process())
            {
                pGit.StartInfo.FileName = "git";
                pGit.StartInfo.Arguments = "-C \"" + szSourceDir + "\" " + szArgs;
                pGit.StartInfo.CreateNoWindow = true;
                pGit.StartInfo.UseShellExecute = false;
                pGit.StartInfo.RedirectStandardOutput = true;
                pGit.StartInfo.RedirectStandardError = true;
                pGit.Start();
                string szOut = pGit.StandardOutput.ReadToEnd().Trim();
                pGit.WaitForExit(5000);
                return pGit.ExitCode == 0 ? szOut : "";
            }
        }

        private bool doBuild()
        {
            bool bSuccess = false;

            var env = GetCurrentBuildEnv();
            bool hasToolchain = env.HasValue && (
                (env.Value.UseLegacyToolchain && !string.IsNullOrEmpty(env.Value.TensaTools)) ||
                (!env.Value.UseLegacyToolchain && !string.IsNullOrEmpty(env.Value.EspX32)));
            if (!env.HasValue || !hasToolchain || string.IsNullOrEmpty(env.Value.EsptoolPy) || string.IsNullOrEmpty(env.Value.ToolsBasePath))
            {
                MessageBox.Show("No valid build environment selected. Ensure build/buildenv and build/buildtools exist (or Arduino15/packages/esp32 for newer libs).", "Build Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            BuildEnvInfo benv = env.Value;

            // Prepare commands
            CDeviceType selectedType = (CDeviceType)cbDevice.Items[cbDevice.SelectedIndex];
            var (resolvedModel, resolvedBoard) = CDeviceHelper.ResolveBoardForBuildEnv(selectedType.Esp32Model, selectedType.Esp32Board, benv.UseLegacyToolchain);
            if (!benv.UseLegacyToolchain && (resolvedModel != selectedType.Esp32Model || resolvedBoard != selectedType.Esp32Board))
                AppendDebugText("[Build] Board resolved for 3.x: " + selectedType.Esp32Model + " → " + resolvedModel + ", " + selectedType.Esp32Board + " → " + resolvedBoard + "\r\n", Color.DarkCyan);

            string szArduinoDir = Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\Arduino15";
            string szBuildDir = Properties.Settings.Default.szBuildDir + "\\_build\\" + selectedType.DeviceType;
            string szCacheDir = Properties.Settings.Default.szBuildDir + "\\_cache\\" + selectedType.DeviceType;
            string szOutFileBin = szBuildDir + "\\AmbiBack.Controller.ino.bin";
            string szOutFilePartition = szBuildDir + "\\AmbiBack.Controller.ino.partitions.bin";
            string szExtraPreProcessor = "";
            string szBuildId = GetBuildId();
            
            szExtraPreProcessor =   "-DF_CPU=240000000L -DARDUINO=10813 " + 
                                    (selectedType.HasPSRam ? "-DBOARD_HAS_PSRAM " : "") +
                                    "-DARDUINO_" + resolvedBoard + " " + 
                                    "-DARDUINO_ARCH_ESP32 " + 
                                    "-DARDUINO_BOARD=\\\"" + resolvedBoard + "\\\" " +
//                                    "-DARDUINO_VARIANT=\\\"" + selectedType.Esp32Model + "\\\" " + 
                                    "-DESP32 " +
                                    "-DAMBIBACK_BUILD_ID=\\\"" + szBuildId + "\\\" " +
                                    (chkEsp32Debug.Checked ? "-DCORE_DEBUG_LEVEL=ESP_LOG_VERBOSE -DLOG_LOCAL_LEVEL=ESP_LOG_VERBOSE" : "-DCORE_DEBUG_LEVEL=0") + " " +
                                    (chkAmbiBackExtraDiag.Checked ? "-DESP32_EXTRA_DIAG " : "") +
                                    (chkAmbiBackVerboseDebug.Checked ? "-DESP32_VERBOSE_DEBUG " : "") +
                                    (selectedType.UsePSRamFix ? "-mfix-esp32-psram-cache-issue" : "");
            // szExtraPreProcessor = "-DBOARD_HAS_PSRAM -DESP32 -DCORE_DEBUG_LEVEL=0 -mfix-esp32-psram-cache-issue";
            string szCompilerArgs = "-logger=machine " +
                                    "-hardware \"" + szBinDir + "\\hardware\" " +
                                    "-hardware \"" + AppRootPath + "\\buildenv\\" + benv.Name + "\\packages\" " +
                                    "-tools \"" + szBinDir + "\\tools-builder\" " +
                                    "-tools \"" + szBinDir + "\\hardware\\tools\\avr\" " +
                                    "-tools \"" + AppRootPath + "\\buildenv\\" + benv.Name + "\\packages\" " +
                                    "-built-in-libraries \"" + szBinDir + "\\libraries\" " +
                                    "-libraries \"" + szExtraIncludeDir + "\\libraries\" " +
/*                                  "-fqbn=esp32:esp32:m5stick-c:                                                                             PartitionScheme=min_spiffs,UploadSpeed=1500000,DebugLevel=none -vid-pid=0403_6001 -ide-version=10813 -build-path " */
                                    "-fqbn=esp32:esp32:" + resolvedModel + ":" + (selectedType.HasPSRam ? "PSRAM=enabled," : "") + "PartitionScheme=min_spiffs," + 
                                        "UploadSpeed=" + selectedType.UploadSpeed + "," +
                                        selectedType.Esp32FqbnExtra + /*"CPUFreq=240,FlashMode=qio,FlashFreq=80,FlashSize=4M," +*/
                                        "DebugLevel=none " +
                                    "-vid-pid=10C4_EA60 " + 
                                    "-ide-version=10813 " +
                                    "-build-path \"" + szBuildDir + "\" " +
                                    "-warnings=none " +
                                    "-build-cache \"" + szCacheDir + "\" " +
                                    "-prefs=build.warn_data_percentage=75 " +
                                    "-prefs=runtime.tools.mkspiffs.path=" + benv.ToolsBasePath + "\\tools\\mkspiffs\\0.2.3 " +
                                    "-prefs=runtime.tools.mkspiffs-0.2.3.path=" + benv.ToolsBasePath + "\\tools\\mkspiffs\\0.2.3 " +
                                    (benv.UseLegacyToolchain
                                        ? "-prefs=runtime.tools.xtensa-esp32-elf-gcc.path=" + benv.ToolsBasePath + "\\tools\\xtensa-esp32-elf-gcc\\" + benv.TensaTools + " " +
                                          "-prefs=runtime.tools.xtensa-esp32-elf-gcc-" + benv.TensaTools + ".path=" + benv.ToolsBasePath + "\\tools\\xtensa-esp32-elf-gcc\\" + benv.TensaTools + " "
                                        : "-prefs=runtime.tools.esp-x32.path=" + benv.ToolsBasePath + "\\tools\\esp-x32\\" + benv.EspX32 + " " +
                                          "-prefs=runtime.tools.esp-x32-" + benv.EspX32 + ".path=" + benv.ToolsBasePath + "\\tools\\esp-x32\\" + benv.EspX32 + " " +
                                          "-prefs=runtime.tools.esp32-libs.path=" + benv.ToolsBasePath + "\\tools\\esp32-libs\\" + benv.Name + " " +
                                          "-prefs=runtime.tools.esp32-libs-" + benv.Name + ".path=" + benv.ToolsBasePath + "\\tools\\esp32-libs\\" + benv.Name + " ") +
                                    "-prefs=runtime.tools.esptool_py.path=" + benv.ToolsBasePath + "\\tools\\esptool_py\\" + benv.EsptoolPy + " " +
                                    "-prefs=runtime.tools.esptool_py-" + benv.EsptoolPy + ".path=" + benv.ToolsBasePath + "\\tools\\esptool_py\\" + benv.EsptoolPy + " " +
                                    "-prefs=\"build.extra_flags=-DESP32_DEVICE_TYPE=" + selectedType.PreprocessorName + " " + szExtraPreProcessor + "\" " +
                                    "-verbose " + 
                                    "\"" + szSourceDir + "\\" + szSourceFile + "\"";
            string szExtraPath = ";C:\\Program Files (x86)\\Arduino\\java\\bin";

            // Create helper dirs
            if (!Directory.Exists(szBuildDir))
            {
                Directory.CreateDirectory(szBuildDir);
            }
            if (!Directory.Exists(szCacheDir))
            {
                Directory.CreateDirectory(szCacheDir);
            }

            // Create child process.
            txtDebug.AppendText("Build Version " + txtVersionCode.Text + " for " + selectedType.DeviceType + " BEGIN\r\n");
            ScrollDebugToEnd();
            progressBarBuild.Value = 0;
            progressBarBuild.Visible = true;
            Process pBuild = new Process();
            pBuild.EnableRaisingEvents = true;
            pBuild.StartInfo.CreateNoWindow = true;
            pBuild.StartInfo.UseShellExecute = false;
            pBuild.StartInfo.RedirectStandardOutput = true;
            pBuild.StartInfo.RedirectStandardError = true;
            pBuild.StartInfo.WorkingDirectory = szBinDir;
            pBuild.StartInfo.EnvironmentVariables["PATH"] += szExtraPath;

            try
            {
                pBuild.StartInfo.FileName = szBinDir + "\\arduino-builder.exe";
                pBuild.StartInfo.Arguments = "-dump-prefs " + szCompilerArgs;
                // Redirect the output stream of the child process.
                pBuild.ErrorDataReceived += ErrorOutputHandler;
                pBuild.OutputDataReceived += StandardOutputHandler;

                // Start the process.
                pBuild.Start();

                txtDebug.AppendText("DEBUG: Command: " + pBuild.StartInfo.FileName + "\r\n");
                txtDebug.AppendText("DEBUG: Arguments: " + pBuild.StartInfo.Arguments + "\r\n");
                ScrollDebugToEnd();

                // Start the asynchronous read
                pBuild.BeginOutputReadLine();
                pBuild.BeginErrorReadLine();

                while (!pBuild.HasExited)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1);
                }
                pBuild.WaitForExit();

                Application.DoEvents();
                txtDebug.AppendText("\r\n\r\nBuild dump-prefs FINISHED\r\n");
                ScrollDebugToEnd();

                // Create child process.
                pBuild = new Process();
                pBuild.EnableRaisingEvents = true;
                pBuild.StartInfo.CreateNoWindow = true;
                pBuild.StartInfo.UseShellExecute = false;
                pBuild.StartInfo.RedirectStandardOutput = true;
                pBuild.StartInfo.RedirectStandardError = true;
                pBuild.StartInfo.WorkingDirectory = szBinDir;
                pBuild.StartInfo.EnvironmentVariables["PATH"] += szExtraPath;

                pBuild.StartInfo.FileName = szBinDir + "\\arduino-builder.exe";
                pBuild.StartInfo.Arguments = "-compile " +
//                                             "-build-options-file \"" + szBuildDir + "\\build.options.json" + "\" " +
                                            szCompilerArgs;
                // Redirect the output stream of the child process.
                pBuild.ErrorDataReceived += ErrorOutputHandler;
                pBuild.OutputDataReceived += StandardOutputHandler;

                // Start the process.
                pBuild.Start();

                txtDebug.AppendText("DEBUG: Command: " + pBuild.StartInfo.FileName + "\r\n");
                txtDebug.AppendText("DEBUG: Arguments: " + pBuild.StartInfo.Arguments + "\r\n");
                ScrollDebugToEnd();

                // Remove prevois builds
                if (File.Exists(szOutFileBin))
                    File.Delete(szOutFileBin);
                if (File.Exists(szOutFilePartition))
                    File.Delete(szOutFilePartition);

                // Start the asynchronous read
                pBuild.BeginOutputReadLine();
                pBuild.BeginErrorReadLine();
                while (!pBuild.HasExited)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1);
                }
                pBuild.WaitForExit();

                Application.DoEvents();
                txtDebug.AppendText("\r\n\r\nBuild compile FINISHED\r\n");
                ScrollDebugToEnd();

                // Build was successfull!
                if (File.Exists(szOutFileBin) && File.Exists(szOutFilePartition))
                {
                    string szOutDirDest = AppRootPath + "\\firmware\\" + selectedType.DeviceType + "\\" + txtVersionCode.Text;
                    string szOutFileBinDest = szOutDirDest + "\\AmbiBack.Controller.ino.bin";
                    string szOutFilePartitionDest = szOutDirDest + "\\AmbiBack.Controller.ino.partitions.bin";

                    // Create helper dirs
                    if (!Directory.Exists(szOutDirDest))
                    {
                        Directory.CreateDirectory(szOutDirDest);
                    }

                    // Backup existing images
                    if (File.Exists(szOutFileBinDest))
                    {
                        if (File.Exists(szOutFileBinDest + ".bak"))
                            File.Delete(szOutFileBinDest + ".bak");
                        File.Move(szOutFileBinDest, szOutFileBinDest + ".bak");
                    }
                    if (File.Exists(szOutFilePartitionDest))
                    {
                        if (File.Exists(szOutFilePartitionDest + ".bak"))
                            File.Delete(szOutFilePartitionDest + ".bak");
                        File.Move(szOutFilePartitionDest, szOutFilePartitionDest + ".bak");
                    }

                    // Copy new builds
                    File.Copy(szOutFileBin, szOutFileBinDest, true);
                    File.Copy(szOutFilePartition, szOutFilePartitionDest, true);

                    // Write build information next to the image (the firmware shows the same values in its web UI)
                    File.WriteAllText(szOutDirDest + "\\buildinfo.txt",
                        "version=" + txtVersionCode.Text + "\r\n" +
                        "device=" + selectedType.DeviceType + "\r\n" +
                        "buildid=" + szBuildId + "\r\n" +
                        "buildenv=" + benv.Name + "\r\n" +
                        "toolchain=" + (benv.UseLegacyToolchain ? "xtensa-esp32-elf-gcc " + benv.TensaTools : "esp-x32 " + benv.EspX32) + "\r\n" +
                        "built=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "\r\n");

                    bSuccess = true;
                }
            }
            catch (Exception ex)
            {
                string szErrorText = "Command: " + pBuild.StartInfo.FileName +
                                        "\r\nArguments: " + pBuild.StartInfo.Arguments +
                                        "\r\n\r\nException occured in cmdFlash" +
                                        "\r\nException Type: " + ex.GetType().ToString() +
                                        "\r\nDetails: \r\n\r\n" + ex.ToString();
                AppendDebugText(szErrorText, Color.Red);
            }
            finally
            {
                progressBarBuild.Visible = false;
            }

            return bSuccess;
        }

        private bool doUpload()
        {
            bool bSuccess = false;
            try
            {
                CDeviceType selectedType = (CDeviceType)cbDevice.Items[cbDevice.SelectedIndex];
                string szVersion = txtVersionCode.Text.Trim();
                if (!Regex.IsMatch(szVersion, @"^\d+\.\d+\.\d+$"))
                {
                    AppendDebugText("Upload: invalid version '" + szVersion + "'\r\n", Color.Red);
                    return false;
                }

                string szSrcFileBin = AppRootPath + "\\firmware\\" + selectedType.DeviceType + "\\" + szVersion + "\\AmbiBack.Controller.ino.bin";
                string szScript = Path.Combine(Application.StartupPath, "scripts", "upload-firmware.ps1");
                string szEnvFile = Path.Combine(AppRootPath, ".env");

                // Start the child process.
                Process pUpload = new Process();
                pUpload.EnableRaisingEvents = true;
                pUpload.StartInfo.CreateNoWindow = true;
                pUpload.StartInfo.UseShellExecute = false;
                pUpload.StartInfo.RedirectStandardOutput = true;
                pUpload.StartInfo.RedirectStandardError = true;

                pUpload.StartInfo.FileName = "powershell.exe";
                pUpload.StartInfo.Arguments = "-NoProfile -NonInteractive -ExecutionPolicy Bypass -File \"" + szScript + "\"" +
                                              " -Device " + selectedType.DeviceType +
                                              " -Version " + szVersion +
                                              " -SourceFile \"" + szSrcFileBin + "\"" +
                                              " -EnvFile \"" + szEnvFile + "\"";

                // Redirect the output stream of the child process.
                pUpload.ErrorDataReceived += ErrorOutputHandler;
                pUpload.OutputDataReceived += StandardOutputHandler;

                // Start the process.
                pUpload.Start();

                txtDebug.AppendText("Starting upload: " + selectedType.DeviceType + " " + szVersion + "\r\n");
                ScrollDebugToEnd();

                // Start the asynchronous read
                pUpload.BeginOutputReadLine();
                pUpload.BeginErrorReadLine();

                while (!pUpload.HasExited)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1);
                }
                pUpload.WaitForExit();

                Application.DoEvents();
                bSuccess = pUpload.ExitCode == 0;
                AppendDebugText(bSuccess ? "\r\n\r\nUpload FINISHED\r\n" : "\r\n\r\nUpload FAILED\r\n", bSuccess ? (Color?)null : Color.Red);
                ScrollDebugToEnd();
            }
            catch (Exception ex)
            {
                string szErrorText = "Exception occured in cmdUpload" +
                                        "\r\nException Type: " + ex.GetType().ToString() +
                                        "\r\nDetails: \r\n\r\n" + ex.ToString();
                AppendDebugText(szErrorText, Color.Red);
            }

            return bSuccess;
        }

        private void cmdBuild_Click(object sender, EventArgs e)
        {
            txtDebug.Clear();
            this.CloseSerial();

            cmdBuild.Enabled = false;
            cmdCleanBuild.Enabled = false;
            cmdBuildAndUpload.Enabled = false;
            cmdUpload.Enabled = false;

            doBuild();

            cmdUpload.Enabled = true;
            cmdBuild.Enabled = true;
            cmdCleanBuild.Enabled = true;
            cmdBuildAndUpload.Enabled = true;
        }

        private void cmdCleanBuild_Click(object sender, EventArgs e)
        {
            // Prepare commands
            CDeviceType selectedType = (CDeviceType)cbDevice.Items[cbDevice.SelectedIndex];
            string szBuildDir = Properties.Settings.Default.szBuildDir + "\\_build\\" + selectedType.DeviceType;
            string szCacheDir = Properties.Settings.Default.szBuildDir + "\\_cache\\" + selectedType.DeviceType;

            try
            {
                Directory.Delete(szBuildDir, true);
                Directory.Delete(szCacheDir, true);
            }
            catch (Exception ex)
            {
                string szErrorText = "Exception occured in cmdCleanBuild" +
                                        "\r\nException Type: " + ex.GetType().ToString() +
                                        "\r\nDetails: \r\n\r\n" + ex.ToString();
                AppendDebugText(szErrorText, Color.Red);
            }

        }

        private void SetButtons(bool bState)
        {
            cmdBuild.Enabled = bState;
            cmdCleanBuild.Enabled = bState;
            cmdBuildAndUpload.Enabled = bState;
            cmdBuildAndFlash.Enabled = bState;
            cmdUpload.Enabled = bState;
        }
        private void cmdUpload_Click(object sender, EventArgs e)
        {
            txtDebug.Clear();
            this.CloseSerial();

            SetButtons(false);
            doUpload();
            SetButtons(true);
        }

        private void cmdBuildAndUpload_Click(object sender, EventArgs e)
        {
            txtDebug.Clear();
            this.CloseSerial();

            SetButtons(false);
            if (doBuild())
                doUpload();
            SetButtons(true);
        }

        private void cmdReboot_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                this.serialPort.Write("%REBOOT:");
            }
        }

        private void cmdFactoryDefaults_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                this.serialPort.Write("%FACTORYDEFAULT:");
            }
        }

        private void cmdBuildAndFlash_Click(object sender, EventArgs e)
        {
            txtDebug.Clear();
            this.CloseSerial();

            SetButtons(false);
            if (doBuild()) {
                this.cmdFlash_Click(sender, e);
            }
            SetButtons(true);
        }

        private void cmdSetRelay_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                // Payload helpers 
                MemoryStream msPacket = new MemoryStream();
                BinaryWriter bwPacket = new BinaryWriter(msPacket);

                // Write Byte array
                bwPacket.Write("%SETRELAY:");
                bwPacket.Write(txtRelayID.Text);
                bwPacket.Write((byte)0x00);

                // Send byte area to serial
                byte[] bData = msPacket.ToArray();
                this.serialPort.Write(bData, 0, bData.Length);
            }
        }

        private void cmdModeOff_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                // Payload helpers 
                MemoryStream msPacket = new MemoryStream();
                BinaryWriter bwPacket = new BinaryWriter(msPacket);

                // Write Byte array
                bwPacket.Write("%LEDMODE:0");
                bwPacket.Write((byte)0x00);

                // Send byte area to serial
                byte[] bData = msPacket.ToArray();
                this.serialPort.Write(bData, 0, bData.Length);
            }
        }

        private void cmdModeDemo_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                // Payload helpers 
                MemoryStream msPacket = new MemoryStream();
                BinaryWriter bwPacket = new BinaryWriter(msPacket);

                // Write Byte array
                bwPacket.Write("%LEDMODE:5");
                bwPacket.Write((byte)0x00);

                // Send byte area to serial
                byte[] bData = msPacket.ToArray();
                this.serialPort.Write(bData, 0, bData.Length);
            }
        }

        private void cmdModeCam_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                // Payload helpers 
                MemoryStream msPacket = new MemoryStream();
                BinaryWriter bwPacket = new BinaryWriter(msPacket);

                // Write Byte array
                bwPacket.Write("%LEDMODE:3");
                bwPacket.Write((byte)0x00);

                // Send byte area to serial
                byte[] bData = msPacket.ToArray();
                this.serialPort.Write(bData, 0, bData.Length);
            }
        }

        private void menuQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cmdSettings_Click(object sender, EventArgs e)
        {
            using (var frm = new frmSettings())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Settings are saved in frmSettings; values are read via properties
                }
            }
        }

        private void cmdResetWifi_Click(object sender, EventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                // Payload helpers 
                MemoryStream msPacket = new MemoryStream();
                BinaryWriter bwPacket = new BinaryWriter(msPacket);

                // Write Byte array
                bwPacket.Write("%RESETWIFI");
                bwPacket.Write((byte)0x00);

                // Send byte area to serial
                byte[] bData = msPacket.ToArray();
                this.serialPort.Write(bData, 0, bData.Length);
            }

        }
    }
}
