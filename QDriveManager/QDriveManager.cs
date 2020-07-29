using MySql.Data.MySqlClient;
using QDriveLib;
using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using System.Xml.Schema;

namespace QDriveManager
{
    public partial class QDriveManager : SfForm
    {
        // General properties
        private bool localConnection = false;
        private bool promptPassword = false;

        private string defaultUsername = "";
        private string defaultPassword = "";

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

                if (promptPassword || string.IsNullOrEmpty(defaultUsername) || string.IsNullOrEmpty(defaultPassword))
                {
                    if(userCanToggleKeepLoggedIn)
                    {
                        chbKeepLoggedIn.Checked = promptPassword;
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

            if (signupSuccess) pnlLogin.BringToFront();
        }

        #endregion

        #region Step D: Manager ======================================================================================



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

                    defaultUsername = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultUsername);
                    defaultPassword = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultPassword);

                    dbHost = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBHost);
                    dbUser = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBUsername);
                    dbPass = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBPassword);
                    dbName = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBName);
                    userID = sql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.UserID);

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

        #endregion
    }
}
