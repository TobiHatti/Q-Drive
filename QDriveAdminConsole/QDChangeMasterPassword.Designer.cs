namespace QDriveAdminConsole
{
    partial class QDChangeMasterPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDChangeMasterPassword));
            this.lblRegisterTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txbConfirmNewPassword = new System.Windows.Forms.TextBox();
            this.txbNewPassword = new System.Windows.Forms.TextBox();
            this.lblSA2Password = new System.Windows.Forms.Label();
            this.txbOldPassword = new System.Windows.Forms.TextBox();
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
            this.lblRegisterTitle.Size = new System.Drawing.Size(277, 32);
            this.lblRegisterTitle.TabIndex = 17;
            this.lblRegisterTitle.Text = "Change Master-Password";
            this.lblRegisterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(76, 260);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 34);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Location = new System.Drawing.Point(202, 260);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(150, 34);
            this.btnSubmit.TabIndex = 4;
            this.btnSubmit.Text = "Change Password";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 21);
            this.label2.TabIndex = 29;
            this.label2.Text = "Confirm New Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 21);
            this.label1.TabIndex = 30;
            this.label1.Text = "New Password:";
            // 
            // txbConfirmNewPassword
            // 
            this.txbConfirmNewPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbConfirmNewPassword.Location = new System.Drawing.Point(6, 214);
            this.txbConfirmNewPassword.Name = "txbConfirmNewPassword";
            this.txbConfirmNewPassword.PasswordChar = '•';
            this.txbConfirmNewPassword.Size = new System.Drawing.Size(346, 29);
            this.txbConfirmNewPassword.TabIndex = 3;
            this.txbConfirmNewPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitForm);
            // 
            // txbNewPassword
            // 
            this.txbNewPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbNewPassword.Location = new System.Drawing.Point(6, 158);
            this.txbNewPassword.Name = "txbNewPassword";
            this.txbNewPassword.PasswordChar = '•';
            this.txbNewPassword.Size = new System.Drawing.Size(346, 29);
            this.txbNewPassword.TabIndex = 2;
            this.txbNewPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitForm);
            // 
            // lblSA2Password
            // 
            this.lblSA2Password.AutoSize = true;
            this.lblSA2Password.ForeColor = System.Drawing.Color.White;
            this.lblSA2Password.Location = new System.Drawing.Point(6, 59);
            this.lblSA2Password.Name = "lblSA2Password";
            this.lblSA2Password.Size = new System.Drawing.Size(107, 21);
            this.lblSA2Password.TabIndex = 28;
            this.lblSA2Password.Text = "Old Password:";
            // 
            // txbOldPassword
            // 
            this.txbOldPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbOldPassword.Location = new System.Drawing.Point(6, 83);
            this.txbOldPassword.Name = "txbOldPassword";
            this.txbOldPassword.PasswordChar = '•';
            this.txbOldPassword.Size = new System.Drawing.Size(346, 29);
            this.txbOldPassword.TabIndex = 1;
            this.txbOldPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitForm);
            // 
            // QDChangeMasterPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(358, 300);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txbConfirmNewPassword);
            this.Controls.Add(this.txbNewPassword);
            this.Controls.Add(this.lblSA2Password);
            this.Controls.Add(this.txbOldPassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lblRegisterTitle);
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(32, 32);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "QDChangeMasterPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Style.InactiveShadowOpacity = ((byte)(20));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Style.ShadowOpacity = ((byte)(30));
            this.Style.TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.Style.TitleBar.BottomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.Text = "Change Master Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRegisterTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbConfirmNewPassword;
        private System.Windows.Forms.TextBox txbNewPassword;
        private System.Windows.Forms.Label lblSA2Password;
        private System.Windows.Forms.TextBox txbOldPassword;
    }
}