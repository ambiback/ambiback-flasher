using System;
using System.IO;
using System.Windows.Forms;

namespace AmbiBack_USB_Flasher
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
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
                dlg.Description = "Select Arduino installation directory";
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
                dlg.Description = "Select source directory (AmbiBack.Controller)";
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
                dlg.Title = "Select source file";
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
                dlg.Description = "Select extra include directory";
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
                dlg.Description = "Select app root directory (firmware, buildenv, buildtools, esptool.exe). Leave empty to use application directory.";
                if (Directory.Exists(txtStartupPath.Text))
                    dlg.SelectedPath = txtStartupPath.Text;
                else
                    dlg.SelectedPath = Application.StartupPath;
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtStartupPath.Text = dlg.SelectedPath;
            }
        }

        private void cmdBrowseBuildDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select build output directory (_build and _cache subdirs)";
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
