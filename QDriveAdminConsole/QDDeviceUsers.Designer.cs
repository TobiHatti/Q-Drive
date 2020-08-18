namespace QDriveAdminConsole
{
    partial class QDDeviceUsers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDDeviceUsers));
            this.lblRegisterTitle = new System.Windows.Forms.Label();
            this.lbxDeviceUsers = new System.Windows.Forms.ListBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnActionLogDevice = new System.Windows.Forms.Button();
            this.btnActionLogUser = new System.Windows.Forms.Button();
            this.lblDeviceInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblRegisterTitle
            // 
            this.lblRegisterTitle.AutoSize = true;
            this.lblRegisterTitle.Font = new System.Drawing.Font("Segoe UI Semilight", 18F);
            this.lblRegisterTitle.ForeColor = System.Drawing.Color.White;
            this.lblRegisterTitle.Location = new System.Drawing.Point(7, 3);
            this.lblRegisterTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRegisterTitle.Name = "lblRegisterTitle";
            this.lblRegisterTitle.Size = new System.Drawing.Size(143, 32);
            this.lblRegisterTitle.TabIndex = 18;
            this.lblRegisterTitle.Text = "Device users";
            this.lblRegisterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbxDeviceUsers
            // 
            this.lbxDeviceUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxDeviceUsers.FormattingEnabled = true;
            this.lbxDeviceUsers.ItemHeight = 21;
            this.lbxDeviceUsers.Location = new System.Drawing.Point(5, 80);
            this.lbxDeviceUsers.Name = "lbxDeviceUsers";
            this.lbxDeviceUsers.Size = new System.Drawing.Size(578, 235);
            this.lbxDeviceUsers.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(5, 322);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(79, 34);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnActionLogDevice
            // 
            this.btnActionLogDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActionLogDevice.Location = new System.Drawing.Point(429, 322);
            this.btnActionLogDevice.Name = "btnActionLogDevice";
            this.btnActionLogDevice.Size = new System.Drawing.Size(154, 34);
            this.btnActionLogDevice.TabIndex = 2;
            this.btnActionLogDevice.Text = "Action log (Device)";
            this.btnActionLogDevice.UseVisualStyleBackColor = true;
            this.btnActionLogDevice.Click += new System.EventHandler(this.btnActionLogDevice_Click);
            // 
            // btnActionLogUser
            // 
            this.btnActionLogUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActionLogUser.Location = new System.Drawing.Point(269, 322);
            this.btnActionLogUser.Name = "btnActionLogUser";
            this.btnActionLogUser.Size = new System.Drawing.Size(154, 34);
            this.btnActionLogUser.TabIndex = 3;
            this.btnActionLogUser.Text = "Action log (User)";
            this.btnActionLogUser.UseVisualStyleBackColor = true;
            this.btnActionLogUser.Click += new System.EventHandler(this.btnActionLogUser_Click);
            // 
            // lblDeviceInfo
            // 
            this.lblDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDeviceInfo.ForeColor = System.Drawing.Color.White;
            this.lblDeviceInfo.Location = new System.Drawing.Point(149, 14);
            this.lblDeviceInfo.Name = "lblDeviceInfo";
            this.lblDeviceInfo.Size = new System.Drawing.Size(434, 21);
            this.lblDeviceInfo.TabIndex = 28;
            this.lblDeviceInfo.Text = "Device Info";
            this.lblDeviceInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(1, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(582, 21);
            this.label1.TabIndex = 29;
            this.label1.Text = "Users which have logged in to Q-Drive at least once on this device\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // QDDeviceUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(588, 361);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDeviceInfo);
            this.Controls.Add(this.btnActionLogUser);
            this.Controls.Add(this.btnActionLogDevice);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbxDeviceUsers);
            this.Controls.Add(this.lblRegisterTitle);
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(32, 32);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "QDDeviceUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Style.InactiveShadowOpacity = ((byte)(20));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Style.ShadowOpacity = ((byte)(30));
            this.Style.TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            this.Style.TitleBar.BottomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            this.Style.TitleBar.ForeColor = System.Drawing.Color.White;
            this.Text = "Device Users";
            this.Load += new System.EventHandler(this.QDDeviceUsers_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRegisterTitle;
        private System.Windows.Forms.ListBox lbxDeviceUsers;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnActionLogDevice;
        private System.Windows.Forms.Button btnActionLogUser;
        private System.Windows.Forms.Label lblDeviceInfo;
        private System.Windows.Forms.Label label1;
    }
}