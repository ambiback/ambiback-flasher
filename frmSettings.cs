using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AmbiBack_USB_Flasher
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        /// <summary>Grey hint text in an empty text box (shows the default that an empty field uses).</summary>
        private static void SetCue(TextBox tb, string text)
        {
            SendMessage(tb.Handle, EM_SETCUEBANNER, (IntPtr)1, text ?? "");
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            Text = L.T("settings.title");
            lblBinDir.Text = L.T("settings.binDir");
            lblSourceDir.Text = L.T("settings.sourceDir");
            lblSourceFile.Text = L.T("settings.sourceFile");
            lblExtraIncludeDir.Text = L.T("settings.libDir");
            lblStartupPath.Text = L.T("settings.rootDir");
            lblBuildDir.Text = L.T("settings.buildDir");
            foreach (var b in new[] { cmdBrowseBinDir, cmdBrowseSourceDir, cmdBrowseSourceFile, cmdBrowseExtraIncludeDir, cmdBrowseStartupPath, cmdBrowseBuildDir })
                b.Text = L.T("settings.browse");
            cmdOK.Text = L.T("settings.ok");
            cmdCancel.Text = L.T("settings.cancel");

            // Hint and "Use Defaults" button below the fields. The layout is built from the
            // (already scaled) control sizes, so it fits at any DPI and font size. The OK and
            // Cancel buttons are anchored to the bottom: resize the form first, then place them,
            // otherwise the resize moves them a second time and cuts them off.
            int gap = txtBuildDir.Left;
            var lblHint = new Label { AutoSize = true, ForeColor = Theme.SecondaryText, Text = L.T("settings.hint"), Location = new Point(gap, txtBuildDir.Bottom + gap / 2) };
            Controls.Add(lblHint);
            int buttonsTop = lblHint.Bottom + gap;
            ClientSize = new Size(ClientSize.Width, buttonsTop + cmdOK.Height + gap);
            cmdOK.Top = cmdCancel.Top = buttonsTop;
            var cmdDefaults = new Button { Text = L.T("settings.reset"), Location = new Point(gap, buttonsTop), AutoSize = true, MinimumSize = new Size(cmdOK.Width + cmdOK.Width / 2, cmdOK.Height), UseVisualStyleBackColor = true };
            cmdDefaults.Click += (s, ev) =>
            {
                txtBinDir.Text = txtSourceDir.Text = txtExtraIncludeDir.Text = txtStartupPath.Text = txtBuildDir.Text = "";
                txtSourceFile.Text = AppPaths.DefaultSourceFile;
            };
            Controls.Add(cmdDefaults);

            SetCue(txtBinDir, AppPaths.BinDir);
            SetCue(txtSourceDir, AppPaths.DefaultSourceDir);
            SetCue(txtSourceFile, AppPaths.DefaultSourceFile);
            SetCue(txtExtraIncludeDir, AppPaths.DefaultSketchbook);
            SetCue(txtStartupPath, AppPaths.DataRoot);
            SetCue(txtBuildDir, "(" + L.T("settings.rootDir") + ")");

            txtBinDir.Text = Properties.Settings.Default.szBinDir;
            txtSourceDir.Text = Properties.Settings.Default.szSourceDir;
            txtSourceFile.Text = Properties.Settings.Default.szSourceFile;
            txtExtraIncludeDir.Text = Properties.Settings.Default.szExtraIncludeDir;
            txtStartupPath.Text = Properties.Settings.Default.szStartupPath;
            txtBuildDir.Text = Properties.Settings.Default.szBuildDir;
        }

        private void cmdBrowseBinDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = L.T("settings.binDir");
                dlg.SelectedPath = txtBinDir.Text;
                if (Directory.Exists(txtBinDir.Text))
                    dlg.SelectedPath = txtBinDir.Text;
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtBinDir.Text = dlg.SelectedPath;
            }
        }

        private void cmdBrowseSourceDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = L.T("settings.sourceDir");
                if (Directory.Exists(txtSourceDir.Text))
                    dlg.SelectedPath = txtSourceDir.Text;
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtSourceDir.Text = dlg.SelectedPath;
            }
        }

        private void cmdBrowseSourceFile_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Arduino sketch (*.ino)|*.ino|All files (*.*)|*.*";
                dlg.Title = L.T("settings.sourceFile");
                if (Directory.Exists(txtSourceDir.Text))
                    dlg.InitialDirectory = txtSourceDir.Text;
                if (File.Exists(Path.Combine(txtSourceDir.Text, txtSourceFile.Text)))
                    dlg.FileName = txtSourceFile.Text;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtSourceDir.Text = Path.GetDirectoryName(dlg.FileName);
                    txtSourceFile.Text = Path.GetFileName(dlg.FileName);
                }
            }
        }

        private void cmdBrowseExtraIncludeDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = L.T("settings.libDir");
                if (Directory.Exists(txtExtraIncludeDir.Text))
                    dlg.SelectedPath = txtExtraIncludeDir.Text;
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtExtraIncludeDir.Text = dlg.SelectedPath;
            }
        }

        private void cmdBrowseStartupPath_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = L.T("settings.rootDir");
                if (Directory.Exists(txtStartupPath.Text))
                    dlg.SelectedPath = txtStartupPath.Text;
                else
                    dlg.SelectedPath = AppPaths.DataRoot;
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtStartupPath.Text = dlg.SelectedPath;
            }
        }

        private void cmdBrowseBuildDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = L.T("settings.buildDir");
                if (Directory.Exists(txtBuildDir.Text))
                    dlg.SelectedPath = txtBuildDir.Text;
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtBuildDir.Text = dlg.SelectedPath;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.szBinDir = txtBinDir.Text;
            Properties.Settings.Default.szSourceDir = txtSourceDir.Text;
            Properties.Settings.Default.szSourceFile = txtSourceFile.Text;
            Properties.Settings.Default.szExtraIncludeDir = txtExtraIncludeDir.Text;
            Properties.Settings.Default.szStartupPath = txtStartupPath.Text.Trim();
            Properties.Settings.Default.szBuildDir = txtBuildDir.Text;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
