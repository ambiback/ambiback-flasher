using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmbiBack_USB_Flasher
{
    /// <summary>
    /// Downloads and installs arduino-cli and/or the Arduino IDE 1.8.x, an ESP32 core, the pinned libraries and the
    /// firmware source. The version lists come from GitHub; without internet access known versions are offered.
    /// </summary>
    public class frmSetup : Form
    {
        private readonly RadioButton rbCli = new RadioButton();
        private readonly RadioButton rbIde = new RadioButton();
        private readonly RadioButton rbBoth = new RadioButton();
        private readonly ComboBox cbCliVersion = new ComboBox();
        private readonly ComboBox cbIdeVersion = new ComboBox();
        private readonly ComboBox cbCoreVersion = new ComboBox();
        private readonly CheckBox chkCore = new CheckBox();
        private readonly CheckBox chkLibs = new CheckBox();
        private readonly CheckBox chkSource = new CheckBox();
        private readonly TextBox txtRef = new TextBox();
        private readonly CheckBox chkDefault = new CheckBox();
        private readonly Label lblIntro = new Label();
        private readonly LinkLabel lnkRoot = new LinkLabel();
        private readonly Label lblState = new Label();
        private readonly ProgressBar pb = new ProgressBar();
        private readonly RichTextBox txtLog = new RichTextBox();
        private readonly Button cmdInstall = new Button();
        private readonly Button cmdClose = new Button();
        private CancellationTokenSource _cts;
        private bool _running;

        public frmSetup()
        {
            Text = L.T("setup.title");
            Font = new Font("Segoe UI", 9F);
            AutoScaleMode = AutoScaleMode.Font;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(620, 600);
            BackColor = Color.White;

            int x = 20, w = 580, y = 16;
            lblIntro.SetBounds(x, y, w, 20);
            lblIntro.Text = L.T("setup.intro");
            y += 20;
            lnkRoot.SetBounds(x, y, w, 20);
            lnkRoot.Text = AppPaths.Root;
            lnkRoot.LinkClicked += (s, e) => AppPaths.OpenFolder(AppPaths.Root);
            y += 34;

            var lblBuilder = new Label { Text = L.T("setup.builder"), Font = Theme.SectionFont, AutoSize = true, Location = new Point(x - 2, y) };
            y += 28;
            rbCli.SetBounds(x, y, 260, 24); rbCli.Text = L.T("setup.cli");
            cbCliVersion.SetBounds(300, y, 140, 23); cbCliVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            y += 30;
            rbIde.SetBounds(x, y, 260, 24); rbIde.Text = L.T("setup.ide");
            cbIdeVersion.SetBounds(300, y, 140, 23); cbIdeVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            y += 30;
            rbBoth.SetBounds(x, y, 260, 24); rbBoth.Text = L.T("setup.both");
            y += 38;

            chkCore.SetBounds(x, y, 270, 24); chkCore.Text = L.T("setup.core"); chkCore.Checked = true;
            cbCoreVersion.SetBounds(300, y, 140, 23); cbCoreVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            y += 30;
            chkLibs.SetBounds(x, y, w, 24); chkLibs.Checked = true;
            chkLibs.Text = L.F("setup.libs", ToolchainInstaller.Libraries.Length);
            new ToolTip().SetToolTip(chkLibs, string.Join(Environment.NewLine, ToolchainInstaller.Libraries.Select(l => l.Repo + " " + l)));
            y += 30;
            chkSource.SetBounds(x, y, 270, 24); chkSource.Text = L.T("setup.source");
            txtRef.SetBounds(300, y, 140, 23); txtRef.Text = Properties.Settings.Default.szFirmwareRef;
            chkSource.Checked = !System.IO.Directory.Exists(System.IO.Path.Combine(AppPaths.SourceDir, ".git"));
            y += 30;
            chkDefault.SetBounds(x, y, w, 24); chkDefault.Text = L.T("setup.useDefault"); chkDefault.Checked = true;
            y += 36;

            lblState.SetBounds(x, y, w, 20);
            lblState.ForeColor = Theme.SecondaryText;
            y += 22;
            pb.SetBounds(x, y, w, 8);
            y += 16;
            txtLog.SetBounds(x, y, w, 600 - y - 60);
            txtLog.ReadOnly = true;
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.BackColor = Theme.LogBack;
            txtLog.ForeColor = Theme.LogText;
            txtLog.Font = Theme.LogFont;
            txtLog.WordWrap = false;

            cmdInstall.SetBounds(620 - 20 - 120 - 8 - 120, 600 - 46, 120, 30);
            cmdInstall.Text = L.T("setup.install");
            Theme.StylePrimary(cmdInstall);
            cmdInstall.Click += async (s, e) => await InstallAsync();
            cmdClose.SetBounds(620 - 20 - 120, 600 - 46, 120, 30);
            cmdClose.Text = L.T("setup.close");
            cmdClose.Click += (s, e) => { if (_running) _cts?.Cancel(); else Close(); };
            AcceptButton = cmdInstall;

            Controls.AddRange(new Control[] { lblIntro, lnkRoot, lblBuilder, rbCli, cbCliVersion, rbIde, cbIdeVersion, rbBoth,
                chkCore, cbCoreVersion, chkLibs, chkSource, txtRef, chkDefault, lblState, pb, txtLog, cmdInstall, cmdClose });

            bool legacy = Properties.Settings.Default.szBuilder == frmMain.BuilderLegacy;
            rbCli.Checked = !legacy;
            rbIde.Checked = legacy;
            FillCombo(cbCliVersion, ToolchainInstaller.FallbackCliVersions, Properties.Settings.Default.szCliVersion);
            FillCombo(cbIdeVersion, ToolchainInstaller.FallbackIdeVersions, Properties.Settings.Default.szIdeVersion);
            FillCombo(cbCoreVersion, ToolchainInstaller.FallbackCoreVersions, Properties.Settings.Default.szCoreVersion);

            Shown += async (s, e) => await LoadVersionsAsync();
            FormClosing += (s, e) => { if (_running) { _cts?.Cancel(); e.Cancel = true; } };
        }

        private static void FillCombo(ComboBox cb, IEnumerable<string> versions, string preferred)
        {
            cb.Items.Clear();
            foreach (string v in versions) cb.Items.Add(v);
            if (!string.IsNullOrEmpty(preferred) && cb.Items.Contains(preferred)) cb.SelectedItem = preferred;
            else if (cb.Items.Count > 0) cb.SelectedIndex = 0;
        }

        private void Log(string s, Color? c = null)
        {
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.SelectionColor = c ?? txtLog.ForeColor;
            txtLog.AppendText(s + "\r\n");
            txtLog.ScrollToCaret();
        }

        private async Task LoadVersionsAsync()
        {
            lblState.Text = L.T("setup.loading");
            pb.Style = ProgressBarStyle.Marquee;
            var errors = new List<string>();
            try { FillCombo(cbCliVersion, await ToolchainInstaller.GetCliVersionsAsync(), Properties.Settings.Default.szCliVersion); }
            catch (Exception ex) { errors.Add(ex.Message); }
            try { FillCombo(cbIdeVersion, await ToolchainInstaller.GetIdeVersionsAsync(), Properties.Settings.Default.szIdeVersion); }
            catch (Exception ex) { errors.Add(ex.Message); }
            try { FillCombo(cbCoreVersion, await ToolchainInstaller.GetCoreVersionsAsync(), Properties.Settings.Default.szCoreVersion); }
            catch (Exception ex) { errors.Add(ex.Message); }
            if (IsDisposed || _running) return;   // closed, or an installation already shows its own progress
            pb.Style = ProgressBarStyle.Continuous;
            pb.Value = 0;
            lblState.Text = errors.Count == 0 ? L.T("setup.loaded") : L.F("setup.offline", errors[0]);
        }

        private async Task InstallAsync()
        {
            if (_running) return;
            _running = true;
            _cts = new CancellationTokenSource();
            cmdInstall.Enabled = false;
            cmdClose.Text = L.T("setup.cancel");
            lblState.Text = L.T("setup.running");
            bool cli = rbCli.Checked || rbBoth.Checked;
            bool ide = rbIde.Checked || rbBoth.Checked;
            string cliVer = cbCliVersion.Text, ideVer = cbIdeVersion.Text, coreVer = cbCoreVersion.Text;

            var inst = new ToolchainInstaller(s => Log(s), (pct, s) =>
            {
                lblState.Text = s;
                pb.Style = pct < 0 ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
                if (pct >= 0) pb.Value = Math.Min(100, pct);
            }, _cts.Token);

            try
            {
                var s = Properties.Settings.Default;
                if (cli)
                {
                    string exe = await inst.InstallArduinoCliAsync(cliVer);
                    if (chkDefault.Checked) s.szCliVersion = cliVer;
                    if (chkCore.Checked)
                    {
                        await inst.InstallCoreForCliAsync(exe, coreVer);
                        if (chkDefault.Checked) s.szCoreVersion = coreVer;
                    }
                }
                if (ide)
                {
                    await inst.InstallArduinoIdeAsync(ideVer);
                    if (chkDefault.Checked) { s.szIdeVersion = ideVer; s.szBinDir = ""; }
                    if (chkCore.Checked)
                    {
                        await inst.InstallCoreForBuilderAsync(coreVer);
                        if (chkDefault.Checked) s.szBuildEnv = coreVer;
                    }
                }
                if (chkDefault.Checked) s.szBuilder = ide && !cli ? frmMain.BuilderLegacy : frmMain.BuilderCli;
                if (chkLibs.Checked)
                {
                    await inst.InstallLibrariesAsync(AppPaths.DefaultSketchbook);
                    if (chkDefault.Checked) s.szExtraIncludeDir = "";
                }
                if (chkSource.Checked)
                {
                    s.szFirmwareRef = string.IsNullOrWhiteSpace(txtRef.Text) ? "main" : txtRef.Text.Trim();
                    // Always into the default source folder; a custom folder (or git checkout) is never overwritten
                    await inst.DownloadFirmwareSourceAsync(s.szFirmwareRef, AppPaths.DefaultSourceDir);
                    if (chkDefault.Checked) s.szSourceDir = "";
                }
                s.Save();
                await Task.Delay(100);   // let queued log lines arrive
                lblState.Text = L.T("setup.done");
                Log(L.T("setup.done"), Theme.LogSuccess);
            }
            catch (OperationCanceledException)
            {
                lblState.Text = L.T("setup.canceled");
                Log(L.T("setup.canceled"), Theme.LogError);
            }
            catch (Exception ex)
            {
                lblState.Text = L.F("setup.failed", ex.Message);
                Log(ex.ToString(), Theme.LogError);
            }
            finally
            {
                pb.Style = ProgressBarStyle.Continuous;
                pb.Value = 0;
                _running = false;
                cmdInstall.Enabled = true;
                cmdClose.Text = L.T("setup.close");
            }
        }
    }
}
