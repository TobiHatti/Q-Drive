namespace QDriveAdminConsole
{
    partial class QDAddPublicDrive
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDAddPublicDrive));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.cbxDriveLetter = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.lblAddDriveTitle = new System.Windows.Forms.Label();
            this.txbDisplayName = new System.Windows.Forms.TextBox();
            this.txbDrivePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(287, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 34);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Location = new System.Drawing.Point(413, 230);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(120, 34);
            this.btnSubmit.TabIndex = 5;
            this.btnSubmit.Text = "Add Drive";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // cbxDriveLetter
            // 
            this.cbxDriveLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDriveLetter.FormattingEnabled = true;
            this.cbxDriveLetter.Items.AddRange(new object[] {
            "A:\\",
            "B:\\",
            "D:\\",
            "E:\\",
            "F:\\",
            "G:\\",
            "H:\\",
            "I:\\",
            "J:\\",
            "K:\\",
            "L:\\",
            "M:\\",
            "N:\\",
            "O:\\",
            "P:\\",
            "Q:\\",
            "R:\\",
            "S:\\",
            "T:\\",
            "U:\\",
            "V:\\",
            "W:\\",
            "X:\\",
            "Y:\\",
            "Z:\\"});
            this.cbxDriveLetter.Location = new System.Drawing.Point(167, 179);
            this.cbxDriveLetter.Name = "cbxDriveLetter";
            this.cbxDriveLetter.Size = new System.Drawing.Size(102, 29);
            this.cbxDriveLetter.TabIndex = 3;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Segoe UI Semilight", 10F);
            this.label23.ForeColor = System.Drawing.Color.DimGray;
            this.label23.Location = new System.Drawing.Point(109, 97);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(122, 19);
            this.label23.TabIndex = 12;
            this.label23.Text = "e.g. \\\\Server\\Share";
            // 
            // lblAddDriveTitle
            // 
            this.lblAddDriveTitle.AutoSize = true;
            this.lblAddDriveTitle.Font = new System.Drawing.Font("Segoe UI Semilight", 18F);
            this.lblAddDriveTitle.ForeColor = System.Drawing.Color.White;
            this.lblAddDriveTitle.Location = new System.Drawing.Point(7, 3);
            this.lblAddDriveTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddDriveTitle.Name = "lblAddDriveTitle";
            this.lblAddDriveTitle.Size = new System.Drawing.Size(301, 32);
            this.lblAddDriveTitle.TabIndex = 16;
            this.lblAddDriveTitle.Text = "Add a public drive template";
            this.lblAddDriveTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txbDisplayName
            // 
            this.txbDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbDisplayName.Location = new System.Drawing.Point(167, 143);
            this.txbDisplayName.Name = "txbDisplayName";
            this.txbDisplayName.Size = new System.Drawing.Size(366, 29);
            this.txbDisplayName.TabIndex = 2;
            this.txbDisplayName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitForm);
            // 
            // txbDrivePath
            // 
            this.txbDrivePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbDrivePath.Location = new System.Drawing.Point(113, 65);
            this.txbDrivePath.Name = "txbDrivePath";
            this.txbDrivePath.Size = new System.Drawing.Size(420, 29);
            this.txbDrivePath.TabIndex = 1;
            this.txbDrivePath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubmitForm);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(18, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 21);
            this.label2.TabIndex = 21;
            this.label2.Text = "Default Drive-Letter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 21);
            this.label1.TabIndex = 25;
            this.label1.Text = "Default Display-Name";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(26, 68);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 21);
            this.label10.TabIndex = 27;
            this.label10.Text = "Drive-Path";
            // 
            // QDAddPublicDrive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(539, 270);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.cbxDriveLetter);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.lblAddDriveTitle);
            this.Controls.Add(this.txbDisplayName);
            this.Controls.Add(this.txbDrivePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(32, 32);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "QDAddPublicDrive";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Style.InactiveShadowOpacity = ((byte)(20));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Style.ShadowOpacity = ((byte)(30));
            this.Style.TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            this.Style.TitleBar.BottomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            this.Style.TitleBar.ForeColor = System.Drawing.Color.White;
            this.Text = "Add Public Drive";
            this.Load += new System.EventHandler(this.QDAddPublicDrive_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.ComboBox cbxDriveLetter;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lblAddDriveTitle;
        private System.Windows.Forms.TextBox txbDisplayName;
        private System.Windows.Forms.TextBox txbDrivePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
    }
}