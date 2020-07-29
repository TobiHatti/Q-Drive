namespace QDriveManager
{
    partial class QDAddDriveSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDAddDriveSelector));
            this.rbnPublicDrive = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rbnPrivateAccountLinked = new System.Windows.Forms.RadioButton();
            this.rbnPrivateDeviceLinked = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbnPublicDrive
            // 
            this.rbnPublicDrive.AutoSize = true;
            this.rbnPublicDrive.Checked = true;
            this.rbnPublicDrive.Location = new System.Drawing.Point(26, 102);
            this.rbnPublicDrive.Name = "rbnPublicDrive";
            this.rbnPublicDrive.Size = new System.Drawing.Size(180, 25);
            this.rbnPublicDrive.TabIndex = 0;
            this.rbnPublicDrive.TabStop = true;
            this.rbnPublicDrive.Text = "Connect a public drive";
            this.rbnPublicDrive.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semilight", 18F);
            this.label3.Location = new System.Drawing.Point(20, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(316, 32);
            this.label3.TabIndex = 3;
            this.label3.Text = "Connect a new network drive\r\n";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbnPrivateAccountLinked
            // 
            this.rbnPrivateAccountLinked.AutoSize = true;
            this.rbnPrivateAccountLinked.Location = new System.Drawing.Point(26, 238);
            this.rbnPrivateAccountLinked.Name = "rbnPrivateAccountLinked";
            this.rbnPrivateAccountLinked.Size = new System.Drawing.Size(350, 25);
            this.rbnPrivateAccountLinked.TabIndex = 0;
            this.rbnPrivateAccountLinked.Text = "Connect a private drive (linked to your account)";
            this.rbnPrivateAccountLinked.UseVisualStyleBackColor = true;
            // 
            // rbnPrivateDeviceLinked
            // 
            this.rbnPrivateDeviceLinked.AutoSize = true;
            this.rbnPrivateDeviceLinked.Location = new System.Drawing.Point(26, 374);
            this.rbnPrivateDeviceLinked.Name = "rbnPrivateDeviceLinked";
            this.rbnPrivateDeviceLinked.Size = new System.Drawing.Size(334, 25);
            this.rbnPrivateDeviceLinked.TabIndex = 0;
            this.rbnPrivateDeviceLinked.Text = "Connect a private drive (linked to this device)";
            this.rbnPrivateDeviceLinked.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(22, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(392, 84);
            this.label5.TabIndex = 14;
            this.label5.Text = "Connect to a network drive publicly provided by your \r\ncompany or organisation.\r\n" +
    "This drive will be linked to your account, so you can \r\naccess it on any compute" +
    "r you log on with your account.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(22, 266);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(415, 84);
            this.label1.TabIndex = 14;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(22, 402);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(424, 42);
            this.label2.TabIndex = 14;
            this.label2.Text = "Connect to a private network-drive. This drive will exclusively \r\nshow up to user" +
    "s that login to q-drive on this device.";
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(361, 488);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(120, 34);
            this.btnContinue.TabIndex = 15;
            this.btnContinue.Text = "Continue >";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(6, 488);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 34);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // QDAddDriveSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 528);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rbnPrivateDeviceLinked);
            this.Controls.Add(this.rbnPrivateAccountLinked);
            this.Controls.Add(this.rbnPublicDrive);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "QDAddDriveSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Style.InactiveShadowOpacity = ((byte)(20));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Style.ShadowOpacity = ((byte)(30));
            this.Style.TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.Style.TitleBar.BottomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.Text = "Add new network drive";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbnPublicDrive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbnPrivateAccountLinked;
        private System.Windows.Forms.RadioButton rbnPrivateDeviceLinked;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnCancel;
    }
}