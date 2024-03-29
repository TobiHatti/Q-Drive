﻿namespace QDriveAdminConsole
{
    partial class QDActionBrowser
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QDActionBrowser));
            this.lblTitle = new System.Windows.Forms.Label();
            this.dgvActionBrowser = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txbSearchbox = new System.Windows.Forms.TextBox();
            this.lbxSearchresult = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblActionType = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblActionDescription = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblDeviceName = new System.Windows.Forms.Label();
            this.lblLogonName = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblAssignedDrives = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblMacAddress = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.lblUserCount = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnShowAllActions = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.btnShowActionsCurrentUser = new System.Windows.Forms.Button();
            this.btnShowActionsCurrentDevice = new System.Windows.Forms.Button();
            this.btnShowActionsCurrentType = new System.Windows.Forms.Button();
            this.tmrSearchCooldown = new System.Windows.Forms.Timer(this.components);
            this.lblActionDescriptor = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxEntryLimit = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblResultRange = new System.Windows.Forms.Label();
            this.btnJumpBack = new System.Windows.Forms.Button();
            this.btnJumpToFirst = new System.Windows.Forms.Button();
            this.btnJumpToNext = new System.Windows.Forms.Button();
            this.btnJumpToLast = new System.Windows.Forms.Button();
            this.lblPageCounter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActionBrowser)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semilight", 18F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(7, 3);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(170, 32);
            this.lblTitle.TabIndex = 19;
            this.lblTitle.Text = "Action Browser";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvActionBrowser
            // 
            this.dgvActionBrowser.AllowUserToAddRows = false;
            this.dgvActionBrowser.AllowUserToDeleteRows = false;
            this.dgvActionBrowser.AllowUserToResizeRows = false;
            this.dgvActionBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvActionBrowser.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvActionBrowser.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvActionBrowser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvActionBrowser.Location = new System.Drawing.Point(5, 38);
            this.dgvActionBrowser.MultiSelect = false;
            this.dgvActionBrowser.Name = "dgvActionBrowser";
            this.dgvActionBrowser.ReadOnly = true;
            this.dgvActionBrowser.RowHeadersVisible = false;
            this.dgvActionBrowser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvActionBrowser.ShowCellErrors = false;
            this.dgvActionBrowser.ShowCellToolTips = false;
            this.dgvActionBrowser.ShowEditingIcon = false;
            this.dgvActionBrowser.Size = new System.Drawing.Size(1092, 688);
            this.dgvActionBrowser.TabIndex = 1;
            this.dgvActionBrowser.SelectionChanged += new System.EventHandler(this.dgvActionBrowser_SelectionChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(5, 732);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 34);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(1103, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 25);
            this.label1.TabIndex = 30;
            this.label1.Text = "Search (Users or Devices)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txbSearchbox
            // 
            this.txbSearchbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txbSearchbox.Location = new System.Drawing.Point(1103, 66);
            this.txbSearchbox.Name = "txbSearchbox";
            this.txbSearchbox.Size = new System.Drawing.Size(396, 29);
            this.txbSearchbox.TabIndex = 2;
            this.txbSearchbox.TextChanged += new System.EventHandler(this.txbSearchbox_TextChanged);
            // 
            // lbxSearchresult
            // 
            this.lbxSearchresult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxSearchresult.FormattingEnabled = true;
            this.lbxSearchresult.ItemHeight = 21;
            this.lbxSearchresult.Location = new System.Drawing.Point(1103, 101);
            this.lbxSearchresult.Name = "lbxSearchresult";
            this.lbxSearchresult.Size = new System.Drawing.Size(396, 88);
            this.lbxSearchresult.TabIndex = 3;
            this.lbxSearchresult.DoubleClick += new System.EventHandler(this.lbxSearchresult_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(-5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 25);
            this.label2.TabIndex = 30;
            this.label2.Text = "Action Information";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDateTime
            // 
            this.lblDateTime.ForeColor = System.Drawing.Color.White;
            this.lblDateTime.Location = new System.Drawing.Point(129, 26);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(241, 21);
            this.lblDateTime.TabIndex = 30;
            this.lblDateTime.Text = "---";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(10, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 21);
            this.label5.TabIndex = 30;
            this.label5.Text = "Date and Time:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblActionType
            // 
            this.lblActionType.ForeColor = System.Drawing.Color.White;
            this.lblActionType.Location = new System.Drawing.Point(129, 47);
            this.lblActionType.Name = "lblActionType";
            this.lblActionType.Size = new System.Drawing.Size(241, 21);
            this.lblActionType.TabIndex = 30;
            this.lblActionType.Text = "---";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(30, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 21);
            this.label6.TabIndex = 30;
            this.label6.Text = "Action Type:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblActionDescription
            // 
            this.lblActionDescription.AutoSize = true;
            this.lblActionDescription.ForeColor = System.Drawing.Color.White;
            this.lblActionDescription.Location = new System.Drawing.Point(129, 68);
            this.lblActionDescription.MaximumSize = new System.Drawing.Size(262, 68);
            this.lblActionDescription.Name = "lblActionDescription";
            this.lblActionDescription.Size = new System.Drawing.Size(28, 63);
            this.lblActionDescription.TabIndex = 30;
            this.lblActionDescription.Text = "---\r\n---\r\n---";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(-4, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 21);
            this.label8.TabIndex = 30;
            this.label8.Text = "Type Description:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(-5, 146);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(158, 25);
            this.label9.TabIndex = 30;
            this.label9.Text = "User Information";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.ForeColor = System.Drawing.Color.White;
            this.lblDisplayName.Location = new System.Drawing.Point(129, 172);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(241, 21);
            this.lblDisplayName.TabIndex = 30;
            this.lblDisplayName.Text = "---";
            // 
            // lblUsername
            // 
            this.lblUsername.ForeColor = System.Drawing.Color.White;
            this.lblUsername.Location = new System.Drawing.Point(129, 193);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(241, 21);
            this.lblUsername.TabIndex = 30;
            this.lblUsername.Text = "---";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(68, 172);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(55, 21);
            this.label13.TabIndex = 30;
            this.label13.Text = "Name:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(40, 193);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 21);
            this.label14.TabIndex = 30;
            this.label14.Text = "Username:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(-5, 245);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(178, 25);
            this.label12.TabIndex = 30;
            this.label12.Text = "Device Information";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.ForeColor = System.Drawing.Color.White;
            this.lblDeviceName.Location = new System.Drawing.Point(129, 271);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.Size = new System.Drawing.Size(241, 21);
            this.lblDeviceName.TabIndex = 30;
            this.lblDeviceName.Text = "---";
            // 
            // lblLogonName
            // 
            this.lblLogonName.ForeColor = System.Drawing.Color.White;
            this.lblLogonName.Location = new System.Drawing.Point(129, 292);
            this.lblLogonName.Name = "lblLogonName";
            this.lblLogonName.Size = new System.Drawing.Size(241, 21);
            this.lblLogonName.TabIndex = 30;
            this.lblLogonName.Text = "---";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(19, 271);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(104, 21);
            this.label17.TabIndex = 30;
            this.label17.Text = "Device Name:";
            this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(21, 292);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(102, 21);
            this.label18.TabIndex = 30;
            this.label18.Text = "Logon Name:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblAssignedDrives
            // 
            this.lblAssignedDrives.ForeColor = System.Drawing.Color.White;
            this.lblAssignedDrives.Location = new System.Drawing.Point(129, 214);
            this.lblAssignedDrives.Name = "lblAssignedDrives";
            this.lblAssignedDrives.Size = new System.Drawing.Size(241, 21);
            this.lblAssignedDrives.TabIndex = 30;
            this.lblAssignedDrives.Text = "---";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(0, 214);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(123, 21);
            this.label20.TabIndex = 30;
            this.label20.Text = "Assigned Drives:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblMacAddress
            // 
            this.lblMacAddress.ForeColor = System.Drawing.Color.White;
            this.lblMacAddress.Location = new System.Drawing.Point(129, 314);
            this.lblMacAddress.Name = "lblMacAddress";
            this.lblMacAddress.Size = new System.Drawing.Size(241, 21);
            this.lblMacAddress.TabIndex = 30;
            this.lblMacAddress.Text = "---";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(16, 314);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(107, 21);
            this.label22.TabIndex = 30;
            this.label22.Text = "MAC Address:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblUserCount
            // 
            this.lblUserCount.ForeColor = System.Drawing.Color.White;
            this.lblUserCount.Location = new System.Drawing.Point(129, 335);
            this.lblUserCount.Name = "lblUserCount";
            this.lblUserCount.Size = new System.Drawing.Size(241, 21);
            this.lblUserCount.TabIndex = 30;
            this.lblUserCount.Text = "---";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(32, 335);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(91, 21);
            this.label24.TabIndex = 30;
            this.label24.Text = "User Count:";
            this.label24.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(1241, 732);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 34);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnShowAllActions
            // 
            this.btnShowAllActions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowAllActions.Location = new System.Drawing.Point(1103, 652);
            this.btnShowAllActions.Name = "btnShowAllActions";
            this.btnShowAllActions.Size = new System.Drawing.Size(195, 34);
            this.btnShowAllActions.TabIndex = 5;
            this.btnShowAllActions.Text = "Show all actions";
            this.btnShowAllActions.UseVisualStyleBackColor = true;
            this.btnShowAllActions.Click += new System.EventHandler(this.btnShowAllActions_Click);
            // 
            // label25
            // 
            this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.White;
            this.label25.Location = new System.Drawing.Point(1103, 624);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(107, 25);
            this.label25.TabIndex = 30;
            this.label25.Text = "Navigation";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnShowActionsCurrentUser
            // 
            this.btnShowActionsCurrentUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowActionsCurrentUser.Location = new System.Drawing.Point(1103, 692);
            this.btnShowActionsCurrentUser.Name = "btnShowActionsCurrentUser";
            this.btnShowActionsCurrentUser.Size = new System.Drawing.Size(195, 34);
            this.btnShowActionsCurrentUser.TabIndex = 7;
            this.btnShowActionsCurrentUser.Text = "Show all from curr. user";
            this.btnShowActionsCurrentUser.UseVisualStyleBackColor = true;
            this.btnShowActionsCurrentUser.Click += new System.EventHandler(this.btnShowActionsCurrentUser_Click);
            // 
            // btnShowActionsCurrentDevice
            // 
            this.btnShowActionsCurrentDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowActionsCurrentDevice.Location = new System.Drawing.Point(1304, 692);
            this.btnShowActionsCurrentDevice.Name = "btnShowActionsCurrentDevice";
            this.btnShowActionsCurrentDevice.Size = new System.Drawing.Size(195, 34);
            this.btnShowActionsCurrentDevice.TabIndex = 8;
            this.btnShowActionsCurrentDevice.Text = "Show all from curr. device";
            this.btnShowActionsCurrentDevice.UseVisualStyleBackColor = true;
            this.btnShowActionsCurrentDevice.Click += new System.EventHandler(this.btnShowActionsCurrentDevice_Click);
            // 
            // btnShowActionsCurrentType
            // 
            this.btnShowActionsCurrentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowActionsCurrentType.Location = new System.Drawing.Point(1304, 652);
            this.btnShowActionsCurrentType.Name = "btnShowActionsCurrentType";
            this.btnShowActionsCurrentType.Size = new System.Drawing.Size(195, 34);
            this.btnShowActionsCurrentType.TabIndex = 6;
            this.btnShowActionsCurrentType.Text = "Show all from curr. type";
            this.btnShowActionsCurrentType.UseVisualStyleBackColor = true;
            this.btnShowActionsCurrentType.Click += new System.EventHandler(this.btnShowActionsCurrentType_Click);
            // 
            // tmrSearchCooldown
            // 
            this.tmrSearchCooldown.Interval = 500;
            this.tmrSearchCooldown.Tick += new System.EventHandler(this.tmrSearchCooldown_Tick);
            // 
            // lblActionDescriptor
            // 
            this.lblActionDescriptor.AutoSize = true;
            this.lblActionDescriptor.ForeColor = System.Drawing.Color.White;
            this.lblActionDescriptor.Location = new System.Drawing.Point(207, 14);
            this.lblActionDescriptor.Name = "lblActionDescriptor";
            this.lblActionDescriptor.Size = new System.Drawing.Size(159, 21);
            this.lblActionDescriptor.TabIndex = 30;
            this.lblActionDescriptor.Text = "ShowingActionsOf ---";
            this.lblActionDescriptor.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(1113, 560);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 21);
            this.label3.TabIndex = 30;
            this.label3.Text = "Result Limit:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbxEntryLimit
            // 
            this.cbxEntryLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxEntryLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEntryLimit.FormattingEnabled = true;
            this.cbxEntryLimit.Items.AddRange(new object[] {
            "50 Entries",
            "100 Entries",
            "250 Entries",
            "500 Entries",
            "1000 Entries",
            "5000 Entries",
            "No Limit (Performance Warning!)"});
            this.cbxEntryLimit.Location = new System.Drawing.Point(1212, 557);
            this.cbxEntryLimit.Name = "cbxEntryLimit";
            this.cbxEntryLimit.Size = new System.Drawing.Size(287, 29);
            this.cbxEntryLimit.TabIndex = 4;
            this.cbxEntryLimit.SelectedIndexChanged += new System.EventHandler(this.cbxEntryLimit_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.lblDateTime);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.lblDisplayName);
            this.panel1.Controls.Add(this.lblDeviceName);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.lblActionType);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.lblUsername);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.lblLogonName);
            this.panel1.Controls.Add(this.label20);
            this.panel1.Controls.Add(this.lblAssignedDrives);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.lblMacAddress);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lblActionDescription);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.lblUserCount);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(1103, 195);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(396, 328);
            this.panel1.TabIndex = 31;
            // 
            // lblResultRange
            // 
            this.lblResultRange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultRange.AutoSize = true;
            this.lblResultRange.ForeColor = System.Drawing.Color.White;
            this.lblResultRange.Location = new System.Drawing.Point(1113, 530);
            this.lblResultRange.Name = "lblResultRange";
            this.lblResultRange.Size = new System.Drawing.Size(259, 21);
            this.lblResultRange.TabIndex = 30;
            this.lblResultRange.Text = "Showing entries N to N (NN in total)";
            this.lblResultRange.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnJumpBack
            // 
            this.btnJumpBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJumpBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJumpBack.Location = new System.Drawing.Point(1258, 592);
            this.btnJumpBack.Name = "btnJumpBack";
            this.btnJumpBack.Size = new System.Drawing.Size(40, 34);
            this.btnJumpBack.TabIndex = 32;
            this.btnJumpBack.Text = "<";
            this.btnJumpBack.UseVisualStyleBackColor = true;
            this.btnJumpBack.Click += new System.EventHandler(this.btnJumpBack_Click);
            // 
            // btnJumpToFirst
            // 
            this.btnJumpToFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJumpToFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJumpToFirst.Location = new System.Drawing.Point(1212, 592);
            this.btnJumpToFirst.Name = "btnJumpToFirst";
            this.btnJumpToFirst.Size = new System.Drawing.Size(40, 34);
            this.btnJumpToFirst.TabIndex = 32;
            this.btnJumpToFirst.Text = "|<";
            this.btnJumpToFirst.UseVisualStyleBackColor = true;
            this.btnJumpToFirst.Click += new System.EventHandler(this.btnJumpToFirst_Click);
            // 
            // btnJumpToNext
            // 
            this.btnJumpToNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJumpToNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJumpToNext.Location = new System.Drawing.Point(1304, 592);
            this.btnJumpToNext.Name = "btnJumpToNext";
            this.btnJumpToNext.Size = new System.Drawing.Size(40, 34);
            this.btnJumpToNext.TabIndex = 32;
            this.btnJumpToNext.Text = ">";
            this.btnJumpToNext.UseVisualStyleBackColor = true;
            this.btnJumpToNext.Click += new System.EventHandler(this.btnJumpToNext_Click);
            // 
            // btnJumpToLast
            // 
            this.btnJumpToLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJumpToLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJumpToLast.Location = new System.Drawing.Point(1350, 592);
            this.btnJumpToLast.Name = "btnJumpToLast";
            this.btnJumpToLast.Size = new System.Drawing.Size(40, 34);
            this.btnJumpToLast.TabIndex = 32;
            this.btnJumpToLast.Text = ">|";
            this.btnJumpToLast.UseVisualStyleBackColor = true;
            this.btnJumpToLast.Click += new System.EventHandler(this.btnJumpToLast_Click);
            // 
            // lblPageCounter
            // 
            this.lblPageCounter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPageCounter.ForeColor = System.Drawing.Color.White;
            this.lblPageCounter.Location = new System.Drawing.Point(1113, 599);
            this.lblPageCounter.Name = "lblPageCounter";
            this.lblPageCounter.Size = new System.Drawing.Size(93, 21);
            this.lblPageCounter.TabIndex = 33;
            this.lblPageCounter.Text = "Page N";
            this.lblPageCounter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // QDActionBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(1504, 771);
            this.Controls.Add(this.lblPageCounter);
            this.Controls.Add(this.btnJumpToFirst);
            this.Controls.Add(this.btnJumpToLast);
            this.Controls.Add(this.btnJumpToNext);
            this.Controls.Add(this.btnJumpBack);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbxEntryLimit);
            this.Controls.Add(this.lbxSearchresult);
            this.Controls.Add(this.txbSearchbox);
            this.Controls.Add(this.lblResultRange);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblActionDescriptor);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShowActionsCurrentType);
            this.Controls.Add(this.btnShowActionsCurrentDevice);
            this.Controls.Add(this.btnShowActionsCurrentUser);
            this.Controls.Add(this.btnShowAllActions);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvActionBrowser);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IconSize = new System.Drawing.Size(32, 32);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(846, 810);
            this.Name = "QDActionBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.Style.InactiveShadowOpacity = ((byte)(20));
            this.Style.MdiChild.IconHorizontalAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.Style.MdiChild.IconVerticalAlignment = System.Windows.Forms.VisualStyles.VerticalAlignment.Center;
            this.Style.ShadowOpacity = ((byte)(30));
            this.Style.TitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            this.Style.TitleBar.BottomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199)))));
            this.Style.TitleBar.ForeColor = System.Drawing.Color.White;
            this.Text = "Action Browser";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.QDActionBrowser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvActionBrowser)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvActionBrowser;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbSearchbox;
        private System.Windows.Forms.ListBox lbxSearchresult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblActionType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblActionDescription;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblDeviceName;
        private System.Windows.Forms.Label lblLogonName;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblAssignedDrives;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblMacAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label lblUserCount;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnShowAllActions;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btnShowActionsCurrentUser;
        private System.Windows.Forms.Button btnShowActionsCurrentDevice;
        private System.Windows.Forms.Button btnShowActionsCurrentType;
        private System.Windows.Forms.Timer tmrSearchCooldown;
        private System.Windows.Forms.Label lblActionDescriptor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxEntryLimit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblResultRange;
        private System.Windows.Forms.Button btnJumpBack;
        private System.Windows.Forms.Button btnJumpToFirst;
        private System.Windows.Forms.Button btnJumpToNext;
        private System.Windows.Forms.Button btnJumpToLast;
        private System.Windows.Forms.Label lblPageCounter;
    }
}