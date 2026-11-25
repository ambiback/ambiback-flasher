using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmbiBack_USB_Flasher
{
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
        public const string BuilderCli = "arduino-cli";
        public const string BuilderLegacy = "arduino-builder";
        private const string ImageFile = "AmbiBack.Controller.ino.bin";
        private const string PartitionsFile = "AmbiBack.Controller.ino.partitions.bin";
        private const string BootloaderFile = "AmbiBack.Controller.ino.bootloader.bin";

        public string szDebugOutputHelper = "";  // Runtime buffer, not persisted

        /// <summary>App root path (firmware, buildenv, buildtools, esptool.exe).</summary>
        public string AppRootPath { get { return AppPaths.Root; } }

        /// <summary>Detected arduino-builder environments from buildenv. Populated on load.</summary>
        private List<BuildEnvInfo> _buildEnvs = new List<BuildEnvInfo>();

        private SerialPort serialPort = null;
        private bool _busy = false;
        private bool _syncingDevice = false;
        private bool _loading = true;
        private string[] _knownPorts = new string[0];

        /// <summary>An entry of the Easy mode firmware list. Version null = build the newest source.</summary>
        private class EasyFirmwareOption
        {
            public string Version;
            public string Text;
            public override string ToString() { return Text; }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private bool IsCliBuilder { get { return cbBuilder.Text != BuilderLegacy; } }

        private CDeviceType SelectedDevice
        {
            get { return cbDevice.SelectedIndex >= 0 ? (CDeviceType)cbDevice.Items[cbDevice.SelectedIndex] : null; }
        }

        /// <summary>Get the currently selected arduino-builder env info. Returns null if none selected or invalid.</summary>
        private BuildEnvInfo? GetCurrentBuildEnv()
        {
            string sel = cbBuildEnv?.Text;
            if (string.IsNullOrEmpty(sel)) return null;
            var env = _buildEnvs.FirstOrDefault(e => e.Name == sel);
            return string.IsNullOrEmpty(env.Name) ? (BuildEnvInfo?)null : env;
        }

        // ================================================================== startup and layout

        private void frmMain_Load(object sender, EventArgs e)
        {
            Globals.MainForm = this;
            L.Init();
            ApplyTheme();

            // Devices: combo for Advanced, picture list for Easy
            cbDevice.DataSource = CDeviceHelper.aDeviceTypes.Clone();
            cbDevice.DisplayMember = "DisplayLabel";
            cbDevice.ValueMember = "DeviceType";
            foreach (CDeviceType t in CDeviceHelper.aDeviceTypes)
            {
                imgDevices.Images.Add(t.DeviceType, DeviceImages.Thumbnail(t.DeviceType, 96));
                lvDevices.Items.Add(new ListViewItem(t.DisplayLabel.Replace("AmbiBack ", ""), t.DeviceType) { Tag = t });
            }

            // Builder and build environments
            cbBuilder.Text = Properties.Settings.Default.szBuilder == BuilderLegacy ? BuilderLegacy : BuilderCli;
            PopulateBuildEnvCombo();
            UpdateComPorts();

            // Easy mode Wi-Fi fields share the settings with the Advanced fields
            txtEasySsid.DataBindings.Add("Text", Properties.Settings.Default, "szWifiSSID", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEasyKey.DataBindings.Add("Text", Properties.Settings.Default, "szWifiKey", true, DataSourceUpdateMode.OnPropertyChanged);
            chkEasyWifi.Checked = Properties.Settings.Default.bEasyWifi;
            chkEasyWifi_CheckedChanged(sender, e);

            // Restore the device (older versions stored the display label)
            string saved = Properties.Settings.Default.szDevice;
            int idx = -1;
            for (int i = 0; i < cbDevice.Items.Count; i++)
            {
                var t = (CDeviceType)cbDevice.Items[i];
                if (t.DisplayLabel == saved || t.DeviceType == saved) { idx = i; break; }
            }
            _loading = false;
            cbDevice.SelectedIndex = idx >= 0 ? idx : 0;
            cbDevice_SelectedIndexChanged(sender, e);
            if (idx < 0) lvDevices.SelectedItems.Clear();

            ApplyLanguage();
            SetMode(Properties.Settings.Default.bAdvancedMode);
            SetStatus(L.T("status.ready"));
        }

        private void ApplyTheme()
        {
            Icon icon = LoadIconResource("ambiback.ico");
            if (icon != null) this.Icon = icon;
            picLogo.Image = LoadImageResource("ambiback-logo.png");

            lblTitle.Font = Theme.TitleFont;
            lblSubtitle.ForeColor = Theme.SecondaryText;
            Theme.StyleSegment(rbEasy);
            Theme.StyleSegment(rbAdvanced);

            foreach (var l in new[] { lblEasyStep1, lblEasyStep2, lblEasyStep3 })
                l.Font = Theme.SectionFont;
            lblEasyDeviceName.Font = new Font("Segoe UI Semibold", 10F);
            lblEasyStatus.ForeColor = Theme.SecondaryText;
            cmdEasyFlash.Font = Theme.BigButtonFont;
            Theme.StylePrimary(cmdEasyFlash);
            Theme.StylePrimary(cmdFlash);
            lvDevices.BackColor = Color.White;

            // Advanced: white cards on the same light surface as Easy mode, flat secondary buttons
            splitAdvanced.BackColor = Theme.Surface;
            panelAdvLeft.BackColor = Theme.Surface;
            foreach (Button b in AllControls(panelAdvanced).OfType<Button>())
            {
                if (b == cmdFlash) continue;
                if (b == cmdFactoryDefaults) Theme.StyleDestructive(b);
                else Theme.StyleSecondary(b);
            }
            // The cards are docked to the top; each one keeps an unpainted gap below itself
            foreach (CardGroupBox card in new[] { gbConnection, gbBuild, gbDeviceInfo, gbDeviceControl, gbWifi })
            {
                card.BottomGap = 8;
                card.Height += 8;
            }
            toolStripLog.BackColor = Theme.Card;
            toolStripLog.Renderer = new FlatToolStripRenderer();
            toolStripLog.Padding = new Padding(2, 2, 2, 4);

            txtDebug.BackColor = Theme.LogBack;
            txtDebug.ForeColor = Theme.LogText;
            txtDebug.Font = Theme.LogFont;
        }

        private static IEnumerable<Control> AllControls(Control root)
        {
            foreach (Control c in root.Controls)
            {
                yield return c;
                foreach (Control child in AllControls(c)) yield return child;
            }
        }

        private static Image LoadImageResource(string name)
        {
            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("AmbiBack_USB_Flasher.Resources." + name))
                return s == null ? null : new Bitmap(Image.FromStream(s));
        }

        private static Icon LoadIconResource(string name)
        {
            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("AmbiBack_USB_Flasher.Resources." + name))
                return s == null ? null : new Icon(s);
        }

        private void ApplyLanguage()
        {
            menuLangEnglish.Checked = L.Language == "en";
            menuLangGerman.Checked = L.Language == "de";

            menuFile.Text = L.T("menu.file");
            menuSettings.Text = L.T("menu.settings");
            menuOpenDataFolder.Text = L.T("menu.openData");
            menuOpenFirmwareFolder.Text = L.T("menu.openFirmware");
            menuQuit.Text = L.T("menu.exit");
            menuView.Text = L.T("menu.view");
            menuViewEasy.Text = L.T("menu.easy");
            menuViewAdvanced.Text = L.T("menu.advanced");
            menuClearLog.Text = L.T("menu.clearLog");
            menuDevice.Text = L.T("menu.device");
            menuGetInfo.Text = L.T("menu.getInfo");
            menuReboot.Text = L.T("menu.reboot");
            menuSetWifi.Text = L.T("menu.setWifi");
            menuResetWifi.Text = L.T("menu.resetWifi");
            menuFactoryDefaults.Text = L.T("menu.factory");
            menuFirmware.Text = L.T("menu.firmware");
            menuBuild.Text = L.T("menu.build");
            menuBuildAndFlash.Text = L.T("menu.buildFlash");
            menuFlash.Text = L.T("menu.flash");
            menuCleanBuild.Text = L.T("menu.clean");
            menuUpload.Text = L.T("menu.upload");
            menuBuildAndUpload.Text = L.T("menu.buildUpload");
            menuTools.Text = L.T("menu.tools");
            menuSetupTools.Text = L.T("menu.setup");
            menuDownloadSource.Text = L.T("menu.downloadSource");
            menuLanguage.Text = L.T("menu.language");
            menuHelp.Text = L.T("menu.help");
            menuProjectPage.Text = L.T("menu.projectPage");
            menuFirmwarePage.Text = L.T("menu.firmwarePage");
            menuAbout.Text = L.T("menu.about");

            lblSubtitle.Text = L.T("header.subtitle");
            rbEasy.Text = L.T("mode.easy");
            rbAdvanced.Text = L.T("mode.advanced");

            lblEasyStep1.Text = L.T("easy.step1");
            lblEasyStep2.Text = L.T("easy.step2");
            lblEasyStep3.Text = L.T("easy.step3");
            chkEasyWifi.Text = L.T("easy.wifi");
            lblEasySsid.Text = L.T("easy.ssid");
            lblEasyKey.Text = L.T("easy.key");
            cmdEasyFlash.Text = L.T("easy.flash");
            lnkEasyDetails.Text = L.T("easy.details");

            gbConnection.Text = L.T("grp.connection");
            gbBuild.Text = L.T("grp.firmware");
            gbDeviceInfo.Text = L.T("grp.deviceInfo");
            gbDeviceControl.Text = L.T("grp.control");
            gbWifi.Text = L.T("grp.wifi");
            gbDebug.Text = L.T("grp.output");
            lblComPort.Text = L.T("lbl.port");
            lblBaudrate.Text = L.T("lbl.baud");
            cmdGetInfo.Text = L.T("btn.getInfo");
            cmdReboot.Text = L.T("btn.restart");
            lblDevice.Text = L.T("lbl.device");
            lblVersion.Text = L.T("lbl.image");
            lblSourceVersion.Text = L.T("lbl.source");
            lblBuilder.Text = L.T("lbl.builder");
            lblBuildEnv.Text = L.T("lbl.core");
            cmdSetupTools.Text = L.T("btn.setup");
            chkEsp32Debug.Text = L.T("chk.coreDebug");
            chkAmbiBackVerboseDebug.Text = L.T("chk.verbose");
            chkAmbiBackExtraDiag.Text = L.T("chk.diag");
            cmdBuild.Text = L.T("btn.build");
            cmdBuildAndFlash.Text = L.T("btn.buildFlash");
            cmdFlash.Text = L.T("btn.flash");
            cmdCleanBuild.Text = L.T("btn.clean");
            cmdBuildAndUpload.Text = L.T("btn.buildUpload");
            cmdUpload.Text = L.T("btn.upload");
            lblInfoVersion.Text = L.T("lbl.fwType");
            lblInfoId.Text = L.T("lbl.idMac");
            lblInfoName.Text = L.T("lbl.name");
            lblInfoIp.Text = L.T("lbl.ip");
            lblLedMode.Text = L.T("lbl.ledMode");
            cmdModeOff.Text = L.T("btn.off");
            cmdModeDemo.Text = L.T("btn.demo");
            cmdModeCam.Text = L.T("btn.camera");
            cmdSendLedTestData.Text = L.T("btn.ledTest");
            lblRelayId.Text = L.T("lbl.relay");
            cmdSetRelay.Text = L.T("btn.setRelay");
            cmdFactoryDefaults.Text = L.T("btn.factory");
            lblSsid.Text = L.T("lbl.ssid");
            lblKey.Text = L.T("lbl.key");
            cmdSetWifi.Text = L.T("btn.sendWifi");
            cmdResetWifi.Text = L.T("btn.resetWifi");
            tsbClearLog.Text = L.T("log.clear");
            tsbCopyLog.Text = L.T("log.copy");

            toolTip.SetToolTip(cbFirmwareVersion, L.T("tip.image"));
            toolTip.SetToolTip(txtVersionCode, L.T("tip.source"));
            toolTip.SetToolTip(cbBuilder, L.T("tip.builder"));
            toolTip.SetToolTip(cmdUpload, L.T("tip.upload"));
            toolTip.SetToolTip(cmdBuildAndUpload, L.T("tip.upload"));

            UpdateConnectionTexts();
            UpdateEasyCard();
            UpdateEasyPortHint();
        }

        private void SetMode(bool advanced)
        {
            panelAdvanced.Visible = advanced;
            panelEasy.Visible = !advanced;
            rbAdvanced.Checked = advanced;
            rbEasy.Checked = !advanced;
            menuViewEasy.Checked = !advanced;
            menuViewAdvanced.Checked = advanced;
            Properties.Settings.Default.bAdvancedMode = advanced;
            if (advanced) SyncAdvancedPort();
        }

        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
                SetMode(sender == rbAdvanced);
        }

        private void menuViewEasy_Click(object sender, EventArgs e) { SetMode(false); }
        private void menuViewAdvanced_Click(object sender, EventArgs e) { SetMode(true); }
        private void lnkEasyDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) { SetMode(true); }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Theme.Separator))
                e.Graphics.DrawLine(pen, 0, panelHeader.Height - 1, panelHeader.Width, panelHeader.Height - 1);
        }

        private void panelEasyCard_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Theme.Separator))
                e.Graphics.DrawLine(pen, 0, 0, 0, panelEasyCard.Height);
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Oemcomma)
            {
                cmdSettings_Click(sender, e);
                e.Handled = true;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_busy && e.CloseReason == CloseReason.UserClosing)
            {
                MessageBox.Show(this, L.T("dlg.busy"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
                return;
            }
            CloseSerial();
            Properties.Settings.Default.Save();
        }

        // ================================================================== status, progress, busy state

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        private void SetEasyStatus(string text, Color? color = null)
        {
            lblEasyStatus.Text = text;
            lblEasyStatus.ForeColor = color ?? Theme.SecondaryText;
            SetStatus(text);
            _easyHintShown = false;
        }

        /// <summary>True while lblEasyStatus shows the port hint of UpdateEasyPortHint (in whatever language).</summary>
        private bool _easyHintShown = false;

        /// <summary>Show progress in the status bar and in the Easy card. -1 = running without percentage, null = hide.</summary>
        private void SetProgress(int? percent)
        {
            if (percent == null)
            {
                progressBarBuild.Visible = false;
                pbEasy.Visible = false;
                return;
            }
            var style = percent < 0 ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            progressBarBuild.Style = style;
            pbEasy.Style = style;
            if (percent >= 0)
            {
                int v = Math.Min(100, percent.Value);
                progressBarBuild.Value = v;
                pbEasy.Value = v;
            }
            progressBarBuild.Visible = true;
            pbEasy.Visible = true;
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;
            SetButtons(!busy);
            cmdFlash.Enabled = !busy;
            cmdEasyFlash.Enabled = !busy;
            lvDevices.Enabled = !busy;
            cbEasyPort.Enabled = !busy;
            cbEasyFirmware.Enabled = !busy;
            chkEasyWifi.Enabled = !busy;
            cbDevice.Enabled = !busy;
            cbBuilder.Enabled = !busy;
            cbBuildEnv.Enabled = !busy;
            cmdSetupTools.Enabled = !busy;
            cmdMonitor.Enabled = !busy;
            menuTools.Enabled = !busy;
            menuSettings.Enabled = !busy;
            UseWaitCursor = false;
        }

        private void SetButtons(bool bState)
        {
            cmdBuild.Enabled = bState;
            cmdCleanBuild.Enabled = bState;
            cmdBuildAndUpload.Enabled = bState;
            cmdBuildAndFlash.Enabled = bState;
            cmdUpload.Enabled = bState;
        }

        /// <summary>Append text to the output with optional color. Use null for the default color.</summary>
        private void AppendDebugText(string text, Color? color = null)
        {
            txtDebug.SelectionStart = txtDebug.TextLength;
            txtDebug.SelectionLength = 0;
            txtDebug.SelectionColor = color.HasValue ? Theme.MapLogColor(color.Value) : txtDebug.ForeColor;
            txtDebug.AppendText(text);
            txtDebug.SelectionColor = txtDebug.ForeColor;
            ScrollDebugToEnd();
        }

        /// <summary>Thread-safe log line.</summary>
        private void LogLine(string line, Color? color = null)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(() => AppendDebugText(line + "\r\n", color)));
            else
                AppendDebugText(line + "\r\n", color);
        }

        /// <summary>Scroll the debug output to show the latest content.</summary>
        private void ScrollDebugToEnd()
        {
            txtDebug.SelectionStart = txtDebug.TextLength;
            txtDebug.ScrollToCaret();
        }

        private void tsbClearLog_Click(object sender, EventArgs e) { txtDebug.Clear(); }

        private void tsbCopyLog_Click(object sender, EventArgs e)
        {
            if (txtDebug.TextLength > 0) Clipboard.SetText(txtDebug.Text);
        }

        // ================================================================== devices

        private void cbDevice_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0) return;
            var t = (CDeviceType)cbDevice.Items[e.Index];
            int size = e.Bounds.Height - 4;
            // ImageList.Images[key] creates a new Bitmap on every access; Draw does not
            int imgIndex = imgDevices.Images.IndexOfKey(t.DeviceType);
            if (imgIndex >= 0)
                imgDevices.Draw(e.Graphics, e.Bounds.X + 2, e.Bounds.Y + 2, size, size, imgIndex);
            var textRect = new Rectangle(e.Bounds.X + size + 8, e.Bounds.Y, e.Bounds.Width - size - 8, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, t.DisplayLabel.Replace("AmbiBack ", ""), e.Font, textRect, e.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            e.DrawFocusRectangle();
        }

        private void lvDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_syncingDevice || lvDevices.SelectedItems.Count == 0) return;
            int idx = lvDevices.SelectedItems[0].Index;
            if (cbDevice.SelectedIndex != idx)
                cbDevice.SelectedIndex = idx;
        }

        private void cbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading) return;
            var selectedType = SelectedDevice;
            if (selectedType == null) return;

            Properties.Settings.Default.szDevice = selectedType.DisplayLabel;

            _syncingDevice = true;
            try
            {
                if (cbDevice.SelectedIndex < lvDevices.Items.Count && (lvDevices.SelectedItems.Count == 0 || lvDevices.SelectedItems[0].Index != cbDevice.SelectedIndex))
                {
                    lvDevices.SelectedItems.Clear();
                    lvDevices.Items[cbDevice.SelectedIndex].Selected = true;
                    lvDevices.Items[cbDevice.SelectedIndex].EnsureVisible();
                }
            }
            finally
            {
                _syncingDevice = false;
            }

            try
            {
                cbFirmwareVersion.Items.Clear();
                var versions = GetFirmwareVersions(selectedType);
                foreach (string v in versions)
                    cbFirmwareVersion.Items.Add(v);
                if (versions.Count > 0)
                    cbFirmwareVersion.SelectedIndex = versions.Count - 1;   // newest image
                else
                    cbFirmwareVersion.Text = "";

                RefreshSourceVersion();
            }
            catch (Exception ex)
            {
                Globals.HandleException("cbDevice_SelectedIndexChanged", ex, false, false);
            }
            UpdateEasyCard();
        }

        /// <summary>Image versions in firmware\&lt;device&gt;, oldest first. Only folders that contain an image count.</summary>
        private List<string> GetFirmwareVersions(CDeviceType type)
        {
            string dir = Path.Combine(AppPaths.FirmwareDir, type.DeviceType);
            if (!Directory.Exists(dir)) return new List<string>();
            return Directory.GetDirectories(dir)
                .Where(d => File.Exists(Path.Combine(d, ImageFile)))
                .Select(Path.GetFileName)
                .OrderBy(AppPaths.VersionKey)
                .ToList();
        }

        /// <summary>Read the firmware version from globalincludes.h of the source folder.</summary>
        private void RefreshSourceVersion()
        {
            string szGlobalIncludesFile = Path.Combine(AppPaths.SourceDir, "globalincludes.h");
            if (File.Exists(szGlobalIncludesFile))
            {
                string szGlobalIncludesCode = File.ReadAllText(szGlobalIncludesFile);
                string szVersion = Globals.GetSubstringFromLine(szGlobalIncludesCode, "#define AMBIBACK_VERSION \"", "\"", false);
                txtVersionCode.Text = (szVersion ?? "").Trim();
            }
            else
                txtVersionCode.Text = "";
        }

        private void UpdateEasyCard()
        {
            var t = SelectedDevice;
            if (t == null)
            {
                picEasyDevice.Image = null;
                lblEasyDeviceName.Text = L.T("easy.noDevice");
                return;
            }
            picEasyDevice.Image = DeviceImages.Get(t.DeviceType);
            lblEasyDeviceName.Text = t.DisplayLabel.Replace("AmbiBack ", "");

            object keep = cbEasyFirmware.SelectedItem;
            cbEasyFirmware.Items.Clear();
            var versions = GetFirmwareVersions(t);
            for (int i = versions.Count - 1; i >= 0; i--)
                cbEasyFirmware.Items.Add(new EasyFirmwareOption { Version = versions[i], Text = L.F("easy.existing", versions[i]) });
            cbEasyFirmware.Items.Add(new EasyFirmwareOption { Version = null, Text = L.T("easy.buildLatest") });
            int sel = 0;
            if (keep is EasyFirmwareOption k)
                for (int i = 0; i < cbEasyFirmware.Items.Count; i++)
                    if (((EasyFirmwareOption)cbEasyFirmware.Items[i]).Version == k.Version) { sel = i; break; }
            cbEasyFirmware.SelectedIndex = sel;
        }

        // ================================================================== COM ports

        private void UpdateComPorts()
        {
            string[] aPorts = SerialPort.GetPortNames().Distinct().OrderBy(p => p.Length).ThenBy(p => p).ToArray();
            string[] added = aPorts.Except(_knownPorts).ToArray();
            bool changed = added.Length > 0 || _knownPorts.Except(aPorts).Any();
            _knownPorts = aPorts;
            if (!changed && cbComPort.Items.Count == aPorts.Length) return;

            string current = cbComPort.Text;
            cbComPort.Items.Clear();
            cbComPort.Items.AddRange(aPorts);
            cbComPort.Text = current;

            // Easy mode: list the ports and pick a newly plugged-in device automatically
            string easyCurrent = cbEasyPort.SelectedItem as string;
            cbEasyPort.Items.Clear();
            cbEasyPort.Items.AddRange(aPorts);
            string pick = null;
            // Never switch to a new port while building or flashing: the job must stay on the chosen port
            if (added.Length > 0 && !_loading && _knownPortsInitialized && !_busy) pick = added.Last();
            else if (easyCurrent != null && aPorts.Contains(easyCurrent)) pick = easyCurrent;
            else if (aPorts.Contains(current)) pick = current;
            else if (aPorts.Length > 0) pick = aPorts.Last();
            if (pick != null) cbEasyPort.SelectedItem = pick;
            SyncAdvancedPort();
            _knownPortsInitialized = true;
            UpdateEasyPortHint();
        }

        private bool _knownPortsInitialized = false;

        private void UpdateEasyPortHint()
        {
            if (_busy) return;
            // Replace only the port hint itself (also after a language change), never a result or error message
            if (cbEasyPort.Items.Count == 0)
                SetEasyStatus(L.T("easy.noPort"), Theme.Destructive);
            else if (_easyHintShown || string.IsNullOrEmpty(lblEasyStatus.Text))
                SetEasyStatus(L.T("easy.hint"));
            else
                return;
            _easyHintShown = true;
        }

        private void cbEasyPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncAdvancedPort();
        }

        /// <summary>The Advanced port follows the Easy selection while no serial connection is open.</summary>
        private void SyncAdvancedPort()
        {
            if (cbEasyPort.SelectedItem is string port && serialPort == null && !_busy)
                SetAdvancedPort(port);
        }

        /// <summary>
        /// Set the Advanced port. cbComPort.Text is bound to szComPort, but the binding only becomes active
        /// once the (hidden) Advanced panel is shown and then overwrites the combo with the stored value,
        /// so the setting is updated as well.
        /// </summary>
        private void SetAdvancedPort(string port)
        {
            Properties.Settings.Default.szComPort = port;
            cbComPort.Text = port;
        }

        private void timerComPort_Tick(object sender, EventArgs e)
        {
            UpdateComPorts();
        }

        // ================================================================== serial monitor and device commands

        private bool IsSerialOpen { get { return serialPort != null && serialPort.IsOpen; } }

        private void UpdateConnectionTexts()
        {
            bool open = IsSerialOpen;
            cmdMonitor.Text = open ? L.T("btn.disconnect") : L.T("btn.connect");
            menuConnect.Text = open ? L.T("menu.disconnect") : L.T("menu.connect");
            lblPortStatus.Text = open ? L.F("port.connected", serialPort.PortName) : L.T("port.none");
            foreach (var b in new Button[] { cmdGetInfo, cmdSendLedTestData, cmdSetWifi, cmdResetWifi, cmdReboot, cmdFactoryDefaults, cmdSetRelay, cmdModeCam, cmdModeDemo, cmdModeOff })
                b.Enabled = open;
        }

        private void cmdMonitor_Click(object sender, EventArgs e)
        {
            if (!IsSerialOpen)
            {
                try
                {
                    txtDebug.Clear();
                    this.OpenSerial();

                    txtConnectedDeviceVersion.Text = "";
                    txtConnectedDeviceType.Text = "";
                    txtConnectedDeviceID.Text = "";
                    txtConnectedDeviceMAC.Text = "";
                    txtConnectedDeviceName.Text = "";
                    txtConnectedDeviceIP.Text = "";

                    // Trigger Info Click
                    if (IsSerialOpen)
                        cmdGetInfo_Click(sender, e);
                }
                catch (Exception ex)
                {
                    Globals.HandleException("cmdMonitor_Click", ex, false, false);
                }
            }
            else
            {
                this.CloseSerial();
            }
        }

        private void OpenSerial(int? baudRate = null)
        {
            this.CloseSerial();
            try
            {
                this.serialPort = new SerialPort();
                serialPort.PortName = cbComPort.Text;
                serialPort.BaudRate = baudRate ?? Int32.Parse(cbBaudrate.Text);
                serialPort.DataReceived += SerialPort_DataReceived;
                serialPort.ErrorReceived += SerialPort_ErrorReceived;
                serialPort.Open();
            }
            catch (Exception ex)
            {
                serialPort = null;
                Globals.HandleException("OpenSerial", ex, true, false);
            }
            UpdateConnectionTexts();
        }

        private void HandleSerialOutput(string szOutput)
        {
            txtDebug.AppendText(szOutput);
            ScrollDebugToEnd();

            try
            {
                if (szOutput.Contains("%info: {") || szDebugOutputHelper.Length > 0)
                {
                    szDebugOutputHelper += szOutput;

                    if (szOutput.IndexOf('}') > 0)
                    {
                        // Handle Info output
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
                                txtConnectedDeviceType.Text == "AmbiBack_Pc_Controller")
                            {
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
                szDebugOutputHelper = "";
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
                if (!sp.IsOpen) return;
                string indata = sp.ReadExisting();
                HandleSerialOutput(indata.Replace("\n", "\r\n") ?? string.Empty);
            }));
        }

        private void CloseSerial()
        {
            if (serialPort != null)
            {
                try { serialPort.Close(); } catch { }
            }
            serialPort = null;
            UpdateConnectionTexts();
        }

        private void cmdGetInfo_Click(object sender, EventArgs e)
        {
            if (IsSerialOpen)
                this.serialPort.Write("%INFO:");
        }

        /// <summary>
        /// Writes the string as raw UTF-8 bytes without a length prefix.
        /// BinaryWriter.Write(string) puts a 7-bit length byte in front of the string; the firmware
        /// parser copies every byte above 0x1F, so for strings of 32+ characters that byte became
        /// part of the stored value (e.g. the Wi-Fi key).
        /// </summary>
        private static void WriteRaw(BinaryWriter bw, string text)
        {
            bw.Write(Encoding.UTF8.GetBytes(text ?? string.Empty));
        }

        private void SendWifi(string ssid, string key)
        {
            if (!IsSerialOpen) return;
            // Payload helpers
            MemoryStream msPacket = new MemoryStream();
            BinaryWriter bwPacket = new BinaryWriter(msPacket);

            // Write Byte array: "%WIFI:" ssid 0x00 key 0x00
            WriteRaw(bwPacket, "%WIFI:");
            WriteRaw(bwPacket, ssid);
            bwPacket.Write((byte)0x00);
            WriteRaw(bwPacket, key);
            bwPacket.Write((byte)0x00);

            // Send byte area to serial
            byte[] bData = msPacket.ToArray();
            this.serialPort.Write(bData, 0, bData.Length);
        }

        private void cmdSetWifi_Click(object sender, EventArgs e)
        {
            SendWifi(txtWifiSSID.Text, txtWifiKey.Text);
        }

        private void cmdSendLedTestData_Click(object sender, EventArgs e)
        {
            if (IsSerialOpen)
            {
                // Payload helpers
                MemoryStream msPacket = new MemoryStream();
                BinaryWriter bwPacket = new BinaryWriter(msPacket);
                byte bLeds = (byte)0xFF; //  0x7F;
                // Write 4 Byte Detection Header
                WriteRaw(bwPacket, "%LEDDATA:");
                bwPacket.Write((byte)0x00);
                int iHeaderEnd = (int)msPacket.Position; // offset of the 4 byte package header

                // Write 4 Byte Package Header
                bwPacket.Write(bLeds); // Count of LEDs (Length)
                bwPacket.Write((byte)0x00); // StartIndex
                bwPacket.Write((byte)0x00); // If set to 0x01 we use CheckSum Check
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
                // Checksum is the last byte of the 4 byte package header
                int iCrcIdx = iHeaderEnd + Globals.TCP_HEADLEN - 1;
                bData[iCrcIdx] = Globals.crc_8(bData, bData.Length, iCrcIdx);

                // Send byte area to serial
                this.serialPort.Write(bData, 0, bData.Length);
            }
        }

        private void cbBaudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(cbBaudrate.Text, out int iBaudrate)) return;
            if (IsSerialOpen && serialPort.BaudRate != iBaudrate)
            {
                if (iBaudrate == 921600 || iBaudrate == 1500000)
                    this.serialPort.Write("%STARTLEDDATA:");
                else if (iBaudrate == 115200)
                    this.serialPort.Write("%ENDLEDDATA:");
                // reopen serial
                this.OpenSerial();
            }
        }

        private void cmdReboot_Click(object sender, EventArgs e)
        {
            if (IsSerialOpen)
                this.serialPort.Write("%REBOOT:");
        }

        private void cmdFactoryDefaults_Click(object sender, EventArgs e)
        {
            if (!IsSerialOpen) return;
            if (MessageBox.Show(this, L.T("confirm.factory.text"), L.T("confirm.factory.title"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;
            this.serialPort.Write("%FACTORYDEFAULT:");
        }

        private void SendSimpleCommand(string command)
        {
            if (!IsSerialOpen) return;
            MemoryStream msPacket = new MemoryStream();
            BinaryWriter bwPacket = new BinaryWriter(msPacket);
            WriteRaw(bwPacket, command);
            bwPacket.Write((byte)0x00);
            byte[] bData = msPacket.ToArray();
            this.serialPort.Write(bData, 0, bData.Length);
        }

        private void cmdSetRelay_Click(object sender, EventArgs e)
        {
            if (!IsSerialOpen) return;
            MemoryStream msPacket = new MemoryStream();
            BinaryWriter bwPacket = new BinaryWriter(msPacket);
            WriteRaw(bwPacket, "%SETRELAY:");
            WriteRaw(bwPacket, txtRelayID.Text);
            bwPacket.Write((byte)0x00);
            byte[] bData = msPacket.ToArray();
            this.serialPort.Write(bData, 0, bData.Length);
        }

        private void cmdModeOff_Click(object sender, EventArgs e) { SendSimpleCommand("%LEDMODE:0"); }
        private void cmdModeDemo_Click(object sender, EventArgs e) { SendSimpleCommand("%LEDMODE:5"); }
        private void cmdModeCam_Click(object sender, EventArgs e) { SendSimpleCommand("%LEDMODE:3"); }
        private void cmdResetWifi_Click(object sender, EventArgs e) { SendSimpleCommand("%RESETWIFI"); }

        // ================================================================== build environments

        private void cbBuilder_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.szBuilder = cbBuilder.Text;
            if (!_loading) PopulateBuildEnvCombo();
        }

        private void cbBuildEnv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_populatingEnvs || string.IsNullOrEmpty(cbBuildEnv.Text)) return;
            if (IsCliBuilder)
                Properties.Settings.Default.szCoreVersion = cbBuildEnv.Text;
            else
                Properties.Settings.Default.szBuildEnv = cbBuildEnv.Text;
        }

        private bool _populatingEnvs = false;

        /// <summary>List the installed ESP32 cores for the selected builder.</summary>
        private void PopulateBuildEnvCombo()
        {
            _populatingEnvs = true;
            try
            {
                cbBuildEnv.Items.Clear();
                if (IsCliBuilder)
                {
                    if (Directory.Exists(AppPaths.ArduinoCliRoot))
                        foreach (string v in Directory.GetDirectories(AppPaths.ArduinoCliRoot).Select(Path.GetFileName)
                                     .Where(AppPaths.IsCliCoreInstalled).OrderBy(AppPaths.VersionKey))
                            cbBuildEnv.Items.Add(v);
                    AppendDebugText("[Build ENV] arduino-cli: " + (AppPaths.ArduinoCliExe ?? "(not installed)") +
                                    ", cores: " + (cbBuildEnv.Items.Count > 0 ? string.Join(", ", cbBuildEnv.Items.Cast<string>()) : "(none)") + "\r\n", Color.Gray);
                    SelectEnv(Properties.Settings.Default.szCoreVersion);
                }
                else
                {
                    PopulateLegacyBuildEnvs();
                    SelectEnv(Properties.Settings.Default.szBuildEnv);
                }
            }
            finally
            {
                _populatingEnvs = false;
            }
        }

        private void SelectEnv(string saved)
        {
            if (!string.IsNullOrEmpty(saved) && cbBuildEnv.Items.Contains(saved))
                cbBuildEnv.SelectedItem = saved;
            else if (cbBuildEnv.Items.Count > 0)
                cbBuildEnv.SelectedIndex = cbBuildEnv.Items.Count - 1;
        }

        /// <summary>Detect arduino-builder environments from buildenv (and buildtools).</summary>
        private void PopulateLegacyBuildEnvs()
        {
            _buildEnvs.Clear();

            string root = AppRootPath;
            string buildenvDir = AppPaths.BuildEnvDir;
            string buildtoolsDir = AppPaths.BuildToolsDir;

            AppendDebugText("[Build ENV] Scanning: " + buildenvDir + "\r\n", Color.Gray);

            if (!Directory.Exists(buildenvDir))
            {
                AppendDebugText("[Build ENV] buildenv directory not found.\r\n", Color.Gray);
                return;
            }

            var subdirs = Directory.GetDirectories(buildenvDir)
                .Select(d => Path.GetFileName(d))
                .Where(n => !string.IsNullOrEmpty(n) && !n.StartsWith("."))
                .OrderBy(AppPaths.VersionKey)
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
        }

        /// <summary>Folder of the ESP32 core (platform.txt) for the selected builder and core, or null.</summary>
        private string GetSelectedCoreDir()
        {
            string env = cbBuildEnv.Text;
            if (string.IsNullOrEmpty(env)) return null;
            string dir = IsCliBuilder
                ? Path.Combine(AppPaths.CliPackagesDir(env), "hardware", "esp32", env)
                : Path.Combine(AppPaths.BuildEnvDir, env, "packages", "esp32", "hardware", "esp32", env);
            return Directory.Exists(dir) ? dir : null;
        }

        /// <summary>Tools folder (with esptool_py) for the selected builder and core, or null.</summary>
        private string GetSelectedToolsDir()
        {
            string env = cbBuildEnv.Text;
            if (string.IsNullOrEmpty(env)) return null;
            if (IsCliBuilder) return Path.Combine(AppPaths.CliPackagesDir(env), "tools");
            var benv = GetCurrentBuildEnv();
            return benv.HasValue && !string.IsNullOrEmpty(benv.Value.ToolsBasePath) ? Path.Combine(benv.Value.ToolsBasePath, "tools") : null;
        }

        // ================================================================== build

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
            if (!Directory.Exists(Path.Combine(AppPaths.SourceDir, ".git"))) return "";
            using (Process pGit = new Process())
            {
                pGit.StartInfo.FileName = "git";
                pGit.StartInfo.Arguments = "-C \"" + AppPaths.SourceDir + "\" " + szArgs;
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

        /// <summary>Preprocessor flags shared by both builders (passed as build.extra_flags).</summary>
        private string GetExtraFlags(CDeviceType selectedType, string resolvedBoard, string szBuildId)
        {
            return "-DESP32_DEVICE_TYPE=" + selectedType.PreprocessorName + " " +
                   "-DF_CPU=240000000L -DARDUINO=10813 " +
                   (selectedType.HasPSRam ? "-DBOARD_HAS_PSRAM " : "") +
                   "-DARDUINO_" + resolvedBoard + " " +
                   "-DARDUINO_ARCH_ESP32 " +
                   "-DARDUINO_BOARD=\\\"" + resolvedBoard + "\\\" " +
                   "-DESP32 " +
                   "-DAMBIBACK_BUILD_ID=\\\"" + szBuildId + "\\\" " +
                   (chkEsp32Debug.Checked ? "-DCORE_DEBUG_LEVEL=ESP_LOG_VERBOSE -DLOG_LOCAL_LEVEL=ESP_LOG_VERBOSE" : "-DCORE_DEBUG_LEVEL=0") + " " +
                   (chkAmbiBackExtraDiag.Checked ? "-DESP32_EXTRA_DIAG " : "") +
                   (chkAmbiBackVerboseDebug.Checked ? "-DESP32_VERBOSE_DEBUG " : "") +
                   (selectedType.UsePSRamFix ? "-mfix-esp32-psram-cache-issue" : "");
        }

        /// <summary>Check that the firmware source exists (offer to download it) and that the web UI headers exist.</summary>
        private async Task<bool> EnsureSourceAsync(bool ask)
        {
            string ino = Path.Combine(AppPaths.SourceDir, AppPaths.SourceFile);
            if (!File.Exists(ino))
            {
                if (ask && MessageBox.Show(this, L.F("dlg.noSource", AppPaths.SourceDir), L.T("dlg.noSource.title"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return false;
                if (!await DownloadSourceAsync()) return false;
            }
            if (!WebUiHeaderGenerator.HeadersExist(AppPaths.SourceDir))
                WebUiHeaderGenerator.EnsureHeaders(AppPaths.SourceDir, s => LogLine(s, Color.DarkCyan));
            RefreshSourceVersion();
            return File.Exists(Path.Combine(AppPaths.SourceDir, AppPaths.SourceFile));
        }

        /// <summary>Build with the selected builder. Returns true when the image was built and copied to the firmware folder.</summary>
        private async Task<bool> DoBuildAsync()
        {
            var selectedType = SelectedDevice;
            if (selectedType == null) return false;

            if (string.IsNullOrEmpty(cbBuildEnv.Text))
            {
                if (MessageBox.Show(this, L.F("dlg.noEnv", cbBuilder.Text), L.T("dlg.noEnv.title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    ShowSetupDialog();   // the build runs while busy, so not through the button handler
                return false;
            }
            if (!await EnsureSourceAsync(true)) return false;
            if (string.IsNullOrWhiteSpace(txtVersionCode.Text))
            {
                AppendDebugText("No AMBIBACK_VERSION found in " + Path.Combine(AppPaths.SourceDir, "globalincludes.h") + "\r\n", Color.Red);
                return false;
            }

            SetStatus(L.F("status.building", selectedType.DisplayLabel));
            bool ok = IsCliBuilder ? await DoBuildCliAsync(selectedType) : doBuildLegacy(selectedType);
            SetStatus(ok ? L.F("status.buildOk", selectedType.DeviceType, txtVersionCode.Text) : L.T("status.buildFail"));
            AppendDebugText(ok ? "\r\nBuild OK\r\n" : "\r\nBuild FAILED\r\n", ok ? Color.Green : Color.Red);
            if (ok) cbDevice_SelectedIndexChanged(this, EventArgs.Empty);
            return ok;
        }

        /// <summary>Build with arduino-cli. Each core version has its own data folder (arduino-cli\&lt;core&gt;).</summary>
        private async Task<bool> DoBuildCliAsync(CDeviceType selectedType)
        {
            string core = cbBuildEnv.Text;
            string cli = AppPaths.ArduinoCliExe;
            if (cli == null || !AppPaths.IsCliCoreInstalled(core))
            {
                AppendDebugText("arduino-cli or ESP32 core " + core + " is not installed. Use Tools > Set Up Arduino and ESP32.\r\n", Color.Red);
                return false;
            }
            ToolchainInstaller.WriteCliConfig(core);
            string cfg = "--config-file \"" + AppPaths.CliConfigFile(core) + "\" ";

            bool legacyCore = core.StartsWith("1.") || core.StartsWith("2.");
            var (resolvedModel, resolvedBoard) = CDeviceHelper.ResolveBoardForBuildEnv(selectedType.Esp32Model, selectedType.Esp32Board, legacyCore);
            if (resolvedModel != selectedType.Esp32Model || resolvedBoard != selectedType.Esp32Board)
                AppendDebugText("[Build] Board resolved for " + core + ": " + selectedType.Esp32Model + " -> " + resolvedModel + ", " + selectedType.Esp32Board + " -> " + resolvedBoard + "\r\n", Color.DarkCyan);

            string szBuildDir = Path.Combine(AppPaths.BuildDir, "_build", selectedType.DeviceType + "-cli-" + core);
            string szBuildId = GetBuildId();
            string sketchDir = CopySketchForCli(selectedType);

            // Only pass board options the board really has (arduino-cli stops on unknown options)
            string wanted = (selectedType.HasPSRam ? "PSRAM=enabled," : "") + "PartitionScheme=min_spiffs,UploadSpeed=" + selectedType.UploadSpeed + "," +
                            selectedType.Esp32FqbnExtra + "DebugLevel=none";
            string fqbnBase = "esp32:esp32:" + resolvedModel;
            string options = FilterBoardOptions(cli, cfg, fqbnBase, wanted);
            string fqbn = fqbnBase + (options.Length > 0 ? ":" + options : "");

            Directory.CreateDirectory(szBuildDir);
            string outBin = Path.Combine(szBuildDir, ImageFile);
            string outPart = Path.Combine(szBuildDir, PartitionsFile);
            if (File.Exists(outBin)) File.Delete(outBin);
            if (File.Exists(outPart)) File.Delete(outPart);

            string args = cfg + "compile --fqbn " + fqbn +
                          " --build-path \"" + szBuildDir + "\"" +
                          " --libraries \"" + Path.Combine(AppPaths.ExtraIncludeDir, "libraries") + "\"" +
                          " --build-property \"build.extra_flags=" + GetExtraFlags(selectedType, resolvedBoard, szBuildId) + "\"" +
                          " --warnings none --verbose \"" + sketchDir + "\"";

            AppendDebugText("Build Version " + txtVersionCode.Text + " for " + selectedType.DeviceType + " with arduino-cli, core " + core + "\r\n");
            SetProgress(-1);
            int rc;
            try
            {
                rc = await ProcessRunner.RunAsync(cli, args, sketchDir, line => LogLine(line, IsErrorLine(line) ? (Color?)Color.Red : null), CancellationToken.None);
            }
            finally
            {
                SetProgress(null);
            }
            if (rc != 0 || !File.Exists(outBin) || !File.Exists(outPart))
            {
                AppendDebugText("arduino-cli exit code " + rc + "\r\n", Color.Red);
                return false;
            }
            CopyBuildOutput(selectedType, szBuildDir, szBuildId, "arduino-cli " + core, "esp32:esp32@" + core);
            return true;
        }

        private static bool IsErrorLine(string line)
        {
            return line.IndexOf("error:", StringComparison.OrdinalIgnoreCase) >= 0 || line.StartsWith("Error", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// arduino-cli needs a sketch folder with the same name as the .ino file. The source is copied to
        /// _build\_sketch\&lt;device&gt;\&lt;sketch name&gt; (without .git, web and scripts).
        /// </summary>
        private string CopySketchForCli(CDeviceType selectedType)
        {
            string name = Path.GetFileNameWithoutExtension(AppPaths.SourceFile);
            string dest = Path.Combine(AppPaths.BuildDir, "_build", "_sketch", selectedType.DeviceType, name);
            if (Directory.Exists(dest)) Directory.Delete(dest, true);
            CopyDir(AppPaths.SourceDir, dest, new[] { ".git", ".claude", ".vscode", "web", "scripts", "node_modules", "build" });
            return dest;
        }

        private static void CopyDir(string src, string dest, string[] skipDirs)
        {
            Directory.CreateDirectory(dest);
            foreach (string f in Directory.GetFiles(src))
                File.Copy(f, Path.Combine(dest, Path.GetFileName(f)), true);
            foreach (string d in Directory.GetDirectories(src))
            {
                string n = Path.GetFileName(d);
                if (skipDirs.Contains(n, StringComparer.OrdinalIgnoreCase)) continue;
                CopyDir(d, Path.Combine(dest, n), new string[0]);
            }
        }

        /// <summary>Keep only the options (Key=Value) that the board offers, using "arduino-cli board details".</summary>
        private string FilterBoardOptions(string cli, string cfg, string fqbn, string wanted)
        {
            string json = ProcessRunner.Capture(cli, cfg + "board details -b " + fqbn + " --format json");
            var pairs = wanted.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(p => p.Split('='))
                              .Where(p => p.Length == 2)
                              .ToList();
            if (string.IsNullOrEmpty(json))
            {
                AppendDebugText("[Build] Board details not available, using options unchanged.\r\n", Color.Gray);
                return string.Join(",", pairs.Select(p => p[0] + "=" + p[1]));
            }
            var keep = new List<string>();
            JSONNode details = JSON.Parse(json);
            foreach (var p in pairs)
            {
                bool found = false;
                foreach (JSONNode opt in details["config_options"].Children)
                {
                    if (opt["option"].Value != p[0]) continue;
                    foreach (JSONNode val in opt["values"].Children)
                        if (val["value"].Value == p[1]) { found = true; break; }
                }
                if (found) keep.Add(p[0] + "=" + p[1]);
                else AppendDebugText("[Build] Board option " + p[0] + "=" + p[1] + " not offered by " + fqbn + ", skipped.\r\n", Color.Gray);
            }
            return string.Join(",", keep);
        }

        /// <summary>Copy the built files to firmware\&lt;device&gt;\&lt;version&gt; and write buildinfo.txt.</summary>
        /// <summary>
        /// runtime.tools paths for every tool installed under &lt;toolsBase&gt;\tools\&lt;name&gt;\&lt;version&gt;, so cores whose
        /// tool names or versions differ from the fixed list above (e.g. esp32-arduino-libs) still find their tools.
        /// </summary>
        private static string InstalledToolPrefs(string toolsBase)
        {
            var sb = new StringBuilder();
            string toolsDir = Path.Combine(toolsBase, "tools");
            if (!Directory.Exists(toolsDir)) return "";
            foreach (string toolDir in Directory.GetDirectories(toolsDir))
            {
                string name = Path.GetFileName(toolDir);
                string verDir = Directory.GetDirectories(toolDir).OrderBy(d => AppPaths.VersionKey(Path.GetFileName(d))).LastOrDefault();
                if (verDir == null) continue;
                string ver = Path.GetFileName(verDir);
                sb.Append("-prefs=\"runtime.tools." + name + ".path=" + verDir + "\" ");
                sb.Append("-prefs=\"runtime.tools." + name + "-" + ver + ".path=" + verDir + "\" ");
            }
            return sb.ToString();
        }

        private void CopyBuildOutput(CDeviceType selectedType, string szBuildDir, string szBuildId, string buildEnv, string toolchain)
        {
            string szOutDirDest = Path.Combine(AppPaths.FirmwareDir, selectedType.DeviceType, txtVersionCode.Text);
            Directory.CreateDirectory(szOutDirDest);

            foreach (string f in new[] { ImageFile, PartitionsFile, BootloaderFile })
            {
                string src = Path.Combine(szBuildDir, f);
                string dst = Path.Combine(szOutDirDest, f);
                if (File.Exists(dst))
                {
                    // Backup existing images
                    if (File.Exists(dst + ".bak")) File.Delete(dst + ".bak");
                    File.Move(dst, dst + ".bak");
                }
                if (File.Exists(src)) File.Copy(src, dst, true);
            }

            // Write build information next to the image (the firmware shows the same values in its web UI)
            File.WriteAllText(Path.Combine(szOutDirDest, "buildinfo.txt"),
                "version=" + txtVersionCode.Text + "\r\n" +
                "device=" + selectedType.DeviceType + "\r\n" +
                "buildid=" + szBuildId + "\r\n" +
                "buildenv=" + buildEnv + "\r\n" +
                "toolchain=" + toolchain + "\r\n" +
                "built=" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "\r\n");
        }

        /// <summary>Build with arduino-builder (Arduino IDE 1.8.x) and the environments in buildenv/buildtools.</summary>
        private bool doBuildLegacy(CDeviceType selectedType)
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
            string szBinDir = AppPaths.BinDir;
            if (!File.Exists(Path.Combine(szBinDir, "arduino-builder.exe")))
            {
                AppendDebugText("arduino-builder.exe not found in " + szBinDir + ". Install the Arduino IDE 1.8.x with Tools > Set Up Arduino and ESP32.\r\n", Color.Red);
                return false;
            }

            // Prepare commands
            var (resolvedModel, resolvedBoard) = CDeviceHelper.ResolveBoardForBuildEnv(selectedType.Esp32Model, selectedType.Esp32Board, benv.UseLegacyToolchain);
            if (!benv.UseLegacyToolchain && (resolvedModel != selectedType.Esp32Model || resolvedBoard != selectedType.Esp32Board))
                AppendDebugText("[Build] Board resolved for 3.x: " + selectedType.Esp32Model + " → " + resolvedModel + ", " + selectedType.Esp32Board + " → " + resolvedBoard + "\r\n", Color.DarkCyan);

            string szBuildDir = Path.Combine(AppPaths.BuildDir, "_build", selectedType.DeviceType);
            string szCacheDir = Path.Combine(AppPaths.BuildDir, "_cache", selectedType.DeviceType);
            string szOutFileBin = Path.Combine(szBuildDir, ImageFile);
            string szOutFilePartition = Path.Combine(szBuildDir, PartitionsFile);
            string szBuildId = GetBuildId();
            string szSourceDir = AppPaths.SourceDir;

            string szCompilerArgs = "-logger=machine " +
                                    "-hardware \"" + szBinDir + "\\hardware\" " +
                                    "-hardware \"" + AppRootPath + "\\buildenv\\" + benv.Name + "\\packages\" " +
                                    "-tools \"" + szBinDir + "\\tools-builder\" " +
                                    "-tools \"" + szBinDir + "\\hardware\\tools\\avr\" " +
                                    "-tools \"" + AppRootPath + "\\buildenv\\" + benv.Name + "\\packages\" " +
                                    "-built-in-libraries \"" + szBinDir + "\\libraries\" " +
                                    "-libraries \"" + AppPaths.ExtraIncludeDir + "\\libraries\" " +
                                    "-fqbn=esp32:esp32:" + resolvedModel + ":" + (selectedType.HasPSRam ? "PSRAM=enabled," : "") + "PartitionScheme=min_spiffs," +
                                        "UploadSpeed=" + selectedType.UploadSpeed + "," +
                                        selectedType.Esp32FqbnExtra +
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
                                    InstalledToolPrefs(benv.ToolsBasePath) +
                                    "-prefs=\"build.extra_flags=" + GetExtraFlags(selectedType, resolvedBoard, szBuildId) + "\" " +
                                    "-verbose " +
                                    "\"" + Path.Combine(szSourceDir, AppPaths.SourceFile) + "\"";
            string szExtraPath = ";" + Path.Combine(szBinDir, "java", "bin");

            Directory.CreateDirectory(szBuildDir);
            Directory.CreateDirectory(szCacheDir);

            // Create child process.
            txtDebug.AppendText("Build Version " + txtVersionCode.Text + " for " + selectedType.DeviceType + " BEGIN\r\n");
            ScrollDebugToEnd();
            SetProgress(0);
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
                pBuild.StartInfo.FileName = Path.Combine(szBinDir, "arduino-builder.exe");
                pBuild.StartInfo.Arguments = "-dump-prefs " + szCompilerArgs;
                pBuild.ErrorDataReceived += ErrorOutputHandler;
                pBuild.OutputDataReceived += StandardOutputHandler;
                pBuild.Start();

                txtDebug.AppendText("DEBUG: Command: " + pBuild.StartInfo.FileName + "\r\n");
                txtDebug.AppendText("DEBUG: Arguments: " + pBuild.StartInfo.Arguments + "\r\n");
                ScrollDebugToEnd();

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

                pBuild = new Process();
                pBuild.EnableRaisingEvents = true;
                pBuild.StartInfo.CreateNoWindow = true;
                pBuild.StartInfo.UseShellExecute = false;
                pBuild.StartInfo.RedirectStandardOutput = true;
                pBuild.StartInfo.RedirectStandardError = true;
                pBuild.StartInfo.WorkingDirectory = szBinDir;
                pBuild.StartInfo.EnvironmentVariables["PATH"] += szExtraPath;

                pBuild.StartInfo.FileName = Path.Combine(szBinDir, "arduino-builder.exe");
                pBuild.StartInfo.Arguments = "-compile " + szCompilerArgs;
                pBuild.ErrorDataReceived += ErrorOutputHandler;
                pBuild.OutputDataReceived += StandardOutputHandler;

                // Remove previous builds
                if (File.Exists(szOutFileBin))
                    File.Delete(szOutFileBin);
                if (File.Exists(szOutFilePartition))
                    File.Delete(szOutFilePartition);

                pBuild.Start();

                txtDebug.AppendText("DEBUG: Command: " + pBuild.StartInfo.FileName + "\r\n");
                txtDebug.AppendText("DEBUG: Arguments: " + pBuild.StartInfo.Arguments + "\r\n");
                ScrollDebugToEnd();

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

                // Build was successful
                if (File.Exists(szOutFileBin) && File.Exists(szOutFilePartition))
                {
                    CopyBuildOutput(selectedType, szBuildDir, szBuildId, benv.Name,
                        benv.UseLegacyToolchain ? "xtensa-esp32-elf-gcc " + benv.TensaTools : "esp-x32 " + benv.EspX32);
                    bSuccess = true;
                }
            }
            catch (Exception ex)
            {
                string szErrorText = "Command: " + pBuild.StartInfo.FileName +
                                        "\r\nArguments: " + pBuild.StartInfo.Arguments +
                                        "\r\n\r\nException occured in doBuild" +
                                        "\r\nException Type: " + ex.GetType().ToString() +
                                        "\r\nDetails: \r\n\r\n" + ex.ToString();
                AppendDebugText(szErrorText, Color.Red);
            }
            finally
            {
                SetProgress(null);
            }

            return bSuccess;
        }

        void StandardOutputHandler(object sender, DataReceivedEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                try
                {
                    if (e.Data != null && e.Data.Length > 0)
                    {
                        // Parse Progress lines from arduino-builder - format: ===info ||| Progress {0} ||| [93.75]
                        if (TryParseBuilderProgress(e.Data)) return;

                        string lineToAppend = null;
                        if (e.Data.Contains("|||"))
                        {
                            string[] aSeparators = new string[] { "|||" };
                            string[] dataSplitted = e.Data.Split(aSeparators, StringSplitOptions.RemoveEmptyEntries);

                            if (dataSplitted.Length > 2)
                            {
                                string[] aDataSeparators = new string[] { " " };
                                string[] dataParams = (dataSplitted[2].Length > 3 ? dataSplitted[2].Substring(2, dataSplitted[2].Length - 3).Split(aDataSeparators, StringSplitOptions.RemoveEmptyEntries) : null);
                                if (dataParams != null)
                                {
                                    string Param(int i) { return dataParams.Length > i ? Uri.UnescapeDataString(dataParams[i]) : string.Empty; }
                                    if (dataSplitted[1].Contains("{3}"))
                                        lineToAppend = "Output: " + String.Format(dataSplitted[1], Param(0), Param(1), Param(2), string.Empty);
                                    else if (dataSplitted[1].Contains("{2}"))
                                        lineToAppend = "Output: " + String.Format(dataSplitted[1], Param(0), Param(1), Param(2));
                                    else if (dataSplitted[1].Contains("{1}"))
                                        lineToAppend = "Output: " + String.Format(dataSplitted[1], Param(0), Param(1));
                                    else
                                        lineToAppend = "Output: " + String.Format(dataSplitted[1], Param(0));
                                }
                                else
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
                    if (TryParseBuilderProgress(e.Data)) return;
                    AppendDebugText("ERROR: " + (e.Data ?? string.Empty).TrimEnd('\r', '\n') + "\r\n", Color.Red);
                }
            }));
        }

        /// <summary>Progress lines from arduino-builder: ===info ||| Progress {0} ||| [93.75]</summary>
        private bool TryParseBuilderProgress(string line)
        {
            if (line.IndexOf("Progress", StringComparison.OrdinalIgnoreCase) < 0) return false;
            var m = Regex.Match(line, @"\[\s*([\d.,]+)\s*\]");
            if (!m.Success) return false;
            if (!double.TryParse(m.Groups[1].Value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double pct)) return false;
            SetProgress((int)Math.Min(100, Math.Max(0, pct)));
            return true;
        }

        // ================================================================== flash

        private string FindFirstFile(string dir, string fileName)
        {
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return null;
            try
            {
                return Directory.GetFiles(dir, fileName, SearchOption.AllDirectories).OrderByDescending(f => AppPaths.VersionKey(f)).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>esptool.exe: data root, then the selected core, then any installed core.</summary>
        private string FindEsptool()
        {
            string root = Path.Combine(AppRootPath, "esptool.exe");
            if (File.Exists(root)) return root;
            string tools = GetSelectedToolsDir();
            string found = FindFirstFile(tools == null ? null : Path.Combine(tools, "esptool_py"), "esptool.exe");
            if (found != null) return found;
            if (Directory.Exists(AppPaths.ArduinoCliRoot))
                foreach (string core in Directory.GetDirectories(AppPaths.ArduinoCliRoot).Select(Path.GetFileName).OrderByDescending(AppPaths.VersionKey))
                {
                    found = FindFirstFile(Path.Combine(AppPaths.CliPackagesDir(core), "tools", "esptool_py"), "esptool.exe");
                    if (found != null) return found;
                }
            if (Directory.Exists(AppPaths.BuildToolsDir))
                foreach (string env in Directory.GetDirectories(AppPaths.BuildToolsDir).OrderByDescending(d => AppPaths.VersionKey(Path.GetFileName(d))))
                {
                    found = FindFirstFile(Path.Combine(env, "tools", "esptool_py"), "esptool.exe");
                    if (found != null) return found;
                }
            return null;
        }

        /// <summary>A file of the selected core, else the first match of any installed core.</summary>
        private string FindCoreFile(string relativePath)
        {
            string coreDir = GetSelectedCoreDir();
            if (coreDir != null && File.Exists(Path.Combine(coreDir, relativePath))) return Path.Combine(coreDir, relativePath);
            var roots = new List<string>();
            if (Directory.Exists(AppPaths.ArduinoCliRoot))
                foreach (string core in Directory.GetDirectories(AppPaths.ArduinoCliRoot).Select(Path.GetFileName))
                    roots.Add(Path.Combine(AppPaths.CliPackagesDir(core), "hardware", "esp32", core));
            if (Directory.Exists(AppPaths.BuildEnvDir))
                foreach (string env in Directory.GetDirectories(AppPaths.BuildEnvDir).Select(Path.GetFileName))
                    roots.Add(Path.Combine(AppPaths.BuildEnvDir, env, "packages", "esp32", "hardware", "esp32", env));
            return roots.Select(r => Path.Combine(r, relativePath)).FirstOrDefault(File.Exists);
        }

        /// <summary>Flash the selected image. Returns true when esptool reports success.</summary>
        private async Task<bool> DoFlashAsync()
        {
            this.CloseSerial();
            var selectedType = SelectedDevice;
            if (selectedType == null) return false;
            string version = cbFirmwareVersion.Text;
            string imageDir = Path.Combine(AppPaths.FirmwareDir, selectedType.DeviceType, version);
            string image = Path.Combine(imageDir, ImageFile);
            string partitions = Path.Combine(imageDir, PartitionsFile);
            if (string.IsNullOrEmpty(version) || !File.Exists(image) || !File.Exists(partitions))
            {
                MessageBox.Show(this, L.F("dlg.noImage", selectedType.DisplayLabel, version), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            string esptool = FindEsptool();
            if (esptool == null)
            {
                MessageBox.Show(this, L.T("dlg.noEsptool"), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // The bootloader built with the image fits the core; older images use the global one
            string bootloader = File.Exists(Path.Combine(imageDir, BootloaderFile)) ? Path.Combine(imageDir, BootloaderFile) : null;
            if (bootloader == null && File.Exists(Path.Combine(AppPaths.FirmwareDir, "bootloader_qio_80m.bin")))
                bootloader = Path.Combine(AppPaths.FirmwareDir, "bootloader_qio_80m.bin");
            if (bootloader == null) bootloader = FindCoreFile(Path.Combine("tools", "sdk", "bin", "bootloader_qio_80m.bin"));
            string bootApp0 = File.Exists(Path.Combine(AppPaths.FirmwareDir, "boot_app0.bin"))
                ? Path.Combine(AppPaths.FirmwareDir, "boot_app0.bin")
                : FindCoreFile(Path.Combine("tools", "partitions", "boot_app0.bin"));
            if (bootloader == null || bootApp0 == null)
            {
                AppendDebugText("Bootloader or boot_app0.bin not found (firmware folder or ESP32 core).\r\n", Color.Red);
                return false;
            }

            // Show the build information of the selected image, if the build wrote one
            string szBuildInfoFile = Path.Combine(imageDir, "buildinfo.txt");
            if (File.Exists(szBuildInfoFile))
                AppendDebugText("Build info:\r\n" + File.ReadAllText(szBuildInfoFile) + "\r\n", Color.DarkCyan);

            string args = "--chip esp32 --port " + cbComPort.Text + " --baud " + selectedType.UploadSpeed +
                          " --before default_reset --after hard_reset write_flash -z --flash_mode dio --flash_freq 80m --flash_size detect " +
                          "0xe000 \"" + bootApp0 + "\" " +
                          "0x1000 \"" + bootloader + "\" " +
                          "0x10000 \"" + image + "\" " +
                          "0x8000 \"" + partitions + "\"";

            SetStatus(L.F("status.flashing", selectedType.DeviceType, version));
            SetProgress(0);
            int rc;
            try
            {
                rc = await ProcessRunner.RunAsync(esptool, args, Path.GetDirectoryName(esptool), line =>
                {
                    var m = Regex.Match(line, @"\((\d+)\s*%\)");
                    if (m.Success)
                        BeginInvoke(new MethodInvoker(() => SetProgress(int.Parse(m.Groups[1].Value))));
                    LogLine(line, line.StartsWith("A fatal error", StringComparison.OrdinalIgnoreCase) ? (Color?)Color.Red : null);
                }, CancellationToken.None);
            }
            finally
            {
                SetProgress(null);
            }
            bool ok = rc == 0;
            AppendDebugText(ok ? "\r\nFirmware flash FINISHED\r\n" : "\r\nFirmware flash FAILED (exit code " + rc + ")\r\n", ok ? Color.Green : Color.Red);
            SetStatus(ok ? L.T("status.flashOk") : L.T("status.flashFail"));
            return ok;
        }

        private async void cmdFlash_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            txtDebug.Clear();
            SetBusy(true);
            try
            {
                if (await DoFlashAsync())
                    this.cmdMonitor_Click(sender, e);   // Open the serial connection next
            }
            catch (Exception ex)
            {
                AppendDebugText(ex.ToString() + "\r\n", Color.Red);
            }
            finally
            {
                SetBusy(false);
            }
        }

        // ================================================================== build / upload buttons

        private bool doUpload()
        {
            bool bSuccess = false;
            try
            {
                CDeviceType selectedType = SelectedDevice;
                string szVersion = txtVersionCode.Text.Trim();
                if (!Regex.IsMatch(szVersion, @"^\d+\.\d+\.\d+$"))
                {
                    AppendDebugText("Upload: invalid version '" + szVersion + "'\r\n", Color.Red);
                    return false;
                }

                string szSrcFileBin = Path.Combine(AppPaths.FirmwareDir, selectedType.DeviceType, szVersion, ImageFile);
                string szScript = Path.Combine(Application.StartupPath, "scripts", "upload-firmware.ps1");
                string szEnvFile = Path.Combine(AppRootPath, ".env");

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

                pUpload.ErrorDataReceived += ErrorOutputHandler;
                pUpload.OutputDataReceived += StandardOutputHandler;
                pUpload.Start();

                SetStatus(L.T("status.uploading"));
                txtDebug.AppendText("Starting upload: " + selectedType.DeviceType + " " + szVersion + "\r\n");
                ScrollDebugToEnd();

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
                SetStatus(bSuccess ? "Upload OK" : "Upload FAILED");
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

        /// <summary>Run an action with the busy state set and the output cleared.</summary>
        private async Task RunBusyAsync(Func<Task> action)
        {
            if (_busy) return;
            txtDebug.Clear();
            this.CloseSerial();
            SetBusy(true);
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                AppendDebugText(ex.ToString() + "\r\n", Color.Red);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void cmdBuild_Click(object sender, EventArgs e)
        {
            await RunBusyAsync(async () => await DoBuildAsync());
        }

        private async void cmdBuildAndFlash_Click(object sender, EventArgs e)
        {
            await RunBusyAsync(async () =>
            {
                if (await DoBuildAsync())
                {
                    cbFirmwareVersion.Text = txtVersionCode.Text;
                    if (await DoFlashAsync())
                        cmdMonitor_Click(sender, e);
                }
            });
        }

        private async void cmdUpload_Click(object sender, EventArgs e)
        {
            await RunBusyAsync(() => { doUpload(); return Task.CompletedTask; });
        }

        private async void cmdBuildAndUpload_Click(object sender, EventArgs e)
        {
            await RunBusyAsync(async () =>
            {
                if (await DoBuildAsync())
                    doUpload();
            });
        }

        private void cmdCleanBuild_Click(object sender, EventArgs e)
        {
            var selectedType = SelectedDevice;
            if (selectedType == null || _busy) return;
            var dirs = new[]
            {
                Path.Combine(AppPaths.BuildDir, "_build", selectedType.DeviceType),
                Path.Combine(AppPaths.BuildDir, "_cache", selectedType.DeviceType),
                Path.Combine(AppPaths.BuildDir, "_build", selectedType.DeviceType + "-cli-" + cbBuildEnv.Text),
            };
            foreach (string d in dirs)
            {
                try
                {
                    if (Directory.Exists(d))
                    {
                        Directory.Delete(d, true);
                        AppendDebugText("Deleted " + d + "\r\n", Color.Gray);
                    }
                }
                catch (Exception ex)
                {
                    AppendDebugText("Exception occured in cmdCleanBuild\r\nException Type: " + ex.GetType() + "\r\nDetails: \r\n\r\n" + ex + "\r\n", Color.Red);
                }
            }
        }

        // ================================================================== Easy mode

        private void chkEasyWifi_CheckedChanged(object sender, EventArgs e)
        {
            bool on = chkEasyWifi.Checked;
            lblEasySsid.Enabled = txtEasySsid.Enabled = lblEasyKey.Enabled = txtEasyKey.Enabled = on;
            Properties.Settings.Default.bEasyWifi = on;
        }

        private async void cmdEasyFlash_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            var device = SelectedDevice;
            if (device == null || lvDevices.SelectedItems.Count == 0) { SetEasyStatus(L.T("easy.errNoDevice"), Theme.Destructive); return; }
            if (!(cbEasyPort.SelectedItem is string port)) { SetEasyStatus(L.T("easy.errNoPort"), Theme.Destructive); return; }
            if (chkEasyWifi.Checked && string.IsNullOrWhiteSpace(txtEasySsid.Text)) { SetEasyStatus(L.T("easy.errNoSsid"), Theme.Destructive); return; }
            var option = cbEasyFirmware.SelectedItem as EasyFirmwareOption;

            txtDebug.Clear();
            CloseSerial();
            cbComPort.Text = port;
            SetBusy(true);
            string version = option?.Version;
            string prevBuilder = cbBuilder.Text;
            try
            {
                if (version == null)
                {
                    // Build the newest firmware with arduino-cli
                    SetEasyStatus(L.T("easy.prepare"));
                    SetProgress(-1);
                    if (!await EnsureCliToolchainAsync()) return;

                    // Update the downloaded source; a git checkout or a custom source folder is used as it is
                    string ino = Path.Combine(AppPaths.SourceDir, AppPaths.SourceFile);
                    bool customSource = !string.IsNullOrWhiteSpace(Properties.Settings.Default.szSourceDir);
                    bool gitSource = Directory.Exists(Path.Combine(AppPaths.SourceDir, ".git"));
                    if (!File.Exists(ino) || (!customSource && !gitSource))
                    {
                        SetEasyStatus(L.T("easy.source"));
                        if (!await DownloadSourceAsync())
                        {
                            if (!File.Exists(ino)) { SetEasyStatus(L.T("easy.errSource"), Theme.Destructive); return; }
                            AppendDebugText("Download failed, building the existing source in " + AppPaths.SourceDir + "\r\n", Color.DarkCyan);
                        }
                    }

                    cbBuilder.Text = BuilderCli;
                    PopulateBuildEnvCombo();
                    SelectEnv(Properties.Settings.Default.szCoreVersion);

                    SetEasyStatus(L.T("easy.build"));
                    if (!await DoBuildAsync()) { SetEasyStatus(L.T("easy.errBuild"), Theme.Destructive); return; }
                    version = txtVersionCode.Text;
                }

                cbFirmwareVersion.Text = version;
                SetEasyStatus(L.T("easy.flashing"));
                cbComPort.Text = port;   // the port chosen at the start, whatever was plugged in meanwhile
                if (!await DoFlashAsync()) { SetEasyStatus(L.T("easy.errFlash"), Theme.Destructive); return; }

                // Let the device start, then send Wi-Fi settings and ask for its info
                cbComPort.Text = port;
                OpenSerial(115200);
                if (IsSerialOpen)
                {
                    await Task.Delay(4000);
                    if (chkEasyWifi.Checked)
                    {
                        SetEasyStatus(L.T("easy.wifiSend"));
                        SendWifi(txtEasySsid.Text, txtEasyKey.Text);
                        await Task.Delay(1500);
                    }
                    cmdGetInfo_Click(this, EventArgs.Empty);
                    await Task.Delay(1500);
                }
                string done = chkEasyWifi.Checked ? L.F("easy.doneWifi", version) : L.F("easy.done", version);
                if (!string.IsNullOrWhiteSpace(txtConnectedDeviceIP.Text) && txtConnectedDeviceIP.Text != "0.0.0.0")
                    done += " IP: " + txtConnectedDeviceIP.Text;
                SetEasyStatus(done, Theme.Success);
            }
            catch (Exception ex)
            {
                AppendDebugText(ex.ToString() + "\r\n", Color.Red);
                SetEasyStatus(L.F("easy.errSetup", ex.Message), Theme.Destructive);
            }
            finally
            {
                // Easy mode builds with arduino-cli but keeps the builder chosen in Advanced mode
                if (cbBuilder.Text != prevBuilder) cbBuilder.Text = prevBuilder;
                SetProgress(null);
                SetBusy(false);
                UpdateEasyCard();
            }
        }

        /// <summary>Install arduino-cli, the ESP32 core and the libraries when they are missing.</summary>
        private async Task<bool> EnsureCliToolchainAsync()
        {
            var inst = CreateInstaller();
            try
            {
                if (AppPaths.ArduinoCliExe == null)
                {
                    string ver;
                    try { ver = (await ToolchainInstaller.GetCliVersionsAsync()).FirstOrDefault(); }
                    catch { ver = null; }
                    ver = ver ?? ToolchainInstaller.FallbackCliVersions[0];
                    await inst.InstallArduinoCliAsync(ver);
                    Properties.Settings.Default.szCliVersion = ver;
                }
                string core = Properties.Settings.Default.szCoreVersion;
                if (string.IsNullOrWhiteSpace(core)) core = Properties.Settings.Default.szCoreVersion = "3.3.6";
                if (!AppPaths.IsCliCoreInstalled(core))
                    await inst.InstallCoreForCliAsync(AppPaths.ArduinoCliExe, core);
                if (string.IsNullOrWhiteSpace(Properties.Settings.Default.szExtraIncludeDir) && !ToolchainInstaller.AreLibrariesInstalled(AppPaths.DefaultSketchbook))
                    await inst.InstallLibrariesAsync(AppPaths.DefaultSketchbook);
                return true;
            }
            catch (Exception ex)
            {
                AppendDebugText(ex.ToString() + "\r\n", Color.Red);
                SetEasyStatus(L.F("easy.errSetup", ex.Message), Theme.Destructive);
                return false;
            }
        }

        private ToolchainInstaller CreateInstaller()
        {
            return new ToolchainInstaller(
                s => AppendDebugText(s + "\r\n", Color.DarkCyan),
                (pct, s) => { SetProgress(pct); SetStatus(s); },
                CancellationToken.None);
        }

        /// <summary>
        /// Download the firmware source from GitHub into the default source folder and use it from now on.
        /// A custom source folder is never overwritten.
        /// </summary>
        private async Task<bool> DownloadSourceAsync()
        {
            try
            {
                SetProgress(-1);
                await CreateInstaller().DownloadFirmwareSourceAsync(Properties.Settings.Default.szFirmwareRef, AppPaths.DefaultSourceDir);
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.szSourceDir))
                {
                    AppendDebugText("Source folder setting changed from " + Properties.Settings.Default.szSourceDir + " to the downloaded copy.\r\n", Color.DarkCyan);
                    Properties.Settings.Default.szSourceDir = "";
                }
                RefreshSourceVersion();
                return true;
            }
            catch (Exception ex)
            {
                AppendDebugText(ex.ToString() + "\r\n", Color.Red);
                return false;
            }
            finally
            {
                SetProgress(null);
            }
        }

        // ================================================================== menus and dialogs

        private void menuQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmdSettings_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            using (var frm = new frmSettings())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    RefreshAfterPathChange();
            }
        }

        private void cmdSetupTools_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            ShowSetupDialog();
        }

        private void ShowSetupDialog()
        {
            CloseSerial();
            using (var frm = new frmSetup())
                frm.ShowDialog(this);
            RefreshAfterPathChange();
        }

        private void RefreshAfterPathChange()
        {
            cbBuilder.Text = Properties.Settings.Default.szBuilder == BuilderLegacy ? BuilderLegacy : BuilderCli;
            PopulateBuildEnvCombo();
            cbDevice_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private async void menuDownloadSource_Click(object sender, EventArgs e)
        {
            await RunBusyAsync(async () =>
            {
                SetStatus(L.T("easy.source"));
                if (await DownloadSourceAsync())
                    SetStatus(L.T("status.ready"));
            });
        }

        private void menuOpenDataFolder_Click(object sender, EventArgs e) { AppPaths.OpenFolder(AppPaths.Root); }
        private void menuOpenFirmwareFolder_Click(object sender, EventArgs e) { AppPaths.OpenFolder(AppPaths.FirmwareDir); }

        private void menuLangEnglish_Click(object sender, EventArgs e) { ChangeLanguage("en"); }
        private void menuLangGerman_Click(object sender, EventArgs e) { ChangeLanguage("de"); }

        private void ChangeLanguage(string lang)
        {
            Properties.Settings.Default.szLanguage = lang;
            L.Set(lang);
            ApplyLanguage();
            if (!_busy) SetStatus(L.T("status.ready"));
        }

        private static void OpenUrl(string url)
        {
            try { Process.Start(url); } catch (Exception ex) { Globals.HandleException("OpenUrl", ex, true, false); }
        }

        private void menuProjectPage_Click(object sender, EventArgs e) { OpenUrl("https://github.com/ambiback/ambiback-flasher"); }
        private void menuFirmwarePage_Click(object sender, EventArgs e) { OpenUrl("https://github.com/" + ToolchainInstaller.FirmwareRepo); }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, L.F("dlg.about", Application.ProductVersion, AppPaths.Root), L.T("menu.about"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Menu items act like their buttons and follow their enabled state
        private void menuDevice_DropDownOpening(object sender, EventArgs e)
        {
            menuConnect.Enabled = cmdMonitor.Enabled;
            menuGetInfo.Enabled = cmdGetInfo.Enabled;
            menuReboot.Enabled = cmdReboot.Enabled;
            menuSetWifi.Enabled = cmdSetWifi.Enabled;
            menuResetWifi.Enabled = cmdResetWifi.Enabled;
            menuFactoryDefaults.Enabled = cmdFactoryDefaults.Enabled;
        }

        private void menuFirmware_DropDownOpening(object sender, EventArgs e)
        {
            menuBuild.Enabled = cmdBuild.Enabled;
            menuBuildAndFlash.Enabled = cmdBuildAndFlash.Enabled;
            menuFlash.Enabled = cmdFlash.Enabled;
            menuCleanBuild.Enabled = cmdCleanBuild.Enabled;
            menuUpload.Enabled = cmdUpload.Enabled;
            menuBuildAndUpload.Enabled = cmdBuildAndUpload.Enabled;
        }

        private void menuConnect_Click(object sender, EventArgs e) { if (cmdMonitor.Enabled) cmdMonitor_Click(sender, e); }
        private void menuGetInfo_Click(object sender, EventArgs e) { cmdGetInfo_Click(sender, e); }
        private void menuReboot_Click(object sender, EventArgs e) { cmdReboot_Click(sender, e); }
        private void menuSetWifi_Click(object sender, EventArgs e) { cmdSetWifi_Click(sender, e); }
        private void menuResetWifi_Click(object sender, EventArgs e) { cmdResetWifi_Click(sender, e); }
        private void menuFactoryDefaults_Click(object sender, EventArgs e) { cmdFactoryDefaults_Click(sender, e); }
        private void menuBuild_Click(object sender, EventArgs e) { if (cmdBuild.Enabled) cmdBuild_Click(sender, e); }
        private void menuBuildAndFlash_Click(object sender, EventArgs e) { if (cmdBuildAndFlash.Enabled) cmdBuildAndFlash_Click(sender, e); }
        private void menuFlash_Click(object sender, EventArgs e) { if (cmdFlash.Enabled) cmdFlash_Click(sender, e); }
        private void menuCleanBuild_Click(object sender, EventArgs e) { cmdCleanBuild_Click(sender, e); }
        private void menuUpload_Click(object sender, EventArgs e) { if (cmdUpload.Enabled) cmdUpload_Click(sender, e); }
        private void menuBuildAndUpload_Click(object sender, EventArgs e) { if (cmdBuildAndUpload.Enabled) cmdBuildAndUpload_Click(sender, e); }
    }
}
