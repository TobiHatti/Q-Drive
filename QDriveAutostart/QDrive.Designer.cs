namespace QDriveAutostart
{
    partial class QDrive
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDrive));
            this.lblVersionInfo = new System.Windows.Forms.Label();
            this.nfiQDriveMenu = new System.Windows.Forms.NotifyIcon(this.components);
            this.tmrDriveCheck = new System.Windows.Forms.Timer(this.components);
            this.pbxQDriveSplash = new System.Windows.Forms.PictureBox();
            this.tmrFade = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbxQDriveSplash)).BeginInit();
            this.SuspendLayout();
            // 
            // lblVersionInfo
            // 
            this.lblVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersionInfo.AutoSize = true;
            this.lblVersionInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(204)))), ((int)(((byte)(242)))));
            this.lblVersionInfo.Font = new System.Drawing.Font("Calibri Light", 13F);
            this.lblVersionInfo.ForeColor = System.Drawing.Color.White;
            this.lblVersionInfo.Location = new System.Drawing.Point(221, 136);
            this.lblVersionInfo.Name = "lblVersionInfo";
            this.lblVersionInfo.Size = new System.Drawing.Size(58, 22);
            this.lblVersionInfo.TabIndex = 2;
            this.lblVersionInfo.Text = "9.9.9.9";
            // 
            // nfiQDriveMenu
            // 
            this.nfiQDriveMenu.Icon = ((System.Drawing.Icon)(resources.GetObject("nfiQDriveMenu.Icon")));
            this.nfiQDriveMenu.Text = "Q-Drive";
            this.nfiQDriveMenu.Visible = true;
            this.nfiQDriveMenu.DoubleClick += new System.EventHandler(this.nfiQDriveMenu_DoubleClick);
            // 
            // tmrDriveCheck
            // 
            this.tmrDriveCheck.Enabled = true;
            this.tmrDriveCheck.Interval = 60000;
            this.tmrDriveCheck.Tick += new System.EventHandler(this.tmrDriveCheck_Tick);
            // 
            // pbxQDriveSplash
            // 
            this.pbxQDriveSplash.BackColor = System.Drawing.Color.Transparent;
            this.pbxQDriveSplash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxQDriveSplash.Image = global::QDriveAutostart.Properties.Resources.QDSplashBase;
            this.pbxQDriveSplash.InitialImage = global::QDriveAutostart.Properties.Resources.QDSplashBase;
            this.pbxQDriveSplash.Location = new System.Drawing.Point(0, 0);
            this.pbxQDriveSplash.Name = "pbxQDriveSplash";
            this.pbxQDriveSplash.Size = new System.Drawing.Size(300, 160);
            this.pbxQDriveSplash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxQDriveSplash.TabIndex = 0;
            this.pbxQDriveSplash.TabStop = false;
            // 
            // tmrFade
            // 
            this.tmrFade.Enabled = true;
            this.tmrFade.Interval = 1;
            this.tmrFade.Tick += new System.EventHandler(this.tmrFade_Tick);
            // 
            // QDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Magenta;
            this.ClientSize = new System.Drawing.Size(300, 160);
            this.Controls.Add(this.lblVersionInfo);
            this.Controls.Add(this.pbxQDriveSplash);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QDrive";
            this.Opacity = 0D;
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QDrive";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QDrive_FormClosing);
            this.Load += new System.EventHandler(this.QDrive_Load);
            this.Shown += new System.EventHandler(this.QDrive_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbxQDriveSplash)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxQDriveSplash;
        private System.Windows.Forms.Label lblVersionInfo;
        private System.Windows.Forms.NotifyIcon nfiQDriveMenu;
        private System.Windows.Forms.Timer tmrDriveCheck;
        private System.Windows.Forms.Timer tmrFade;
    }
}

