namespace QDriveManager
{
    partial class QDriveManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDriveManager));
            this.pnlNotConfigured = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.btnRunSetup = new System.Windows.Forms.Button();
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.chbKeepLoggedIn = new System.Windows.Forms.CheckBox();
            this.lnkCreateNewAccount = new System.Windows.Forms.LinkLabel();
            this.pbxLoginLogo = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSA2ConfirmPassword = new System.Windows.Forms.Label();
            this.txbUsername = new System.Windows.Forms.TextBox();
            this.txbPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlManager = new System.Windows.Forms.Panel();
            this.btnAddDrive = new System.Windows.Forms.Button();
            this.grvConnectedDrives = new Syncfusion.Windows.Forms.Tools.GroupView();
            this.pbxLoginConnectionState = new System.Windows.Forms.PictureBox();
            this.pbxManagerConnectionState = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripEx1 = new Syncfusion.Windows.Forms.Tools.ToolStripEx();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnEditDrive = new System.Windows.Forms.Button();
            this.btnReconnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnRemoveDrive = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.pnlNotConfigured.SuspendLayout();
            this.pnlLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoginLogo)).BeginInit();
            this.pnlManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoginConnectionState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxManagerConnectionState)).BeginInit();
            this.toolStripEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlNotConfigured
            // 
            this.pnlNotConfigured.Controls.Add(this.label19);
            this.pnlNotConfigured.Controls.Add(this.label21);
            this.pnlNotConfigured.Controls.Add(this.btnRunSetup);
            this.pnlNotConfigured.Location = new System.Drawing.Point(6, 6);
            this.pnlNotConfigured.Name = "pnlNotConfigured";
            this.pnlNotConfigured.Size = new System.Drawing.Size(778, 555);
            this.pnlNotConfigured.TabIndex = 0;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ForeColor = System.Drawing.Color.DimGray;
            this.label19.Location = new System.Drawing.Point(20, 77);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(382, 126);
            this.label19.TabIndex = 4;
            this.label19.Text = "We noticed you haven\'t run the Q-Drive Setup yet. \r\n\r\nThe setup has to be complet" +
    "ed at least once to ensure \r\nthe correct functionality of the program.\r\n\r\nClick " +
    "on the button below to start the setup.";
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Segoe UI Semilight", 18F);
            this.label21.Location = new System.Drawing.Point(4, 16);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(771, 32);
            this.label21.TabIndex = 3;
            this.label21.Text = "Welcome to Q-Drive";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRunSetup
            // 
            this.btnRunSetup.Location = new System.Drawing.Point(279, 321);
            this.btnRunSetup.Name = "btnRunSetup";
            this.btnRunSetup.Size = new System.Drawing.Size(188, 42);
            this.btnRunSetup.TabIndex = 0;
            this.btnRunSetup.Text = "Run Setup";
            this.btnRunSetup.UseVisualStyleBackColor = true;
            this.btnRunSetup.Click += new System.EventHandler(this.btnRunSetup_Click);
            // 
            // pnlLogin
            // 
            this.pnlLogin.Controls.Add(this.chbKeepLoggedIn);
            this.pnlLogin.Controls.Add(this.lnkCreateNewAccount);
            this.pnlLogin.Controls.Add(this.pbxLoginConnectionState);
            this.pnlLogin.Controls.Add(this.pbxLoginLogo);
            this.pnlLogin.Controls.Add(this.label2);
            this.pnlLogin.Controls.Add(this.lblSA2ConfirmPassword);
            this.pnlLogin.Controls.Add(this.txbUsername);
            this.pnlLogin.Controls.Add(this.txbPassword);
            this.pnlLogin.Controls.Add(this.btnLogin);
            this.pnlLogin.Controls.Add(this.label1);
            this.pnlLogin.Location = new System.Drawing.Point(790, 6);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(778, 555);
            this.pnlLogin.TabIndex = 0;
            // 
            // chbKeepLoggedIn
            // 
            this.chbKeepLoggedIn.AutoSize = true;
            this.chbKeepLoggedIn.Location = new System.Drawing.Point(309, 428);
            this.chbKeepLoggedIn.Name = "chbKeepLoggedIn";
            this.chbKeepLoggedIn.Size = new System.Drawing.Size(157, 25);
            this.chbKeepLoggedIn.TabIndex = 13;
            this.chbKeepLoggedIn.Text = "Keep me logged in";
            this.chbKeepLoggedIn.UseVisualStyleBackColor = true;
            // 
            // lnkCreateNewAccount
            // 
            this.lnkCreateNewAccount.Location = new System.Drawing.Point(134, 525);
            this.lnkCreateNewAccount.Name = "lnkCreateNewAccount";
            this.lnkCreateNewAccount.Size = new System.Drawing.Size(510, 23);
            this.lnkCreateNewAccount.TabIndex = 12;
            this.lnkCreateNewAccount.TabStop = true;
            this.lnkCreateNewAccount.Text = "Account_Creation_Option";
            this.lnkCreateNewAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbxLoginLogo
            // 
            this.pbxLoginLogo.Location = new System.Drawing.Point(138, 75);
            this.pbxLoginLogo.Name = "pbxLoginLogo";
            this.pbxLoginLogo.Size = new System.Drawing.Size(506, 128);
            this.pbxLoginLogo.TabIndex = 11;
            this.pbxLoginLogo.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semilight", 15F);
            this.label2.Location = new System.Drawing.Point(165, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 28);
            this.label2.TabIndex = 10;
            this.label2.Text = "Username";
            // 
            // lblSA2ConfirmPassword
            // 
            this.lblSA2ConfirmPassword.AutoSize = true;
            this.lblSA2ConfirmPassword.Font = new System.Drawing.Font("Segoe UI Semilight", 15F);
            this.lblSA2ConfirmPassword.Location = new System.Drawing.Point(165, 326);
            this.lblSA2ConfirmPassword.Name = "lblSA2ConfirmPassword";
            this.lblSA2ConfirmPassword.Size = new System.Drawing.Size(91, 28);
            this.lblSA2ConfirmPassword.TabIndex = 10;
            this.lblSA2ConfirmPassword.Text = "Password";
            // 
            // txbUsername
            // 
            this.txbUsername.Font = new System.Drawing.Font("Segoe UI Semilight", 17F);
            this.txbUsername.Location = new System.Drawing.Point(170, 263);
            this.txbUsername.Name = "txbUsername";
            this.txbUsername.Size = new System.Drawing.Size(413, 38);
            this.txbUsername.TabIndex = 4;
            // 
            // txbPassword
            // 
            this.txbPassword.Font = new System.Drawing.Font("Segoe UI Semilight", 17F);
            this.txbPassword.Location = new System.Drawing.Point(170, 357);
            this.txbPassword.Name = "txbPassword";
            this.txbPassword.PasswordChar = '•';
            this.txbPassword.Size = new System.Drawing.Size(413, 38);
            this.txbPassword.TabIndex = 4;
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI Semilight", 15F);
            this.btnLogin.Location = new System.Drawing.Point(296, 459);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(188, 42);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI Semilight", 18F);
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(770, 32);
            this.label1.TabIndex = 3;
            this.label1.Text = "Q-Drive • Login";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlManager
            // 
            this.pnlManager.Controls.Add(this.label5);
            this.pnlManager.Controls.Add(this.btnDisconnect);
            this.pnlManager.Controls.Add(this.btnReconnect);
            this.pnlManager.Controls.Add(this.btnRemoveDrive);
            this.pnlManager.Controls.Add(this.btnEditDrive);
            this.pnlManager.Controls.Add(this.btnAddDrive);
            this.pnlManager.Controls.Add(this.pbxManagerConnectionState);
            this.pnlManager.Controls.Add(this.pictureBox2);
            this.pnlManager.Controls.Add(this.grvConnectedDrives);
            this.pnlManager.Controls.Add(this.label4);
            this.pnlManager.Controls.Add(this.label3);
            this.pnlManager.Controls.Add(this.toolStripEx1);
            this.pnlManager.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlManager.Location = new System.Drawing.Point(1574, 6);
            this.pnlManager.Name = "pnlManager";
            this.pnlManager.Size = new System.Drawing.Size(778, 555);
            this.pnlManager.TabIndex = 0;
            // 
            // btnAddDrive
            // 
            this.btnAddDrive.Location = new System.Drawing.Point(111, 103);
            this.btnAddDrive.Name = "btnAddDrive";
            this.btnAddDrive.Size = new System.Drawing.Size(200, 54);
            this.btnAddDrive.TabIndex = 6;
            this.btnAddDrive.Text = "Add Drive";
            this.btnAddDrive.UseVisualStyleBackColor = true;
            // 
            // grvConnectedDrives
            // 
            this.grvConnectedDrives.BeforeTouchSize = new System.Drawing.Size(354, 483);
            this.grvConnectedDrives.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grvConnectedDrives.Location = new System.Drawing.Point(421, 65);
            this.grvConnectedDrives.Name = "grvConnectedDrives";
            this.grvConnectedDrives.Size = new System.Drawing.Size(354, 483);
            this.grvConnectedDrives.TabIndex = 5;
            this.grvConnectedDrives.Text = "groupView1";
            // 
            // pbxLoginConnectionState
            // 
            this.pbxLoginConnectionState.Location = new System.Drawing.Point(0, 427);
            this.pbxLoginConnectionState.Name = "pbxLoginConnectionState";
            this.pbxLoginConnectionState.Size = new System.Drawing.Size(128, 128);
            this.pbxLoginConnectionState.TabIndex = 11;
            this.pbxLoginConnectionState.TabStop = false;
            // 
            // pbxManagerConnectionState
            // 
            this.pbxManagerConnectionState.Location = new System.Drawing.Point(0, 427);
            this.pbxManagerConnectionState.Name = "pbxManagerConnectionState";
            this.pbxManagerConnectionState.Size = new System.Drawing.Size(128, 128);
            this.pbxManagerConnectionState.TabIndex = 11;
            this.pbxManagerConnectionState.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semilight", 18F);
            this.label3.Location = new System.Drawing.Point(4, 31);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 32);
            this.label3.TabIndex = 3;
            this.label3.Text = "Q-Drive Manager";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripButton1.Text = "File";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton2.Text = "Stuff";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(50, 22);
            this.toolStripButton3.Text = "Banana";
            // 
            // toolStripEx1
            // 
            this.toolStripEx1.CaptionAlignment = Syncfusion.Windows.Forms.Tools.CaptionAlignment.Near;
            this.toolStripEx1.CaptionStyle = Syncfusion.Windows.Forms.Tools.CaptionStyle.Bottom;
            this.toolStripEx1.CaptionTextStyle = Syncfusion.Windows.Forms.Tools.CaptionTextStyle.Plain;
            this.toolStripEx1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.toolStripEx1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripEx1.Image = null;
            this.toolStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStripEx1.Location = new System.Drawing.Point(0, 0);
            this.toolStripEx1.Name = "toolStripEx1";
            this.toolStripEx1.Office12Mode = false;
            this.toolStripEx1.ShowCaption = false;
            this.toolStripEx1.Size = new System.Drawing.Size(778, 25);
            this.toolStripEx1.TabIndex = 13;
            this.toolStripEx1.Text = "ä";
            this.toolStripEx1.ThemeName = "Office2016White";
            this.toolStripEx1.VisualStyle = Syncfusion.Windows.Forms.Tools.ToolStripExStyle.Office2016White;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox2.Location = new System.Drawing.Point(0, 25);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(778, 3);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semilight", 14F);
            this.label4.Location = new System.Drawing.Point(416, 37);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "Connected Drives";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnEditDrive
            // 
            this.btnEditDrive.Location = new System.Drawing.Point(9, 163);
            this.btnEditDrive.Name = "btnEditDrive";
            this.btnEditDrive.Size = new System.Drawing.Size(200, 30);
            this.btnEditDrive.TabIndex = 6;
            this.btnEditDrive.Text = "Edit Drive";
            this.btnEditDrive.UseVisualStyleBackColor = true;
            // 
            // btnReconnect
            // 
            this.btnReconnect.Location = new System.Drawing.Point(215, 482);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(200, 30);
            this.btnReconnect.TabIndex = 6;
            this.btnReconnect.Text = "Update / Reconnect";
            this.btnReconnect.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(215, 518);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(200, 30);
            this.btnDisconnect.TabIndex = 6;
            this.btnDisconnect.Text = "Log Off / Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // btnRemoveDrive
            // 
            this.btnRemoveDrive.Location = new System.Drawing.Point(215, 163);
            this.btnRemoveDrive.Name = "btnRemoveDrive";
            this.btnRemoveDrive.Size = new System.Drawing.Size(200, 30);
            this.btnRemoveDrive.TabIndex = 6;
            this.btnRemoveDrive.Text = "Remove Drive";
            this.btnRemoveDrive.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(107, 238);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(204, 147);
            this.label5.TabIndex = 14;
            this.label5.Text = "Information Text Placeholder\r\n\r\nInformation Text Placeholder\r\n\r\nInformation Text " +
    "Placeholder\r\n\r\nInformation Text Placeholder";
            // 
            // QDriveManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(3797, 632);
            this.Controls.Add(this.pnlManager);
            this.Controls.Add(this.pnlLogin);
            this.Controls.Add(this.pnlNotConfigured);
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "QDriveManager";
            this.Style.InactiveShadowOpacity = ((byte)(20));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Style.ShadowOpacity = ((byte)(30));
            this.Style.TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.Style.TitleBar.BottomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(216)))), ((int)(((byte)(255)))));
            this.Text = "Q-Drive";
            this.Load += new System.EventHandler(this.QDriveManager_Load);
            this.pnlNotConfigured.ResumeLayout(false);
            this.pnlNotConfigured.PerformLayout();
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoginLogo)).EndInit();
            this.pnlManager.ResumeLayout(false);
            this.pnlManager.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxLoginConnectionState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxManagerConnectionState)).EndInit();
            this.toolStripEx1.ResumeLayout(false);
            this.toolStripEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlNotConfigured;
        private System.Windows.Forms.Button btnRunSetup;
        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txbUsername;
        private System.Windows.Forms.TextBox txbPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSA2ConfirmPassword;
        private System.Windows.Forms.LinkLabel lnkCreateNewAccount;
        private System.Windows.Forms.PictureBox pbxLoginLogo;
        private System.Windows.Forms.CheckBox chbKeepLoggedIn;
        private System.Windows.Forms.Panel pnlManager;
        private System.Windows.Forms.Button btnAddDrive;
        private Syncfusion.Windows.Forms.Tools.GroupView grvConnectedDrives;
        private System.Windows.Forms.PictureBox pbxLoginConnectionState;
        private System.Windows.Forms.PictureBox pbxManagerConnectionState;
        private System.Windows.Forms.Label label3;
        private Syncfusion.Windows.Forms.Tools.ToolStripEx toolStripEx1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.Button btnRemoveDrive;
        private System.Windows.Forms.Button btnEditDrive;
        private System.Windows.Forms.Label label5;
    }
}

