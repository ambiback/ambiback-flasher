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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenDataFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenFirmwareFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewEasy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewAdvanced = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGetInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReboot = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeviceSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSetWifi = new System.Windows.Forms.ToolStripMenuItem();
            this.menuResetWifi = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDeviceSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFactoryDefaults = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFirmware = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBuildAndFlash = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFlash = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFirmwareSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCleanBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBuildAndUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetupTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDownloadSource = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLangEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLangGerman = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjectPage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFirmwarePage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.rbEasy = new System.Windows.Forms.RadioButton();
            this.rbAdvanced = new System.Windows.Forms.RadioButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBarBuild = new System.Windows.Forms.ToolStripProgressBar();
            this.lblPortStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelEasy = new System.Windows.Forms.Panel();
            this.panelEasyLeft = new System.Windows.Forms.Panel();
            this.lvDevices = new System.Windows.Forms.ListView();
            this.imgDevices = new System.Windows.Forms.ImageList(this.components);
            this.lblEasyStep1 = new System.Windows.Forms.Label();
            this.panelEasyCard = new System.Windows.Forms.Panel();
            this.picEasyDevice = new System.Windows.Forms.PictureBox();
            this.lblEasyDeviceName = new System.Windows.Forms.Label();
            this.lblEasyStep2 = new System.Windows.Forms.Label();
            this.cbEasyPort = new System.Windows.Forms.ComboBox();
            this.lblEasyStep3 = new System.Windows.Forms.Label();
            this.cbEasyFirmware = new System.Windows.Forms.ComboBox();
            this.chkEasyWifi = new System.Windows.Forms.CheckBox();
            this.lblEasySsid = new System.Windows.Forms.Label();
            this.txtEasySsid = new System.Windows.Forms.TextBox();
            this.lblEasyKey = new System.Windows.Forms.Label();
            this.txtEasyKey = new System.Windows.Forms.TextBox();
            this.cmdEasyFlash = new System.Windows.Forms.Button();
            this.pbEasy = new System.Windows.Forms.ProgressBar();
            this.lblEasyStatus = new System.Windows.Forms.Label();
            this.lnkEasyDetails = new System.Windows.Forms.LinkLabel();
            this.panelAdvanced = new System.Windows.Forms.Panel();
            this.splitAdvanced = new System.Windows.Forms.SplitContainer();
            this.panelAdvLeft = new System.Windows.Forms.Panel();
            this.gbWifi = new AmbiBack_USB_Flasher.CardGroupBox();
            this.lblSsid = new System.Windows.Forms.Label();
            this.txtWifiSSID = new System.Windows.Forms.TextBox();
            this.lblKey = new System.Windows.Forms.Label();
            this.txtWifiKey = new System.Windows.Forms.TextBox();
            this.cmdSetWifi = new System.Windows.Forms.Button();
            this.cmdResetWifi = new System.Windows.Forms.Button();
            this.gbDeviceControl = new AmbiBack_USB_Flasher.CardGroupBox();
            this.lblLedMode = new System.Windows.Forms.Label();
            this.cmdModeOff = new System.Windows.Forms.Button();
            this.cmdModeDemo = new System.Windows.Forms.Button();
            this.cmdModeCam = new System.Windows.Forms.Button();
            this.cmdSendLedTestData = new System.Windows.Forms.Button();
            this.lblRelayId = new System.Windows.Forms.Label();
            this.txtRelayID = new System.Windows.Forms.TextBox();
            this.cmdSetRelay = new System.Windows.Forms.Button();
            this.cmdFactoryDefaults = new System.Windows.Forms.Button();
            this.gbDeviceInfo = new AmbiBack_USB_Flasher.CardGroupBox();
            this.lblInfoVersion = new System.Windows.Forms.Label();
            this.txtConnectedDeviceVersion = new System.Windows.Forms.TextBox();
            this.txtConnectedDeviceType = new System.Windows.Forms.TextBox();
            this.lblInfoId = new System.Windows.Forms.Label();
            this.txtConnectedDeviceID = new System.Windows.Forms.TextBox();
            this.txtConnectedDeviceMAC = new System.Windows.Forms.TextBox();
            this.lblInfoName = new System.Windows.Forms.Label();
            this.txtConnectedDeviceName = new System.Windows.Forms.TextBox();
            this.lblInfoIp = new System.Windows.Forms.Label();
            this.txtConnectedDeviceIP = new System.Windows.Forms.TextBox();
            this.gbBuild = new AmbiBack_USB_Flasher.CardGroupBox();
            this.lblDevice = new System.Windows.Forms.Label();
            this.cbDevice = new System.Windows.Forms.ComboBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.cbFirmwareVersion = new System.Windows.Forms.ComboBox();
            this.lblSourceVersion = new System.Windows.Forms.Label();
            this.txtVersionCode = new System.Windows.Forms.TextBox();
            this.lblBuilder = new System.Windows.Forms.Label();
            this.cbBuilder = new System.Windows.Forms.ComboBox();
            this.lblBuildEnv = new System.Windows.Forms.Label();
            this.cbBuildEnv = new System.Windows.Forms.ComboBox();
            this.cmdSetupTools = new System.Windows.Forms.Button();
            this.chkEsp32Debug = new System.Windows.Forms.CheckBox();
            this.chkAmbiBackVerboseDebug = new System.Windows.Forms.CheckBox();
            this.chkAmbiBackExtraDiag = new System.Windows.Forms.CheckBox();
            this.cmdBuild = new System.Windows.Forms.Button();
            this.cmdBuildAndFlash = new System.Windows.Forms.Button();
            this.cmdFlash = new System.Windows.Forms.Button();
            this.cmdCleanBuild = new System.Windows.Forms.Button();
            this.cmdBuildAndUpload = new System.Windows.Forms.Button();
            this.cmdUpload = new System.Windows.Forms.Button();
            this.gbConnection = new AmbiBack_USB_Flasher.CardGroupBox();
            this.lblComPort = new System.Windows.Forms.Label();
            this.cbComPort = new System.Windows.Forms.ComboBox();
            this.lblBaudrate = new System.Windows.Forms.Label();
            this.cbBaudrate = new System.Windows.Forms.ComboBox();
            this.cmdMonitor = new System.Windows.Forms.Button();
            this.cmdGetInfo = new System.Windows.Forms.Button();
            this.cmdReboot = new System.Windows.Forms.Button();
            this.gbDebug = new AmbiBack_USB_Flasher.CardGroupBox();
            this.txtDebug = new System.Windows.Forms.RichTextBox();
            this.toolStripLog = new System.Windows.Forms.ToolStrip();
            this.tsbClearLog = new System.Windows.Forms.ToolStripButton();
            this.tsbCopyLog = new System.Windows.Forms.ToolStripButton();
            this.timerComPort = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.panelEasy.SuspendLayout();
            this.panelEasyLeft.SuspendLayout();
            this.panelEasyCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEasyDevice)).BeginInit();
            this.panelAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitAdvanced)).BeginInit();
            this.splitAdvanced.Panel1.SuspendLayout();
            this.splitAdvanced.Panel2.SuspendLayout();
            this.splitAdvanced.SuspendLayout();
            this.panelAdvLeft.SuspendLayout();
            this.gbWifi.SuspendLayout();
            this.gbDeviceControl.SuspendLayout();
            this.gbDeviceInfo.SuspendLayout();
            this.gbBuild.SuspendLayout();
            this.gbConnection.SuspendLayout();
            this.gbDebug.SuspendLayout();
            this.toolStripLog.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip
            //
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuView,
            this.menuDevice,
            this.menuFirmware,
            this.menuTools,
            this.menuLanguage,
            this.menuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1100, 24);
            this.menuStrip.TabIndex = 0;
            //
            // menuFile
            //
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSettings,
            this.menuOpenDataFolder,
            this.menuOpenFirmwareFolder,
            this.menuFileSep1,
            this.menuQuit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Text = "&File";
            //
            // menuSettings
            //
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.ShortcutKeyDisplayString = "Ctrl+,";
            this.menuSettings.Text = "Settings...";
            this.menuSettings.Click += new System.EventHandler(this.cmdSettings_Click);
            //
            // menuOpenDataFolder
            //
            this.menuOpenDataFolder.Name = "menuOpenDataFolder";
            this.menuOpenDataFolder.Text = "Open Data Folder";
            this.menuOpenDataFolder.Click += new System.EventHandler(this.menuOpenDataFolder_Click);
            //
            // menuOpenFirmwareFolder
            //
            this.menuOpenFirmwareFolder.Name = "menuOpenFirmwareFolder";
            this.menuOpenFirmwareFolder.Text = "Open Firmware Folder";
            this.menuOpenFirmwareFolder.Click += new System.EventHandler(this.menuOpenFirmwareFolder_Click);
            //
            // menuFileSep1
            //
            this.menuFileSep1.Name = "menuFileSep1";
            //
            // menuQuit
            //
            this.menuQuit.Name = "menuQuit";
            this.menuQuit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuQuit.Text = "Exit";
            this.menuQuit.Click += new System.EventHandler(this.menuQuit_Click);
            //
            // menuView
            //
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewEasy,
            this.menuViewAdvanced,
            this.menuViewSep1,
            this.menuClearLog});
            this.menuView.Name = "menuView";
            this.menuView.Text = "&View";
            //
            // menuViewEasy
            //
            this.menuViewEasy.Name = "menuViewEasy";
            this.menuViewEasy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.menuViewEasy.Text = "Easy Mode";
            this.menuViewEasy.Click += new System.EventHandler(this.menuViewEasy_Click);
            //
            // menuViewAdvanced
            //
            this.menuViewAdvanced.Name = "menuViewAdvanced";
            this.menuViewAdvanced.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.menuViewAdvanced.Text = "Advanced Mode";
            this.menuViewAdvanced.Click += new System.EventHandler(this.menuViewAdvanced_Click);
            //
            // menuViewSep1
            //
            this.menuViewSep1.Name = "menuViewSep1";
            //
            // menuClearLog
            //
            this.menuClearLog.Name = "menuClearLog";
            this.menuClearLog.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.menuClearLog.Text = "Clear Output";
            this.menuClearLog.Click += new System.EventHandler(this.tsbClearLog_Click);
            //
            // menuDevice
            //
            this.menuDevice.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuConnect,
            this.menuGetInfo,
            this.menuReboot,
            this.menuDeviceSep1,
            this.menuSetWifi,
            this.menuResetWifi,
            this.menuDeviceSep2,
            this.menuFactoryDefaults});
            this.menuDevice.Name = "menuDevice";
            this.menuDevice.Text = "&Device";
            this.menuDevice.DropDownOpening += new System.EventHandler(this.menuDevice_DropDownOpening);
            //
            // menuConnect
            //
            this.menuConnect.Name = "menuConnect";
            this.menuConnect.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuConnect.Text = "Connect";
            this.menuConnect.Click += new System.EventHandler(this.menuConnect_Click);
            //
            // menuGetInfo
            //
            this.menuGetInfo.Name = "menuGetInfo";
            this.menuGetInfo.Text = "Get Device Info";
            this.menuGetInfo.Click += new System.EventHandler(this.menuGetInfo_Click);
            //
            // menuReboot
            //
            this.menuReboot.Name = "menuReboot";
            this.menuReboot.Text = "Restart Device";
            this.menuReboot.Click += new System.EventHandler(this.menuReboot_Click);
            //
            // menuDeviceSep1
            //
            this.menuDeviceSep1.Name = "menuDeviceSep1";
            //
            // menuSetWifi
            //
            this.menuSetWifi.Name = "menuSetWifi";
            this.menuSetWifi.Text = "Send Wi-Fi Settings";
            this.menuSetWifi.Click += new System.EventHandler(this.menuSetWifi_Click);
            //
            // menuResetWifi
            //
            this.menuResetWifi.Name = "menuResetWifi";
            this.menuResetWifi.Text = "Reset Wi-Fi";
            this.menuResetWifi.Click += new System.EventHandler(this.menuResetWifi_Click);
            //
            // menuDeviceSep2
            //
            this.menuDeviceSep2.Name = "menuDeviceSep2";
            //
            // menuFactoryDefaults
            //
            this.menuFactoryDefaults.Name = "menuFactoryDefaults";
            this.menuFactoryDefaults.Text = "Reset to Factory Defaults...";
            this.menuFactoryDefaults.Click += new System.EventHandler(this.menuFactoryDefaults_Click);
            //
            // menuFirmware
            //
            this.menuFirmware.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBuild,
            this.menuBuildAndFlash,
            this.menuFlash,
            this.menuFirmwareSep1,
            this.menuCleanBuild,
            this.menuUpload,
            this.menuBuildAndUpload});
            this.menuFirmware.Name = "menuFirmware";
            this.menuFirmware.Text = "F&irmware";
            this.menuFirmware.DropDownOpening += new System.EventHandler(this.menuFirmware_DropDownOpening);
            //
            // menuBuild
            //
            this.menuBuild.Name = "menuBuild";
            this.menuBuild.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.menuBuild.Text = "Build";
            this.menuBuild.Click += new System.EventHandler(this.menuBuild_Click);
            //
            // menuBuildAndFlash
            //
            this.menuBuildAndFlash.Name = "menuBuildAndFlash";
            this.menuBuildAndFlash.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F7)));
            this.menuBuildAndFlash.Text = "Build and Flash";
            this.menuBuildAndFlash.Click += new System.EventHandler(this.menuBuildAndFlash_Click);
            //
            // menuFlash
            //
            this.menuFlash.Name = "menuFlash";
            this.menuFlash.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.menuFlash.Text = "Flash";
            this.menuFlash.Click += new System.EventHandler(this.menuFlash_Click);
            //
            // menuFirmwareSep1
            //
            this.menuFirmwareSep1.Name = "menuFirmwareSep1";
            //
            // menuCleanBuild
            //
            this.menuCleanBuild.Name = "menuCleanBuild";
            this.menuCleanBuild.Text = "Clean Build Folder";
            this.menuCleanBuild.Click += new System.EventHandler(this.menuCleanBuild_Click);
            //
            // menuUpload
            //
            this.menuUpload.Name = "menuUpload";
            this.menuUpload.Text = "Upload Image";
            this.menuUpload.Click += new System.EventHandler(this.menuUpload_Click);
            //
            // menuBuildAndUpload
            //
            this.menuBuildAndUpload.Name = "menuBuildAndUpload";
            this.menuBuildAndUpload.Text = "Build and Upload";
            this.menuBuildAndUpload.Click += new System.EventHandler(this.menuBuildAndUpload_Click);
            //
            // menuTools
            //
            this.menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSetupTools,
            this.menuDownloadSource});
            this.menuTools.Name = "menuTools";
            this.menuTools.Text = "&Tools";
            //
            // menuSetupTools
            //
            this.menuSetupTools.Name = "menuSetupTools";
            this.menuSetupTools.Text = "Set Up Arduino and ESP32...";
            this.menuSetupTools.Click += new System.EventHandler(this.cmdSetupTools_Click);
            //
            // menuDownloadSource
            //
            this.menuDownloadSource.Name = "menuDownloadSource";
            this.menuDownloadSource.Text = "Download Firmware Source from GitHub";
            this.menuDownloadSource.Click += new System.EventHandler(this.menuDownloadSource_Click);
            //
            // menuLanguage
            //
            this.menuLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLangEnglish,
            this.menuLangGerman});
            this.menuLanguage.Name = "menuLanguage";
            this.menuLanguage.Text = "&Language";
            //
            // menuLangEnglish
            //
            this.menuLangEnglish.Name = "menuLangEnglish";
            this.menuLangEnglish.Text = "English";
            this.menuLangEnglish.Click += new System.EventHandler(this.menuLangEnglish_Click);
            //
            // menuLangGerman
            //
            this.menuLangGerman.Name = "menuLangGerman";
            this.menuLangGerman.Text = "Deutsch";
            this.menuLangGerman.Click += new System.EventHandler(this.menuLangGerman_Click);
            //
            // menuHelp
            //
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProjectPage,
            this.menuFirmwarePage,
            this.menuHelpSep1,
            this.menuAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Text = "&Help";
            //
            // menuProjectPage
            //
            this.menuProjectPage.Name = "menuProjectPage";
            this.menuProjectPage.Text = "Flasher on GitHub";
            this.menuProjectPage.Click += new System.EventHandler(this.menuProjectPage_Click);
            //
            // menuFirmwarePage
            //
            this.menuFirmwarePage.Name = "menuFirmwarePage";
            this.menuFirmwarePage.Text = "Firmware on GitHub";
            this.menuFirmwarePage.Click += new System.EventHandler(this.menuFirmwarePage_Click);
            //
            // menuHelpSep1
            //
            this.menuHelpSep1.Name = "menuHelpSep1";
            //
            // menuAbout
            //
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Text = "About AmbiBack USB Flasher";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            //
            // panelHeader
            //
            this.panelHeader.BackColor = System.Drawing.Color.White;
            this.panelHeader.Controls.Add(this.picLogo);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblSubtitle);
            this.panelHeader.Controls.Add(this.rbEasy);
            this.panelHeader.Controls.Add(this.rbAdvanced);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 24);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1100, 64);
            this.panelHeader.TabIndex = 1;
            this.panelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHeader_Paint);
            //
            // picLogo
            //
            this.picLogo.Location = new System.Drawing.Point(14, 10);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(44, 44);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(66, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 25);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "AmbiBack USB Flasher";
            //
            // lblSubtitle
            //
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Location = new System.Drawing.Point(68, 37);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(250, 15);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "Install AmbiBack firmware on your device";
            //
            // rbEasy
            //
            this.rbEasy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbEasy.Location = new System.Drawing.Point(870, 17);
            this.rbEasy.Name = "rbEasy";
            this.rbEasy.Size = new System.Drawing.Size(105, 30);
            this.rbEasy.TabIndex = 3;
            this.rbEasy.Text = "Easy";
            this.rbEasy.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            //
            // rbAdvanced
            //
            this.rbAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbAdvanced.Location = new System.Drawing.Point(974, 17);
            this.rbAdvanced.Name = "rbAdvanced";
            this.rbAdvanced.Size = new System.Drawing.Size(110, 30);
            this.rbAdvanced.TabIndex = 4;
            this.rbAdvanced.Text = "Advanced";
            this.rbAdvanced.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.progressBarBuild,
            this.lblPortStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 698);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1100, 22);
            this.statusStrip.TabIndex = 4;
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Ready";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // progressBarBuild
            //
            this.progressBarBuild.Name = "progressBarBuild";
            this.progressBarBuild.Size = new System.Drawing.Size(200, 16);
            this.progressBarBuild.Visible = false;
            //
            // lblPortStatus
            //
            this.lblPortStatus.Name = "lblPortStatus";
            this.lblPortStatus.Text = "Not connected";
            //
            // panelEasy
            //
            this.panelEasy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.panelEasy.Controls.Add(this.panelEasyLeft);
            this.panelEasy.Controls.Add(this.panelEasyCard);
            this.panelEasy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEasy.Location = new System.Drawing.Point(0, 88);
            this.panelEasy.Name = "panelEasy";
            this.panelEasy.Padding = new System.Windows.Forms.Padding(12);
            this.panelEasy.Size = new System.Drawing.Size(1100, 610);
            this.panelEasy.TabIndex = 2;
            //
            // panelEasyLeft
            //
            this.panelEasyLeft.BackColor = System.Drawing.Color.White;
            this.panelEasyLeft.Controls.Add(this.lvDevices);
            this.panelEasyLeft.Controls.Add(this.lblEasyStep1);
            this.panelEasyLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEasyLeft.Location = new System.Drawing.Point(12, 12);
            this.panelEasyLeft.Name = "panelEasyLeft";
            this.panelEasyLeft.Padding = new System.Windows.Forms.Padding(12, 4, 12, 12);
            this.panelEasyLeft.Size = new System.Drawing.Size(724, 586);
            this.panelEasyLeft.TabIndex = 0;
            //
            // lvDevices
            //
            this.lvDevices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDevices.HideSelection = false;
            this.lvDevices.LargeImageList = this.imgDevices;
            this.lvDevices.Location = new System.Drawing.Point(12, 44);
            this.lvDevices.MultiSelect = false;
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.Size = new System.Drawing.Size(700, 530);
            this.lvDevices.TabIndex = 1;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.SelectedIndexChanged += new System.EventHandler(this.lvDevices_SelectedIndexChanged);
            //
            // imgDevices
            //
            this.imgDevices.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imgDevices.ImageSize = new System.Drawing.Size(96, 96);
            this.imgDevices.TransparentColor = System.Drawing.Color.Transparent;
            //
            // lblEasyStep1
            //
            this.lblEasyStep1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEasyStep1.Location = new System.Drawing.Point(12, 4);
            this.lblEasyStep1.Name = "lblEasyStep1";
            this.lblEasyStep1.Size = new System.Drawing.Size(700, 40);
            this.lblEasyStep1.TabIndex = 0;
            this.lblEasyStep1.Text = "1   Choose your device";
            this.lblEasyStep1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // panelEasyCard
            //
            this.panelEasyCard.AutoScroll = true;
            this.panelEasyCard.BackColor = System.Drawing.Color.White;
            this.panelEasyCard.Controls.Add(this.picEasyDevice);
            this.panelEasyCard.Controls.Add(this.lblEasyDeviceName);
            this.panelEasyCard.Controls.Add(this.lblEasyStep2);
            this.panelEasyCard.Controls.Add(this.cbEasyPort);
            this.panelEasyCard.Controls.Add(this.lblEasyStep3);
            this.panelEasyCard.Controls.Add(this.cbEasyFirmware);
            this.panelEasyCard.Controls.Add(this.chkEasyWifi);
            this.panelEasyCard.Controls.Add(this.lblEasySsid);
            this.panelEasyCard.Controls.Add(this.txtEasySsid);
            this.panelEasyCard.Controls.Add(this.lblEasyKey);
            this.panelEasyCard.Controls.Add(this.txtEasyKey);
            this.panelEasyCard.Controls.Add(this.cmdEasyFlash);
            this.panelEasyCard.Controls.Add(this.pbEasy);
            this.panelEasyCard.Controls.Add(this.lblEasyStatus);
            this.panelEasyCard.Controls.Add(this.lnkEasyDetails);
            this.panelEasyCard.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelEasyCard.Location = new System.Drawing.Point(736, 12);
            this.panelEasyCard.Name = "panelEasyCard";
            this.panelEasyCard.Padding = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.panelEasyCard.Size = new System.Drawing.Size(352, 586);
            this.panelEasyCard.TabIndex = 1;
            this.panelEasyCard.Paint += new System.Windows.Forms.PaintEventHandler(this.panelEasyCard_Paint);
            //
            // picEasyDevice
            //
            this.picEasyDevice.Location = new System.Drawing.Point(96, 14);
            this.picEasyDevice.Name = "picEasyDevice";
            this.picEasyDevice.Size = new System.Drawing.Size(150, 120);
            this.picEasyDevice.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEasyDevice.TabIndex = 0;
            this.picEasyDevice.TabStop = false;
            //
            // lblEasyDeviceName
            //
            this.lblEasyDeviceName.Location = new System.Drawing.Point(24, 136);
            this.lblEasyDeviceName.Name = "lblEasyDeviceName";
            this.lblEasyDeviceName.Size = new System.Drawing.Size(296, 42);
            this.lblEasyDeviceName.TabIndex = 1;
            this.lblEasyDeviceName.Text = "No device selected";
            this.lblEasyDeviceName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // lblEasyStep2
            //
            this.lblEasyStep2.AutoSize = true;
            this.lblEasyStep2.Location = new System.Drawing.Point(22, 184);
            this.lblEasyStep2.Name = "lblEasyStep2";
            this.lblEasyStep2.Size = new System.Drawing.Size(120, 15);
            this.lblEasyStep2.TabIndex = 2;
            this.lblEasyStep2.Text = "2   USB port";
            //
            // cbEasyPort
            //
            this.cbEasyPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEasyPort.FormattingEnabled = true;
            this.cbEasyPort.Location = new System.Drawing.Point(24, 210);
            this.cbEasyPort.Name = "cbEasyPort";
            this.cbEasyPort.Size = new System.Drawing.Size(296, 23);
            this.cbEasyPort.TabIndex = 3;
            this.cbEasyPort.SelectedIndexChanged += new System.EventHandler(this.cbEasyPort_SelectedIndexChanged);
            //
            // lblEasyStep3
            //
            this.lblEasyStep3.AutoSize = true;
            this.lblEasyStep3.Location = new System.Drawing.Point(22, 248);
            this.lblEasyStep3.Name = "lblEasyStep3";
            this.lblEasyStep3.Size = new System.Drawing.Size(120, 15);
            this.lblEasyStep3.TabIndex = 4;
            this.lblEasyStep3.Text = "3   Firmware";
            //
            // cbEasyFirmware
            //
            this.cbEasyFirmware.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEasyFirmware.FormattingEnabled = true;
            this.cbEasyFirmware.Location = new System.Drawing.Point(24, 274);
            this.cbEasyFirmware.Name = "cbEasyFirmware";
            this.cbEasyFirmware.Size = new System.Drawing.Size(296, 23);
            this.cbEasyFirmware.TabIndex = 5;
            //
            // chkEasyWifi
            //
            this.chkEasyWifi.AutoSize = true;
            this.chkEasyWifi.Location = new System.Drawing.Point(24, 314);
            this.chkEasyWifi.Name = "chkEasyWifi";
            this.chkEasyWifi.Size = new System.Drawing.Size(230, 19);
            this.chkEasyWifi.TabIndex = 6;
            this.chkEasyWifi.Text = "Set up Wi-Fi after flashing";
            this.chkEasyWifi.UseVisualStyleBackColor = true;
            this.chkEasyWifi.CheckedChanged += new System.EventHandler(this.chkEasyWifi_CheckedChanged);
            //
            // lblEasySsid
            //
            this.lblEasySsid.AutoSize = true;
            this.lblEasySsid.Location = new System.Drawing.Point(40, 340);
            this.lblEasySsid.Name = "lblEasySsid";
            this.lblEasySsid.Size = new System.Drawing.Size(100, 15);
            this.lblEasySsid.TabIndex = 7;
            this.lblEasySsid.Text = "Network name";
            //
            // txtEasySsid
            //
            this.txtEasySsid.Location = new System.Drawing.Point(42, 358);
            this.txtEasySsid.MaxLength = 32;
            this.txtEasySsid.Name = "txtEasySsid";
            this.txtEasySsid.Size = new System.Drawing.Size(278, 23);
            this.txtEasySsid.TabIndex = 8;
            //
            // lblEasyKey
            //
            this.lblEasyKey.AutoSize = true;
            this.lblEasyKey.Location = new System.Drawing.Point(40, 388);
            this.lblEasyKey.Name = "lblEasyKey";
            this.lblEasyKey.Size = new System.Drawing.Size(100, 15);
            this.lblEasyKey.TabIndex = 9;
            this.lblEasyKey.Text = "Password";
            //
            // txtEasyKey
            //
            this.txtEasyKey.Location = new System.Drawing.Point(42, 406);
            this.txtEasyKey.MaxLength = 64;
            this.txtEasyKey.Name = "txtEasyKey";
            this.txtEasyKey.Size = new System.Drawing.Size(278, 23);
            this.txtEasyKey.TabIndex = 10;
            this.txtEasyKey.UseSystemPasswordChar = true;
            //
            // cmdEasyFlash
            //
            this.cmdEasyFlash.Location = new System.Drawing.Point(24, 446);
            this.cmdEasyFlash.Name = "cmdEasyFlash";
            this.cmdEasyFlash.Size = new System.Drawing.Size(296, 44);
            this.cmdEasyFlash.TabIndex = 11;
            this.cmdEasyFlash.Text = "Install Firmware";
            this.cmdEasyFlash.Click += new System.EventHandler(this.cmdEasyFlash_Click);
            //
            // pbEasy
            //
            this.pbEasy.Location = new System.Drawing.Point(24, 498);
            this.pbEasy.Name = "pbEasy";
            this.pbEasy.Size = new System.Drawing.Size(296, 8);
            this.pbEasy.TabIndex = 12;
            this.pbEasy.Visible = false;
            //
            // lblEasyStatus
            //
            this.lblEasyStatus.Location = new System.Drawing.Point(22, 512);
            this.lblEasyStatus.Name = "lblEasyStatus";
            this.lblEasyStatus.Size = new System.Drawing.Size(298, 48);
            this.lblEasyStatus.TabIndex = 13;
            //
            // lnkEasyDetails
            //
            this.lnkEasyDetails.AutoSize = true;
            this.lnkEasyDetails.Location = new System.Drawing.Point(22, 562);
            this.lnkEasyDetails.Name = "lnkEasyDetails";
            this.lnkEasyDetails.Size = new System.Drawing.Size(160, 15);
            this.lnkEasyDetails.TabIndex = 14;
            this.lnkEasyDetails.TabStop = true;
            this.lnkEasyDetails.Text = "Show details (Advanced mode)";
            this.lnkEasyDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEasyDetails_LinkClicked);
            //
            // panelAdvanced
            //
            this.panelAdvanced.Controls.Add(this.splitAdvanced);
            this.panelAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAdvanced.Location = new System.Drawing.Point(0, 88);
            this.panelAdvanced.Name = "panelAdvanced";
            this.panelAdvanced.Size = new System.Drawing.Size(1100, 610);
            this.panelAdvanced.TabIndex = 3;
            this.panelAdvanced.Visible = false;
            //
            // splitAdvanced
            //
            this.splitAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitAdvanced.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitAdvanced.Location = new System.Drawing.Point(0, 0);
            this.splitAdvanced.Name = "splitAdvanced";
            //
            // splitAdvanced.Panel1
            //
            this.splitAdvanced.Panel1.Controls.Add(this.panelAdvLeft);
            this.splitAdvanced.Panel1MinSize = 400;
            //
            // splitAdvanced.Panel2
            //
            this.splitAdvanced.Panel2.Controls.Add(this.gbDebug);
            this.splitAdvanced.Panel2.Padding = new System.Windows.Forms.Padding(0, 8, 8, 8);
            this.splitAdvanced.Size = new System.Drawing.Size(1100, 610);
            this.splitAdvanced.SplitterDistance = 458;
            this.splitAdvanced.TabIndex = 0;
            //
            // panelAdvLeft
            //
            this.panelAdvLeft.AutoScroll = true;
            this.panelAdvLeft.Controls.Add(this.gbWifi);
            this.panelAdvLeft.Controls.Add(this.gbDeviceControl);
            this.panelAdvLeft.Controls.Add(this.gbDeviceInfo);
            this.panelAdvLeft.Controls.Add(this.gbBuild);
            this.panelAdvLeft.Controls.Add(this.gbConnection);
            this.panelAdvLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAdvLeft.Location = new System.Drawing.Point(0, 0);
            this.panelAdvLeft.Name = "panelAdvLeft";
            this.panelAdvLeft.Padding = new System.Windows.Forms.Padding(8, 4, 4, 8);
            this.panelAdvLeft.Size = new System.Drawing.Size(458, 610);
            this.panelAdvLeft.TabIndex = 0;
            //
            // gbWifi
            //
            this.gbWifi.Controls.Add(this.lblSsid);
            this.gbWifi.Controls.Add(this.txtWifiSSID);
            this.gbWifi.Controls.Add(this.lblKey);
            this.gbWifi.Controls.Add(this.txtWifiKey);
            this.gbWifi.Controls.Add(this.cmdSetWifi);
            this.gbWifi.Controls.Add(this.cmdResetWifi);
            this.gbWifi.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbWifi.Location = new System.Drawing.Point(8, 702);
            this.gbWifi.Name = "gbWifi";
            this.gbWifi.Size = new System.Drawing.Size(429, 122);
            this.gbWifi.TabIndex = 4;
            this.gbWifi.TabStop = false;
            this.gbWifi.Text = "Wi-Fi";
            //
            // lblSsid
            //
            this.lblSsid.AutoSize = true;
            this.lblSsid.Location = new System.Drawing.Point(12, 27);
            this.lblSsid.Name = "lblSsid";
            this.lblSsid.Size = new System.Drawing.Size(85, 15);
            this.lblSsid.TabIndex = 0;
            this.lblSsid.Text = "Network name";
            //
            // txtWifiSSID
            //
            this.txtWifiSSID.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szWifiSSID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtWifiSSID.Location = new System.Drawing.Point(116, 24);
            this.txtWifiSSID.MaxLength = 32;
            this.txtWifiSSID.Name = "txtWifiSSID";
            this.txtWifiSSID.Size = new System.Drawing.Size(298, 23);
            this.txtWifiSSID.TabIndex = 1;
            this.txtWifiSSID.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szWifiSSID;
            //
            // lblKey
            //
            this.lblKey.AutoSize = true;
            this.lblKey.Location = new System.Drawing.Point(12, 57);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(57, 15);
            this.lblKey.TabIndex = 2;
            this.lblKey.Text = "Password";
            //
            // txtWifiKey
            //
            this.txtWifiKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szWifiKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtWifiKey.Location = new System.Drawing.Point(116, 54);
            this.txtWifiKey.MaxLength = 64;
            this.txtWifiKey.Name = "txtWifiKey";
            this.txtWifiKey.Size = new System.Drawing.Size(298, 23);
            this.txtWifiKey.TabIndex = 3;
            this.txtWifiKey.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szWifiKey;
            this.txtWifiKey.UseSystemPasswordChar = true;
            //
            // cmdSetWifi
            //
            this.cmdSetWifi.Enabled = false;
            this.cmdSetWifi.Location = new System.Drawing.Point(116, 86);
            this.cmdSetWifi.Name = "cmdSetWifi";
            this.cmdSetWifi.Size = new System.Drawing.Size(146, 27);
            this.cmdSetWifi.TabIndex = 4;
            this.cmdSetWifi.Text = "Send Wi-Fi";
            this.cmdSetWifi.UseVisualStyleBackColor = true;
            this.cmdSetWifi.Click += new System.EventHandler(this.cmdSetWifi_Click);
            //
            // cmdResetWifi
            //
            this.cmdResetWifi.Enabled = false;
            this.cmdResetWifi.Location = new System.Drawing.Point(268, 86);
            this.cmdResetWifi.Name = "cmdResetWifi";
            this.cmdResetWifi.Size = new System.Drawing.Size(146, 27);
            this.cmdResetWifi.TabIndex = 5;
            this.cmdResetWifi.Text = "Reset Wi-Fi";
            this.cmdResetWifi.UseVisualStyleBackColor = true;
            this.cmdResetWifi.Click += new System.EventHandler(this.cmdResetWifi_Click);
            //
            // gbDeviceControl
            //
            this.gbDeviceControl.Controls.Add(this.lblLedMode);
            this.gbDeviceControl.Controls.Add(this.cmdModeOff);
            this.gbDeviceControl.Controls.Add(this.cmdModeDemo);
            this.gbDeviceControl.Controls.Add(this.cmdModeCam);
            this.gbDeviceControl.Controls.Add(this.cmdSendLedTestData);
            this.gbDeviceControl.Controls.Add(this.lblRelayId);
            this.gbDeviceControl.Controls.Add(this.txtRelayID);
            this.gbDeviceControl.Controls.Add(this.cmdSetRelay);
            this.gbDeviceControl.Controls.Add(this.cmdFactoryDefaults);
            this.gbDeviceControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDeviceControl.Location = new System.Drawing.Point(8, 562);
            this.gbDeviceControl.Name = "gbDeviceControl";
            this.gbDeviceControl.Size = new System.Drawing.Size(429, 160);
            this.gbDeviceControl.TabIndex = 3;
            this.gbDeviceControl.TabStop = false;
            this.gbDeviceControl.Text = "Device Control";
            //
            // lblLedMode
            //
            this.lblLedMode.AutoSize = true;
            this.lblLedMode.Location = new System.Drawing.Point(12, 28);
            this.lblLedMode.Name = "lblLedMode";
            this.lblLedMode.Size = new System.Drawing.Size(61, 15);
            this.lblLedMode.TabIndex = 0;
            this.lblLedMode.Text = "LED mode";
            //
            // cmdModeOff
            //
            this.cmdModeOff.Enabled = false;
            this.cmdModeOff.Location = new System.Drawing.Point(116, 23);
            this.cmdModeOff.Name = "cmdModeOff";
            this.cmdModeOff.Size = new System.Drawing.Size(96, 27);
            this.cmdModeOff.TabIndex = 1;
            this.cmdModeOff.Text = "Off";
            this.cmdModeOff.UseVisualStyleBackColor = true;
            this.cmdModeOff.Click += new System.EventHandler(this.cmdModeOff_Click);
            //
            // cmdModeDemo
            //
            this.cmdModeDemo.Enabled = false;
            this.cmdModeDemo.Location = new System.Drawing.Point(217, 23);
            this.cmdModeDemo.Name = "cmdModeDemo";
            this.cmdModeDemo.Size = new System.Drawing.Size(96, 27);
            this.cmdModeDemo.TabIndex = 2;
            this.cmdModeDemo.Text = "Demo";
            this.cmdModeDemo.UseVisualStyleBackColor = true;
            this.cmdModeDemo.Click += new System.EventHandler(this.cmdModeDemo_Click);
            //
            // cmdModeCam
            //
            this.cmdModeCam.Enabled = false;
            this.cmdModeCam.Location = new System.Drawing.Point(318, 23);
            this.cmdModeCam.Name = "cmdModeCam";
            this.cmdModeCam.Size = new System.Drawing.Size(96, 27);
            this.cmdModeCam.TabIndex = 3;
            this.cmdModeCam.Text = "Camera";
            this.cmdModeCam.UseVisualStyleBackColor = true;
            this.cmdModeCam.Click += new System.EventHandler(this.cmdModeCam_Click);
            //
            // cmdSendLedTestData
            //
            this.cmdSendLedTestData.Enabled = false;
            this.cmdSendLedTestData.Location = new System.Drawing.Point(116, 55);
            this.cmdSendLedTestData.Name = "cmdSendLedTestData";
            this.cmdSendLedTestData.Size = new System.Drawing.Size(298, 27);
            this.cmdSendLedTestData.TabIndex = 4;
            this.cmdSendLedTestData.Text = "Send LED Test Data";
            this.cmdSendLedTestData.UseVisualStyleBackColor = true;
            this.cmdSendLedTestData.Click += new System.EventHandler(this.cmdSendLedTestData_Click);
            //
            // lblRelayId
            //
            this.lblRelayId.AutoSize = true;
            this.lblRelayId.Location = new System.Drawing.Point(12, 93);
            this.lblRelayId.Name = "lblRelayId";
            this.lblRelayId.Size = new System.Drawing.Size(52, 15);
            this.lblRelayId.TabIndex = 5;
            this.lblRelayId.Text = "Relay ID";
            //
            // txtRelayID
            //
            this.txtRelayID.Location = new System.Drawing.Point(116, 90);
            this.txtRelayID.MaxLength = 32;
            this.txtRelayID.Name = "txtRelayID";
            this.txtRelayID.Size = new System.Drawing.Size(196, 23);
            this.txtRelayID.TabIndex = 6;
            //
            // cmdSetRelay
            //
            this.cmdSetRelay.Enabled = false;
            this.cmdSetRelay.Location = new System.Drawing.Point(318, 88);
            this.cmdSetRelay.Name = "cmdSetRelay";
            this.cmdSetRelay.Size = new System.Drawing.Size(96, 27);
            this.cmdSetRelay.TabIndex = 7;
            this.cmdSetRelay.Text = "Set Relay";
            this.cmdSetRelay.UseVisualStyleBackColor = true;
            this.cmdSetRelay.Click += new System.EventHandler(this.cmdSetRelay_Click);
            //
            // cmdFactoryDefaults
            //
            this.cmdFactoryDefaults.Enabled = false;
            this.cmdFactoryDefaults.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(38)))), ((int)(((byte)(30)))));
            this.cmdFactoryDefaults.Location = new System.Drawing.Point(116, 120);
            this.cmdFactoryDefaults.Name = "cmdFactoryDefaults";
            this.cmdFactoryDefaults.Size = new System.Drawing.Size(298, 27);
            this.cmdFactoryDefaults.TabIndex = 8;
            this.cmdFactoryDefaults.Text = "Reset to Factory Defaults...";
            this.cmdFactoryDefaults.UseVisualStyleBackColor = true;
            this.cmdFactoryDefaults.Click += new System.EventHandler(this.cmdFactoryDefaults_Click);
            //
            // gbDeviceInfo
            //
            this.gbDeviceInfo.Controls.Add(this.lblInfoVersion);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceVersion);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceType);
            this.gbDeviceInfo.Controls.Add(this.lblInfoId);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceID);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceMAC);
            this.gbDeviceInfo.Controls.Add(this.lblInfoName);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceName);
            this.gbDeviceInfo.Controls.Add(this.lblInfoIp);
            this.gbDeviceInfo.Controls.Add(this.txtConnectedDeviceIP);
            this.gbDeviceInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbDeviceInfo.Location = new System.Drawing.Point(8, 418);
            this.gbDeviceInfo.Name = "gbDeviceInfo";
            this.gbDeviceInfo.Size = new System.Drawing.Size(429, 144);
            this.gbDeviceInfo.TabIndex = 2;
            this.gbDeviceInfo.TabStop = false;
            this.gbDeviceInfo.Text = "Connected Device";
            //
            // lblInfoVersion
            //
            this.lblInfoVersion.AutoSize = true;
            this.lblInfoVersion.Location = new System.Drawing.Point(12, 27);
            this.lblInfoVersion.Name = "lblInfoVersion";
            this.lblInfoVersion.Size = new System.Drawing.Size(90, 15);
            this.lblInfoVersion.TabIndex = 0;
            this.lblInfoVersion.Text = "Firmware / type";
            //
            // txtConnectedDeviceVersion
            //
            this.txtConnectedDeviceVersion.Location = new System.Drawing.Point(116, 24);
            this.txtConnectedDeviceVersion.MaxLength = 32;
            this.txtConnectedDeviceVersion.Name = "txtConnectedDeviceVersion";
            this.txtConnectedDeviceVersion.ReadOnly = true;
            this.txtConnectedDeviceVersion.Size = new System.Drawing.Size(96, 23);
            this.txtConnectedDeviceVersion.TabIndex = 1;
            //
            // txtConnectedDeviceType
            //
            this.txtConnectedDeviceType.Location = new System.Drawing.Point(217, 24);
            this.txtConnectedDeviceType.MaxLength = 64;
            this.txtConnectedDeviceType.Name = "txtConnectedDeviceType";
            this.txtConnectedDeviceType.ReadOnly = true;
            this.txtConnectedDeviceType.Size = new System.Drawing.Size(197, 23);
            this.txtConnectedDeviceType.TabIndex = 2;
            //
            // lblInfoId
            //
            this.lblInfoId.AutoSize = true;
            this.lblInfoId.Location = new System.Drawing.Point(12, 56);
            this.lblInfoId.Name = "lblInfoId";
            this.lblInfoId.Size = new System.Drawing.Size(48, 15);
            this.lblInfoId.TabIndex = 3;
            this.lblInfoId.Text = "ID / MAC";
            //
            // txtConnectedDeviceID
            //
            this.txtConnectedDeviceID.Location = new System.Drawing.Point(116, 53);
            this.txtConnectedDeviceID.MaxLength = 32;
            this.txtConnectedDeviceID.Name = "txtConnectedDeviceID";
            this.txtConnectedDeviceID.ReadOnly = true;
            this.txtConnectedDeviceID.Size = new System.Drawing.Size(96, 23);
            this.txtConnectedDeviceID.TabIndex = 4;
            //
            // txtConnectedDeviceMAC
            //
            this.txtConnectedDeviceMAC.Location = new System.Drawing.Point(217, 53);
            this.txtConnectedDeviceMAC.MaxLength = 32;
            this.txtConnectedDeviceMAC.Name = "txtConnectedDeviceMAC";
            this.txtConnectedDeviceMAC.ReadOnly = true;
            this.txtConnectedDeviceMAC.Size = new System.Drawing.Size(197, 23);
            this.txtConnectedDeviceMAC.TabIndex = 5;
            //
            // lblInfoName
            //
            this.lblInfoName.AutoSize = true;
            this.lblInfoName.Location = new System.Drawing.Point(12, 85);
            this.lblInfoName.Name = "lblInfoName";
            this.lblInfoName.Size = new System.Drawing.Size(39, 15);
            this.lblInfoName.TabIndex = 6;
            this.lblInfoName.Text = "Name";
            //
            // txtConnectedDeviceName
            //
            this.txtConnectedDeviceName.Location = new System.Drawing.Point(116, 82);
            this.txtConnectedDeviceName.MaxLength = 64;
            this.txtConnectedDeviceName.Name = "txtConnectedDeviceName";
            this.txtConnectedDeviceName.ReadOnly = true;
            this.txtConnectedDeviceName.Size = new System.Drawing.Size(298, 23);
            this.txtConnectedDeviceName.TabIndex = 7;
            //
            // lblInfoIp
            //
            this.lblInfoIp.AutoSize = true;
            this.lblInfoIp.Location = new System.Drawing.Point(12, 114);
            this.lblInfoIp.Name = "lblInfoIp";
            this.lblInfoIp.Size = new System.Drawing.Size(62, 15);
            this.lblInfoIp.TabIndex = 8;
            this.lblInfoIp.Text = "IP address";
            //
            // txtConnectedDeviceIP
            //
            this.txtConnectedDeviceIP.Location = new System.Drawing.Point(116, 111);
            this.txtConnectedDeviceIP.MaxLength = 64;
            this.txtConnectedDeviceIP.Name = "txtConnectedDeviceIP";
            this.txtConnectedDeviceIP.ReadOnly = true;
            this.txtConnectedDeviceIP.Size = new System.Drawing.Size(298, 23);
            this.txtConnectedDeviceIP.TabIndex = 9;
            //
            // gbBuild
            //
            this.gbBuild.Controls.Add(this.lblDevice);
            this.gbBuild.Controls.Add(this.cbDevice);
            this.gbBuild.Controls.Add(this.lblVersion);
            this.gbBuild.Controls.Add(this.cbFirmwareVersion);
            this.gbBuild.Controls.Add(this.lblSourceVersion);
            this.gbBuild.Controls.Add(this.txtVersionCode);
            this.gbBuild.Controls.Add(this.lblBuilder);
            this.gbBuild.Controls.Add(this.cbBuilder);
            this.gbBuild.Controls.Add(this.lblBuildEnv);
            this.gbBuild.Controls.Add(this.cbBuildEnv);
            this.gbBuild.Controls.Add(this.cmdSetupTools);
            this.gbBuild.Controls.Add(this.chkEsp32Debug);
            this.gbBuild.Controls.Add(this.chkAmbiBackVerboseDebug);
            this.gbBuild.Controls.Add(this.chkAmbiBackExtraDiag);
            this.gbBuild.Controls.Add(this.cmdBuild);
            this.gbBuild.Controls.Add(this.cmdBuildAndFlash);
            this.gbBuild.Controls.Add(this.cmdFlash);
            this.gbBuild.Controls.Add(this.cmdCleanBuild);
            this.gbBuild.Controls.Add(this.cmdBuildAndUpload);
            this.gbBuild.Controls.Add(this.cmdUpload);
            this.gbBuild.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbBuild.Location = new System.Drawing.Point(8, 100);
            this.gbBuild.Name = "gbBuild";
            this.gbBuild.Size = new System.Drawing.Size(429, 318);
            this.gbBuild.TabIndex = 1;
            this.gbBuild.TabStop = false;
            this.gbBuild.Text = "Firmware";
            //
            // lblDevice
            //
            this.lblDevice.AutoSize = true;
            this.lblDevice.Location = new System.Drawing.Point(12, 33);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(42, 15);
            this.lblDevice.TabIndex = 0;
            this.lblDevice.Text = "Device";
            //
            // cbDevice
            //
            this.cbDevice.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevice.FormattingEnabled = true;
            this.cbDevice.ItemHeight = 28;
            this.cbDevice.Location = new System.Drawing.Point(116, 24);
            this.cbDevice.Name = "cbDevice";
            this.cbDevice.Size = new System.Drawing.Size(298, 34);
            this.cbDevice.TabIndex = 1;
            this.cbDevice.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbDevice_DrawItem);
            this.cbDevice.SelectedIndexChanged += new System.EventHandler(this.cbDevice_SelectedIndexChanged);
            //
            // lblVersion
            //
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(12, 70);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(76, 15);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Image";
            //
            // cbFirmwareVersion
            //
            this.cbFirmwareVersion.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szFirmwareVersion", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbFirmwareVersion.FormattingEnabled = true;
            this.cbFirmwareVersion.Location = new System.Drawing.Point(116, 67);
            this.cbFirmwareVersion.Name = "cbFirmwareVersion";
            this.cbFirmwareVersion.Size = new System.Drawing.Size(110, 23);
            this.cbFirmwareVersion.TabIndex = 3;
            this.cbFirmwareVersion.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szFirmwareVersion;
            //
            // lblSourceVersion
            //
            this.lblSourceVersion.AutoSize = true;
            this.lblSourceVersion.Location = new System.Drawing.Point(234, 70);
            this.lblSourceVersion.Name = "lblSourceVersion";
            this.lblSourceVersion.Size = new System.Drawing.Size(43, 15);
            this.lblSourceVersion.TabIndex = 4;
            this.lblSourceVersion.Text = "Source";
            //
            // txtVersionCode
            //
            this.txtVersionCode.Location = new System.Drawing.Point(304, 67);
            this.txtVersionCode.Name = "txtVersionCode";
            this.txtVersionCode.ReadOnly = true;
            this.txtVersionCode.Size = new System.Drawing.Size(110, 23);
            this.txtVersionCode.TabIndex = 5;
            //
            // lblBuilder
            //
            this.lblBuilder.AutoSize = true;
            this.lblBuilder.Location = new System.Drawing.Point(12, 102);
            this.lblBuilder.Name = "lblBuilder";
            this.lblBuilder.Size = new System.Drawing.Size(45, 15);
            this.lblBuilder.TabIndex = 6;
            this.lblBuilder.Text = "Builder";
            //
            // cbBuilder
            //
            this.cbBuilder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuilder.FormattingEnabled = true;
            this.cbBuilder.Items.AddRange(new object[] {
            "arduino-cli",
            "arduino-builder"});
            this.cbBuilder.Location = new System.Drawing.Point(116, 99);
            this.cbBuilder.Name = "cbBuilder";
            this.cbBuilder.Size = new System.Drawing.Size(150, 23);
            this.cbBuilder.TabIndex = 7;
            this.cbBuilder.SelectedIndexChanged += new System.EventHandler(this.cbBuilder_SelectedIndexChanged);
            //
            // lblBuildEnv
            //
            this.lblBuildEnv.AutoSize = true;
            this.lblBuildEnv.Location = new System.Drawing.Point(12, 134);
            this.lblBuildEnv.Name = "lblBuildEnv";
            this.lblBuildEnv.Size = new System.Drawing.Size(66, 15);
            this.lblBuildEnv.TabIndex = 8;
            this.lblBuildEnv.Text = "ESP32 core";
            //
            // cbBuildEnv
            //
            this.cbBuildEnv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuildEnv.FormattingEnabled = true;
            this.cbBuildEnv.Location = new System.Drawing.Point(116, 131);
            this.cbBuildEnv.Name = "cbBuildEnv";
            this.cbBuildEnv.Size = new System.Drawing.Size(150, 23);
            this.cbBuildEnv.TabIndex = 9;
            this.cbBuildEnv.SelectedIndexChanged += new System.EventHandler(this.cbBuildEnv_SelectedIndexChanged);
            //
            // cmdSetupTools
            //
            this.cmdSetupTools.Location = new System.Drawing.Point(272, 114);
            this.cmdSetupTools.Name = "cmdSetupTools";
            this.cmdSetupTools.Size = new System.Drawing.Size(142, 27);
            this.cmdSetupTools.TabIndex = 10;
            this.cmdSetupTools.Text = "Set Up Tools...";
            this.cmdSetupTools.UseVisualStyleBackColor = true;
            this.cmdSetupTools.Click += new System.EventHandler(this.cmdSetupTools_Click);
            //
            // chkEsp32Debug
            //
            this.chkEsp32Debug.AutoSize = true;
            this.chkEsp32Debug.Location = new System.Drawing.Point(116, 164);
            this.chkEsp32Debug.Name = "chkEsp32Debug";
            this.chkEsp32Debug.Size = new System.Drawing.Size(200, 19);
            this.chkEsp32Debug.TabIndex = 11;
            this.chkEsp32Debug.Text = "ESP32 core: verbose debug log";
            this.chkEsp32Debug.UseVisualStyleBackColor = true;
            //
            // chkAmbiBackVerboseDebug
            //
            this.chkAmbiBackVerboseDebug.AutoSize = true;
            this.chkAmbiBackVerboseDebug.Location = new System.Drawing.Point(116, 186);
            this.chkAmbiBackVerboseDebug.Name = "chkAmbiBackVerboseDebug";
            this.chkAmbiBackVerboseDebug.Size = new System.Drawing.Size(200, 19);
            this.chkAmbiBackVerboseDebug.TabIndex = 12;
            this.chkAmbiBackVerboseDebug.Text = "AmbiBack: verbose debug log";
            this.chkAmbiBackVerboseDebug.UseVisualStyleBackColor = true;
            //
            // chkAmbiBackExtraDiag
            //
            this.chkAmbiBackExtraDiag.AutoSize = true;
            this.chkAmbiBackExtraDiag.Location = new System.Drawing.Point(116, 208);
            this.chkAmbiBackExtraDiag.Name = "chkAmbiBackExtraDiag";
            this.chkAmbiBackExtraDiag.Size = new System.Drawing.Size(200, 19);
            this.chkAmbiBackExtraDiag.TabIndex = 13;
            this.chkAmbiBackExtraDiag.Text = "AmbiBack: memory diagnostics";
            this.chkAmbiBackExtraDiag.UseVisualStyleBackColor = true;
            //
            // cmdBuild
            //
            this.cmdBuild.Location = new System.Drawing.Point(12, 240);
            this.cmdBuild.Name = "cmdBuild";
            this.cmdBuild.Size = new System.Drawing.Size(130, 30);
            this.cmdBuild.TabIndex = 14;
            this.cmdBuild.Text = "Build";
            this.cmdBuild.UseVisualStyleBackColor = true;
            this.cmdBuild.Click += new System.EventHandler(this.cmdBuild_Click);
            //
            // cmdBuildAndFlash
            //
            this.cmdBuildAndFlash.Location = new System.Drawing.Point(148, 240);
            this.cmdBuildAndFlash.Name = "cmdBuildAndFlash";
            this.cmdBuildAndFlash.Size = new System.Drawing.Size(130, 30);
            this.cmdBuildAndFlash.TabIndex = 15;
            this.cmdBuildAndFlash.Text = "Build and Flash";
            this.cmdBuildAndFlash.UseVisualStyleBackColor = true;
            this.cmdBuildAndFlash.Click += new System.EventHandler(this.cmdBuildAndFlash_Click);
            //
            // cmdFlash
            //
            this.cmdFlash.Location = new System.Drawing.Point(284, 240);
            this.cmdFlash.Name = "cmdFlash";
            this.cmdFlash.Size = new System.Drawing.Size(130, 30);
            this.cmdFlash.TabIndex = 16;
            this.cmdFlash.Text = "Flash";
            this.cmdFlash.Click += new System.EventHandler(this.cmdFlash_Click);
            //
            // cmdCleanBuild
            //
            this.cmdCleanBuild.Location = new System.Drawing.Point(12, 276);
            this.cmdCleanBuild.Name = "cmdCleanBuild";
            this.cmdCleanBuild.Size = new System.Drawing.Size(130, 30);
            this.cmdCleanBuild.TabIndex = 17;
            this.cmdCleanBuild.Text = "Clean";
            this.cmdCleanBuild.UseVisualStyleBackColor = true;
            this.cmdCleanBuild.Click += new System.EventHandler(this.cmdCleanBuild_Click);
            //
            // cmdBuildAndUpload
            //
            this.cmdBuildAndUpload.Location = new System.Drawing.Point(148, 276);
            this.cmdBuildAndUpload.Name = "cmdBuildAndUpload";
            this.cmdBuildAndUpload.Size = new System.Drawing.Size(130, 30);
            this.cmdBuildAndUpload.TabIndex = 18;
            this.cmdBuildAndUpload.Text = "Build and Upload";
            this.cmdBuildAndUpload.UseVisualStyleBackColor = true;
            this.cmdBuildAndUpload.Click += new System.EventHandler(this.cmdBuildAndUpload_Click);
            //
            // cmdUpload
            //
            this.cmdUpload.Location = new System.Drawing.Point(284, 276);
            this.cmdUpload.Name = "cmdUpload";
            this.cmdUpload.Size = new System.Drawing.Size(130, 30);
            this.cmdUpload.TabIndex = 19;
            this.cmdUpload.Text = "Upload Image";
            this.cmdUpload.UseVisualStyleBackColor = true;
            this.cmdUpload.Click += new System.EventHandler(this.cmdUpload_Click);
            //
            // gbConnection
            //
            this.gbConnection.Controls.Add(this.lblComPort);
            this.gbConnection.Controls.Add(this.cbComPort);
            this.gbConnection.Controls.Add(this.lblBaudrate);
            this.gbConnection.Controls.Add(this.cbBaudrate);
            this.gbConnection.Controls.Add(this.cmdMonitor);
            this.gbConnection.Controls.Add(this.cmdGetInfo);
            this.gbConnection.Controls.Add(this.cmdReboot);
            this.gbConnection.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbConnection.Location = new System.Drawing.Point(8, 4);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Size = new System.Drawing.Size(429, 96);
            this.gbConnection.TabIndex = 0;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection";
            //
            // lblComPort
            //
            this.lblComPort.AutoSize = true;
            this.lblComPort.Location = new System.Drawing.Point(12, 27);
            this.lblComPort.Name = "lblComPort";
            this.lblComPort.Size = new System.Drawing.Size(55, 15);
            this.lblComPort.TabIndex = 0;
            this.lblComPort.Text = "USB port";
            //
            // cbComPort
            //
            this.cbComPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szComPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbComPort.FormattingEnabled = true;
            this.cbComPort.Location = new System.Drawing.Point(116, 24);
            this.cbComPort.Name = "cbComPort";
            this.cbComPort.Size = new System.Drawing.Size(110, 23);
            this.cbComPort.TabIndex = 1;
            this.cbComPort.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szComPort;
            //
            // lblBaudrate
            //
            this.lblBaudrate.AutoSize = true;
            this.lblBaudrate.Location = new System.Drawing.Point(234, 27);
            this.lblBaudrate.Name = "lblBaudrate";
            this.lblBaudrate.Size = new System.Drawing.Size(54, 15);
            this.lblBaudrate.TabIndex = 2;
            this.lblBaudrate.Text = "Baud rate";
            //
            // cbBaudrate
            //
            this.cbBaudrate.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::AmbiBack_USB_Flasher.Properties.Settings.Default, "szBautRate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbBaudrate.FormattingEnabled = true;
            this.cbBaudrate.Items.AddRange(new object[] {
            "115200",
            "921600",
            "1500000"});
            this.cbBaudrate.Location = new System.Drawing.Point(304, 24);
            this.cbBaudrate.Name = "cbBaudrate";
            this.cbBaudrate.Size = new System.Drawing.Size(110, 23);
            this.cbBaudrate.TabIndex = 3;
            this.cbBaudrate.Text = global::AmbiBack_USB_Flasher.Properties.Settings.Default.szBautRate;
            this.cbBaudrate.SelectedIndexChanged += new System.EventHandler(this.cbBaudrate_SelectedIndexChanged);
            //
            // cmdMonitor
            //
            this.cmdMonitor.Location = new System.Drawing.Point(116, 57);
            this.cmdMonitor.Name = "cmdMonitor";
            this.cmdMonitor.Size = new System.Drawing.Size(96, 28);
            this.cmdMonitor.TabIndex = 4;
            this.cmdMonitor.Text = "Connect";
            this.cmdMonitor.UseVisualStyleBackColor = true;
            this.cmdMonitor.Click += new System.EventHandler(this.cmdMonitor_Click);
            //
            // cmdGetInfo
            //
            this.cmdGetInfo.Enabled = false;
            this.cmdGetInfo.Location = new System.Drawing.Point(217, 57);
            this.cmdGetInfo.Name = "cmdGetInfo";
            this.cmdGetInfo.Size = new System.Drawing.Size(96, 28);
            this.cmdGetInfo.TabIndex = 5;
            this.cmdGetInfo.Text = "Get Info";
            this.cmdGetInfo.UseVisualStyleBackColor = true;
            this.cmdGetInfo.Click += new System.EventHandler(this.cmdGetInfo_Click);
            //
            // cmdReboot
            //
            this.cmdReboot.Enabled = false;
            this.cmdReboot.Location = new System.Drawing.Point(318, 57);
            this.cmdReboot.Name = "cmdReboot";
            this.cmdReboot.Size = new System.Drawing.Size(96, 28);
            this.cmdReboot.TabIndex = 6;
            this.cmdReboot.Text = "Restart";
            this.cmdReboot.UseVisualStyleBackColor = true;
            this.cmdReboot.Click += new System.EventHandler(this.cmdReboot_Click);
            //
            // gbDebug
            //
            this.gbDebug.Controls.Add(this.txtDebug);
            this.gbDebug.Controls.Add(this.toolStripLog);
            this.gbDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDebug.Location = new System.Drawing.Point(0, 8);
            this.gbDebug.Name = "gbDebug";
            this.gbDebug.Padding = new System.Windows.Forms.Padding(8, 4, 8, 8);
            this.gbDebug.Size = new System.Drawing.Size(630, 594);
            this.gbDebug.TabIndex = 0;
            this.gbDebug.TabStop = false;
            this.gbDebug.Text = "Output and Serial Monitor";
            //
            // txtDebug
            //
            this.txtDebug.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDebug.Location = new System.Drawing.Point(8, 45);
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.ReadOnly = true;
            this.txtDebug.Size = new System.Drawing.Size(614, 541);
            this.txtDebug.TabIndex = 1;
            this.txtDebug.Text = "";
            this.txtDebug.WordWrap = false;
            //
            // toolStripLog
            //
            this.toolStripLog.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClearLog,
            this.tsbCopyLog});
            this.toolStripLog.Location = new System.Drawing.Point(8, 20);
            this.toolStripLog.Name = "toolStripLog";
            this.toolStripLog.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripLog.Size = new System.Drawing.Size(614, 25);
            this.toolStripLog.TabIndex = 0;
            //
            // tsbClearLog
            //
            this.tsbClearLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClearLog.Name = "tsbClearLog";
            this.tsbClearLog.Text = "Clear";
            this.tsbClearLog.Click += new System.EventHandler(this.tsbClearLog_Click);
            //
            // tsbCopyLog
            //
            this.tsbCopyLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCopyLog.Name = "tsbCopyLog";
            this.tsbCopyLog.Text = "Copy All";
            this.tsbCopyLog.Click += new System.EventHandler(this.tsbCopyLog_Click);
            //
            // timerComPort
            //
            this.timerComPort.Enabled = true;
            this.timerComPort.Interval = 1000;
            this.timerComPort.Tick += new System.EventHandler(this.timerComPort_Tick);
            //
            // frmMain
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 720);
            this.Controls.Add(this.panelAdvanced);
            this.Controls.Add(this.panelEasy);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(960, 680);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AmbiBack USB Flasher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelEasy.ResumeLayout(false);
            this.panelEasyLeft.ResumeLayout(false);
            this.panelEasyCard.ResumeLayout(false);
            this.panelEasyCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEasyDevice)).EndInit();
            this.panelAdvanced.ResumeLayout(false);
            this.splitAdvanced.Panel1.ResumeLayout(false);
            this.splitAdvanced.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitAdvanced)).EndInit();
            this.splitAdvanced.ResumeLayout(false);
            this.panelAdvLeft.ResumeLayout(false);
            this.gbWifi.ResumeLayout(false);
            this.gbWifi.PerformLayout();
            this.gbDeviceControl.ResumeLayout(false);
            this.gbDeviceControl.PerformLayout();
            this.gbDeviceInfo.ResumeLayout(false);
            this.gbDeviceInfo.PerformLayout();
            this.gbBuild.ResumeLayout(false);
            this.gbBuild.PerformLayout();
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.gbDebug.ResumeLayout(false);
            this.gbDebug.PerformLayout();
            this.toolStripLog.ResumeLayout(false);
            this.toolStripLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuOpenDataFolder;
        private System.Windows.Forms.ToolStripMenuItem menuOpenFirmwareFolder;
        private System.Windows.Forms.ToolStripSeparator menuFileSep1;
        private System.Windows.Forms.ToolStripMenuItem menuQuit;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuViewEasy;
        private System.Windows.Forms.ToolStripMenuItem menuViewAdvanced;
        private System.Windows.Forms.ToolStripSeparator menuViewSep1;
        private System.Windows.Forms.ToolStripMenuItem menuClearLog;
        private System.Windows.Forms.ToolStripMenuItem menuDevice;
        private System.Windows.Forms.ToolStripMenuItem menuConnect;
        private System.Windows.Forms.ToolStripMenuItem menuGetInfo;
        private System.Windows.Forms.ToolStripMenuItem menuReboot;
        private System.Windows.Forms.ToolStripSeparator menuDeviceSep1;
        private System.Windows.Forms.ToolStripMenuItem menuSetWifi;
        private System.Windows.Forms.ToolStripMenuItem menuResetWifi;
        private System.Windows.Forms.ToolStripSeparator menuDeviceSep2;
        private System.Windows.Forms.ToolStripMenuItem menuFactoryDefaults;
        private System.Windows.Forms.ToolStripMenuItem menuFirmware;
        private System.Windows.Forms.ToolStripMenuItem menuBuild;
        private System.Windows.Forms.ToolStripMenuItem menuBuildAndFlash;
        private System.Windows.Forms.ToolStripMenuItem menuFlash;
        private System.Windows.Forms.ToolStripSeparator menuFirmwareSep1;
        private System.Windows.Forms.ToolStripMenuItem menuCleanBuild;
        private System.Windows.Forms.ToolStripMenuItem menuUpload;
        private System.Windows.Forms.ToolStripMenuItem menuBuildAndUpload;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripMenuItem menuSetupTools;
        private System.Windows.Forms.ToolStripMenuItem menuDownloadSource;
        private System.Windows.Forms.ToolStripMenuItem menuLanguage;
        private System.Windows.Forms.ToolStripMenuItem menuLangEnglish;
        private System.Windows.Forms.ToolStripMenuItem menuLangGerman;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuProjectPage;
        private System.Windows.Forms.ToolStripMenuItem menuFirmwarePage;
        private System.Windows.Forms.ToolStripSeparator menuHelpSep1;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.RadioButton rbEasy;
        private System.Windows.Forms.RadioButton rbAdvanced;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripProgressBar progressBarBuild;
        private System.Windows.Forms.ToolStripStatusLabel lblPortStatus;
        private System.Windows.Forms.Panel panelEasy;
        private System.Windows.Forms.Panel panelEasyLeft;
        private System.Windows.Forms.ListView lvDevices;
        private System.Windows.Forms.ImageList imgDevices;
        private System.Windows.Forms.Label lblEasyStep1;
        private System.Windows.Forms.Panel panelEasyCard;
        private System.Windows.Forms.PictureBox picEasyDevice;
        private System.Windows.Forms.Label lblEasyDeviceName;
        private System.Windows.Forms.Label lblEasyStep2;
        private System.Windows.Forms.ComboBox cbEasyPort;
        private System.Windows.Forms.Label lblEasyStep3;
        private System.Windows.Forms.ComboBox cbEasyFirmware;
        private System.Windows.Forms.CheckBox chkEasyWifi;
        private System.Windows.Forms.Label lblEasySsid;
        private System.Windows.Forms.TextBox txtEasySsid;
        private System.Windows.Forms.Label lblEasyKey;
        private System.Windows.Forms.TextBox txtEasyKey;
        private System.Windows.Forms.Button cmdEasyFlash;
        private System.Windows.Forms.ProgressBar pbEasy;
        private System.Windows.Forms.Label lblEasyStatus;
        private System.Windows.Forms.LinkLabel lnkEasyDetails;
        private System.Windows.Forms.Panel panelAdvanced;
        private System.Windows.Forms.SplitContainer splitAdvanced;
        private System.Windows.Forms.Panel panelAdvLeft;
        private AmbiBack_USB_Flasher.CardGroupBox gbWifi;
        private System.Windows.Forms.Label lblSsid;
        private System.Windows.Forms.TextBox txtWifiSSID;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txtWifiKey;
        private System.Windows.Forms.Button cmdSetWifi;
        private System.Windows.Forms.Button cmdResetWifi;
        private AmbiBack_USB_Flasher.CardGroupBox gbDeviceControl;
        private System.Windows.Forms.Label lblLedMode;
        private System.Windows.Forms.Button cmdModeOff;
        private System.Windows.Forms.Button cmdModeDemo;
        private System.Windows.Forms.Button cmdModeCam;
        private System.Windows.Forms.Button cmdSendLedTestData;
        private System.Windows.Forms.Label lblRelayId;
        private System.Windows.Forms.TextBox txtRelayID;
        private System.Windows.Forms.Button cmdSetRelay;
        private System.Windows.Forms.Button cmdFactoryDefaults;
        private AmbiBack_USB_Flasher.CardGroupBox gbDeviceInfo;
        private System.Windows.Forms.Label lblInfoVersion;
        private System.Windows.Forms.TextBox txtConnectedDeviceVersion;
        private System.Windows.Forms.TextBox txtConnectedDeviceType;
        private System.Windows.Forms.Label lblInfoId;
        private System.Windows.Forms.TextBox txtConnectedDeviceID;
        private System.Windows.Forms.TextBox txtConnectedDeviceMAC;
        private System.Windows.Forms.Label lblInfoName;
        private System.Windows.Forms.TextBox txtConnectedDeviceName;
        private System.Windows.Forms.Label lblInfoIp;
        private System.Windows.Forms.TextBox txtConnectedDeviceIP;
        private AmbiBack_USB_Flasher.CardGroupBox gbBuild;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.ComboBox cbDevice;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ComboBox cbFirmwareVersion;
        private System.Windows.Forms.Label lblSourceVersion;
        private System.Windows.Forms.TextBox txtVersionCode;
        private System.Windows.Forms.Label lblBuilder;
        private System.Windows.Forms.ComboBox cbBuilder;
        private System.Windows.Forms.Label lblBuildEnv;
        private System.Windows.Forms.ComboBox cbBuildEnv;
        private System.Windows.Forms.Button cmdSetupTools;
        private System.Windows.Forms.CheckBox chkEsp32Debug;
        private System.Windows.Forms.CheckBox chkAmbiBackVerboseDebug;
        private System.Windows.Forms.CheckBox chkAmbiBackExtraDiag;
        private System.Windows.Forms.Button cmdBuild;
        private System.Windows.Forms.Button cmdBuildAndFlash;
        private System.Windows.Forms.Button cmdFlash;
        private System.Windows.Forms.Button cmdCleanBuild;
        private System.Windows.Forms.Button cmdBuildAndUpload;
        private System.Windows.Forms.Button cmdUpload;
        private AmbiBack_USB_Flasher.CardGroupBox gbConnection;
        private System.Windows.Forms.Label lblComPort;
        private System.Windows.Forms.ComboBox cbComPort;
        private System.Windows.Forms.Label lblBaudrate;
        private System.Windows.Forms.ComboBox cbBaudrate;
        private System.Windows.Forms.Button cmdMonitor;
        private System.Windows.Forms.Button cmdGetInfo;
        private System.Windows.Forms.Button cmdReboot;
        private AmbiBack_USB_Flasher.CardGroupBox gbDebug;
        private System.Windows.Forms.RichTextBox txtDebug;
        private System.Windows.Forms.ToolStrip toolStripLog;
        private System.Windows.Forms.ToolStripButton tsbClearLog;
        private System.Windows.Forms.ToolStripButton tsbCopyLog;
        private System.Windows.Forms.Timer timerComPort;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
