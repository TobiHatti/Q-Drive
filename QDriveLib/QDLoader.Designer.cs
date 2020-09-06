namespace QDriveLib
{
    partial class QDLoader
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
            this.lblWait = new System.Windows.Forms.Label();
            this.lblLoading = new System.Windows.Forms.Label();
            this.pbxLoader = new System.Windows.Forms.PictureBox();
            this.pbxBackground = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWait
            // 
            this.lblWait.AutoSize = true;
            this.lblWait.BackColor = System.Drawing.Color.Transparent;
            this.lblWait.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.lblWait.Location = new System.Drawing.Point(112, 41);
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size(98, 21);
            this.lblWait.TabIndex = 4;
            this.lblWait.Text = "Please wait...";
            // 
            // lblLoading
            // 
            this.lblLoading.AutoSize = true;
            this.lblLoading.BackColor = System.Drawing.Color.Transparent;
            this.lblLoading.Font = new System.Drawing.Font("Segoe UI Semilight", 18F);
            this.lblLoading.Location = new System.Drawing.Point(110, 9);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(113, 32);
            this.lblLoading.TabIndex = 5;
            this.lblLoading.Text = "Loading...";
            // 
            // pbxLoader
            // 
            this.pbxLoader.BackColor = System.Drawing.Color.Transparent;
            this.pbxLoader.Location = new System.Drawing.Point(2, 2);
            this.pbxLoader.Name = "pbxLoader";
            this.pbxLoader.Size = new System.Drawing.Size(80, 80);
            this.pbxLoader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxLoader.TabIndex = 3;
            this.pbxLoader.TabStop = false;
            // 
            // pbxBackground
            // 
            this.pbxBackground.Location = new System.Drawing.Point(0, 0);
            this.pbxBackground.Name = "pbxBackground";
            this.pbxBackground.Size = new System.Drawing.Size(260, 84);
            this.pbxBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxBackground.TabIndex = 6;
            this.pbxBackground.TabStop = false;
            this.pbxBackground.Paint += new System.Windows.Forms.PaintEventHandler(this.pbxBackground_Paint);
            // 
            // QDLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(260, 84);
            this.Controls.Add(this.lblWait);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.pbxLoader);
            this.Controls.Add(this.pbxBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QDLoader";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QDLoader";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxBackground)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWait;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.PictureBox pbxLoader;
        private System.Windows.Forms.PictureBox pbxBackground;
    }
}