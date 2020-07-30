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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDrive));
            this.pbxQDriveSplash = new System.Windows.Forms.PictureBox();
            this.pgbNetDriveProgress = new System.Windows.Forms.ProgressBar();
            this.lblVersionInfo = new System.Windows.Forms.Label();
            this.bgwNetDriveConnector = new System.ComponentModel.BackgroundWorker();
            this.lblCurrentOperation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbxQDriveSplash)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxQDriveSplash
            // 
            this.pbxQDriveSplash.BackColor = System.Drawing.Color.Transparent;
            this.pbxQDriveSplash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxQDriveSplash.Location = new System.Drawing.Point(0, 0);
            this.pbxQDriveSplash.Name = "pbxQDriveSplash";
            this.pbxQDriveSplash.Size = new System.Drawing.Size(650, 195);
            this.pbxQDriveSplash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxQDriveSplash.TabIndex = 0;
            this.pbxQDriveSplash.TabStop = false;
            // 
            // pgbNetDriveProgress
            // 
            this.pgbNetDriveProgress.BackColor = System.Drawing.Color.White;
            this.pgbNetDriveProgress.Location = new System.Drawing.Point(3, 163);
            this.pgbNetDriveProgress.Maximum = 10;
            this.pgbNetDriveProgress.Name = "pgbNetDriveProgress";
            this.pgbNetDriveProgress.Size = new System.Drawing.Size(219, 6);
            this.pgbNetDriveProgress.TabIndex = 1;
            this.pgbNetDriveProgress.Value = 5;
            // 
            // lblVersionInfo
            // 
            this.lblVersionInfo.AutoSize = true;
            this.lblVersionInfo.BackColor = System.Drawing.Color.White;
            this.lblVersionInfo.Font = new System.Drawing.Font("Calibri Light", 12F);
            this.lblVersionInfo.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblVersionInfo.Location = new System.Drawing.Point(568, 168);
            this.lblVersionInfo.Name = "lblVersionInfo";
            this.lblVersionInfo.Size = new System.Drawing.Size(41, 19);
            this.lblVersionInfo.TabIndex = 2;
            this.lblVersionInfo.Text = "1.0.0";
            // 
            // bgwNetDriveConnector
            // 
            this.bgwNetDriveConnector.WorkerReportsProgress = true;
            this.bgwNetDriveConnector.WorkerSupportsCancellation = true;
            this.bgwNetDriveConnector.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwNetDriveConnector_ProgressChanged);
            this.bgwNetDriveConnector.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwNetDriveConnector_RunWorkerCompleted);
            // 
            // lblCurrentOperation
            // 
            this.lblCurrentOperation.AutoEllipsis = true;
            this.lblCurrentOperation.BackColor = System.Drawing.Color.White;
            this.lblCurrentOperation.Font = new System.Drawing.Font("Calibri Light", 10F);
            this.lblCurrentOperation.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblCurrentOperation.Location = new System.Drawing.Point(-1, 146);
            this.lblCurrentOperation.Name = "lblCurrentOperation";
            this.lblCurrentOperation.Size = new System.Drawing.Size(223, 19);
            this.lblCurrentOperation.TabIndex = 2;
            this.lblCurrentOperation.Text = "Current progress";
            // 
            // QDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(650, 195);
            this.Controls.Add(this.lblCurrentOperation);
            this.Controls.Add(this.lblVersionInfo);
            this.Controls.Add(this.pgbNetDriveProgress);
            this.Controls.Add(this.pbxQDriveSplash);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QDrive";
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QDrive";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Gainsboro;
            ((System.ComponentModel.ISupportInitialize)(this.pbxQDriveSplash)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxQDriveSplash;
        private System.Windows.Forms.ProgressBar pgbNetDriveProgress;
        private System.Windows.Forms.Label lblVersionInfo;
        private System.ComponentModel.BackgroundWorker bgwNetDriveConnector;
        private System.Windows.Forms.Label lblCurrentOperation;
    }
}

