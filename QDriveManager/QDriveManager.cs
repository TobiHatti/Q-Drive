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
            if (VerifyPassword(localConnection, txbUsername.Text, txbRegPassword.Text))
            {

            }
            else
            {
                MessageBox.Show("Username or password are invalid!", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        #endregion

        #region Step C: SignUp =======================================================================================

        private void txbRegName_TextChanged(object sender, EventArgs e) => txbRegUsername.Text = txbRegName.Text.Replace(' ', '.').ToLower();

        private void btnRegCancel_Click(object sender, EventArgs e) => pnlLogin.BringToFront();

        private void btnSignUp_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Step D: Manager ======================================================================================

        #endregion

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
                // Hash password and compare with online db
            }
            else
            {
                // Cipher password and compare with local db
            }

            return passwordValid;
        }
    }
}
