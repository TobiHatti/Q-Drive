using QDriveLib;
using QDriveManager;
using Syncfusion.Windows.Forms;
using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QDrive
{
    public partial class QDriveSetup : SfForm
    {
        private bool localConnection = false;

        private bool alwaysPromptPassword = false;
        private string localPassword = string.Empty;

        private string onlineDBHost = string.Empty;
        private string onlineDBUsername = string.Empty;
        private string onlineDBPassword = string.Empty;
        private string onlineDBName = string.Empty;

        private bool onlineAlreadyConfigured;

        private string onlineMasterPassword = string.Empty;
        private bool onlineConfigureAsNewDB;

        private bool errorEncountered = false;

        #region Page Layout and Initial Loading

        private List<Panel> panels = new List<Panel>();
        
        public QDriveSetup()
        {
            InitializeComponent();

            panels.Add(pnlS0Welcome);
            panels.Add(pnlS1ConnectionType);
            panels.Add(pnlS2LocalConnection);
            panels.Add(pnlS2OnlineConnectionA);
            panels.Add(pnlS2OnlineConnectionB);
            panels.Add(pnlS3Finish);
            panels.Add(pnlS3Error);

            AlignPanels();
        }

        private void AlignPanels()
        {
            this.Width = 680;
            this.Height = 500;
            foreach(Panel panel in panels) panel.Dock = DockStyle.Fill;
        }

        private void QDriveSetup_Load(object sender, EventArgs e) => pnlS0Welcome.BringToFront();

        #endregion

        #region Step 0: Welcome ======================================================================================

        private void btnS0Next_Click(object sender, EventArgs e) => pnlS1ConnectionType.BringToFront();


        #endregion

        #region Step 1: Connection type ==============================================================================

        private void btnS1Prev_Click(object sender, EventArgs e) => pnlS0Welcome.BringToFront();

        private void btnS1Next_Click(object sender, EventArgs e)
        {
            if(rbnS1Local.Checked)
            {
                localConnection = true;
                pnlS2LocalConnection.BringToFront();
            }
            else
            {
                localConnection = false;
                pnlS2OnlineConnectionA.BringToFront();
            }
        }

        #endregion

        #region Step 2A: Local connection ============================================================================
        
        private void chbSA2PromptPassword_CheckedChanged(object sender, EventArgs e)
        {
            if(chbSA2PromptPassword.Checked)
            {
                txbSA2Password.Enabled = true;
                txbSA2ConfirmPassword.Enabled = true;
                lblSA2Password.Enabled = true;
                lblSA2ConfirmPassword.Enabled = true;
            }
            else
            {
                txbSA2Password.Enabled = false;
                txbSA2ConfirmPassword.Enabled = false;
                lblSA2Password.Enabled = false;
                lblSA2ConfirmPassword.Enabled = false;
            }
        }

        private void btnSA2Prev_Click(object sender, EventArgs e) => pnlS1ConnectionType.BringToFront();

        private void btnSA2Next_Click(object sender, EventArgs e)
        {
            alwaysPromptPassword = chbSA2PromptPassword.Checked;

            if(alwaysPromptPassword)
            {
                if (txbSA2Password.Text != txbSA2ConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords are not identical.", "Password invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if(string.IsNullOrEmpty(txbSA2Password.Text))
                {
                    MessageBox.Show("Please enter a password.", "Password invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    localPassword = txbSA2Password.Text;
                }
            }

            SaveConfiguration(localConnection);

            if (!errorEncountered) pnlS3Finish.BringToFront();
            else pnlS3Error.BringToFront();
        }


        #endregion

        #region Step 2B1: Global connection 1/2 ======================================================================

        private void btnSB2TestConnection_Click(object sender, EventArgs e)
        {
            onlineDBHost = txbSB2DBHostname.Text;
            onlineDBUsername = txbSB2DBUsername.Text;
            onlineDBPassword = txbSB2DBPassword.Text;
            onlineDBName = txbSB2DBName.Text;

            TestConnection();
        }

        private void btnSB2APrev_Click(object sender, EventArgs e) => pnlS1ConnectionType.BringToFront();

        private void btnSB2ANext_Click(object sender, EventArgs e)
        {
            onlineDBHost = txbSB2DBHostname.Text;
            onlineDBUsername = txbSB2DBUsername.Text;
            onlineDBPassword = txbSB2DBPassword.Text;
            onlineDBName = txbSB2DBName.Text;

            if (TestConnection())
            {
                pnlS2OnlineConnectionB.BringToFront();


                if(IsConfiguredDB())
                {
                    rbnSB2ExistingDB.Enabled = true;
                    txbSB2ExistingDBPassword.Enabled = true;
                    lblSB2AExistingMasterPW.Enabled = true;

                    rbnSB2ExistingDB.Checked = true;

                    onlineAlreadyConfigured = true;
                }
                else
                {
                    rbnSB2ExistingDB.Enabled = false;
                    txbSB2ExistingDBPassword.Enabled = false;
                    lblSB2AExistingMasterPW.Enabled = false;

                    rbnSB2NewDB.Checked = true;

                    onlineAlreadyConfigured = false;
                }
            }
            else return;
        }

        #endregion

        #region Step 2B2: Global connection 2/2 ======================================================================

        private void rbnSB2ExistingDB_CheckedChanged(object sender, EventArgs e) => EnableDisableNewExisting();

        private void rbnSB2NewDB_CheckedChanged(object sender, EventArgs e) => EnableDisableNewExisting();

        private void EnableDisableNewExisting()
        {
            if(rbnSB2ExistingDB.Checked)
            {
                lblSB2AExistingMasterPW.Enabled = true;
                txbSB2ExistingDBPassword.Enabled = true;

                lblSB2BMasterPassword.Enabled = false;
                lblSB2BConfirmMasterPassword.Enabled = false;
                txbSB2NewDBPassword.Enabled = false;
                txbSB2NewDBConfirmPassword.Enabled = false;
            }
            else
            {
                lblSB2AExistingMasterPW.Enabled = false;
                txbSB2ExistingDBPassword.Enabled = false;

                lblSB2BMasterPassword.Enabled = true;
                lblSB2BConfirmMasterPassword.Enabled = true;
                txbSB2NewDBPassword.Enabled = true;
                txbSB2NewDBConfirmPassword.Enabled = true;
            }
        }

        private void btnSB2BNext_Click(object sender, EventArgs e)
        {
            if(rbnSB2ExistingDB.Checked)
            {
                if(!QDLib.VerifyMasterPassword(txbSB2ExistingDBPassword.Text, onlineDBHost, onlineDBName, onlineDBUsername, onlineDBPassword))
                {
                    MessageBox.Show("Master-Password is not valid. Please enter the corrent Master-Password, which has been set when the database was first initialised.", "Invalid Master-Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    onlineMasterPassword = txbSB2ExistingDBPassword.Text;
                }
            }
            else
            {
                if (txbSB2NewDBPassword.Text != txbSB2NewDBConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords are not identical.", "Password invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (string.IsNullOrEmpty(txbSB2NewDBPassword.Text))
                {
                    MessageBox.Show("Please enter a password.", "Password invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    onlineMasterPassword = txbSB2NewDBPassword.Text;
                }

                if (onlineAlreadyConfigured)
                {
                    if (MessageBox.Show("The given database has already been initialised before. Do you really want to delete the existing database and configure it as a new one?", "Database already initialised", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        VerifyMasterPW verify = new VerifyMasterPW()
                        {
                            DBHost = onlineDBHost,
                            DBUser = onlineDBUsername,
                            DBPass = onlineDBPassword,
                            DBName = onlineDBName
                        };

                        if (verify.ShowDialog() != DialogResult.OK) return;
                    }
                    else return;
                }

                
            }

            alwaysPromptPassword = chbS2B2PromptUserPassword.Checked;

            onlineConfigureAsNewDB = rbnSB2NewDB.Checked;

            SaveConfiguration(localConnection);

            if (!errorEncountered) pnlS3Finish.BringToFront();
            else pnlS3Error.BringToFront();
        }

        private void btnSB2BPrev_Click(object sender, EventArgs e) => pnlS2OnlineConnectionA.BringToFront();

        #endregion

        #region Step 3: Finish =======================================================================================

        private void btnS3Finish_Click(object sender, EventArgs e)
        {
            if (chbS3LaunchManager.Checked)
            {
                string qdManager = "QDriveManager.exe";
                if (File.Exists(qdManager)) Process.Start(qdManager);
            }
            
            this.Close();   
        }

        #endregion

        #region Step 3: Error ========================================================================================

        private void lklSupportLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(lklSupportLink.Text);
        private void btnS3ErrorClose_Click(object sender, EventArgs e) => this.Close();

        #endregion

        #region Methods ==============================================================================================
        private bool TestConnection()
        {
            bool success = false;

            using(WrapMySQL sql = new WrapMySQL(onlineDBHost, onlineDBName, onlineDBUsername, onlineDBPassword))
            {
                try
                {
                    sql.Open();
                    success = true;
                }
                catch { success = false; }
                finally { sql.Close(); }
            }

            if (success) MessageBox.Show("Connection to the database established successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Could not connect to the database.", "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return success;
        }

        private bool IsConfiguredDB()
        {
            bool isConfigured = false;

            using (WrapMySQL sql = new WrapMySQL(onlineDBHost, onlineDBName, onlineDBUsername, onlineDBPassword))
            {
                try
                {
                    sql.Open();
                    if (sql.ExecuteScalar("SHOW TABLES LIKE 'qd_info'") != null) isConfigured = true;
                    else isConfigured = false;
                }
                catch { isConfigured = false; }
                finally { sql.Close(); }
            }

            return isConfigured;
        }

        private void SaveConfiguration(bool pIsLocal)
        {
            if(!pIsLocal) CreateOnlineDB();
            CreateLocalDB(!pIsLocal);
        }

        private void CreateOnlineDB()
        {
            try
            {
                using (WrapMySQL sql = new WrapMySQL(onlineDBHost, onlineDBName, onlineDBUsername, onlineDBPassword))
                {
                    if (onlineConfigureAsNewDB)
                    {
                        sql.Open();
                        sql.TransactionBegin();
                        try
                        {
                            // Delete old tables
                            sql.ExecuteNonQuery("DROP TABLE IF EXISTS `qd_info`");
                            sql.ExecuteNonQuery("DROP TABLE IF EXISTS `qd_drives`");
                            sql.ExecuteNonQuery("DROP TABLE IF EXISTS `qd_users`");
                            sql.ExecuteNonQuery("DROP TABLE IF EXISTS `qd_assigns`");

                            // Create new tables
                            sql.ExecuteNonQuery("CREATE TABLE `qd_info` ( `QDKey` VARCHAR(255) NOT NULL , `QDValue` VARCHAR(255) NOT NULL , PRIMARY KEY (`QDKey`))");
                            sql.ExecuteNonQuery("CREATE TABLE `qd_drives` ( `ID` VARCHAR(50) NOT NULL , `DefaultName` VARCHAR(50) NOT NULL , `DefaultDriveLetter` VARCHAR(1) NOT NULL , `LocalPath` VARCHAR(255) NOT NULL , `RemotePath` VARCHAR(255) NOT NULL , `IsPublic` BOOLEAN NOT NULL , `IsDeployable` BOOLEAN NOT NULL , PRIMARY KEY (`ID`))");
                            sql.ExecuteNonQuery("CREATE TABLE `qd_users` ( `ID` VARCHAR(50) NOT NULL , `Name` VARCHAR(100) NOT NULL , PRIMARY KEY (`ID`))");
                            sql.ExecuteNonQuery("CREATE TABLE `qd_assigns` ( `ID` VARCHAR(50) NOT NULL , `UserID` VARCHAR(50) NOT NULL , `DriveID` VARCHAR(50) NOT NULL , `CustomDriveName` VARCHAR(50) NOT NULL , `CustomDriveLetter` VARCHAR(1) NOT NULL , PRIMARY KEY (`ID`))");

                            // Create pre-defined settings
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES ('UserCanToggleKeepLoggedIn', ?)", false);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES ('UserCanAddPrivateDrive', ?)", false);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES ('UserCanAddPublicDrive', ?)", true);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES ('UserCanSelfRegister', ?)", true);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES ('VerificationKey', ?)", Cipher.Encrypt(QDInfo.VerifyKey, onlineMasterPassword));

                            sql.TransactionCommit();
                        }
                        catch (Exception ex)
                        {
                            sql.TransactionRollback();
                            MessageBox.Show("Could not create online database. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errorEncountered = true;
                            txbS3ErrorLog.Text = ex.Message + " " + ex.StackTrace;
                        }
                        sql.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create online database. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorEncountered = true;
                txbS3ErrorLog.Text = ex.Message + " " + ex.StackTrace;
            }
        }

        private void CreateLocalDB(bool onlineLinked)
        {
            try
            {
                using (WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile, true))
                {
                    sql.Open();
                    sql.TransactionBegin();
                    try
                    {
                        // Delete old tables
                        sql.ExecuteNonQuery("DROP TABLE IF EXISTS qd_info");
                        sql.ExecuteNonQuery("DROP TABLE IF EXISTS qd_drives");

                        // Create new tables
                        sql.ExecuteNonQuery(@"CREATE TABLE ""qd_info"" ( ""QDKey"" TEXT, ""QDValue"" TEXT, PRIMARY KEY(""QDKey""));");
                        sql.ExecuteNonQuery(@"CREATE TABLE ""qd_drives"" (""ID"" TEXT, ""RemoteID"" TEXT, ""Path"" TEXT, ""DriveLetter"" TEXT, ""DriveName"" TEXT, PRIMARY KEY(""ID""));");

                        // Create pre-defined settings
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""IsOnlineLinked"", ?)", onlineLinked);
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""AlwaysPromptPassword"", ?)", (!onlineLinked && alwaysPromptPassword));
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""SetupSuccess"", ?)", true);

                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""DBHost"", ?)", onlineDBHost);
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""DBName"",?)", onlineDBName);
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""DBUsername"", ?)", onlineDBUsername);
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""DBPassword"", ?)", onlineDBPassword);
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""UserID"", ?)", DBNull.Value);
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""DefaultUsername"", ?)", DBNull.Value);
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (""DefaultPassword"", ?)", DBNull.Value);

                        sql.TransactionCommit();
                    }
                    catch (Exception ex)
                    {
                        sql.TransactionRollback();
                        MessageBox.Show("Could not create local database. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorEncountered = true;
                        txbS3ErrorLog.Text = ex.Message + " " + ex.StackTrace;
                    }
                    sql.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create local database. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorEncountered = true;
                txbS3ErrorLog.Text = ex.Message + " " + ex.StackTrace;
            }
        }


        #endregion
    }
}
