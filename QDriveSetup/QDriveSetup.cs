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

namespace QDrive
{
    public partial class QDriveSetup : SfForm
    {
        private readonly List<Panel> panels = new List<Panel>();

        private bool localConnection = false;

        private bool alwaysPromptPassword = false;
        private string localPassword = string.Empty;

        private WrapMySQLData onlineDBConDat = new WrapMySQLData();

        private bool onlineAlreadyConfigured;

        private string onlineMasterPassword = string.Empty;
        private bool onlineConfigureAsNewDB;

        private bool errorEncountered = false;

        #region Page Layout and Initial Loading =================================================================[RF]=

        public QDriveSetup()
        {
            InitializeComponent();

            // Add Panels to panel-collection
            panels.Add(pnlS0Welcome);
            panels.Add(pnlS1ConnectionType);
            panels.Add(pnlS2LocalConnection);
            panels.Add(pnlS2OnlineConnectionA);
            panels.Add(pnlS2OnlineConnectionB);
            panels.Add(pnlS3Finish);
            panels.Add(pnlS3Error);

            QDLib.AlignPanels(this, panels, 680, 500);

            this.Style.Border = new Pen(Color.FromArgb(77, 216, 255), 2);
            this.Style.InactiveBorder = new Pen(Color.FromArgb(77, 216, 255), 2);
        }

        private void QDriveSetup_Load(object sender, EventArgs e)
        {
            QDLib.AddToAutostart();
            pnlS0Welcome.BringToFront();
        }

        #endregion

        #region Step 0: Welcome =================================================================================[RF]=

        private void btnS0Next_Click(object sender, EventArgs e)
        {
            pnlS1ConnectionType.BringToFront();
            rbnS1Local.Focus();
        }

        #endregion

        #region Step 1: Connection type =========================================================================[RF]=

        private void btnS1Prev_Click(object sender, EventArgs e) => pnlS0Welcome.BringToFront();

        private void btnS1Next_Click(object sender, EventArgs e)
        {
            localConnection = rbnS1Local.Checked;

            if (localConnection)
            {
                pnlS2LocalConnection.BringToFront();
                chbSA2PromptPassword.Focus();
            }
            else
            {
                pnlS2OnlineConnectionA.BringToFront();
                txbSB2DBHostname.Focus();
            }
        }

        #endregion

        #region Step 2A: Local connection =======================================================================[RF]=
        
        private void chbSA2PromptPassword_CheckedChanged(object sender, EventArgs e)
        {
            txbSA2Password.Enabled = chbSA2PromptPassword.Checked;
            txbSA2ConfirmPassword.Enabled = chbSA2PromptPassword.Checked;
            lblSA2Password.Enabled = chbSA2PromptPassword.Checked;
            lblSA2ConfirmPassword.Enabled = chbSA2PromptPassword.Checked;
        }

        private void btnSA2Prev_Click(object sender, EventArgs e) => pnlS1ConnectionType.BringToFront();

        private void btnSA2Next_Click(object sender, EventArgs e) => SubmitSA2();

        private void SubmitSA2Form(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SubmitSA2();
        }

        private void SubmitSA2()
        {
            alwaysPromptPassword = chbSA2PromptPassword.Checked;

            if (alwaysPromptPassword)
            {
                // Check if both passwords are valid
                if (QDLib.ValidatePasswords(txbSA2Password.Text, txbSA2ConfirmPassword.Text)) localPassword = txbSA2Password.Text;
                else return;
            }

            // (Try to) save the QD-Config and create DBs 
            SaveConfiguration(localConnection);

            // Check if an error occured and show error-page
            if (!errorEncountered)
            {
                pnlS3Finish.BringToFront();
                chbS3LaunchManager.Focus();
            }
            else
            {
                pnlS3Error.BringToFront();
                btnS3ErrorClose.Focus();
            }
        }

        #endregion

        #region Step 2B1: Global connection 1/2 =================================================================[RF]=

        private void btnSB2TestConnection_Click(object sender, EventArgs e) => QDLib.TestConnection(new WrapMySQLData(){
            Hostname = txbSB2DBHostname.Text,
            Database = txbSB2DBName.Text,
            Username = txbSB2DBUsername.Text,
            Password = txbSB2DBPassword.Text 
        });

        private void btnSB2APrev_Click(object sender, EventArgs e) => pnlS1ConnectionType.BringToFront();

        private void btnSB2ANext_Click(object sender, EventArgs e) => SubmitSB2A();

        private void SubmitSB2AForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SubmitSB2A();
        }

        private void SubmitSB2A()
        {
            onlineDBConDat = new WrapMySQLData()
            {
                Hostname = txbSB2DBHostname.Text,
                Database = txbSB2DBName.Text,
                Username = txbSB2DBUsername.Text,
                Password = txbSB2DBPassword.Text
            };


            if (QDLib.TestConnection(onlineDBConDat, false))
            {
                onlineAlreadyConfigured = IsConfiguredDB();

                rbnSB2ExistingDB.Enabled = onlineAlreadyConfigured;
                txbSB2ExistingDBPassword.Enabled = onlineAlreadyConfigured;
                lblSB2AExistingMasterPW.Enabled = onlineAlreadyConfigured;

                if (onlineAlreadyConfigured) rbnSB2ExistingDB.Checked = true;
                else rbnSB2NewDB.Checked = true;

                pnlS2OnlineConnectionB.BringToFront();
            }
            else return;

            rbnSB2ExistingDB.Focus();
        }

        #endregion

        #region Step 2B2: Global connection 2/2 =================================================================[RF]=

        private void rbnSB2ExistingDB_CheckedChanged(object sender, EventArgs e) => EnableDisableNewExisting();

        private void rbnSB2NewDB_CheckedChanged(object sender, EventArgs e) => EnableDisableNewExisting();

        private void EnableDisableNewExisting()
        {
            lblSB2AExistingMasterPW.Enabled = rbnSB2ExistingDB.Checked;
            txbSB2ExistingDBPassword.Enabled = rbnSB2ExistingDB.Checked;

            lblSB2BMasterPassword.Enabled = !rbnSB2ExistingDB.Checked;
            lblSB2BConfirmMasterPassword.Enabled = !rbnSB2ExistingDB.Checked;
            txbSB2NewDBPassword.Enabled = !rbnSB2ExistingDB.Checked;
            txbSB2NewDBConfirmPassword.Enabled = !rbnSB2ExistingDB.Checked;
        }

        private void btnSB2BNext_Click(object sender, EventArgs e) => SubmitSB2B();

        private void SubmitSB2BForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SubmitSB2B();
        }

        private void SubmitSB2B()
        {
            if (rbnSB2ExistingDB.Checked)
            {
                if (!QDLib.VerifyMasterPassword(txbSB2ExistingDBPassword.Text, onlineDBConDat))
                {
                    MessageBox.Show("Master-Password is not valid. Please enter the corrent Master-Password, which has been set when the database was first initialised.", "Invalid Master-Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else onlineMasterPassword = txbSB2ExistingDBPassword.Text;
            }
            else
            {
                if (QDLib.ValidatePasswords(txbSB2NewDBPassword.Text, txbSB2NewDBConfirmPassword.Text)) onlineMasterPassword = txbSB2NewDBPassword.Text;
                else return;

                if (onlineAlreadyConfigured)
                {
                    if (MessageBox.Show("The given database has already been initialised before. Do you really want to delete the existing database and configure it as a new one?", "Database already initialised", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        VerifyMasterPW verify = new VerifyMasterPW() { DBData = onlineDBConDat };

                        if (verify.ShowDialog() != DialogResult.OK) return;
                    }
                    else return;
                }
            }

            alwaysPromptPassword = chbS2B2PromptUserPassword.Checked;
            onlineConfigureAsNewDB = rbnSB2NewDB.Checked;

            SaveConfiguration(localConnection);

            if (!errorEncountered)
            {
                pnlS3Finish.BringToFront();
                chbS3LaunchManager.Focus();
            }
            else
            {
                pnlS3Error.BringToFront();
                btnS3ErrorClose.Focus();
            }
        }

        private void btnSB2BPrev_Click(object sender, EventArgs e) => pnlS2OnlineConnectionA.BringToFront();

        #endregion

        #region Step 3: Finish ==================================================================================[RF]=

        private void btnS3Finish_Click(object sender, EventArgs e)
        {
            string qdManager = "QDriveManager.exe";
            if (chbS3LaunchManager.Checked && File.Exists(qdManager)) Process.Start(qdManager);
            this.Close();   
        }

        #endregion

        #region Step 3: Error ===================================================================================[RF]=

        private void lklSupportLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(lklSupportLink.Text);
        private void btnS3ErrorClose_Click(object sender, EventArgs e) => this.Close();

        #endregion

        #region Methods =========================================================================================[RF]=

        private bool IsConfiguredDB()
        {
            using (WrapMySQL sql = new WrapMySQL(onlineDBConDat))
            {
                if (sql.ExecuteScalarACon("SHOW TABLES LIKE 'qd_info'") != null) return true;
                else return false;
            }
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
                using (WrapMySQL sql = new WrapMySQL(onlineDBConDat))
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
                            sql.ExecuteNonQuery("DROP TABLE IF EXISTS `qd_conlog`");
                            sql.ExecuteNonQuery("DROP TABLE IF EXISTS `qd_devices`");

                            // Create new tables
                            sql.ExecuteNonQuery("CREATE TABLE `qd_info` ( `QDKey` VARCHAR(255) NOT NULL , `QDValue` VARCHAR(255) NOT NULL , PRIMARY KEY (`QDKey`))");
                            sql.ExecuteNonQuery("CREATE TABLE `qd_drives` ( `ID` VARCHAR(50) NOT NULL , `DefaultName` VARCHAR(50) NOT NULL , `DefaultDriveLetter` VARCHAR(1) NOT NULL , `LocalPath` VARCHAR(255) NOT NULL , `RemotePath` VARCHAR(255) NOT NULL , `IsPublic` BOOLEAN NOT NULL , `IsDeployable` BOOLEAN NOT NULL , PRIMARY KEY (`ID`))");
                            sql.ExecuteNonQuery("CREATE TABLE `qd_users` ( `ID` varchar(50) NOT NULL, `Name` varchar(100) NOT NULL, `Username` varchar(100) NOT NULL, `Password` varchar(100) NOT NULL, PRIMARY KEY(`ID`), UNIQUE KEY `Username` (`Username`))");
                            sql.ExecuteNonQuery("CREATE TABLE `qd_assigns` ( `ID` varchar(50) NOT NULL, `UserID` varchar(50) NOT NULL, `DriveID` varchar(50) NOT NULL, `CustomDriveName` varchar(50) NOT NULL, `CustomDriveLetter` varchar(1) NOT NULL, `DUsername` varchar(200) NOT NULL, `DPassword` varchar(200) NOT NULL, `DDomain` varchar(200) NOT NULL, PRIMARY KEY(`ID`))");
                            sql.ExecuteNonQuery("CREATE TABLE `qd_conlog` ( `ID` varchar(50) COLLATE utf8_unicode_ci NOT NULL, `UserID` varchar(50) COLLATE utf8_unicode_ci NOT NULL, `DeviceID` varchar(50) COLLATE utf8_unicode_ci NOT NULL, `LogTime` datetime NOT NULL, `LogAction` int(11) NOT NULL, PRIMARY KEY(`ID`))");
                            sql.ExecuteNonQuery("CREATE TABLE `qd_devices` ( `ID` varchar(50) COLLATE utf8_unicode_ci NOT NULL, `MacAddress` varchar(50) COLLATE utf8_unicode_ci NOT NULL, `LogonName` varchar(150) COLLATE utf8_unicode_ci NOT NULL, `DeviceName` varchar(150) COLLATE utf8_unicode_ci NOT NULL, PRIMARY KEY(`ID`)) ");

                            // Create pre-defined settings
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.UserCanToggleKeepLoggedIn,  false);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.UserCanAddPrivateDrive,     false);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.UserCanAddPublicDrive,      true);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.UserCanSelfRegister,        true);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.MasterPassword,             QDLib.HashPassword(onlineMasterPassword));
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.DefaultDomain,              "");
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.UseLoginAsDriveAuthentication, false);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.ForceLoginAsDriveAuthentication, false);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.DisconnectDrivesAtShutdown, false);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.LogUserActions, false);
                            sql.ExecuteNonQuery($"INSERT INTO `qd_info` (`QDKey`, `QDValue`) VALUES (?, ?)", QDInfo.DBO.UserCanChangeManagerSettings, true);

                            sql.TransactionCommit();
                        }
                        catch (Exception ex)
                        {
                            sql.TransactionRollback();
                            errorEncountered = true;
                            txbS3ErrorLog.Text = ex.Message + " " + ex.StackTrace;
                        }
                        sql.Close();

                        if(errorEncountered) MessageBox.Show("Could not create online database. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                using (WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile))
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
                        sql.ExecuteNonQuery(@"CREATE TABLE ""qd_drives"" (""ID"" TEXT, ""LocalPath"" TEXT, ""RemotePath"" TEXT, ""Username"" TEXT, ""Password"" TEXT, ""Domain"" TEXT, ""DriveLetter"" TEXT, ""DriveName"" TEXT, PRIMARY KEY(""ID""));");

                        // Create pre-defined settings
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.IsOnlineLinked,          onlineLinked);
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.AlwaysPromptPassword,    alwaysPromptPassword);
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.SetupSuccess,            true);

                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DBHost,      Cipher.Encrypt(onlineDBConDat.Hostname,QDInfo.LocalCipherKey));
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DBName,      Cipher.Encrypt(onlineDBConDat.Database, QDInfo.LocalCipherKey));
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DBUsername,  Cipher.Encrypt(onlineDBConDat.Username, QDInfo.LocalCipherKey));
                        sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DBPassword,  Cipher.Encrypt(onlineDBConDat.Password, QDInfo.LocalCipherKey));

                        if (onlineLinked)
                        {
                            sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultUsername, DBNull.Value);
                            sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultPassword, DBNull.Value);
                        }
                        else
                        {
                            sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultUsername, "local");
                            sql.ExecuteNonQuery($@"INSERT INTO qd_info (QDKey, QDValue) VALUES (?, ?)", QDInfo.DBL.DefaultPassword, Cipher.Encrypt(localPassword, QDInfo.LocalCipherKey));
                        }
                        

                        sql.TransactionCommit();
                    }
                    catch (Exception ex)
                    {
                        sql.TransactionRollback();
                        errorEncountered = true;
                        txbS3ErrorLog.Text = ex.Message + " " + ex.StackTrace;
                    }
                    sql.Close();

                    if(errorEncountered) MessageBox.Show("Could not create local database. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
