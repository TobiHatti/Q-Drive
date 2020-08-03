namespace QDriveManager
{
    partial class QDDatabaseConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDDatabaseConnection));
            this.txbName = new System.Windows.Forms.TextBox();
            this.txbPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txbUsername = new System.Windows.Forms.TextBox();
            this.txbHostname = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txbName
            // 
            this.txbName.Location = new System.Drawing.Point(135, 248);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(357, 29);
            this.txbName.TabIndex = 4;
            // 
            // txbPassword
            // 
            this.txbPassword.Location = new System.Drawing.Point(135, 213);
            this.txbPassword.Name = "txbPassword";
            this.txbPassword.PasswordChar = '•';
            this.txbPassword.Size = new System.Drawing.Size(357, 29);
            this.txbPassword.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(9, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(446, 42);
            this.label7.TabIndex = 11;
            this.label7.Text = "Please enter the login-information for the MySQL-database you \r\nwant to use.";
            // 
            // txbUsername
            // 
            this.txbUsername.Location = new System.Drawing.Point(135, 178);
            this.txbUsername.Name = "txbUsername";
            this.txbUsername.Size = new System.Drawing.Size(357, 29);
            this.txbUsername.TabIndex = 2;
            // 
            // txbHostname
            // 
            this.txbHostname.Location = new System.Drawing.Point(135, 143);
            this.txbHostname.Name = "txbHostname";
            this.txbHostname.Size = new System.Drawing.Size(357, 29);
            this.txbHostname.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semilight", 18F);
            this.label5.Location = new System.Drawing.Point(7, 14);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(202, 32);
            this.label5.TabIndex = 15;
            this.label5.Text = "Online connection";
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(354, 283);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(138, 36);
            this.btnTestConnection.TabIndex = 5;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(55, 251);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(74, 21);
            this.label13.TabIndex = 17;
            this.label13.Text = "Database";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(55, 216);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 21);
            this.label12.TabIndex = 18;
            this.label12.Text = "Password";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(50, 181);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 21);
            this.label11.TabIndex = 19;
            this.label11.Text = "Username";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(49, 146);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 21);
            this.label10.TabIndex = 20;
            this.label10.Text = "Hostname";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 119);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(132, 21);
            this.label9.TabIndex = 21;
            this.label9.Text = "MySQL-Database:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(6, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 36);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Location = new System.Drawing.Point(397, 354);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(95, 36);
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.Text = "Update";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // QDDatabaseConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 396);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txbName);
            this.Controls.Add(this.txbPassword);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txbUsername);
            this.Controls.Add(this.txbHostname);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(32, 32);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "QDDatabaseConnection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Style.InactiveShadowOpacity = ((byte)(20));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Style.ShadowOpacity = ((byte)(30));
            this.Style.TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.Style.TitleBar.BottomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.Text = "Change online database connection";
            this.Load += new System.EventHandler(this.QDDatabaseConnection_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbName;
        private System.Windows.Forms.TextBox txbPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txbUsername;
        private System.Windows.Forms.TextBox txbHostname;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSubmit;
    }
}