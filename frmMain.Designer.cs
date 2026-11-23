namespace AmbiBack_USB_Flasher
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblComPort = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.cmdMonitor = new System.Windows.Forms.Button();
            this.cmdFlash = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.cbComPort = new System.Windows.Forms.ComboBox();
            this.cbBaudrate = new System.Windows.Forms.ComboBox();
            this.cmdGetInfo = new System.Windows.Forms.Button();
            this.cmdReboot = new System.Windows.Forms.Button();
            this.cmdSendLedTestData = new System.Windows.Forms.Button();
            this.gbBuild = new System.Windows.Forms.GroupBox();
            this.cbDevice = new System.Windows.Forms.ComboBox();
            this.cbFirmwareVersion = new System.Windows.Forms.ComboBox();
            this.txtVersionCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbBuildEnv = new System.Windows.Forms.ComboBox();
            this.cmdSettings = new System.Windows.Forms.Button();
            this.cmdBuild = new System.Windows.Forms.Button();
            this.cmdCleanBuild = new System.Windows.Forms.Button();
            this.cmdBuildAndFlash = new System.Windows.Forms.Button();
            this.cmdBuildAndUpload = new System.Windows.Forms.Button();
            this.cmdUpload = new System.Windows.Forms.Button();
            this.chkEsp32Debug = new System.Windows.Forms.CheckBox();
            this.chkAmbiBackVerboseDebug = new System.Windows.Forms.CheckBox();
            this.chkAmbiBackExtraDiag = new System.Windows.Forms.CheckBox();
            this.gbDeviceInfo = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtConnectedDeviceVersion = new System.Windows.Forms.TextBox();
            this.txtConnectedDeviceType = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConnectedDeviceID = new System.Windows.Forms.TextBox();
            this.txtConnectedDeviceMAC = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtConnectedDeviceName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtConnectedDeviceIP = new System.Windows.Forms.TextBox();
            this.gbDeviceControl = new System.Windows.Forms.GroupBox();
            this.cmdModeOff = new System.Windows.Forms.Button();
            this.cmdModeDemo = new System.Windows.Forms.Button();
            this.cmdModeCam = new System.Windows.Forms.Button();
            this.cmdSetRelay = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRelayID = new System.Windows.Forms.TextBox();
            this.cmdFactoryDefaults = new System.Windows.Forms.Button();
            this.gbWifi = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWifiSSID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtWifiKey = new System.Windows.Forms.TextBox();
            this.cmdResetWifi = new System.Windows.Forms.Button();
            this.cmdSetWifi = new System.Windows.Forms.Button();
            this.gbDebug = new System.Windows.Forms.GroupBox();
            this.txtDebug = new System.Windows.Forms.RichTextBox();
            this.timerComPort = new System.Windows.Forms.Timer(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBarBuild = new System.Windows.Forms.ProgressBar();
            this.gbConnection.SuspendLayout();
            this.gbBuild.SuspendLayout();
            this.gbDeviceInfo.SuspendLayout();
            this.gbDeviceControl.SuspendLayout();
            this.gbWifi.SuspendLayout();
            this.gbDebug.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblComPort
            // 
            this.lblComPort.AutoSize = true;
            this.lblComPort.Location = new System.Drawing.Point(8, 20);
            this.lblComPort.Name = "lblComPort";
            this.lblComPort.Size = new System.Drawing.Size(50, 13);
            this.lblComPort.TabIndex = 0;
            this.lblComPort.Text = "Com Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Device";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(8, 48);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Version";
            // 
            // cmdMonitor
            // 
            this.cmdMonitor.Location = new System.Drawing.Point(68, 45);
            this.cmdMonitor.Name = "cmdMonitor";
            this.cmdMonitor.Size = new System.Drawing.Size(90, 23);
            this.cmdMonitor.TabIndex = 6;
            this.cmdMonitor.Text = "Monitor";
            this.cmdMonitor.UseVisualStyleBackColor = true;
            this.cmdMonitor.Click += new System.EventHandler(this.cmdMonitor_Click);
            // 
            // cmdFlash
            // 
            this.cmdFlash.Location = new System.Drawing.Point(360, 71);
            this.cmdFlash.Name = "cmdFlash";
            this.cmdFlash.Size = new System.Drawing.Size(100, 23);
            this.cmdFlash.TabIndex = 7;
            this.cmdFlash.Text = "Flash";
            this.cmdFlash.UseVisualStyleBackColor = true;
            this.cmdFlash.Click += new System.EventHandler(this.cmdFlash_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Baudrate";
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.lblComPort);
            this.gbConnection.Controls.Add(this.cbComPort);
            this.gbConnection.Controls.Add(this.label2);
            this.gbConnection.Controls.Add(this.cbBaudrate);
            this.gbConnection.Controls.Add(this.cmdMonitor);
            this.gbConnection.Controls.Add(this.cmdGetInfo);
            this.gbConnection.Controls.Add(this.cmdReboot);
            this.gbConnection.Controls.Add(this.cmdSendLedTestData);
            this.gbConnection.Location = new System.Drawing.Point(12, 27);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Size = new System.Drawing.Size(520, 75);
            this.gbConnection.TabIndex = 65;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            // 
            // cbComPort
            // 
            this.cbComPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szComPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbComPort.FormattingEnabled = true;
            this.cbComPort.Location = new System.Drawing.Point(68, 17);
            this.cbComPort.Name = "cbComPort";
            this.cbComPort.Size = new System.Drawing.Size(173, 21);
            this.cbComPort.TabIndex = 1;
            this.cbComPort.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szComPort;
            // 
            // cbBaudrate
            // 
            this.cbBaudrate.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szBautRate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbBaudrate.FormattingEnabled = true;
            this.cbBaudrate.Items.AddRange(new object[] {
            "115200",
            "921600",
            "1500000"});
            this.cbBaudrate.Location = new System.Drawing.Point(305, 17);
            this.cbBaudrate.Name = "cbBaudrate";
            this.cbBaudrate.Size = new System.Drawing.Size(91, 21);
            this.cbBaudrate.TabIndex = 32;
            this.cbBaudrate.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szBautRate;
            this.cbBaudrate.SelectedIndexChanged += new System.EventHandler(this.cbBaudrate_SelectedIndexChanged);
            // 
            // cmdGetInfo
            // 
            this.cmdGetInfo.Enabled = false;
            this.cmdGetInfo.Location = new System.Drawing.Point(173, 45);
            this.cmdGetInfo.Name = "cmdGetInfo";
            this.cmdGetInfo.Size = new System.Drawing.Size(90, 23);
            this.cmdGetInfo.TabIndex = 25;
            this.cmdGetInfo.Text = "Get Info";
            this.cmdGetInfo.UseVisualStyleBackColor = true;
            this.cmdGetInfo.Click += new System.EventHandler(this.cmdGetInfo_Click);
            // 
            // cmdReboot
            // 
            this.cmdReboot.Enabled = false;
            this.cmdReboot.Location = new System.Drawing.Point(278, 45);
            this.cmdReboot.Name = "cmdReboot";
            this.cmdReboot.Size = new System.Drawing.Size(90, 23);
            this.cmdReboot.TabIndex = 35;
            this.cmdReboot.Text = "REBOOT";
            this.cmdReboot.UseVisualStyleBackColor = true;
            this.cmdReboot.Click += new System.EventHandler(this.cmdReboot_Click);
            // 
            // cmdSendLedTestData
            // 
            this.cmdSendLedTestData.Enabled = false;
            this.cmdSendLedTestData.Location = new System.Drawing.Point(378, 45);
            this.cmdSendLedTestData.Name = "cmdSendLedTestData";
            this.cmdSendLedTestData.Size = new System.Drawing.Size(115, 23);
            this.cmdSendLedTestData.TabIndex = 26;
            this.cmdSendLedTestData.Text = "Send LED Data";
            this.cmdSendLedTestData.UseVisualStyleBackColor = true;
            this.cmdSendLedTestData.Click += new System.EventHandler(this.cmdSendLedTestData_Click);
            // 
            // gbBuild
            // 
            this.gbBuild.Controls.Add(this.label1);
            this.gbBuild.Controls.Add(this.cbDevice);
            this.gbBuild.Controls.Add(this.lblVersion);
            this.gbBuild.Controls.Add(this.cbFirmwareVersion);
            this.gbBuild.Controls.Add(this.txtVersionCode);
            this.gbBuild.Controls.Add(this.label10);
            this.gbBuild.Controls.Add(this.cbBuildEnv);
            this.gbBuild.Controls.Add(this.cmdSettings);
            this.gbBuild.Controls.Add(this.cmdBuild);
            this.gbBuild.Controls.Add(this.cmdCleanBuild);
            this.gbBuild.Controls.Add(this.cmdBuildAndFlash);
            this.gbBuild.Controls.Add(this.cmdFlash);
            this.gbBuild.Controls.Add(this.cmdBuildAndUpload);
            this.gbBuild.Controls.Add(this.cmdUpload);
            this.gbBuild.Controls.Add(this.chkEsp32Debug);
            this.gbBuild.Controls.Add(this.chkAmbiBackVerboseDebug);
            this.gbBuild.Controls.Add(this.chkAmbiBackExtraDiag);
            this.gbBuild.Location = new System.Drawing.Point(12, 108);
            this.gbBuild.Name = "gbBuild";
            this.gbBuild.Size = new System.Drawing.Size(520, 195);
            this.gbBuild.TabIndex = 66;
            this.gbBuild.TabStop = false;
            this.gbBuild.Text = "Build & Flash";
            // 
            // cbDevice
            // 
            this.cbDevice.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szDevice", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbDevice.FormattingEnabled = true;
            this.cbDevice.Location = new System.Drawing.Point(68, 19);
            this.cbDevice.Name = "cbDevice";
            this.cbDevice.Size = new System.Drawing.Size(245, 21);
            this.cbDevice.TabIndex = 3;
            this.cbDevice.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szDevice;
            this.cbDevice.SelectedIndexChanged += new System.EventHandler(this.cbDevice_SelectedIndexChanged);
            // 
            // cbFirmwareVersion
            // 
            this.cbFirmwareVersion.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szFirmwareVersion", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbFirmwareVersion.FormattingEnabled = true;
            this.cbFirmwareVersion.Location = new System.Drawing.Point(68, 45);
            this.cbFirmwareVersion.Name = "cbFirmwareVersion";
            this.cbFirmwareVersion.Size = new System.Drawing.Size(84, 21);
            this.cbFirmwareVersion.TabIndex = 5;
            this.cbFirmwareVersion.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szFirmwareVersion;
            // 
            // txtVersionCode
            // 
            this.txtVersionCode.Location = new System.Drawing.Point(158, 45);
            this.txtVersionCode.Name = "txtVersionCode";
            this.txtVersionCode.ReadOnly = true;
            this.txtVersionCode.Size = new System.Drawing.Size(83, 20);
            this.txtVersionCode.TabIndex = 34;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 60;
            this.label10.Text = "Build ENV";
            // 
            // cbBuildEnv
            // 
            this.cbBuildEnv.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szBuildEnv", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbBuildEnv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuildEnv.FormattingEnabled = true;
            this.cbBuildEnv.Location = new System.Drawing.Point(68, 71);
            this.cbBuildEnv.Name = "cbBuildEnv";
            this.cbBuildEnv.Size = new System.Drawing.Size(173, 21);
            this.cbBuildEnv.TabIndex = 61;
            this.cbBuildEnv.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szBuildEnv;
            // 
            // cmdSettings
            // 
            this.cmdSettings.Location = new System.Drawing.Point(320, 19);
            this.cmdSettings.Name = "cmdSettings";
            this.cmdSettings.Size = new System.Drawing.Size(91, 23);
            this.cmdSettings.TabIndex = 62;
            this.cmdSettings.Text = "Settings...";
            this.cmdSettings.UseVisualStyleBackColor = true;
            this.cmdSettings.Click += new System.EventHandler(this.cmdSettings_Click);
            // 
            // cmdBuild
            // 
            this.cmdBuild.Location = new System.Drawing.Point(250, 45);
            this.cmdBuild.Name = "cmdBuild";
            this.cmdBuild.Size = new System.Drawing.Size(100, 23);
            this.cmdBuild.TabIndex = 33;
            this.cmdBuild.Text = "Build Image";
            this.cmdBuild.UseVisualStyleBackColor = true;
            this.cmdBuild.Click += new System.EventHandler(this.cmdBuild_Click);
            // 
            // cmdCleanBuild
            // 
            this.cmdCleanBuild.Location = new System.Drawing.Point(360, 45);
            this.cmdCleanBuild.Name = "cmdCleanBuild";
            this.cmdCleanBuild.Size = new System.Drawing.Size(100, 23);
            this.cmdCleanBuild.TabIndex = 39;
            this.cmdCleanBuild.Text = "Clean Build";
            this.cmdCleanBuild.UseVisualStyleBackColor = true;
            this.cmdCleanBuild.Click += new System.EventHandler(this.cmdCleanBuild_Click);
            // 
            // cmdBuildAndFlash
            // 
            this.cmdBuildAndFlash.Location = new System.Drawing.Point(250, 71);
            this.cmdBuildAndFlash.Name = "cmdBuildAndFlash";
            this.cmdBuildAndFlash.Size = new System.Drawing.Size(100, 23);
            this.cmdBuildAndFlash.TabIndex = 40;
            this.cmdBuildAndFlash.Text = "Build && Flash ";
            this.cmdBuildAndFlash.UseVisualStyleBackColor = true;
            this.cmdBuildAndFlash.Click += new System.EventHandler(this.cmdBuildAndFlash_Click);
            // 
            // cmdBuildAndUpload
            // 
            this.cmdBuildAndUpload.Location = new System.Drawing.Point(250, 97);
            this.cmdBuildAndUpload.Name = "cmdBuildAndUpload";
            this.cmdBuildAndUpload.Size = new System.Drawing.Size(100, 23);
            this.cmdBuildAndUpload.TabIndex = 38;
            this.cmdBuildAndUpload.Text = "Build && Upload";
            this.cmdBuildAndUpload.UseVisualStyleBackColor = true;
            this.cmdBuildAndUpload.Click += new System.EventHandler(this.cmdBuildAndUpload_Click);
            // 
            // cmdUpload
            // 
            this.cmdUpload.Location = new System.Drawing.Point(360, 97);
            this.cmdUpload.Name = "cmdUpload";
            this.cmdUpload.Size = new System.Drawing.Size(105, 23);
            this.cmdUpload.TabIndex = 37;
            this.cmdUpload.Text = "Upload Image";
            this.cmdUpload.UseVisualStyleBackColor = true;
            this.cmdUpload.Click += new System.EventHandler(this.cmdUpload_Click);
            // 
            // chkEsp32Debug
            // 
            this.chkEsp32Debug.AutoSize = true;
            this.chkEsp32Debug.Location = new System.Drawing.Point(8, 128);
            this.chkEsp32Debug.Name = "chkEsp32Debug";
            this.chkEsp32Debug.Size = new System.Drawing.Size(173, 17);
            this.chkEsp32Debug.TabIndex = 41;
            this.chkEsp32Debug.Text = "ESP32 Core Debug VERBOSE";
            this.chkEsp32Debug.UseVisualStyleBackColor = true;
            // 
            // chkAmbiBackVerboseDebug
            // 
            this.chkAmbiBackVerboseDebug.AutoSize = true;
            this.chkAmbiBackVerboseDebug.Location = new System.Drawing.Point(205, 128);
            this.chkAmbiBackVerboseDebug.Name = "chkAmbiBackVerboseDebug";
            this.chkAmbiBackVerboseDebug.Size = new System.Drawing.Size(169, 17);
            this.chkAmbiBackVerboseDebug.TabIndex = 43;
            this.chkAmbiBackVerboseDebug.Text = "AmbiBack VERBOSE DEBUG";
            this.chkAmbiBackVerboseDebug.UseVisualStyleBackColor = true;
            // 
            // chkAmbiBackExtraDiag
            // 
            this.chkAmbiBackExtraDiag.AutoSize = true;
            this.chkAmbiBackExtraDiag.Location = new System.Drawing.Point(205, 148);
            this.chkAmbiBackExtraDiag.Name = "chkAmbiBackExtraDiag";
            this.chkAmbiBackExtraDiag.Size = new System.Drawing.Size(176, 17);
            this.chkAmbiBackExtraDiag.TabIndex = 42;
            this.chkAmbiBackExtraDiag.Text = "AmbiBack EXTRA DIAG (MEM)";
            this.chkAmbiBackExtraDiag.UseVisualStyleBackColor = true;
            // 
            // gbDeviceInfo
            // 
            this.gbDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDeviceInfo.Controls.Add(this.label5);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceVersion);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceType);
            this.gbDeviceInfo.Controls.Add(this.label6);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceID);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceMAC);
            this.gbDeviceInfo.Controls.Add(this.label7);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceName);
            this.gbDeviceInfo.Controls.Add(this.label8);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceIP);
            this.gbDeviceInfo.Location = new System.Drawing.Point(538, 27);
            this.gbDeviceInfo.Name = "gbDeviceInfo";
            this.gbDeviceInfo.Size = new System.Drawing.Size(714, 145);
            this.gbDeviceInfo.TabIndex = 67;
            this.gbDeviceInfo.TabStop = false;
            this.gbDeviceInfo.Text = "Device Info";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 45;
            this.label5.Text = "FW Ver / Type:";
            // 
            // txtConnectedDeviceVersion
            // 
            this.txtConnectedDeviceVersion.Location = new System.Drawing.Point(100, 19);
            this.txtConnectedDeviceVersion.MaxLength = 32;
            this.txtConnectedDeviceVersion.Name = "txtConnectedDeviceVersion";
            this.txtConnectedDeviceVersion.ReadOnly = true;
            this.txtConnectedDeviceVersion.Size = new System.Drawing.Size(92, 20);
            this.txtConnectedDeviceVersion.TabIndex = 44;
            // 
            // txtConnectedDeviceType
            // 
            this.txtConnectedDeviceType.Location = new System.Drawing.Point(200, 19);
            this.txtConnectedDeviceType.MaxLength = 32;
            this.txtConnectedDeviceType.Name = "txtConnectedDeviceType";
            this.txtConnectedDeviceType.ReadOnly = true;
            this.txtConnectedDeviceType.Size = new System.Drawing.Size(92, 20);
            this.txtConnectedDeviceType.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 47;
            this.label6.Text = "DeviceID / MAC:";
            // 
            // txtConnectedDeviceID
            // 
            this.txtConnectedDeviceID.Location = new System.Drawing.Point(100, 44);
            this.txtConnectedDeviceID.MaxLength = 32;
            this.txtConnectedDeviceID.Name = "txtConnectedDeviceID";
            this.txtConnectedDeviceID.ReadOnly = true;
            this.txtConnectedDeviceID.Size = new System.Drawing.Size(92, 20);
            this.txtConnectedDeviceID.TabIndex = 46;
            // 
            // txtConnectedDeviceMAC
            // 
            this.txtConnectedDeviceMAC.Location = new System.Drawing.Point(200, 44);
            this.txtConnectedDeviceMAC.MaxLength = 32;
            this.txtConnectedDeviceMAC.Name = "txtConnectedDeviceMAC";
            this.txtConnectedDeviceMAC.ReadOnly = true;
            this.txtConnectedDeviceMAC.Size = new System.Drawing.Size(92, 20);
            this.txtConnectedDeviceMAC.TabIndex = 58;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 49;
            this.label7.Text = "DeviceName:";
            // 
            // txtConnectedDeviceName
            // 
            this.txtConnectedDeviceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectedDeviceName.Location = new System.Drawing.Point(100, 69);
            this.txtConnectedDeviceName.MaxLength = 32;
            this.txtConnectedDeviceName.Name = "txtConnectedDeviceName";
            this.txtConnectedDeviceName.ReadOnly = true;
            this.txtConnectedDeviceName.Size = new System.Drawing.Size(606, 20);
            this.txtConnectedDeviceName.TabIndex = 48;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 51;
            this.label8.Text = "DeviceIP:";
            // 
            // txtConnectedDeviceIP
            // 
            this.txtConnectedDeviceIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectedDeviceIP.Location = new System.Drawing.Point(100, 94);
            this.txtConnectedDeviceIP.MaxLength = 32;
            this.txtConnectedDeviceIP.Name = "txtConnectedDeviceIP";
            this.txtConnectedDeviceIP.ReadOnly = true;
            this.txtConnectedDeviceIP.Size = new System.Drawing.Size(606, 20);
            this.txtConnectedDeviceIP.TabIndex = 50;
            // 
            // gbDeviceControl
            // 
            this.gbDeviceControl.Controls.Add(this.cmdModeOff);
            this.gbDeviceControl.Controls.Add(this.cmdModeDemo);
            this.gbDeviceControl.Controls.Add(this.cmdModeCam);
            this.gbDeviceControl.Controls.Add(this.cmdSetRelay);
            this.gbDeviceControl.Controls.Add(this.label9);
            this.gbDeviceControl.Controls.Add(this.txtRelayID);
            this.gbDeviceControl.Controls.Add(this.cmdFactoryDefaults);
            this.gbDeviceControl.Location = new System.Drawing.Point(12, 309);
            this.gbDeviceControl.Name = "gbDeviceControl";
            this.gbDeviceControl.Size = new System.Drawing.Size(520, 100);
            this.gbDeviceControl.TabIndex = 68;
            this.gbDeviceControl.TabStop = false;
            this.gbDeviceControl.Text = "Device Control";
            // 
            // cmdModeOff
            // 
            this.cmdModeOff.Location = new System.Drawing.Point(8, 22);
            this.cmdModeOff.Name = "cmdModeOff";
            this.cmdModeOff.Size = new System.Drawing.Size(85, 23);
            this.cmdModeOff.TabIndex = 55;
            this.cmdModeOff.Text = "LED OFF";
            this.cmdModeOff.UseVisualStyleBackColor = true;
            this.cmdModeOff.Click += new System.EventHandler(this.cmdModeOff_Click);
            // 
            // cmdModeDemo
            // 
            this.cmdModeDemo.Location = new System.Drawing.Point(98, 22);
            this.cmdModeDemo.Name = "cmdModeDemo";
            this.cmdModeDemo.Size = new System.Drawing.Size(85, 23);
            this.cmdModeDemo.TabIndex = 56;
            this.cmdModeDemo.Text = "LED DEMO";
            this.cmdModeDemo.UseVisualStyleBackColor = true;
            this.cmdModeDemo.Click += new System.EventHandler(this.cmdModeDemo_Click);
            // 
            // cmdModeCam
            // 
            this.cmdModeCam.Location = new System.Drawing.Point(188, 22);
            this.cmdModeCam.Name = "cmdModeCam";
            this.cmdModeCam.Size = new System.Drawing.Size(85, 23);
            this.cmdModeCam.TabIndex = 57;
            this.cmdModeCam.Text = "LED CAM";
            this.cmdModeCam.UseVisualStyleBackColor = true;
            this.cmdModeCam.Click += new System.EventHandler(this.cmdModeCam_Click);
            // 
            // cmdSetRelay
            // 
            this.cmdSetRelay.Location = new System.Drawing.Point(8, 54);
            this.cmdSetRelay.Name = "cmdSetRelay";
            this.cmdSetRelay.Size = new System.Drawing.Size(100, 23);
            this.cmdSetRelay.TabIndex = 52;
            this.cmdSetRelay.Text = "Set RELAY";
            this.cmdSetRelay.UseVisualStyleBackColor = true;
            this.cmdSetRelay.Click += new System.EventHandler(this.cmdSetRelay_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(115, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 54;
            this.label9.Text = "Relay ID";
            // 
            // txtRelayID
            // 
            this.txtRelayID.Location = new System.Drawing.Point(165, 54);
            this.txtRelayID.MaxLength = 32;
            this.txtRelayID.Name = "txtRelayID";
            this.txtRelayID.Size = new System.Drawing.Size(130, 20);
            this.txtRelayID.TabIndex = 53;
            // 
            // cmdFactoryDefaults
            // 
            this.cmdFactoryDefaults.BackColor = System.Drawing.Color.Red;
            this.cmdFactoryDefaults.Enabled = false;
            this.cmdFactoryDefaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdFactoryDefaults.ForeColor = System.Drawing.Color.White;
            this.cmdFactoryDefaults.Location = new System.Drawing.Point(283, 22);
            this.cmdFactoryDefaults.Name = "cmdFactoryDefaults";
            this.cmdFactoryDefaults.Size = new System.Drawing.Size(120, 23);
            this.cmdFactoryDefaults.TabIndex = 36;
            this.cmdFactoryDefaults.Text = "FACTORY DEFAULTS";
            this.cmdFactoryDefaults.UseVisualStyleBackColor = false;
            this.cmdFactoryDefaults.Click += new System.EventHandler(this.cmdFactoryDefaults_Click);
            // 
            // gbWifi
            // 
            this.gbWifi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbWifi.Controls.Add(this.label3);
            this.gbWifi.Controls.Add(this.txtWifiSSID);
            this.gbWifi.Controls.Add(this.label4);
            this.gbWifi.Controls.Add(this.txtWifiKey);
            this.gbWifi.Controls.Add(this.cmdResetWifi);
            this.gbWifi.Controls.Add(this.cmdSetWifi);
            this.gbWifi.Location = new System.Drawing.Point(538, 178);
            this.gbWifi.Name = "gbWifi";
            this.gbWifi.Size = new System.Drawing.Size(714, 125);
            this.gbWifi.TabIndex = 69;
            this.gbWifi.TabStop = false;
            this.gbWifi.Text = "WiFi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "SSID";
            // 
            // txtWifiSSID
            // 
            this.txtWifiSSID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWifiSSID.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szWifiSSID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtWifiSSID.Location = new System.Drawing.Point(55, 22);
            this.txtWifiSSID.MaxLength = 32;
            this.txtWifiSSID.Name = "txtWifiSSID";
            this.txtWifiSSID.Size = new System.Drawing.Size(651, 20);
            this.txtWifiSSID.TabIndex = 28;
            this.txtWifiSSID.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szWifiSSID;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Key";
            // 
            // txtWifiKey
            // 
            this.txtWifiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWifiKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szWifiKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtWifiKey.Location = new System.Drawing.Point(55, 49);
            this.txtWifiKey.MaxLength = 64;
            this.txtWifiKey.Name = "txtWifiKey";
            this.txtWifiKey.Size = new System.Drawing.Size(651, 20);
            this.txtWifiKey.TabIndex = 31;
            this.txtWifiKey.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szWifiKey;
            // 
            // cmdResetWifi
            // 
            this.cmdResetWifi.Location = new System.Drawing.Point(55, 78);
            this.cmdResetWifi.Name = "cmdResetWifi";
            this.cmdResetWifi.Size = new System.Drawing.Size(91, 35);
            this.cmdResetWifi.TabIndex = 59;
            this.cmdResetWifi.Text = "RESET WIFI";
            this.cmdResetWifi.UseVisualStyleBackColor = true;
            this.cmdResetWifi.Click += new System.EventHandler(this.cmdResetWifi_Click);
            // 
            // cmdSetWifi
            // 
            this.cmdSetWifi.Enabled = false;
            this.cmdSetWifi.Location = new System.Drawing.Point(152, 78);
            this.cmdSetWifi.Name = "cmdSetWifi";
            this.cmdSetWifi.Size = new System.Drawing.Size(91, 35);
            this.cmdSetWifi.TabIndex = 27;
            this.cmdSetWifi.Text = "Set WIFI";
            this.cmdSetWifi.UseVisualStyleBackColor = true;
            this.cmdSetWifi.Click += new System.EventHandler(this.cmdSetWifi_Click);
            // 
            // gbDebug
            // 
            this.gbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDebug.Controls.Add(this.txtDebug);
            this.gbDebug.Location = new System.Drawing.Point(12, 410);
            this.gbDebug.Name = "gbDebug";
            this.gbDebug.Size = new System.Drawing.Size(1240, 505);
            this.gbDebug.TabIndex = 24;
            this.gbDebug.TabStop = false;
            this.gbDebug.Text = "Firmware / Monitor Output";
            // 
            // txtDebug
            // 
            this.txtDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDebug.Location = new System.Drawing.Point(8, 19);
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.ReadOnly = true;
            this.txtDebug.Size = new System.Drawing.Size(1216, 475);
            this.txtDebug.TabIndex = 22;
            this.txtDebug.Text = "";
            this.txtDebug.WordWrap = false;
            // 
            // timerComPort
            // 
            this.timerComPort.Enabled = true;
            this.timerComPort.Interval = 1000;
            this.timerComPort.Tick += new System.EventHandler(this.timerComPort_Tick);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1264, 24);
            this.menuStrip.TabIndex = 63;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSettings,
            this.menuQuit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "File";
            // 
            // menuSettings
            // 
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(125, 22);
            this.menuSettings.Text = "Settings...";
            this.menuSettings.Click += new System.EventHandler(this.cmdSettings_Click);
            // 
            // menuQuit
            // 
            this.menuQuit.Name = "menuQuit";
            this.menuQuit.Size = new System.Drawing.Size(125, 22);
            this.menuQuit.Text = "Quit";
            this.menuQuit.Click += new System.EventHandler(this.menuQuit_Click);
            // 
            // progressBarBuild
            // 
            this.progressBarBuild.Location = new System.Drawing.Point(12, 930);
            this.progressBarBuild.Name = "progressBarBuild";
            this.progressBarBuild.Size = new System.Drawing.Size(1576, 18);
            this.progressBarBuild.TabIndex = 64;
            this.progressBarBuild.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 861);
            this.Controls.Add(this.gbConnection);
            this.Controls.Add(this.gbBuild);
            this.Controls.Add(this.gbDeviceInfo);
            this.Controls.Add(this.gbDeviceControl);
            this.Controls.Add(this.gbWifi);
            this.Controls.Add(this.gbDebug);
            this.Controls.Add(this.progressBarBuild);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AmbiBack USB Flasher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.gbBuild.ResumeLayout(false);
            this.gbBuild.PerformLayout();
            this.gbDeviceInfo.ResumeLayout(false);
            this.gbDeviceInfo.PerformLayout();
            this.gbDeviceControl.ResumeLayout(false);
            this.gbDeviceControl.PerformLayout();
            this.gbWifi.ResumeLayout(false);
            this.gbWifi.PerformLayout();
            this.gbDebug.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.GroupBox gbBuild;
        private System.Windows.Forms.GroupBox gbDeviceInfo;
        private System.Windows.Forms.GroupBox gbDeviceControl;
        private System.Windows.Forms.GroupBox gbWifi;
        private System.Windows.Forms.Label lblComPort;
        private System.Windows.Forms.ComboBox cbComPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ComboBox cbFirmwareVersion;
        private System.Windows.Forms.Button cmdMonitor;
        private System.Windows.Forms.Button cmdFlash;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbDebug;
        private System.Windows.Forms.RichTextBox txtDebug;
        private System.Windows.Forms.Timer timerComPort;
        private System.Windows.Forms.Button cmdGetInfo;
        private System.Windows.Forms.Button cmdSendLedTestData;
        private System.Windows.Forms.Button cmdSetWifi;
        private System.Windows.Forms.TextBox txtWifiSSID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtWifiKey;
        private System.Windows.Forms.ComboBox cbBaudrate;
        private System.Windows.Forms.Button cmdBuild;
        private System.Windows.Forms.TextBox txtVersionCode;
        private System.Windows.Forms.Button cmdReboot;
        private System.Windows.Forms.Button cmdFactoryDefaults;
        private System.Windows.Forms.Button cmdUpload;
        private System.Windows.Forms.Button cmdBuildAndUpload;
        private System.Windows.Forms.Button cmdCleanBuild;
        private System.Windows.Forms.Button cmdBuildAndFlash;
        private System.Windows.Forms.CheckBox chkEsp32Debug;
        private System.Windows.Forms.CheckBox chkAmbiBackExtraDiag;
        private System.Windows.Forms.CheckBox chkAmbiBackVerboseDebug;
        private System.Windows.Forms.TextBox txtConnectedDeviceVersion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtConnectedDeviceID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtConnectedDeviceName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtConnectedDeviceIP;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRelayID;
        private System.Windows.Forms.Button cmdSetRelay;
        private System.Windows.Forms.Button cmdModeOff;
        private System.Windows.Forms.Button cmdModeDemo;
        private System.Windows.Forms.Button cmdModeCam;
        private System.Windows.Forms.TextBox txtConnectedDeviceMAC;
        private System.Windows.Forms.TextBox txtConnectedDeviceType;
        private System.Windows.Forms.Button cmdResetWifi;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbBuildEnv;
        private System.Windows.Forms.Button cmdSettings;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuQuit;
        private System.Windows.Forms.ProgressBar progressBarBuild;
    }
}

