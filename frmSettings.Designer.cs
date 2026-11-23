namespace AmbiBack_USB_Flasher
{
    partial class frmSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblBinDir = new System.Windows.Forms.Label();
            this.txtBinDir = new System.Windows.Forms.TextBox();
            this.cmdBrowseBinDir = new System.Windows.Forms.Button();
            this.lblSourceDir = new System.Windows.Forms.Label();
            this.txtSourceDir = new System.Windows.Forms.TextBox();
            this.cmdBrowseSourceDir = new System.Windows.Forms.Button();
            this.lblSourceFile = new System.Windows.Forms.Label();
            this.txtSourceFile = new System.Windows.Forms.TextBox();
            this.cmdBrowseSourceFile = new System.Windows.Forms.Button();
            this.lblExtraIncludeDir = new System.Windows.Forms.Label();
            this.txtExtraIncludeDir = new System.Windows.Forms.TextBox();
            this.cmdBrowseExtraIncludeDir = new System.Windows.Forms.Button();
            this.lblStartupPath = new System.Windows.Forms.Label();
            this.txtStartupPath = new System.Windows.Forms.TextBox();
            this.cmdBrowseStartupPath = new System.Windows.Forms.Button();
            this.lblBuildDir = new System.Windows.Forms.Label();
            this.txtBuildDir = new System.Windows.Forms.TextBox();
            this.cmdBrowseBuildDir = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblBinDir
            //
            this.lblBinDir.AutoSize = true;
            this.lblBinDir.Location = new System.Drawing.Point(12, 15);
            this.lblBinDir.Name = "lblBinDir";
            this.lblBinDir.Size = new System.Drawing.Size(100, 13);
            this.lblBinDir.TabIndex = 0;
            this.lblBinDir.Text = "Arduino Bin Directory";
            //
            // txtBinDir
            //
            this.txtBinDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBinDir.Location = new System.Drawing.Point(12, 31);
            this.txtBinDir.Name = "txtBinDir";
            this.txtBinDir.Size = new System.Drawing.Size(380, 20);
            this.txtBinDir.TabIndex = 1;
            //
            // cmdBrowseBinDir
            //
            this.cmdBrowseBinDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseBinDir.Location = new System.Drawing.Point(398, 29);
            this.cmdBrowseBinDir.Name = "cmdBrowseBinDir";
            this.cmdBrowseBinDir.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseBinDir.TabIndex = 2;
            this.cmdBrowseBinDir.Text = "Browse...";
            this.cmdBrowseBinDir.UseVisualStyleBackColor = true;
            this.cmdBrowseBinDir.Click += new System.EventHandler(this.cmdBrowseBinDir_Click);
            //
            // lblSourceDir
            //
            this.lblSourceDir.AutoSize = true;
            this.lblSourceDir.Location = new System.Drawing.Point(12, 58);
            this.lblSourceDir.Name = "lblSourceDir";
            this.lblSourceDir.Size = new System.Drawing.Size(76, 13);
            this.lblSourceDir.TabIndex = 3;
            this.lblSourceDir.Text = "Source Directory";
            //
            // txtSourceDir
            //
            this.txtSourceDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceDir.Location = new System.Drawing.Point(12, 74);
            this.txtSourceDir.Name = "txtSourceDir";
            this.txtSourceDir.Size = new System.Drawing.Size(380, 20);
            this.txtSourceDir.TabIndex = 4;
            //
            // cmdBrowseSourceDir
            //
            this.cmdBrowseSourceDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseSourceDir.Location = new System.Drawing.Point(398, 72);
            this.cmdBrowseSourceDir.Name = "cmdBrowseSourceDir";
            this.cmdBrowseSourceDir.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseSourceDir.TabIndex = 5;
            this.cmdBrowseSourceDir.Text = "Browse...";
            this.cmdBrowseSourceDir.UseVisualStyleBackColor = true;
            this.cmdBrowseSourceDir.Click += new System.EventHandler(this.cmdBrowseSourceDir_Click);
            //
            // lblSourceFile
            //
            this.lblSourceFile.AutoSize = true;
            this.lblSourceFile.Location = new System.Drawing.Point(12, 101);
            this.lblSourceFile.Name = "lblSourceFile";
            this.lblSourceFile.Size = new System.Drawing.Size(60, 13);
            this.lblSourceFile.TabIndex = 6;
            this.lblSourceFile.Text = "Source File";
            //
            // txtSourceFile
            //
            this.txtSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceFile.Location = new System.Drawing.Point(12, 117);
            this.txtSourceFile.Name = "txtSourceFile";
            this.txtSourceFile.Size = new System.Drawing.Size(380, 20);
            this.txtSourceFile.TabIndex = 7;
            //
            // cmdBrowseSourceFile
            //
            this.cmdBrowseSourceFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseSourceFile.Location = new System.Drawing.Point(398, 115);
            this.cmdBrowseSourceFile.Name = "cmdBrowseSourceFile";
            this.cmdBrowseSourceFile.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseSourceFile.TabIndex = 8;
            this.cmdBrowseSourceFile.Text = "Browse...";
            this.cmdBrowseSourceFile.UseVisualStyleBackColor = true;
            this.cmdBrowseSourceFile.Click += new System.EventHandler(this.cmdBrowseSourceFile_Click);
            //
            // lblExtraIncludeDir
            //
            this.lblExtraIncludeDir.AutoSize = true;
            this.lblExtraIncludeDir.Location = new System.Drawing.Point(12, 144);
            this.lblExtraIncludeDir.Name = "lblExtraIncludeDir";
            this.lblExtraIncludeDir.Size = new System.Drawing.Size(98, 13);
            this.lblExtraIncludeDir.TabIndex = 9;
            this.lblExtraIncludeDir.Text = "Extra Include Directory";
            //
            // txtExtraIncludeDir
            //
            this.txtExtraIncludeDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExtraIncludeDir.Location = new System.Drawing.Point(12, 160);
            this.txtExtraIncludeDir.Name = "txtExtraIncludeDir";
            this.txtExtraIncludeDir.Size = new System.Drawing.Size(380, 20);
            this.txtExtraIncludeDir.TabIndex = 10;
            //
            // cmdBrowseExtraIncludeDir
            //
            this.cmdBrowseExtraIncludeDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseExtraIncludeDir.Location = new System.Drawing.Point(398, 158);
            this.cmdBrowseExtraIncludeDir.Name = "cmdBrowseExtraIncludeDir";
            this.cmdBrowseExtraIncludeDir.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseExtraIncludeDir.TabIndex = 11;
            this.cmdBrowseExtraIncludeDir.Text = "Browse...";
            this.cmdBrowseExtraIncludeDir.UseVisualStyleBackColor = true;
            this.cmdBrowseExtraIncludeDir.Click += new System.EventHandler(this.cmdBrowseExtraIncludeDir_Click);
            //
            // lblStartupPath
            //
            this.lblStartupPath.AutoSize = true;
            this.lblStartupPath.Location = new System.Drawing.Point(12, 187);
            this.lblStartupPath.Name = "lblStartupPath";
            this.lblStartupPath.Size = new System.Drawing.Size(180, 13);
            this.lblStartupPath.TabIndex = 12;
            this.lblStartupPath.Text = "App Root Path (empty = exe directory)";
            //
            // txtStartupPath
            //
            this.txtStartupPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStartupPath.Location = new System.Drawing.Point(12, 203);
            this.txtStartupPath.Name = "txtStartupPath";
            this.txtStartupPath.Size = new System.Drawing.Size(380, 20);
            this.txtStartupPath.TabIndex = 13;
            //
            // cmdBrowseStartupPath
            //
            this.cmdBrowseStartupPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseStartupPath.Location = new System.Drawing.Point(398, 201);
            this.cmdBrowseStartupPath.Name = "cmdBrowseStartupPath";
            this.cmdBrowseStartupPath.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseStartupPath.TabIndex = 14;
            this.cmdBrowseStartupPath.Text = "Browse...";
            this.cmdBrowseStartupPath.UseVisualStyleBackColor = true;
            this.cmdBrowseStartupPath.Click += new System.EventHandler(this.cmdBrowseStartupPath_Click);
            //
            // lblBuildDir
            //
            this.lblBuildDir.AutoSize = true;
            this.lblBuildDir.Location = new System.Drawing.Point(12, 230);
            this.lblBuildDir.Name = "lblBuildDir";
            this.lblBuildDir.Size = new System.Drawing.Size(52, 13);
            this.lblBuildDir.TabIndex = 15;
            this.lblBuildDir.Text = "Build Dir";
            //
            // txtBuildDir
            //
            this.txtBuildDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBuildDir.Location = new System.Drawing.Point(12, 246);
            this.txtBuildDir.Name = "txtBuildDir";
            this.txtBuildDir.Size = new System.Drawing.Size(380, 20);
            this.txtBuildDir.TabIndex = 16;
            //
            // cmdBrowseBuildDir
            //
            this.cmdBrowseBuildDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseBuildDir.Location = new System.Drawing.Point(398, 244);
            this.cmdBrowseBuildDir.Name = "cmdBrowseBuildDir";
            this.cmdBrowseBuildDir.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseBuildDir.TabIndex = 17;
            this.cmdBrowseBuildDir.Text = "Browse...";
            this.cmdBrowseBuildDir.UseVisualStyleBackColor = true;
            this.cmdBrowseBuildDir.Click += new System.EventHandler(this.cmdBrowseBuildDir_Click);
            //
            // cmdOK
            //
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(317, 280);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 14;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            //
            // cmdCancel
            //
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(398, 280);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 15;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            //
            // frmSettings
            //
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(485, 315);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdBrowseBuildDir);
            this.Controls.Add(this.txtBuildDir);
            this.Controls.Add(this.lblBuildDir);
            this.Controls.Add(this.cmdBrowseStartupPath);
            this.Controls.Add(this.txtStartupPath);
            this.Controls.Add(this.lblStartupPath);
            this.Controls.Add(this.cmdBrowseExtraIncludeDir);
            this.Controls.Add(this.txtExtraIncludeDir);
            this.Controls.Add(this.lblExtraIncludeDir);
            this.Controls.Add(this.cmdBrowseSourceFile);
            this.Controls.Add(this.txtSourceFile);
            this.Controls.Add(this.lblSourceFile);
            this.Controls.Add(this.cmdBrowseSourceDir);
            this.Controls.Add(this.txtSourceDir);
            this.Controls.Add(this.lblSourceDir);
            this.Controls.Add(this.cmdBrowseBinDir);
            this.Controls.Add(this.txtBinDir);
            this.Controls.Add(this.lblBinDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblBinDir;
        private System.Windows.Forms.TextBox txtBinDir;
        private System.Windows.Forms.Button cmdBrowseBinDir;
        private System.Windows.Forms.Label lblSourceDir;
        private System.Windows.Forms.TextBox txtSourceDir;
        private System.Windows.Forms.Button cmdBrowseSourceDir;
        private System.Windows.Forms.Label lblSourceFile;
        private System.Windows.Forms.TextBox txtSourceFile;
        private System.Windows.Forms.Button cmdBrowseSourceFile;
        private System.Windows.Forms.Label lblExtraIncludeDir;
        private System.Windows.Forms.TextBox txtExtraIncludeDir;
        private System.Windows.Forms.Button cmdBrowseExtraIncludeDir;
        private System.Windows.Forms.Label lblStartupPath;
        private System.Windows.Forms.TextBox txtStartupPath;
        private System.Windows.Forms.Button cmdBrowseStartupPath;
        private System.Windows.Forms.Label lblBuildDir;
        private System.Windows.Forms.TextBox txtBuildDir;
        private System.Windows.Forms.Button cmdBrowseBuildDir;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
    }
}
