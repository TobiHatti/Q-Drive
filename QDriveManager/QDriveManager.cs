using MySql.Data.MySqlClient;
using QDriveLib;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WrapSQL;

// Q-Drive Network-Drive Manager
// Copyright(C) 2020 Tobias Hattinger

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<https://www.gnu.org/licenses/>.

namespace QDriveManager
{
    public partial class QDriveManager : SfForm
    {
        public bool AutostartLogin = false;

        private bool localUserNoPassword = false;

        // General properties
        private bool localConnection = false;
        private bool promptPassword = false;

        public string uUsername = "";
        public string uPassword = "";

        // New Drive predefined values
        private bool useLoginAsDriveAuth = false;
        private bool forceLoginDriveAuth = false;
        private string ndDefaultDomain = "";
        private string ndUsername = "";
        private string ndPassword = "";

        // Online-specific properties
        private bool userCanToggleKeepLoggedIn = true;
        private bool userCanAddPrivateDrive = true;
        private bool userCanAddPublicDrive = true;
        private bool userCanSelfRegister = true;
        private bool logUserActions = false;
        private bool userCanChangeManagerSettings = false;

        public string userID = "";

        private readonly WrapMySQLData dbData = new WrapMySQLData() { Pooling = false };

        private WrapSQLite sqlite = null;
        private WrapMySQL mysql = null;

        private List<DriveViewItem> drives = new List<DriveViewItem>();

        private readonly ImageList imgList = null;

        #region Page Layout and Initial Loading =================================================================[RF]=

        private readonly List<Panel> panels = new List<Panel>();

        public QDriveManager()
        {
            InitializeComponent();

            this.DialogResult = DialogResult.Cancel;

            panels.Add(pnlNotConfigured);
            panels.Add(pnlLogin);
            panels.Add(pnlSignUp);
            panels.Add(pnlManager);
            panels.Add(pnlLoading);

            this.Style.Border = new Pen(Color.FromArgb(77, 216, 255), 2);
            this.Style.InactiveBorder = new Pen(Color.FromArgb(77, 216, 255), 2);

            pbxLoginLogo.Image = Properties.Resources.QDriveProgramBanner;
            pbxSignUpLogo.Image = Properties.Resources.QDriveProgramBanner;

            pbxAddDriveBtn.Image = Properties.Resources.QDriveAddDrives;
            pbxEditDriveBtn.Image = Properties.Resources.QDriveEditDriveDisabled;
            pbxRemoveDriveBtn.Image = Properties.Resources.QDriveRemoveDriveDisabled;

            pbxUpdateBtn.Image = Properties.Resources.QDriveUpdate;
            pbxDisconnectBtn.Image = Properties.Resources.QDriveLogOff;

            QDLib.AlignPanels(this, panels, 800, 600);

            imgList = new ImageList()
            {
                ImageSize = new Size(80, 80),
                ColorDepth = ColorDepth.Depth32Bit,
            };

            imgList.Images.Add("LocalUp", Properties.Resources.QDriveLocalUp);
            imgList.Images.Add("OnlinePrivateUp", Properties.Resources.QDriveOnlinePrivateUp);
            imgList.Images.Add("OnlinePublicUp", Properties.Resources.QDriveOnlinePublicUp);
            imgList.Images.Add("LocalDown", Properties.Resources.QDriveLocalDown);
            imgList.Images.Add("OnlinePrivateDown", Properties.Resources.QDriveOnlinePrivateDown);
            imgList.Images.Add("OnlinePublicDown", Properties.Resources.QDriveOnlinePublicDown);

            grvConnectedDrives.SmallImageList = imgList;

            // Check if QD-Autostart is running, if not, start it
            bool qdAutostartRunning = false;
            foreach (Process process in Process.GetProcesses())
                if (process.ProcessName.Contains("QDriveAutostart")) qdAutostartRunning = true;

            if (!qdAutostartRunning)
            {
                try { Process.Start("QDriveAutostart.exe"); }
                catch { }
            }
        }

        private void QDriveManager_Load(object sender, EventArgs e)
        {
            sqlite = new WrapSQLite(QDInfo.ConfigFile);

            pnlLoading.BringToFront();


            if (!QDLib.IsQDConfigured()) pnlNotConfigured.BringToFront();
            else
            {
                int loadStatusCode = LoadQDData();

                Image statusImage;

                if (localConnection) statusImage = Properties.Resources.QDriveLocalConnection;
                else statusImage = Properties.Resources.QDriveOnlineConnection;

                pbxLoginConnectionState.Image = statusImage;
                pbxSignUpConnectionState.Image = statusImage;
                pbxManagerConnectionState.Image = statusImage;

                pbxNoDrivesConnected.Image = Properties.Resources.QDriveNoDrives;

                switch (loadStatusCode)
                {
                    case 1:
                        MessageBox.Show("Could not load local database (Error E001).\r\n\r\nPlease try again later. If this problem remains, please contact your system administrator.", "Could not start Q-Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case 2:
                        MessageBox.Show("Could not connect to the online database (Error E002).\r\n\r\nPlease try again later. If this problem remains, please contact your system administrator.", "Could not start Q-Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case 3:
                        MessageBox.Show("Could not load data from online database (Error E003).\r\n\r\nPlease try again later. If this problem remains, please contact your system administrator.", "Could not start Q-Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                    case 99:
                        MessageBox.Show("A fatal error occured (Error E099).\r\n\r\nPlease try again later. If this problem remains, please contact your system administrator or try to re-install the program.", "Could not start Q-Drive", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        break;
                }

                // Check if user uses local connection and no password
                try
                {
                    if (localConnection && uPassword == string.Empty)
                    {
                        localUserNoPassword = true;
                        txbUsername.ReadOnly = true;
                        txbPassword.ReadOnly = true;
                        chbKeepLoggedIn.Enabled = false;
                        lblKeepLoggedInInfo.Visible = false;
                        lnkCreateNewAccount.Visible = false;

                        txbUsername.Visible = false;
                        txbPassword.Visible = false;
                        chbKeepLoggedIn.Visible = false;
                        lblUsername.Visible = false;
                        lblPassword.Visible = false;

                    }
                }
                catch { }


                chbKeepLoggedIn.Checked = false;
                chbKeepLoggedIn.Enabled = userCanToggleKeepLoggedIn;

                if (userCanToggleKeepLoggedIn || localConnection) lblKeepLoggedInInfo.Text = string.Empty;
                else lblKeepLoggedInInfo.Text = "(disabled by administrator)";

                lnkCreateNewAccount.Enabled = userCanSelfRegister;

                if (userCanSelfRegister) lnkCreateNewAccount.Text = "Create a new Account";
                else lnkCreateNewAccount.Text = "If you do not have an account yet, contact your system administrator.";

                txbUsername.ReadOnly = localConnection;
                lnkCreateNewAccount.Visible = !localConnection;

                if (localConnection) txbUsername.Text = "local";
                else txbUsername.Text = string.Empty;

                if (localConnection) tsmChangeOnlineDBConnection.Enabled = false;

                if (AutostartLogin || (!localUserNoPassword && (promptPassword || string.IsNullOrEmpty(uUsername) || string.IsNullOrEmpty(uPassword))))
                {
                    pnlLogin.BringToFront();
                    txbUsername.Focus();
                }
                else
                {
                    UpdateManagerData();
                    pnlManager.BringToFront();
                }
            }
        }

        #endregion

        #region Step A: Not Configured ==========================================================================[RF]=

        private void btnRunSetup_Click(object sender, EventArgs e)
        {
            if (QDLib.RunQDriveSetup()) this.Close();
        }

        private void btnNotConfigured_Click(object sender, EventArgs e)
        {
            ImportQDConfig();
            if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserLoadedBackup, dbData, logUserActions);

            Process.Start("QDriveManager.exe");
            this.Close();
        }

        #endregion

        #region Step B: Login ===================================================================================[RF]=

        private void lnkCreateNewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pnlSignUp.BringToFront();
            txbRegName.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e) => SubmitLogin();

        private void SubmitLoginForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SubmitLogin();
        }

        private void SubmitLogin()
        {
            if (localUserNoPassword)
            {
                if (AutostartLogin)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    pnlManager.BringToFront();
                    UpdateManagerData();
                    return;
                }
            }

            if (!QDLib.VerifyPassword(localConnection, txbUsername.Text, txbPassword.Text, out userID, dbData))
            {
                MessageBox.Show("Username or password are invalid!", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!QDLib.ManagedDBOpen(sqlite)) { QDLib.DBOpenFailed(); return; }
            sqlite.TransactionBegin();
            try
            {
                sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", !chbKeepLoggedIn.Checked, QDInfo.DBL.AlwaysPromptPassword);

                if (chbKeepLoggedIn.Checked && !localConnection)
                {
                    sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", txbUsername.Text, QDInfo.DBL.DefaultUsername);
                    sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(txbPassword.Text, QDInfo.LocalCipherKey), QDInfo.DBL.DefaultPassword);
                }
                else if (!localConnection)
                {
                    sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", DBNull.Value, QDInfo.DBL.DefaultUsername);
                    sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", DBNull.Value, QDInfo.DBL.DefaultPassword);
                }

                sqlite.TransactionCommit();
            }
            catch
            {
                sqlite.TransactionRollback();
            }

            sqlite.Close();

            uUsername = txbUsername.Text;
            uPassword = txbPassword.Text;

            if (useLoginAsDriveAuth)
            {
                ndUsername = uUsername;
                ndPassword = uPassword;
            }

            txbPassword.Text = string.Empty;

            if (AutostartLogin)
            {
                if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserLoggedInAutoStart, dbData, logUserActions);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserLoggedIn, dbData, logUserActions);
                UpdateManagerData();
                pnlManager.BringToFront();
            }            
        }

        #endregion

        #region Step C: SignUp ==================================================================================[RF]=

        private void txbRegName_TextChanged(object sender, EventArgs e) => txbRegUsername.Text = txbRegName.Text.Replace(' ', '.').ToLower();

        private void btnRegCancel_Click(object sender, EventArgs e) => pnlLogin.BringToFront();

        private void btnSignUp_Click(object sender, EventArgs e) => SubmitRegister();

        private void SubmitRegisterForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SubmitRegister();
        }

        private void SubmitRegister()
        {
            bool signupSuccess = false;

            string newUserID = Guid.NewGuid().ToString();

            if (!QDLib.ValidatePasswords(txbRegPassword.Text, txbRegConfirmPassword.Text)) return;

            if (mysql.ExecuteScalarACon<int>("SELECT COUNT(*) FROM qd_users WHERE Username = ?", txbRegUsername.Text) == 0)
            {
                try
                {
                    mysql.ExecuteNonQueryACon("INSERT INTO qd_users (ID, Name, Username, Password) VALUES (?,?,?,?)", newUserID, txbRegName.Text, txbRegUsername.Text, QDLib.HashPassword(txbRegPassword.Text));

                    MessageBox.Show("User signed up successfully! Please log in with your username and password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    signupSuccess = true;
                }
                catch
                {
                    MessageBox.Show("An error occured whilst trying to register the user.", "Server error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else MessageBox.Show("Username already in use. Please choose another username", "Username already in use", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (signupSuccess)
            {
                if (!localConnection) QDLib.LogUserConnection(newUserID, QDLogAction.UserRegistered, dbData, logUserActions);

                txbUsername.Text = txbRegUsername.Text;
                pnlLogin.BringToFront();
                txbUsername.Focus();
            }
        }

        #endregion

        #region Step D: Manager =================================================================================[RF]=

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserDrivelistUpdated, dbData, logUserActions);
            UpdateManagerData();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            QDLib.DisconnectAllDrives(drives);
            if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.QDDrivesDisconnect, dbData, logUserActions);
            
            pnlLogin.BringToFront();
        }

        private void pbxButtons_MouseLeave(object sender, EventArgs e)
        {
            (sender as PictureBox).BackColor = Color.Transparent;
        }

        private void pbxButtons_MouseOver(object sender, EventArgs e)
        {
            if((sender as PictureBox).Enabled)
                (sender as PictureBox).BackColor = Color.FromArgb(229, 243, 255);
        }

        private void pbxButtons_MouseDown(object sender, MouseEventArgs e)
        {
            if ((sender as PictureBox).Enabled)
                (sender as PictureBox).BackColor = Color.FromArgb(204, 232, 255);
        }

        private void btnAddDrive_Click(object sender, EventArgs e) => ShowAddDriveDialog();

        private void grvConnectedDrives_Click(object sender, EventArgs e)
        {
            if (grvConnectedDrives.SelectedItem != -1)
            {
                pbxEditDriveBtn.Enabled = true;
                pbxRemoveDriveBtn.Enabled = true;

                pbxEditDriveBtn.Image = Properties.Resources.QDriveEditDrive;
                pbxRemoveDriveBtn.Image = Properties.Resources.QDriveRemoveDrive;
            }
        }

        private void grvConnectedDrives_GroupViewItemDoubleClick(GroupView sender, GroupViewItemDoubleClickEventArgs e)
        {
            try
            {
                DriveViewItem drive = ((GroupViewItemEx)sender.GroupViewItems[sender.SelectedItem]).Drive;
                Process.Start(drive.DriveLetter + ":\\");
            }
            catch
            {
                MessageBox.Show("Could not connect to network drive.\r\n\r\nPlease ensure that the authentication-information is correct and try again. \r\n\r\nNote: It may cause problems if you try to connect to several drives using different user credentials.", "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveDrive_Click(object sender, EventArgs e)
        {
            DriveViewItem drive = ((GroupViewItemEx)grvConnectedDrives.GroupViewItems[grvConnectedDrives.SelectedItem]).Drive;

            if (MessageBox.Show("Do you really want to remove the selected drive from your drive-list?", "Remove Drive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (drive.IsLocalDrive) sqlite.ExecuteNonQueryACon("DELETE FROM qd_drives WHERE ID = ?", drive.ID);
                else mysql.ExecuteNonQueryACon("DELETE FROM qd_assigns WHERE ID = ?", drive.ID);

                if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.DriveRemoved, dbData, logUserActions);

                UpdateManagerData();

                pbxEditDriveBtn.Enabled = false;
                pbxRemoveDriveBtn.Enabled = false;

                pbxEditDriveBtn.Image = Properties.Resources.QDriveEditDriveDisabled;
                pbxRemoveDriveBtn.Image = Properties.Resources.QDriveRemoveDriveDisabled;
            }

        }

        private void btnEditDrive_Click(object sender, EventArgs e)
        {
            DriveViewItem drive = ((GroupViewItemEx)grvConnectedDrives.GroupViewItems[grvConnectedDrives.SelectedItem]).Drive;

            // Public drive
            if (drive.IsPublicDrive)
            {
                QDAddPublicDrive editPublic = new QDAddPublicDrive()
                {
                    DBEntryID = drive.ID,
                    DriveID = drive.ParentDriveID,
                    CustomDriveName = drive.DisplayName,
                    CustomDriveLetter = drive.DriveLetter,
                    Username = drive.Username,
                    Password = drive.Password,
                    Domain = drive.Domain,
                    ForceAutofill = forceLoginDriveAuth,
                    DBData = dbData
                };

                if (editPublic.ShowDialog() == DialogResult.OK)
                {
                    mysql.ExecuteNonQueryACon("UPDATE qd_assigns SET DriveID = ?, CustomDriveName = ?, CustomDriveLetter = ?, DUsername = ?, DPassword = ?, DDomain = ? WHERE ID = ?",
                        editPublic.DriveID,
                        editPublic.CustomDriveName,
                        editPublic.CustomDriveLetter,
                        Cipher.Encrypt(editPublic.Username, uPassword),
                        Cipher.Encrypt(editPublic.Password, uPassword),
                        Cipher.Encrypt(editPublic.Domain, uPassword),
                        editPublic.DBEntryID
                    );

                    if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.DrivePublicEdited, dbData, logUserActions);
                    UpdateManagerData();
                }
            }
            else
            {
                QDAddPrivateDrive editPrivate = new QDAddPrivateDrive()
                {
                    DBEntryID = drive.ID,
                    DrivePath = drive.DrivePath,
                    DisplayName = drive.DisplayName,
                    DriveLetter = drive.DriveLetter,
                    Username = drive.Username,
                    Password = drive.Password,
                    Domain = drive.Domain,
                    ForceAutofill = forceLoginDriveAuth
                };

                if (editPrivate.ShowDialog() == DialogResult.OK)
                {
                    // Private drive (device linked)
                    if (drive.IsLocalDrive)
                    {
                        sqlite.ExecuteNonQueryACon("UPDATE qd_drives SET LocalPath = ?, Username = ?, Password = ?, Domain = ?, DriveLetter = ?, DriveName = ? WHERE ID = ?",
                            editPrivate.DrivePath,
                            Cipher.Encrypt(editPrivate.Username, QDInfo.LocalCipherKey),
                            Cipher.Encrypt(editPrivate.Password, QDInfo.LocalCipherKey),
                            Cipher.Encrypt(editPrivate.Domain, QDInfo.LocalCipherKey),
                            editPrivate.DriveLetter,
                            editPrivate.DisplayName,
                            editPrivate.DBEntryID
                        );
                    }
                    // Private drive (user linked)
                    else
                    {
                        if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return; }

                        string qdDriveID = mysql.ExecuteScalar<string>("SELECT DriveID FROM qd_assigns WHERE ID = ?", editPrivate.DBEntryID);

                        mysql.ExecuteNonQuery("UPDATE qd_assigns SET CustomDriveName = ?, CustomDriveLetter = ?, DUsername = ?, DPassword = ?, DDomain = ? WHERE ID = ?",
                            editPrivate.DisplayName,
                            editPrivate.DriveLetter,
                            Cipher.Encrypt(editPrivate.Username, uPassword),
                            Cipher.Encrypt(editPrivate.Password, uPassword),
                            Cipher.Encrypt(editPrivate.Domain, uPassword),
                            editPrivate.DBEntryID
                        );

                        mysql.ExecuteNonQuery("UPDATE qd_drives SET DefaultName = ?, DefaultDriveLetter = ?, LocalPath = ? WHERE ID = ?",
                            editPrivate.DisplayName,
                            editPrivate.DriveLetter,
                            editPrivate.DrivePath,
                            qdDriveID
                        );

                        mysql.Close();
                    }

                    if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.DrivePrivateEdited, dbData, logUserActions);
                    UpdateManagerData();
                }
            }
        }

        #endregion

        #region Step E: Manager Toolstrip =======================================================================[RF]=

        private void tsbQDrive_Click(object sender, EventArgs e) => cmsQDrive.Show(tseToolstrip, 0, tseToolstrip.Size.Height);

        private void tsbFile_Click(object sender, EventArgs e) => cmsFile.Show(tseToolstrip, 52, tseToolstrip.Size.Height);

        private void tsbSettings_Click(object sender, EventArgs e) => cmsSettings.Show(tseToolstrip, 80, tseToolstrip.Size.Height);

        private void tsmAddDrive_Click(object sender, EventArgs e) => ShowAddDriveDialog();

        private void tsmLogOffDisconnect_Click(object sender, EventArgs e)
        {
            QDLib.DisconnectAllDrives(drives);
            if(!localConnection) QDLib.LogUserConnection(userID, QDLogAction.QDDrivesDisconnect, dbData, logUserActions);

            pnlLogin.BringToFront();
        }

        private void tsmChangePassword_Click(object sender, EventArgs e)
        {
            QDChangePassword changePW = new QDChangePassword()
            {
                OldPassword = uPassword,
                NoOldPassword = localUserNoPassword
            };

            if(changePW.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (localConnection) sqlite.ExecuteNonQueryACon("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(changePW.NewPassword, QDInfo.LocalCipherKey), QDInfo.DBL.DefaultPassword);
                    else mysql.ExecuteNonQueryACon("UPDATE qd_users SET Password = ? WHERE ID = ?", QDLib.HashPassword(changePW.NewPassword), userID);

                    MessageBox.Show("Password sucessfully changed! Please restart the program for the changes to take effect.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserChangedPassword, dbData, logUserActions);
                }
                catch
                {
                    MessageBox.Show("Could not change password. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsmCheckForUpdates_Click(object sender, EventArgs e)
        {
            QDVersionInfo version = new QDVersionInfo();
            version.ShowDialog();
        }

        private void tsmCloseQDrive_Click(object sender, EventArgs e) => this.Close();

        private void tsmExportUserData_Click(object sender, EventArgs e)
        {
            bool success = false;
            if (sfdSaveConfig.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(sfdSaveConfig.FileName)) File.Delete(sfdSaveConfig.FileName);

                using (WrapSQLite backup = new WrapSQLite(sfdSaveConfig.FileName))
                {
                    if (!QDLib.ManagedDBOpen(backup)) { QDLib.DBOpenFailed(); return; }
                    if (!QDLib.ManagedDBOpen(sqlite)) { QDLib.DBOpenFailed(); return; }
                    backup.TransactionBegin();

                    try
                    {
                        // Create new tables
                        backup.ExecuteNonQuery(@"CREATE TABLE ""qd_info"" ( ""QDKey"" TEXT, ""QDValue"" TEXT, PRIMARY KEY(""QDKey""));");
                        backup.ExecuteNonQuery(@"CREATE TABLE ""qd_drives"" (""ID"" TEXT, ""LocalPath"" TEXT, ""RemotePath"" TEXT, ""Username"" TEXT, ""Password"" TEXT, ""Domain"" TEXT, ""DriveLetter"" TEXT, ""DriveName"" TEXT, PRIMARY KEY(""ID""));");

                        // Create settings
                        backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                            QDInfo.DBL.IsOnlineLinked,
                            sqlite.ExecuteScalar("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.IsOnlineLinked)
                        );

                        backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                            QDInfo.DBL.AlwaysPromptPassword,
                            sqlite.ExecuteScalar("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.AlwaysPromptPassword)
                        );

                        backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                            QDInfo.DBL.SetupSuccess,
                            sqlite.ExecuteScalar("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.SetupSuccess)
                        );

                        try
                        {
                            backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                QDInfo.DBL.DBHost,
                                Cipher.Encrypt(Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBHost), QDInfo.LocalCipherKey), QDInfo.GlobalCipherKey)
                            );
                        }
                        catch { backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultPassword, DBNull.Value); }

                        try
                        {
                            backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                QDInfo.DBL.DBName,
                                Cipher.Encrypt(Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBName), QDInfo.LocalCipherKey), QDInfo.GlobalCipherKey)
                            );
                        }
                        catch { backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultPassword, DBNull.Value); }

                        try
                        {
                            backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                QDInfo.DBL.DBUsername,
                                Cipher.Encrypt(Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBUsername), QDInfo.LocalCipherKey), QDInfo.GlobalCipherKey)
                            );
                        }
                        catch { backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultPassword, DBNull.Value); }

                        try
                        {
                            backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                QDInfo.DBL.DBPassword,
                                Cipher.Encrypt(Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBPassword), QDInfo.LocalCipherKey), QDInfo.GlobalCipherKey)
                            );
                        }
                        catch { backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultPassword, DBNull.Value); }

                        backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                            QDInfo.DBL.DefaultUsername,
                            Cipher.Encrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultUsername), QDInfo.GlobalCipherKey)
                        );

                        try
                        {
                            backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                QDInfo.DBL.DefaultPassword,
                                Cipher.Encrypt(Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultPassword), QDInfo.LocalCipherKey), QDInfo.GlobalCipherKey)
                            );
                        }
                        catch { backup.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultPassword, DBNull.Value); }

                        using(SQLiteDataReader reader = (SQLiteDataReader)sqlite.ExecuteQuery("SELECT * FROM qd_drives"))
                        {
                            while(reader.Read())
                            {
                                backup.ExecuteNonQuery("INSERT INTO qd_drives (ID, LocalPath, RemotePath, Username, Password, Domain, DriveLetter, DriveName) VALUES (?,?,?,?,?,?,?,?)",
                                    Guid.NewGuid(),
                                    Cipher.Encrypt(Convert.ToString(reader["LocalPath"]), QDInfo.GlobalCipherKey),
                                    Cipher.Encrypt(Convert.ToString(reader["RemotePath"]), QDInfo.GlobalCipherKey),
                                    Cipher.Encrypt(Cipher.Decrypt(Convert.ToString(reader["Username"]), QDInfo.LocalCipherKey), QDInfo.GlobalCipherKey),
                                    Cipher.Encrypt(Cipher.Decrypt(Convert.ToString(reader["Password"]), QDInfo.LocalCipherKey), QDInfo.GlobalCipherKey),
                                    Cipher.Encrypt(Cipher.Decrypt(Convert.ToString(reader["Domain"]), QDInfo.LocalCipherKey), QDInfo.GlobalCipherKey),
                                    Convert.ToString(reader["DriveLetter"]),
                                    Convert.ToString(reader["DriveName"])
                                );
                            }
                        }

                        success = true;

                        backup.TransactionCommit();
                    }
                    catch
                    {
                        backup.TransactionRollback();
                    }

                    sqlite.Close();
                    backup.Close();
                }

                if (success)
                {
                    MessageBox.Show("Backup created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserCreactedBackup, dbData, logUserActions);
                }
                else MessageBox.Show("An error occured trying to create the backup. please try again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmImportUserData_Click(object sender, EventArgs e)
        {
            ImportQDConfig();
            if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserLoadedBackup, dbData, logUserActions);
            Process.Start("QDriveManager.exe");
            this.Close();
        }

        private void ImportQDConfig()
        {
            bool success = false;
            if (ofdOpenConfig.ShowDialog() == DialogResult.OK)
            {
                using (WrapSQLite backup = new WrapSQLite(ofdOpenConfig.FileName))
                {
                    try
                    {
                        if (!QDLib.ManagedDBOpen(backup)) { QDLib.DBOpenFailed(); return; }
                        if (!QDLib.ManagedDBOpen(sqlite)) { QDLib.DBOpenFailed(); return; }
                        sqlite.TransactionBegin();

                        try
                        {
                            // Delete old tables
                            sqlite.ExecuteNonQuery("DROP TABLE IF EXISTS qd_info");
                            sqlite.ExecuteNonQuery("DROP TABLE IF EXISTS qd_drives");

                            // Create new tables
                            sqlite.ExecuteNonQuery(@"CREATE TABLE ""qd_info"" ( ""QDKey"" TEXT, ""QDValue"" TEXT, PRIMARY KEY(""QDKey""));");
                            sqlite.ExecuteNonQuery(@"CREATE TABLE ""qd_drives"" (""ID"" TEXT, ""LocalPath"" TEXT, ""RemotePath"" TEXT, ""Username"" TEXT, ""Password"" TEXT, ""Domain"" TEXT, ""DriveLetter"" TEXT, ""DriveName"" TEXT, PRIMARY KEY(""ID""));");


                            // Create settings
                            sqlite.ExecuteNonQuery($@"INSERT INTO qd_info(QDKey, QDValue) VALUES(?, ?)",
                                QDInfo.DBL.IsOnlineLinked,
                                backup.ExecuteScalar("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.IsOnlineLinked)
                            );


                            sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                QDInfo.DBL.AlwaysPromptPassword,
                                backup.ExecuteScalar("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.AlwaysPromptPassword)
                            );

                            sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                QDInfo.DBL.SetupSuccess,
                                backup.ExecuteScalar("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.SetupSuccess)    
                            );

                            try
                            {
                                sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                    QDInfo.DBL.DBHost,
                                    Cipher.Encrypt(Cipher.Decrypt(backup.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBHost), QDInfo.GlobalCipherKey), QDInfo.LocalCipherKey)
                                );
                            }
                            catch { sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DBHost, DBNull.Value); }

                            try
                            {
                                sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                    QDInfo.DBL.DBName,
                                    Cipher.Encrypt(Cipher.Decrypt(backup.ExecuteScalar<string>($@"SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBName), QDInfo.GlobalCipherKey), QDInfo.LocalCipherKey)
                                );
                            }
                            catch { sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DBName, DBNull.Value); }

                            try
                            {
                                sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                    QDInfo.DBL.DBUsername,
                                    Cipher.Encrypt(Cipher.Decrypt(backup.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBUsername), QDInfo.GlobalCipherKey), QDInfo.LocalCipherKey)
                                );
                            }
                            catch { sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DBUsername, DBNull.Value); }

                            try
                            {
                                sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                    QDInfo.DBL.DBPassword,
                                    Cipher.Encrypt(Cipher.Decrypt(backup.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBPassword), QDInfo.GlobalCipherKey), QDInfo.LocalCipherKey)
                                );
                            }
                            catch { sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DBPassword, DBNull.Value); }

                            sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                QDInfo.DBL.DefaultUsername,
                                Cipher.Decrypt(backup.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultUsername), QDInfo.GlobalCipherKey)
                            );

                            try
                            {
                                sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)",
                                    QDInfo.DBL.DefaultPassword,
                                    Cipher.Encrypt(Cipher.Decrypt(backup.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultPassword), QDInfo.GlobalCipherKey), QDInfo.LocalCipherKey)
                                );
                            }
                            catch { sqlite.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultPassword, DBNull.Value); }

                            using (SQLiteDataReader reader = (SQLiteDataReader)backup.ExecuteQuery("SELECT * FROM qd_drives"))
                            {
                                while (reader.Read())
                                {
                                    sqlite.ExecuteNonQuery("INSERT INTO qd_drives (ID, LocalPath, RemotePath, Username, Password, Domain, DriveLetter, DriveName) VALUES (?,?,?,?,?,?,?,?)",
                                        Guid.NewGuid(),
                                        Cipher.Decrypt(Convert.ToString(reader["LocalPath"]), QDInfo.GlobalCipherKey),
                                        Cipher.Decrypt(Convert.ToString(reader["RemotePath"]), QDInfo.GlobalCipherKey),
                                        Cipher.Encrypt(Cipher.Decrypt(Convert.ToString(reader["Username"]), QDInfo.GlobalCipherKey), QDInfo.LocalCipherKey),
                                        Cipher.Encrypt(Cipher.Decrypt(Convert.ToString(reader["Password"]), QDInfo.GlobalCipherKey), QDInfo.LocalCipherKey),
                                        Cipher.Encrypt(Cipher.Decrypt(Convert.ToString(reader["Domain"]), QDInfo.GlobalCipherKey), QDInfo.LocalCipherKey),
                                        Convert.ToString(reader["DriveLetter"]),
                                        Convert.ToString(reader["DriveName"])
                                    );
                                }
                            }

                            success = true;

                            sqlite.TransactionCommit();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            sqlite.TransactionRollback();
                        }

                        sqlite.Close();
                        backup.Close();

                        if (success) MessageBox.Show("Backup loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("An error occured trying to load the backup. please try again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch
                    {
                        MessageBox.Show("Could not load the Q-Drive backup. Please make sure the file is a valid *.qdbackup file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void tsmEnableAutostart_Click(object sender, EventArgs e)
        {
            //string autoStartLinkFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Q-Drive.lnk");
            //if(File.Exists("Q-Drive.lnk"))
            //    File.Copy("Q-Drive.lnk", autoStartLinkFile);

            QDLib.AddToAutostart();
            MessageBox.Show("Added Q-Drive to your Autostart.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserEnabledAutostart, dbData, logUserActions);
        }

        private void tsmDisableAutostart_Click(object sender, EventArgs e)
        {
            //string autoStartLinkFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Q-Drive.lnk");
            //if(File.Exists(autoStartLinkFile))
            //    File.Delete(autoStartLinkFile);

            QDLib.RemoveFromAutostart();
            MessageBox.Show("Removed Q-Drive to your Autostart.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserDisabledAutostart, dbData, logUserActions);
        }

        private void tsmRunQDriveSetup_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to reconfigure Q-Drive?\r\n\r\nAny localy saved data will be gone once the QD-Setup has finished. Do you want to continue?", "Q-Drive Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                if (QDLib.RunQDriveSetup()) this.Close();
        }

        private void tsmChangeOnlineDBConnection_Click(object sender, EventArgs e)
        {
            QDDatabaseConnection dbConForm = new QDDatabaseConnection() { dbConDat = dbData };

            bool success = false;

            if(dbConForm.ShowDialog() == DialogResult.OK)
            {
                if (!QDLib.ManagedDBOpen(sqlite)) { QDLib.DBOpenFailed(); return; }
                sqlite.TransactionBegin();

                try
                {
                    sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(dbConForm.dbConDat.Hostname, QDInfo.LocalCipherKey), QDInfo.DBL.DBHost);
                    sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(dbConForm.dbConDat.Database, QDInfo.LocalCipherKey), QDInfo.DBL.DBName);
                    sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(dbConForm.dbConDat.Username, QDInfo.LocalCipherKey), QDInfo.DBL.DBUsername);
                    sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(dbConForm.dbConDat.Password, QDInfo.LocalCipherKey), QDInfo.DBL.DBPassword);
                    sqlite.TransactionCommit();
                    success = true;
                }
                catch
                {
                    sqlite.TransactionRollback();
                }

                sqlite.Close();

                if (success) MessageBox.Show("Successfully updated Online-DB connection. Please restart the program for the changes to take effect.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("Could not updated Online-DB connection. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmResetLocalDatabase_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete any localy saved Q-Drive data? Please consider creating a backup of your current Q-Drive configuration. Do you want to continue and reset Q-Drive?", "Reset local database", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    sqlite.ExecuteNonQueryACon("DELETE FROM qd_drives");
                    MessageBox.Show("Sucessfully deleted drive-info for local drives.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserResetLocalDatabase, dbData, logUserActions);
                }
                catch
                {
                    MessageBox.Show("Could not delete local Q-Drive data. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            UpdateManagerData();
        }

        private void tsmUpdateReconnectDrives_Click(object sender, EventArgs e)
        {
            if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.UserDrivelistUpdated, dbData, logUserActions);
            UpdateManagerData();
        }

        #endregion

        #region Methods =========================================================================================[RF]=

        private void ShowAddDriveDialog()
        {
            int connectionOption = 0;
            if (!localConnection)
            {
                QDAddDriveSelector selector = new QDAddDriveSelector()
                {
                    CanAddPrivateDrive = userCanAddPrivateDrive,
                    CanAddPublicDrive = userCanAddPublicDrive
                };

                if (selector.ShowDialog() == DialogResult.OK) connectionOption = selector.SelectedOption;
            }
            else connectionOption = 3;


            if (connectionOption == 1)
            {
                QDAddPublicDrive addPublic = new QDAddPublicDrive() 
                { 
                    DBData = dbData,
                    Username = ndUsername,
                    Password = ndPassword,
                    Domain = ndDefaultDomain,
                    ForceAutofill = forceLoginDriveAuth
                };

                if (addPublic.ShowDialog() == DialogResult.OK)
                {
                    mysql.ExecuteNonQueryACon("INSERT INTO qd_assigns (ID, UserID, DriveID, CustomDriveName, CustomDriveLetter, DUsername, DPassword, DDomain) VALUES (?,?,?,?,?,?,?,?)",
                        Guid.NewGuid(),
                        userID,
                        addPublic.DriveID,
                        addPublic.CustomDriveName,
                        addPublic.CustomDriveLetter,
                        Cipher.Encrypt(addPublic.Username, uPassword),
                        Cipher.Encrypt(addPublic.Password, uPassword),
                        Cipher.Encrypt(addPublic.Domain, uPassword)
                    );

                    if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.DrivePublicAdded, dbData, logUserActions);
                    UpdateManagerData();
                }
            }
            else if (connectionOption == 2 || connectionOption == 3)
            {
                QDAddPrivateDrive addPrivate = new QDAddPrivateDrive()
                {
                    Username = ndUsername,
                    Password = ndPassword,
                    Domain = ndDefaultDomain,
                    ForceAutofill = forceLoginDriveAuth
                };

                if (addPrivate.ShowDialog() == DialogResult.OK)
                {
                    if (connectionOption == 2)
                    {
                        int mboxMessage;

                        if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return; }
                        mysql.TransactionBegin();
                        try
                        {
                            Guid driveGuid = Guid.NewGuid();

                            mysql.ExecuteNonQuery("INSERT INTO qd_drives (ID, DefaultName, DefaultDriveLetter, LocalPath, IsPublic, IsDeployable) VALUES (?,?,?,?,?,?)",
                                driveGuid,
                                addPrivate.DisplayName,
                                addPrivate.DriveLetter,
                                addPrivate.DrivePath,
                                false,
                                false
                            );

                            mysql.ExecuteNonQuery("INSERT INTO qd_assigns (ID, UserID, DriveID, CustomDriveName, CustomDriveLetter, DUsername, DPassword, DDomain) VALUES (?,?,?,?,?,?,?,?)",
                                Guid.NewGuid(),
                                userID,
                                driveGuid,
                                addPrivate.DisplayName,
                                addPrivate.DriveLetter,
                                Cipher.Encrypt(addPrivate.Username, uPassword),
                                Cipher.Encrypt(addPrivate.Password, uPassword),
                                Cipher.Encrypt(addPrivate.Domain, uPassword)
                            );

                            mboxMessage = 1;
                            mysql.TransactionCommit();
                        }
                        catch
                        {
                            mysql.TransactionRollback();
                            mboxMessage = 2;
                        }
                        mysql.Close();

                        if (mboxMessage == 1) MessageBox.Show("Successfully Added network-drive!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (mboxMessage == 2) MessageBox.Show("Could not add network drive. Please try again later.", "Could not add drive.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        try
                        {
                            Guid driveGuid = Guid.NewGuid();

                            sqlite.ExecuteNonQueryACon("INSERT INTO qd_drives (ID, LocalPath, Username, Password, Domain, DriveLetter, DriveName) VALUES (?,?,?,?,?,?,?)",
                                Guid.NewGuid().ToString(),
                                addPrivate.DrivePath,
                                Cipher.Encrypt(addPrivate.Username, QDInfo.LocalCipherKey),
                                Cipher.Encrypt(addPrivate.Password, QDInfo.LocalCipherKey),
                                Cipher.Encrypt(addPrivate.Domain, QDInfo.LocalCipherKey),
                                addPrivate.DriveLetter,
                                addPrivate.DisplayName
                            );

                            MessageBox.Show("Successfully Added network-drive!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch
                        {
                            MessageBox.Show("Could not add network drive. Please try again later.", "Could not add drive.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.DrivePrivateAdded, dbData, logUserActions);
                    UpdateManagerData();
                }
            }
        }
        
        private void UpdateManagerData()
        {
            if (localConnection) QDLib.ConnectQDDrives("", "", dbData, logUserActions, true, drives);
            else QDLib.ConnectQDDrives(userID, uPassword, dbData, logUserActions, true, drives);

            UpdateDriveListView();

            if (grvConnectedDrives.GroupViewItems.Count == 0) pbxNoDrivesConnected.Visible = true;
            else pbxNoDrivesConnected.Visible = false;


            lblQDriveManagerInfo.Text = 
            "Q-Drive Version " + QDInfo.QDVersion + Environment.NewLine + 
            Environment.NewLine + 
            "Connected Drives: " + grvConnectedDrives.GroupViewItems.Count + Environment.NewLine + 
            "Last updated: " + DateTime.Now.ToShortTimeString();
        }

        private int LoadQDData()
        {
            // Load local Data

            if (!QDLib.ManagedDBOpen(sqlite)) { QDLib.DBOpenFailed(); return -1; }

            localConnection = !Convert.ToBoolean(sqlite.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.IsOnlineLinked));
            promptPassword = Convert.ToBoolean(sqlite.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.AlwaysPromptPassword));

            uUsername = sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultUsername);
            uPassword = sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultPassword);

            dbData.Hostname = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBHost), QDInfo.LocalCipherKey);
            dbData.Username = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBUsername), QDInfo.LocalCipherKey);
            dbData.Password = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBPassword), QDInfo.LocalCipherKey);
            dbData.Database = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBName), QDInfo.LocalCipherKey);

            sqlite.Close();

            if (!string.IsNullOrEmpty(uPassword)) uPassword = Cipher.Decrypt(uPassword, QDInfo.LocalCipherKey);

            if (!localConnection)
            {
                try
                {
                    mysql = new WrapMySQL(dbData);

                    if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return -1; }
                    userCanToggleKeepLoggedIn = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanToggleKeepLoggedIn));
                    userCanAddPrivateDrive = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanAddPrivateDrive));
                    userCanAddPublicDrive = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanAddPublicDrive));
                    userCanSelfRegister = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanSelfRegister));
                    useLoginAsDriveAuth = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UseLoginAsDriveAuthentication));
                    forceLoginDriveAuth = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.ForceLoginAsDriveAuthentication));
                    logUserActions = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.LogUserActions));
                    userCanChangeManagerSettings = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanChangeManagerSettings));


                    if (useLoginAsDriveAuth) ndDefaultDomain = mysql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.DefaultDomain);

                    mysql.Close();

                    if (userCanChangeManagerSettings) cmsSettings.Enabled = true;
                    else cmsSettings.Enabled = false;

                    
                }
                catch { return 2; }
            }
            else
            {
                userCanToggleKeepLoggedIn = true;
                userCanAddPrivateDrive = true;
                userCanAddPublicDrive = false;
                userCanSelfRegister = false;
                useLoginAsDriveAuth = false;
                forceLoginDriveAuth = false;
                logUserActions = false;
                userCanChangeManagerSettings = true;
            }

            if (!promptPassword)
            {
                QDLib.VerifyPassword(localConnection, uUsername, uPassword, out userID, dbData);

                if(useLoginAsDriveAuth)
                {
                    ndUsername = uUsername;
                    ndPassword = uPassword;
                }
            }

            return 0;
        }

        private void UpdateDriveListView()
        {
            drives = QDLib.CreateDriveList(localConnection, userID, uPassword, dbData);


            grvConnectedDrives.GroupViewItems.Clear();

            // Populate groupview
            foreach (DriveViewItem drive in drives)
            {
                int imgIndex;

                if (drive.IsLocalDrive) imgIndex = 0;
                else if (drive.IsPublicDrive) imgIndex = 2;
                else imgIndex = 1;

                if (!Directory.Exists($"{drive.DriveLetter}:\\")) imgIndex += 3;

                grvConnectedDrives.GroupViewItems.Add(
                    new GroupViewItemEx(
                        $"({drive.DriveLetter}:\\) {drive.DisplayName}\r\n({drive.DrivePath})",
                        drive,
                        imgIndex
                    )
                );
            }

            if (!localConnection) QDLib.LogUserConnection(userID, QDLogAction.QDDrivelistUpdated, dbData, logUserActions);
        }

        #endregion


        private void tmrUpdateDriveStatus_Tick(object sender, EventArgs e)
        {
            try
            {
                // Disabled.
                // Syncfusion GroupView crashes on paint.
                //UpdateDriveListView();
            }
            catch { }
        }

        
    }

    public class GroupViewItemEx : Syncfusion.Windows.Forms.Tools.GroupViewItem
    {
        public DriveViewItem Drive { get; set; } = null;
        public GroupViewItemEx(string name, DriveViewItem drive, int image) : base(name, image) => this.Drive = drive;
    }
}
