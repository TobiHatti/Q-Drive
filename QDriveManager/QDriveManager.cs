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

        private string dbHost = "";
        private string dbUser = "";
        private string dbPass = "";
        private string dbName = "";
        private string userID = "";

        #region Page Layout and Initial Loading

        private List<Panel> panels = new List<Panel>();

        public QDriveManager(params string[] args)
        {
            InitializeComponent();

            panels.Add(pnlNotConfigured);
            panels.Add(pnlLogin);
            panels.Add(pnlSignUp);
            panels.Add(pnlManager);
            panels.Add(pnlLoading);

            this.Style.Border = new Pen(Color.FromArgb(77, 216, 255), 2);

            AlignPanels();
        }

        private void AlignPanels()
        {
            this.Width = 800;
            this.Height = 600;
            foreach (Panel panel in panels) panel.Dock = DockStyle.Fill;
        }

        private void QDriveManager_Load(object sender, EventArgs e)
        {
            pnlLoading.BringToFront();

            if(!IsQDConfigured())
            {
                pnlNotConfigured.BringToFront();
            }
            else
            {
                int loadStatusCode = LoadQDData();


                if(localConnection)
                {
                    pbxLoginConnectionState.Image = Properties.Resources.QDriveLocalConnection;
                    pbxSignUpConnectionState.Image = Properties.Resources.QDriveLocalConnection;
                    pbxManagerConnectionState.Image = Properties.Resources.QDriveLocalConnection;
                }
                else
                {
                    pbxLoginConnectionState.Image = Properties.Resources.QDriveOnlineConnection;
                    pbxSignUpConnectionState.Image = Properties.Resources.QDriveOnlineConnection;
                    pbxManagerConnectionState.Image = Properties.Resources.QDriveOnlineConnection;
                }

                pbxLoginLogo.Image = Properties.Resources.QDriveProgramBanner;
                pbxSignUpLogo.Image = Properties.Resources.QDriveProgramBanner;

                switch(loadStatusCode)
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
                    if (localConnection && Cipher.Decrypt(uPassword, QDInfo.LocalCipherKey) == string.Empty)
                    {
                        localUserNoPassword = true;
                        txbUsername.ReadOnly = true;
                        txbPassword.ReadOnly = true;
                        chbKeepLoggedIn.Enabled = false;
                        lblKeepLoggedInInfo.Visible = false;
                        lnkCreateNewAccount.Visible = false;
                    }
                }
                catch { }


                if (!localUserNoPassword && (promptPassword || string.IsNullOrEmpty(uUsername) || string.IsNullOrEmpty(uPassword)))
                {
                    if(userCanToggleKeepLoggedIn)
                    {
                        chbKeepLoggedIn.Checked = false;
                        chbKeepLoggedIn.Enabled = true;
                        lblKeepLoggedInInfo.Text = string.Empty;
                    }
                    else
                    {
                        chbKeepLoggedIn.Checked = false;
                        chbKeepLoggedIn.Enabled = false;
                        lblKeepLoggedInInfo.Text = "(disabled by administrator)";
                    }

                    if(userCanSelfRegister)
                    {
                        lnkCreateNewAccount.Enabled = true;
                        lnkCreateNewAccount.Text = "Create a new Account";
                    }
                    else
                    {
                        lnkCreateNewAccount.Enabled = false;
                        lnkCreateNewAccount.Text = "If you do not have an account yet, contact your system administrator.";
                    }

                    if(localConnection)
                    {
                        txbUsername.Text = "local";
                        txbUsername.ReadOnly = true;
                        lnkCreateNewAccount.Visible = false;
                    }
                    else
                    {
                        txbUsername.Text = string.Empty;
                        txbUsername.ReadOnly = false;
                        lnkCreateNewAccount.Visible = true;
                    }

                    pnlLogin.BringToFront();
                }
                else
                    pnlManager.BringToFront();
            }
        }

        #endregion

        #region Step A: Not Configured ===============================================================================
        private void btnRunSetup_Click(object sender, EventArgs e)
        {
            string qdSetupProgram = "QDriveSetup.exe";

            if (File.Exists(qdSetupProgram))
            {
                Process.Start(qdSetupProgram);
                this.Close();
            }
            else
            {
                MessageBox.Show($"Could not start setup!\r\n\r\nThe setup could not be started > The file \"{qdSetupProgram}\" could not be found. If this error remains, please try to re-install the program.", "Could not start setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Step B: Login ========================================================================================

        private void lnkCreateNewAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => pnlSignUp.BringToFront();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(localUserNoPassword)
            {
                pnlManager.BringToFront();
                return;
            }

            if (!VerifyPassword(localConnection, txbUsername.Text, txbPassword.Text))
            {
                MessageBox.Show("Username or password are invalid!", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            using(WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile, true))
            {
                sql.Open();
                sql.TransactionBegin();
                try
                {
                    if (chbKeepLoggedIn.Checked)
                    {
                        sql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", false, QDInfo.DBL.AlwaysPromptPassword);
                        
                        if(!localConnection)
                        {
                            sql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", txbUsername.Text, QDInfo.DBL.DefaultUsername);
                            sql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(txbPassword.Text, QDInfo.LocalCipherKey), QDInfo.DBL.DefaultPassword);
                        }
                    }
                    else
                    {
                        sql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", true, QDInfo.DBL.AlwaysPromptPassword);

                        if (!localConnection)
                        {
                            sql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", DBNull.Value, QDInfo.DBL.DefaultUsername);
                            sql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", DBNull.Value, QDInfo.DBL.DefaultPassword);
                        }
                    }

                    sql.TransactionCommit();
                }
                catch
                {
                    sql.TransactionRollback();
                }

                sql.Close();
            }

            txbPassword.Text = string.Empty;

            uUsername = txbUsername.Text;
            uPassword = txbPassword.Text;
           
            if (localConnection) QDLib.ConnectQDDrives("", "", dbHost, dbName, dbUser, dbPass, true);
            else QDLib.ConnectQDDrives(userID, uPassword, dbHost, dbName, dbUser, dbPass, true);

            UpdateDriveListView();

            pnlManager.BringToFront();
        }

        #endregion

        #region Step C: SignUp =======================================================================================

        private void txbRegName_TextChanged(object sender, EventArgs e) => txbRegUsername.Text = txbRegName.Text.Replace(' ', '.').ToLower();

        private void btnRegCancel_Click(object sender, EventArgs e) => pnlLogin.BringToFront();

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            bool signupSuccess = false;

            if (txbRegPassword.Text != txbRegConfirmPassword.Text)
            {
                MessageBox.Show("Passwords are not identical.", "Password invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (string.IsNullOrEmpty(txbRegPassword.Text))
            {
                MessageBox.Show("Please enter a password.", "Password invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using(WrapMySQL sql = new WrapMySQL(dbHost, dbName, dbUser, dbPass))
            {
                sql.Open();

                if (sql.ExecuteScalar<int>("SELECT COUNT(*) FROM qd_users WHERE Username = ?", txbRegUsername.Text) == 0)
                {
                    sql.TransactionBegin();

                    try
                    {
                        sql.ExecuteNonQuery("INSERT INTO qd_users (ID, Name, Username, Password) VALUES (?,?,?,?)", Guid.NewGuid(), txbRegName.Text, txbRegUsername.Text, QDLib.HashPassword(txbRegPassword.Text));

                        MessageBox.Show("User signed up successfully! Please log in with your username and password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sql.TransactionCommit();
                        signupSuccess = true;
                    }
                    catch
                    {
                        MessageBox.Show("An error occured whilst trying to register the user.", "Server error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sql.TransactionRollback();
                    }
                }
                else
                {
                    MessageBox.Show("Username already in use. Please choose another username", "Username already in use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                sql.Close();
            }

            if (signupSuccess)
            {
                txbUsername.Text = txbRegUsername.Text;
                pnlLogin.BringToFront();
            }
        }

        #endregion

        #region Step D: Manager ======================================================================================

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            if (localConnection) QDLib.ConnectQDDrives("", "", dbHost, dbName, dbUser, dbPass, true);
            else QDLib.ConnectQDDrives(userID, uPassword, dbHost, dbName, dbUser, dbPass, true);

            UpdateDriveListView();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            QDLib.DisconnectAllDrives();
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

                if (selector.ShowDialog() == DialogResult.OK)
                {
                    connectionOption = selector.SelectedOption;
                }
            }
            else connectionOption = 3;


            if(connectionOption == 1)
            {
                QDAddPublicDrive addPublic = new QDAddPublicDrive()
                {
                    DBHost = dbHost,
                    DBName = dbName,
                    DBUser = dbUser,
                    DBPass = dbPass
                };

                if(addPublic.ShowDialog() == DialogResult.OK)
                {

                }
            }
            else if(connectionOption == 2 || connectionOption == 3)
            {
                QDAddPrivateDrive addPrivate = new QDAddPrivateDrive();

                if (addPrivate.ShowDialog() == DialogResult.OK)
                {
                    if(connectionOption == 2)
                    {
                        using (WrapMySQL sql = new WrapMySQL(dbHost, dbName, dbUser, dbPass))
                        {
                            sql.Open();
                            sql.TransactionBegin();
                            try
                            {
                                Guid driveGuid = Guid.NewGuid();

                                sql.ExecuteNonQuery("INSERT INTO qd_drives (ID, DefaultName, DefaultDriveLetter, LocalPath, IsPublic, IsDeployable) VALUES (?,?,?,?,?,?)",
                                    driveGuid,
                                    addPrivate.DisplayName,
                                    addPrivate.DriveLetter,
                                    addPrivate.DrivePath,
                                    false,
                                    false
                                );

                                sql.ExecuteNonQuery("INSERT INTO qd_assigns (ID, UserID, DriveID, CustomDriveName, CustomDriveLetter, DUsername, DPassword, DDomain) VALUES (?,?,?,?,?,?,?,?)",
                                    Guid.NewGuid(),
                                    userID,
                                    driveGuid,
                                    addPrivate.DisplayName,
                                    addPrivate.DriveLetter,
                                    Cipher.Encrypt(addPrivate.Username, uPassword),
                                    Cipher.Encrypt(addPrivate.Password, uPassword),
                                    Cipher.Encrypt(addPrivate.Domain, uPassword)
                                );

                                MessageBox.Show("Successfully Added network-drive!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                sql.TransactionCommit();
                            }
                            catch
                            {
                                sql.TransactionRollback();
                                MessageBox.Show("Could not add network drive. Please try again later.", "Could not add drive.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            sql.Close();
                        }
                    }
                    else
                    {
                        using (WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile, true))
                        {
                            sql.Open();
                            sql.TransactionBegin();
                            try
                            {
                                Guid driveGuid = Guid.NewGuid();

                                sql.ExecuteNonQuery("INSERT INTO qd_drives (ID, LocalPath, Username, Password, Domain, DriveLetter, DriveName) VALUES (?,?,?,?,?,?,?)",
                                    Guid.NewGuid().ToString(),
                                    addPrivate.DrivePath,
                                    Cipher.Encrypt(addPrivate.Username, QDInfo.LocalCipherKey),
                                    Cipher.Encrypt(addPrivate.Password, QDInfo.LocalCipherKey),
                                    Cipher.Encrypt(addPrivate.Domain, QDInfo.LocalCipherKey),
                                    addPrivate.DriveLetter,
                                    addPrivate.DisplayName
                                );

                                MessageBox.Show("Successfully Added network-drive!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                sql.TransactionCommit();
                            }
                            catch
                            {
                                sql.TransactionRollback();
                                MessageBox.Show("Could not add network drive. Please try again later.", "Could not add drive.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            sql.Close();
                        }
                    }
                }
            }

            QDLib.ConnectQDDrives(userID, uPassword, dbHost, dbName, dbUser, dbPass, true);
            UpdateDriveListView();
        }

        private void grvConnectedDrives_GroupViewItemSelected(object sender, EventArgs e)
        {
            btnEditDrive.Enabled = true;
            btnRemoveDrive.Enabled = true;
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
            DriveViewItem drive = ((GroupViewItemEx)sender.GroupViewItems[sender.SelectedItem]).Drive;
            Process.Start(drive.DriveLetter + ":\\");
        }

        private void btnRemoveDrive_Click(object sender, EventArgs e)
        {
            DriveViewItem drive = ((GroupViewItemEx)grvConnectedDrives.GroupViewItems[grvConnectedDrives.SelectedItem]).Drive;

            if (MessageBox.Show("Do you really want to remove the selected drive from your drive-list?", "Remove Drive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(drive.IsLocalDrive)
                {
                    using(WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile, true))
                    {
                        sql.ExecuteNonQueryACon("DELETE FROM qd_drives WHERE ID = ?", drive.ID);
                    }
                }
                else
                {
                    using (WrapMySQL sql = new WrapMySQL(dbHost, dbName, dbUser, dbPass))
                    {
                        sql.ExecuteNonQueryACon("DELETE FROM qd_assigns WHERE ID = ?", drive.ID);
                    }
                }

                QDLib.ConnectQDDrives(userID, uPassword, dbHost, dbName, dbUser, dbPass, true);
                UpdateDriveListView();

                btnEditDrive.Enabled = false;
                btnRemoveDrive.Enabled = false;
            }

        }

        private void btnEditDrive_Click(object sender, EventArgs e)
        {
            DriveViewItem drive = ((GroupViewItemEx)grvConnectedDrives.GroupViewItems[grvConnectedDrives.SelectedItem]).Drive;
        }

        #endregion

        #region Methods ==============================================================================================

        private bool IsQDConfigured()
        {
            bool configurationFinished = false;

            if (File.Exists(QDInfo.ConfigFile))
            {
                using (WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile, true))
                {
                    object result = sql.ExecuteScalarACon("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.SetupSuccess);

                    if (result != null && Convert.ToBoolean(Convert.ToInt16(result))) configurationFinished = true;
                    else configurationFinished = false;
                }
            }
            else configurationFinished = false;

            return configurationFinished;
        }

        private int LoadQDData()
        {
            // Load local Data
            using (WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile, true))
            {
                sql.Open();
                sql.TransactionBegin();
                try
                {
                    localConnection = !Convert.ToBoolean(sql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.IsOnlineLinked));
                    promptPassword = Convert.ToBoolean(sql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",  QDInfo.DBL.AlwaysPromptPassword));

                    uUsername = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultUsername);
                    uPassword = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultPassword);

                    dbHost = Cipher.Decrypt(sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBHost), QDInfo.LocalCipherKey);
                    dbUser = Cipher.Decrypt(sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBUsername), QDInfo.LocalCipherKey);
                    dbPass = Cipher.Decrypt(sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBPassword), QDInfo.LocalCipherKey);
                    dbName = Cipher.Decrypt(sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBName), QDInfo.LocalCipherKey);

                    sql.TransactionCommit();
                }
                catch
                {
                    sql.TransactionRollback();
                    sql.Close();
                    return 1;
                }
            }

            if(!localConnection)
            {
                try
                {
                    using (WrapMySQL sql = new WrapMySQL(dbHost, dbName, dbUser, dbPass))
                    {
                        sql.Open();
                        sql.TransactionBegin();
                        try
                        {
                            userCanToggleKeepLoggedIn = Convert.ToBoolean(sql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",   QDInfo.DBO.UserCanToggleKeepLoggedIn));
                            userCanAddPrivateDrive = Convert.ToBoolean(sql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",      QDInfo.DBO.UserCanAddPrivateDrive));
                            userCanAddPublicDrive = Convert.ToBoolean(sql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",       QDInfo.DBO.UserCanAddPublicDrive));
                            userCanSelfRegister = Convert.ToBoolean(sql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?",         QDInfo.DBO.UserCanSelfRegister));

                            sql.TransactionCommit();
                        }
                        catch
                        {
                            sql.TransactionRollback();
                            sql.Close();
                            return 3;
                        }
                    }
                }
                catch
                {
                    return 2;
                }
            }
            else
            {
                userCanToggleKeepLoggedIn = true;
                userCanAddPrivateDrive = true;
                userCanAddPublicDrive = false;
                userCanSelfRegister = false;
            }

            return 0;
        }

        private bool VerifyPassword(bool pIsLocalConnection, string pUsername, string pPassword)
        {
            bool passwordValid = false;

            if(pIsLocalConnection)
            {
                using (WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile, true))
                {
                    sql.Open();
                    try
                    {
                        string dbUsername = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultUsername);
                        string dbCipher = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultPassword);

                        string pwDecrypt = Cipher.Decrypt(dbCipher, QDInfo.LocalCipherKey);
                        if (dbUsername == pUsername && pwDecrypt == pPassword) passwordValid = true;
                    }
                    catch
                    {
                        MessageBox.Show("An error occured whilst trying to authenticate the user.", "Authentication error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sql.Close();
                }
            }
            else
            {
                using (WrapMySQL sql = new WrapMySQL(dbHost, dbName, dbUser, dbPass))
                {
                    sql.Open();
                    using (MySqlDataReader reader = sql.ExecuteQuery("SELECT * FROM qd_users WHERE Username = ? AND Password = ?", pUsername, QDLib.HashPassword(pPassword)))
                    {
                        while (reader.Read())
                        {
                            userID = Convert.ToString(reader["ID"]);
                            passwordValid = true;
                        }
                    }
                    sql.Close();
                }
            }

            return passwordValid;
        }

        private void UpdateDriveListView()
        {
            List<DriveViewItem> drives = new List<DriveViewItem>();

            using(WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile, true))
            {
                sql.Open();
                using (SQLiteDataReader reader = sql.ExecuteQuery("SELECT * FROM qd_drives"))
                {
                    while(reader.Read())
                    {
                        drives.Add(new DriveViewItem(
                            Convert.ToString(reader["ID"]),
                            Convert.ToString(reader["DriveName"]),
                            Convert.ToString(reader["LocalPath"]),
                            Convert.ToString(reader["DriveLetter"]),
                            true,
                            false
                        ));
                    }
                }
                sql.Close();
            }

            if (!localConnection)
            {
                using (WrapMySQL sql = new WrapMySQL(dbHost, dbName, dbUser, dbPass))
                {
                    sql.Open();
                    using (MySqlDataReader reader = sql.ExecuteQuery("SELECT *, qd_assigns.ID as AID FROM qd_drives INNER JOIN qd_assigns ON qd_drives.ID = qd_assigns.DriveID WHERE qd_assigns.UserID = ?", userID))
                    {
                        while (reader.Read())
                        {
                            drives.Add(new DriveViewItem(
                            Convert.ToString(reader["AID"]),
                            Convert.ToString(reader["CustomDriveName"]),
                            Convert.ToString(reader["LocalPath"]),
                            Convert.ToString(reader["CustomDriveLetter"]),
                            false,
                            Convert.ToBoolean(Convert.ToInt16(reader["IsPublic"]))
                        ));
                        }
                    }
                    sql.Close();
                }
            }

            // Sort List
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

    public class DriveViewItem : IComparable
    {
        public string ID { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string DrivePath { get; set; } = string.Empty;
        public string DriveLetter { get; set; } = string.Empty;
        public bool IsLocalDrive { get; set; } = false;
        public bool IsPublicDrive { get; set; } = false;

        public DriveViewItem(string pID, string pDisplayName, string pDrivePath, string pDriveLetter, bool pIsLocalDrive, bool pIsPublicDrive)
        {
            ID = pID;
            DisplayName = pDisplayName;
            DrivePath = pDrivePath;
            DriveLetter = pDriveLetter;
            IsLocalDrive = pIsLocalDrive;
            IsPublicDrive = pIsPublicDrive;
        }

        public int CompareTo(object obj)
        {
            if (Convert.ToInt32(Convert.ToChar((obj as DriveViewItem).DriveLetter)) > Convert.ToInt32(Convert.ToChar(DriveLetter))) return -1;
            else return 1;
        }
    }
}
