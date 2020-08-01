using MySql.Data;
using MySql.Data.MySqlClient;
using QDriveLib;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Schema;

namespace QDriveManager
{
    public partial class QDriveManager : SfForm
    {
        private bool localUserNoPassword = false;

        // General properties
        private bool localConnection = false;
        private bool promptPassword = false;

        private string uUsername = "";
        private string uPassword = "";

        // Online-specific properties
        private bool userCanToggleKeepLoggedIn = true;
        private bool userCanAddPrivateDrive = true;
        private bool userCanAddPublicDrive = true;
        private bool userCanSelfRegister = true;

        private string userID = "";

        private readonly WrapMySQLConDat dbData = new WrapMySQLConDat();

        private WrapSQLite sqlite = null;
        private WrapMySQL mysql = null;

        private readonly List<DriveViewItem> drives = new List<DriveViewItem>();

        #region Page Layout and Initial Loading =================================================================[RF]=

        private readonly List<Panel> panels = new List<Panel>();

        public QDriveManager(params string[] args)
        {
            InitializeComponent();

            panels.Add(pnlNotConfigured);
            panels.Add(pnlLogin);
            panels.Add(pnlSignUp);
            panels.Add(pnlManager);
            panels.Add(pnlLoading);

            this.Style.Border = new Pen(Color.FromArgb(77, 216, 255), 2);

            pbxLoginLogo.Image = Properties.Resources.QDriveProgramBanner;
            pbxSignUpLogo.Image = Properties.Resources.QDriveProgramBanner;

            QDLib.AlignPanels(this, panels, 800, 600);
        }

        private void QDriveManager_Load(object sender, EventArgs e)
        {
            sqlite = new WrapSQLite(QDInfo.ConfigFile, true);

            pnlLoading.BringToFront();

            if (!IsQDConfigured()) pnlNotConfigured.BringToFront();
            else
            {
                int loadStatusCode = LoadQDData();

                Image statusImage;

                if (localConnection) statusImage = Properties.Resources.QDriveLocalConnection;
                else statusImage = Properties.Resources.QDriveOnlineConnection;

                pbxLoginConnectionState.Image = statusImage;
                pbxSignUpConnectionState.Image = statusImage;
                pbxManagerConnectionState.Image = statusImage;

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


                if (!localUserNoPassword && (promptPassword || string.IsNullOrEmpty(uUsername) || string.IsNullOrEmpty(uPassword)))
                {
                    pnlLogin.BringToFront();
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
            string qdSetupProgram = "QDriveSetup.exe";

            if (File.Exists(qdSetupProgram))
            {
                Process.Start(qdSetupProgram);
                this.Close();
            }
            else MessageBox.Show($"Could not start setup!\r\n\r\nThe setup could not be started > The file \"{qdSetupProgram}\" could not be found. If this error remains, please try to re-install the program.", "Could not start setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region Step B: Login ===================================================================================[RF]=

        private void lnkCreateNewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => pnlSignUp.BringToFront();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(localUserNoPassword)
            {
                pnlManager.BringToFront();
                UpdateManagerData();
                return;
            }

            if (!VerifyPassword(localConnection, txbUsername.Text, txbPassword.Text))
            {
                MessageBox.Show("Username or password are invalid!", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sqlite.Open();
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

            txbPassword.Text = string.Empty;

            UpdateManagerData();

            pnlManager.BringToFront();
        }

        #endregion

        #region Step C: SignUp ==================================================================================[RF]=

        private void txbRegName_TextChanged(object sender, EventArgs e) => txbRegUsername.Text = txbRegName.Text.Replace(' ', '.').ToLower();

        private void btnRegCancel_Click(object sender, EventArgs e) => pnlLogin.BringToFront();

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            bool signupSuccess = false;

            if (!QDLib.ValidatePasswords(txbRegPassword.Text, txbRegConfirmPassword.Text)) return;

            if (mysql.ExecuteScalarACon<int>("SELECT COUNT(*) FROM qd_users WHERE Username = ?", txbRegUsername.Text) == 0)
            {
                try
                {
                    mysql.ExecuteNonQueryACon("INSERT INTO qd_users (ID, Name, Username, Password) VALUES (?,?,?,?)", Guid.NewGuid(), txbRegName.Text, txbRegUsername.Text, QDLib.HashPassword(txbRegPassword.Text));

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
                txbUsername.Text = txbRegUsername.Text;
                pnlLogin.BringToFront();
            }
        }

        #endregion

        #region Step D: Manager =================================================================================[RF]=

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            UpdateManagerData();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            QDLib.DisconnectAllDrives(drives);
            pnlLogin.BringToFront();
        }

        private void btnAddDrive_Click(object sender, EventArgs e)
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


            if(connectionOption == 1)
            {
                QDAddPublicDrive addPublic = new QDAddPublicDrive() { DBData = dbData };

                if(addPublic.ShowDialog() == DialogResult.OK)
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

                    UpdateManagerData();
                }
            }
            else if(connectionOption == 2 || connectionOption == 3)
            {
                QDAddPrivateDrive addPrivate = new QDAddPrivateDrive();

                if (addPrivate.ShowDialog() == DialogResult.OK)
                {
                    if(connectionOption == 2)
                    {
                        int mboxMessage = 0;

                        mysql.Open();
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
                      
                        if(mboxMessage == 1) MessageBox.Show("Successfully Added network-drive!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if(mboxMessage == 2) MessageBox.Show("Could not add network drive. Please try again later.", "Could not add drive.", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    UpdateManagerData();
                }
            }

            
        }

        private void grvConnectedDrives_Click(object sender, EventArgs e)
        {
            if(grvConnectedDrives.SelectedItem != -1)
            {
                btnEditDrive.Enabled = true;
                btnRemoveDrive.Enabled = true;
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
                if(drive.IsLocalDrive) sqlite.ExecuteNonQueryACon("DELETE FROM qd_drives WHERE ID = ?", drive.ID);
                else mysql.ExecuteNonQueryACon("DELETE FROM qd_assigns WHERE ID = ?", drive.ID);

                UpdateManagerData();

                btnEditDrive.Enabled = false;
                btnRemoveDrive.Enabled = false;
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
                    Domain = drive.Domain
                };

                if(editPublic.ShowDialog() == DialogResult.OK)
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
                    Domain = drive.Domain
                };

                if(editPrivate.ShowDialog() == DialogResult.OK)
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
                        mysql.Open();

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

                    UpdateManagerData();
                }
            }
        }

        #endregion

        #region Methods =========================================================================================[RF]=

        private void UpdateManagerData()
        {
            if (localConnection) QDLib.ConnectQDDrives("", "", dbData, true);
            else QDLib.ConnectQDDrives(userID, uPassword, dbData, true);

            UpdateDriveListView();
        }

        private bool IsQDConfigured()
        {
            if (File.Exists(QDInfo.ConfigFile))
            {
                object result = sqlite.ExecuteScalarACon("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.SetupSuccess);

                if (result != null && Convert.ToBoolean(Convert.ToInt16(result))) return true;
                else return false;
            }
            else return false;
        }

        private int LoadQDData()
        {
            // Load local Data

            sqlite.Open();

            localConnection = !Convert.ToBoolean(sqlite.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.IsOnlineLinked));
            promptPassword = Convert.ToBoolean(sqlite.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",  QDInfo.DBL.AlwaysPromptPassword));

            uUsername = sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultUsername);
            uPassword = sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultPassword);

            if (!string.IsNullOrEmpty(uPassword)) uPassword = Cipher.Decrypt(uPassword, QDInfo.LocalCipherKey);

            dbData.Hostname = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBHost), QDInfo.LocalCipherKey);
            dbData.Username = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBUsername), QDInfo.LocalCipherKey);
            dbData.Password = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBPassword), QDInfo.LocalCipherKey);
            dbData.Database = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBName), QDInfo.LocalCipherKey);

            sqlite.Close();

            if (!localConnection)
            {
                try
                {
                    mysql = new WrapMySQL(dbData);

                    mysql.Open();
                    userCanToggleKeepLoggedIn = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",   QDInfo.DBO.UserCanToggleKeepLoggedIn));
                    userCanAddPrivateDrive = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",      QDInfo.DBO.UserCanAddPrivateDrive));
                    userCanAddPublicDrive = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",       QDInfo.DBO.UserCanAddPublicDrive));
                    userCanSelfRegister = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",         QDInfo.DBO.UserCanSelfRegister));
                    mysql.Close();
                }
                catch { return 2; }
            }
            else
            {
                userCanToggleKeepLoggedIn = true;
                userCanAddPrivateDrive = true;
                userCanAddPublicDrive = false;
                userCanSelfRegister = false;
            }

            if (!promptPassword) VerifyPassword(localConnection, uUsername, uPassword);

            return 0;
        }

        private bool VerifyPassword(bool pIsLocalConnection, string pUsername, string pPassword)
        {
            bool passwordValid = false;

            if(pIsLocalConnection)
            {
                bool errorEncountered = false;
                sqlite.Open();
                try
                {
                    string dbUsername = sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultUsername);
                    string dbCipher = sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultPassword);

                    string pwDecrypt = Cipher.Decrypt(dbCipher, QDInfo.LocalCipherKey);
                    if (dbUsername == pUsername && pwDecrypt == pPassword) passwordValid = true;
                }
                catch
                {
                    errorEncountered = true;
                }
                sqlite.Close();

                if(errorEncountered) MessageBox.Show("An error occured whilst trying to authenticate the user.", "Authentication error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                mysql.Open();
                using (MySqlDataReader reader = mysql.ExecuteQuery("SELECT * FROM qd_users WHERE Username = ? AND Password = ?", pUsername, QDLib.HashPassword(pPassword)))
                {
                    while (reader.Read())
                    {
                        userID = Convert.ToString(reader["ID"]);
                        passwordValid = true;
                    }
                }
                mysql.Close();
            }

            return passwordValid;
        }

        private void UpdateDriveListView()
        {
            drives.Clear();

            sqlite.Open();
            using (SQLiteDataReader reader = sqlite.ExecuteQuery("SELECT * FROM qd_drives"))
            {
                while(reader.Read())
                {
                    drives.Add(new DriveViewItem(
                        Convert.ToString(reader["ID"]),
                        Convert.ToString(reader["DriveName"]),
                        Convert.ToString(reader["LocalPath"]),
                        Convert.ToString(reader["DriveLetter"]),
                        true,
                        false,
                        Cipher.Decrypt(Convert.ToString(reader["Username"]), QDInfo.LocalCipherKey),
                        Cipher.Decrypt(Convert.ToString(reader["Password"]), QDInfo.LocalCipherKey),
                        Cipher.Decrypt(Convert.ToString(reader["Domain"]), QDInfo.LocalCipherKey)
                    ));
                }
            }
            sqlite.Close();
 
            if (!localConnection)
            {
                mysql.Open();
                using (MySqlDataReader reader = mysql.ExecuteQuery("SELECT *, qd_assigns.ID as AID, qd_drives.ID AS DID FROM qd_drives INNER JOIN qd_assigns ON qd_drives.ID = qd_assigns.DriveID WHERE qd_assigns.UserID = ?", userID))
                {
                    while (reader.Read())
                    {
                        drives.Add(new DriveViewItem(
                        Convert.ToString(reader["AID"]),
                        Convert.ToString(reader["CustomDriveName"]),
                        Convert.ToString(reader["LocalPath"]),
                        Convert.ToString(reader["CustomDriveLetter"]),
                        false,
                        Convert.ToBoolean(Convert.ToInt16(reader["IsPublic"])),
                        Cipher.Decrypt(Convert.ToString(reader["DUsername"]), uPassword),
                        Cipher.Decrypt(Convert.ToString(reader["DPassword"]), uPassword),
                        Cipher.Decrypt(Convert.ToString(reader["DDomain"]), uPassword),
                        Convert.ToString(reader["DID"])
                    ));
                    }
                }
                mysql.Close();
            }

            drives.Sort();

            ImageList imgList = new ImageList()
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
        }

        #endregion

    }

    public class GroupViewItemEx : Syncfusion.Windows.Forms.Tools.GroupViewItem
    {
        public DriveViewItem Drive { get; set; } = null;
        public GroupViewItemEx(string name, DriveViewItem drive, int image) : base(name, image) => this.Drive = drive;
    }    
}
