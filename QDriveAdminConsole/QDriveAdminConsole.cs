﻿using QDriveLib;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

namespace QDriveAdminConsole
{
    public partial class QDriveAdminConsole : SfForm
    {
        private bool userCanToggleKeepLoggedIn = false;
        private bool userCanAddPrivateDrive = false;
        private bool userCanAddPublicDrive = false;
        private bool userCanSelfRegister = false;
        private bool useLoginAsDriveAuth = false;
        private bool forceLoginDriveAuth = false;
        private bool disconnectDrivesAtShutdown = false;
        private bool logUserActions = false;
        private bool userCanChangeManagerSettings = false;
        private string defaultDomain = "";
        private string masterPassword = "";


        private readonly WrapMySQLData dbData = new WrapMySQLData() { Pooling = false };
        private WrapMySQL mysql = null;

        #region Page Layout and Initial Loading =================================================================[RF]=

        private readonly List<Panel> panels = new List<Panel>();

        public QDriveAdminConsole()
        {
            InitializeComponent();

            this.Style.Border = new Pen(Color.FromArgb(1, 115, 199), 2);
            this.Style.InactiveBorder = new Pen(Color.FromArgb(1, 115, 199), 2);

            panels.Add(pnlLogin);
            panels.Add(pnlSettings);
            panels.Add(pnlLoading);
            panels.Add(pnlLocal);
            panels.Add(pnlNotConfigured);

            QDLib.AlignPanels(this, panels, 716, 440);

            pbxQDLogo.Image = Properties.Resources.QDriveProgamFavicon;
            pbxQDLogoLocal.Image = Properties.Resources.QDriveProgamFavicon;
            pbxQDLogoLoading.Image = Properties.Resources.QDriveProgamFavicon;
            pbxLogoNotConfigured.Image = Properties.Resources.QDriveProgamFavicon;

            lblVersion.Text = "Version " + QDInfo.QDVersion;

            pnlLoading.BringToFront();
            txbMasterPassword.Focus();
        }

        private void QDriveAdminConsole_Load(object sender, EventArgs e)
        {
            if (!QDLib.IsQDConfigured()) pnlNotConfigured.BringToFront();
            else
            {
                LoadAllData();
                UpdateAll();

                pnlLogin.BringToFront();

                txbMasterPassword.Focus();
            }
        }



        #endregion

        #region Settings: General ===============================================================================[RF]=

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save the changes made before closing the Q-Drive Admin-Console?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) SaveChanges();
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e) => SaveChanges();

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            QDLoader qdLoader = new QDLoader();
            qdLoader.Show();

            LoadAllData();
            UpdateAll();

            qdLoader.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (SaveChanges()) this.Close();
        }

        #endregion

        #region Settings: Users =================================================================================[RF]=

        private void btnRegisterNewUser_Click(object sender, EventArgs e)
        {
            QDAddUser addUser = new QDAddUser()
            {
                DBData = dbData
            };

            if (addUser.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    mysql.ExecuteNonQueryACon("INSERT INTO qd_users (ID, Name, Username, Password) VALUES (?,?,?,?)",
                        Guid.NewGuid(),
                        addUser.DisplayName,
                        addUser.Username,
                        QDLib.HashPassword(addUser.Password)
                    );
                }
                catch
                {
                    MessageBox.Show("Could not add new user. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                UpdateUsersSettings();
            }
        }

        private void btnEditUserAccount_Click(object sender, EventArgs e) => EditSelectedUser();

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            if (lbxUserList.SelectedIndex != -1)
            {
                if (MessageBox.Show("Do you really want to delete the selected user?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return; }
                    mysql.TransactionBegin();

                    try
                    {
                        mysql.ExecuteNonQuery("DELETE FROM qd_users WHERE ID = ?", lbxUserList.SelectedValue.ToString());
                        mysql.ExecuteNonQuery("DELETE FROM qd_assigns WHERE UserID = ?", lbxUserList.SelectedValue.ToString());
                        mysql.TransactionCommit();
                    }
                    catch
                    {
                        mysql.TransactionRollback();
                    }

                    mysql.Close();

                    UpdateUsersSettings();
                }
            }
        }

        private void lbxUserList_DoubleClick(object sender, EventArgs e) => EditSelectedUser();

        private void EditSelectedUser()
        {
            if (lbxUserList.SelectedIndex != -1)
            {
                QDAddUser editUser = new QDAddUser
                {
                    EditID = lbxUserList.SelectedValue.ToString(),
                    DBData = dbData
                };

                if (editUser.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(editUser.Password))
                        {
                            mysql.ExecuteNonQueryACon("UPDATE qd_users SET Name = ?, Username = ? WHERE ID = ?",
                                editUser.DisplayName,
                                editUser.Username,
                                editUser.EditID
                            );
                        }
                        else
                        {
                            mysql.ExecuteNonQueryACon("UPDATE qd_users SET Name = ?, Username = ?, Password = ? WHERE ID = ?",

                                editUser.DisplayName,
                                editUser.Username,
                                QDLib.HashPassword(editUser.Password),
                                editUser.EditID
                            );
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Could not update user data. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    UpdateUsersSettings();
                }
            }
        }

        private void btnUsersShowActions_Click(object sender, EventArgs e)
        {
            QDActionBrowser actBrowser = new QDActionBrowser()
            {
                SelectedObjectID = lbxUserList.SelectedValue.ToString(),
                DBData = dbData
            };
            actBrowser.ShowDialog();
        }

        #endregion

        #region Settings: Devices ===============================================================================[RF]=

        private void lbxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return; }
            txbDeviceMac.Text = mysql.ExecuteScalar<string>("SELECT MacAddress FROM qd_devices WHERE ID = ?", lbxDevices.SelectedValue);
            txbDeviceName.Text = mysql.ExecuteScalar<string>("SELECT DeviceName FROM qd_devices WHERE ID = ?", lbxDevices.SelectedValue);
            txbLogonName.Text = mysql.ExecuteScalar<string>("SELECT LogonName FROM qd_devices WHERE ID = ?", lbxDevices.SelectedValue);
            mysql.Close();

            int userCount = QDLib.UserCountAtDevice(lbxDevices.SelectedValue.ToString(), dbData);

            if (userCount == 1) lblDeviceUserCount.Text = $"{userCount} User logged into Q-Drive on this{Environment.NewLine}machine / account";
            else lblDeviceUserCount.Text = $"{userCount} Users logged into Q-Drive on this{Environment.NewLine}machine / account";

            
        }

        private void btnDeviceShowActions_Click(object sender, EventArgs e)
        {
            QDActionBrowser actBrowser = new QDActionBrowser()
            {
                SelectedObjectID = lbxDevices.SelectedValue.ToString(),
                DBData = dbData
            };
            actBrowser.ShowDialog();
        }

        private void btnDeviceShowUsers_Click(object sender, EventArgs e)
        {
            QDDeviceUsers deviceUser = new QDDeviceUsers()
            {
                DeviceID = lbxDevices.SelectedValue.ToString(),
                DBData = dbData
            };
            deviceUser.ShowDialog();
        }

        #endregion

        #region Settings: MySQL-Connection ======================================================================[RF]=

        private void btnTestDBConnection_Click(object sender, EventArgs e)
        {
            QDLib.TestConnection(new WrapMySQLData()
            {
                Hostname = txbDBHostname.Text,
                Database = txbDBDatabase.Text,
                Username = txbDBUsername.Text,
                Password = txbDBPassword.Text,
                Pooling = false
            });
        }

        #endregion

        #region Settings: Online Drives =========================================================================[RF]=

        private void btnAddOnlineDrive_Click(object sender, EventArgs e)
        {
            QDAddPublicDrive addDrive = new QDAddPublicDrive();

            if (addDrive.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    mysql.ExecuteNonQueryACon("INSERT INTO qd_drives (ID, DefaultName, DefaultDriveLetter, LocalPath, IsPublic, IsDeployable) VALUES (?,?,?,?,?,?)",

                        Guid.NewGuid(),
                        addDrive.DriveName,
                        addDrive.DriveLetter,
                        addDrive.DrivePath,
                        true,
                        addDrive.CanBeDeployed
                    );
                }
                catch
                {
                    MessageBox.Show("Could not add drive. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                UpdateOnlineDrives();
            }
        }

        private void btnEditOnlineDrive_Click(object sender, EventArgs e) => EditSelectedDrive();

        private void btnRemoveOnlineDrive_Click(object sender, EventArgs e)
        {
            if (lbxOnlineDrives.SelectedIndex != -1)
            {
                bool success = false;
                if (MessageBox.Show("Do you really want to remove the selected drive?", "Remove Drive", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return; }
                    mysql.TransactionBegin();

                    try
                    {
                        mysql.ExecuteNonQuery("DELETE FROM qd_drives WHERE ID = ?", lbxOnlineDrives.SelectedValue.ToString());
                        mysql.ExecuteNonQuery("DELETE FROM qd_assigns WHERE DriveID = ?", lbxOnlineDrives.SelectedValue.ToString());
                        success = true;
                        mysql.TransactionCommit();
                    }
                    catch
                    {
                        mysql.TransactionRollback();
                    }

                    mysql.Close();

                    if (!success) MessageBox.Show("Could not remove drive. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    UpdateOnlineDrives();
                }
            }
        }

        private void lbxOnlineDrives_DoubleClick(object sender, EventArgs e) => EditSelectedDrive();

        private void EditSelectedDrive()
        {
            if (lbxOnlineDrives.SelectedIndex != -1)
            {
                
                string driveID = lbxOnlineDrives.SelectedValue.ToString();

                if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return; }
                string drivePath = mysql.ExecuteScalar<string>("SELECT LocalPath FROM qd_drives WHERE ID = ?", driveID);
                string defaultName = mysql.ExecuteScalar<string>("SELECT DefaultName FROM qd_drives WHERE ID = ?", driveID);
                string defaultLetter = mysql.ExecuteScalar<string>("SELECT DefaultDriveLetter FROM qd_drives WHERE ID = ?", driveID);
                bool canBeDeployed = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT IsDeployable FROM qd_drives WHERE ID = ?", driveID));
                mysql.Close();

                QDAddPublicDrive editDrive = new QDAddPublicDrive
                {
                    EditMode = true,
                    DrivePath = drivePath,
                    DriveName = defaultName,
                    DriveLetter = defaultLetter,
                    CanBeDeployed = canBeDeployed
                };

                if (editDrive.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        mysql.ExecuteNonQueryACon("UPDATE qd_drives SET DefaultName = ?, DefaultDriveLetter = ?, LocalPath = ?, IsDeployable = ? WHERE ID = ?",
                            editDrive.DriveName,
                            editDrive.DriveLetter,
                            editDrive.DrivePath,
                            editDrive.CanBeDeployed,
                            driveID
                        );
                    }
                    catch
                    {
                        MessageBox.Show("Could not update drive data. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    UpdateOnlineDrives();
                }
            }
        }

        #endregion

        #region Settings: Action Logger =========================================================================[RF]=

        private void btnOpenActionLog_Click(object sender, EventArgs e)
        {
            QDActionBrowser actBrowser = new QDActionBrowser()
            {
                SelectedObjectID = "",
                DBData = dbData
            };
            actBrowser.ShowDialog();
        }

        #endregion

        #region Settings: Info and More =========================================================================[RF]=

        private void lnkReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://endev.at/p/q-drive");
        }

        private void btnChangeMasterPassword_Click(object sender, EventArgs e)
        {
            QDChangeMasterPassword changeMasterPassword = new QDChangeMasterPassword() { MasterPassword = masterPassword };

            if (changeMasterPassword.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    mysql.ExecuteNonQueryACon("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", QDLib.HashPassword(changeMasterPassword.MasterPassword), QDInfo.DBO.MasterPassword);
                    MessageBox.Show("Successfully changed Master-Password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Could not change password. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Login ===========================================================================================[RF]=

        private void btnLogin_Click(object sender, EventArgs e) => Submit();

        private void SubmitForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Submit();
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void Submit()
        {
            QDLoader qdLoader = new QDLoader();
            qdLoader.Show();

            if (masterPassword == QDLib.HashPassword(txbMasterPassword.Text)) pnlSettings.BringToFront();
            else MessageBox.Show("Password not valid.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            qdLoader.Close();
        }

        #endregion

        #region Not Configured ==================================================================================[RF]=

        private void btnRunSetup_Click(object sender, EventArgs e)
        {
            if (QDLib.RunQDriveSetup()) this.Close();
        }

        #endregion

        #region Local Version ===================================================================================[RF]=

        private void btnLocalClose_Click(object sender, EventArgs e) => this.Close();

        #endregion

        #region Methods =========================================================================================[RF]=

        private void UpdateAll()
        {
            UpdateQDSettings();
            UpdateUsersSettings();
            UpdateDeviceSettings();
            UpdateMySQLSettings();
            UpdateOnlineDrives();
            UpdateActionLog();
        }

        private void UpdateQDSettings()
        {
            tglUserCanToggleKeepLoggedIn.ToggleState = userCanToggleKeepLoggedIn ? ToggleButtonState.Active : ToggleButtonState.Inactive;
            tglUserCanSelfRegister.ToggleState = userCanSelfRegister ? ToggleButtonState.Active : ToggleButtonState.Inactive;
            tglUserCanAddPublicDrives.ToggleState = userCanAddPublicDrive ? ToggleButtonState.Active : ToggleButtonState.Inactive;
            tglUserCanAddPrivateDrives.ToggleState = userCanAddPrivateDrive ? ToggleButtonState.Active : ToggleButtonState.Inactive;
            tglUseLoginAsDriveAuthentication.ToggleState = useLoginAsDriveAuth ? ToggleButtonState.Active : ToggleButtonState.Inactive;
            tglForceLoginAsDriveAuthentication.ToggleState = forceLoginDriveAuth ? ToggleButtonState.Active : ToggleButtonState.Inactive;
            tglDisconnectDrivesAtShutdown.ToggleState = disconnectDrivesAtShutdown ? ToggleButtonState.Active : ToggleButtonState.Inactive;
            tglLogUserActions.ToggleState = logUserActions ? ToggleButtonState.Active : ToggleButtonState.Inactive;
            tglUserCanChangeManagerSettings.ToggleState = userCanChangeManagerSettings ? ToggleButtonState.Active : ToggleButtonState.Inactive;
            txbDefaultDomain.Text = defaultDomain;

            if (tglLogUserActions.ToggleState == ToggleButtonState.Active) lblDeviceActionLoggingNote.Visible = false;
            else lblDeviceActionLoggingNote.Visible = true;
        }

        private void UpdateUsersSettings()
        {
            lbxUserList.DataSource = null;
            lbxUserList.Items.Clear();

            lbxUserList.DisplayMember = "UserDisplay";
            lbxUserList.ValueMember = "ID";
            lbxUserList.DataSource = mysql.CreateDataTable("SELECT CONCAT(Name, ' (', Username, ')') AS UserDisplay, ID FROM qd_users ORDER BY Name ASC");
        }

        private void UpdateDeviceSettings()
        {
            lbxDevices.DataSource = null;
            lbxDevices.Items.Clear();

            lbxDevices.DisplayMember = "DeviceDisplay";
            lbxDevices.ValueMember = "ID";
            lbxDevices.DataSource = mysql.CreateDataTable("SELECT CONCAT(DeviceName, ' (User: ', LogonName, ', MAC: ', MacAddress, ')') AS DeviceDisplay, ID FROM qd_devices ORDER BY DeviceName ASC");
        }

        private void UpdateMySQLSettings()
        {
            txbDBHostname.Text = dbData.Hostname;
            txbDBUsername.Text = dbData.Username;
            txbDBPassword.Text = dbData.Password;
            txbDBDatabase.Text = dbData.Database;
        }

        private void UpdateOnlineDrives()
        {
            lbxOnlineDrives.DataSource = null;
            lbxOnlineDrives.Items.Clear();

            lbxOnlineDrives.DisplayMember = "DriveDisplay";
            lbxOnlineDrives.ValueMember = "ID";
            lbxOnlineDrives.DataSource = mysql.CreateDataTable(@"SELECT CONCAT('(', DefaultDriveLetter, ':\\) ', DefaultName, ' (', LocalPath, ')') AS DriveDisplay, ID FROM qd_drives WHERE IsPublic = 1 ORDER BY DefaultDriveLetter ASC");
        }

        private void UpdateActionLog()
        {

        }

        private bool SaveChanges()
        {
            QDLoader qdLoader = new QDLoader();
            qdLoader.Show();

            bool successOnline = false;
            bool successLocal = false;

            if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return false; }
            mysql.TransactionBegin();
            try
            {
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", tglUserCanToggleKeepLoggedIn.ToggleState == ToggleButtonState.Active, QDInfo.DBO.UserCanToggleKeepLoggedIn);
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", tglUserCanSelfRegister.ToggleState == ToggleButtonState.Active, QDInfo.DBO.UserCanSelfRegister);
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", tglUserCanAddPublicDrives.ToggleState == ToggleButtonState.Active, QDInfo.DBO.UserCanAddPublicDrive);
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", tglUserCanAddPrivateDrives.ToggleState == ToggleButtonState.Active, QDInfo.DBO.UserCanAddPrivateDrive);
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", tglUseLoginAsDriveAuthentication.ToggleState == ToggleButtonState.Active, QDInfo.DBO.UseLoginAsDriveAuthentication);
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", tglForceLoginAsDriveAuthentication.ToggleState == ToggleButtonState.Active, QDInfo.DBO.ForceLoginAsDriveAuthentication);
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", tglDisconnectDrivesAtShutdown.ToggleState == ToggleButtonState.Active, QDInfo.DBO.DisconnectDrivesAtShutdown);
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", tglLogUserActions.ToggleState == ToggleButtonState.Active, QDInfo.DBO.LogUserActions);
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", tglUserCanChangeManagerSettings.ToggleState == ToggleButtonState.Active, QDInfo.DBO.UserCanChangeManagerSettings);
                mysql.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", txbDefaultDomain.Text, QDInfo.DBO.DefaultDomain);

                mysql.TransactionCommit();
                successOnline = true;
            }
            catch
            {
                mysql.TransactionRollback();
            }

            mysql.Close();

            WrapMySQLData newDBConnection = new WrapMySQLData()
            {
                Hostname = txbDBHostname.Text,
                Database = txbDBDatabase.Text,
                Username = txbDBUsername.Text,
                Password = txbDBPassword.Text,
                Pooling = false
            };

            using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile))
            {
                if (QDLib.TestConnection(newDBConnection, false))
                {
                    if (!QDLib.ManagedDBOpen(sqlite)) { QDLib.DBOpenFailed(); return false; }
                    sqlite.TransactionBegin();

                    try
                    {
                        sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(newDBConnection.Hostname, QDInfo.LocalCipherKey), QDInfo.DBL.DBHost);
                        sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(newDBConnection.Database, QDInfo.LocalCipherKey), QDInfo.DBL.DBName);
                        sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(newDBConnection.Username, QDInfo.LocalCipherKey), QDInfo.DBL.DBUsername);
                        sqlite.ExecuteNonQuery("UPDATE qd_info SET QDValue = ? WHERE QDKey = ?", Cipher.Encrypt(newDBConnection.Password, QDInfo.LocalCipherKey), QDInfo.DBL.DBPassword);

                        sqlite.TransactionCommit();
                        successLocal = true;
                    }
                    catch
                    {
                        sqlite.TransactionRollback();
                    }

                    sqlite.Close();
                }
            }

            if (!successOnline || !successLocal)
            {
                if (MessageBox.Show("Could not save the changes made. Please check your MySQL-Connection and try again. \r\n\r\nDo you want to close the admin-console anyway?", "Could not save settins", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) successOnline = true;
            }

            qdLoader.Close();

            return successOnline && successLocal;
        }

        private void LoadAllData()
        {
            try
            {
                using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile))
                {

                    if (!QDLib.ManagedDBOpen(sqlite)) { QDLib.DBOpenFailed(); return; }

                    bool localConnection = !Convert.ToBoolean(sqlite.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.IsOnlineLinked));

                    sqlite.Close();

                    if (localConnection)
                    {
                        pnlLocal.BringToFront();
                        return;
                    }

                    if (!QDLib.ManagedDBOpen(sqlite)) { QDLib.DBOpenFailed(); return; }
                    dbData.Hostname = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBHost), QDInfo.LocalCipherKey);
                    dbData.Username = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBUsername), QDInfo.LocalCipherKey);
                    dbData.Password = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBPassword), QDInfo.LocalCipherKey);
                    dbData.Database = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBName), QDInfo.LocalCipherKey);
                    sqlite.Close();
                }

                mysql = new WrapMySQL(dbData);

                if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return; }
                userCanToggleKeepLoggedIn = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanToggleKeepLoggedIn));
                userCanAddPrivateDrive = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanAddPrivateDrive));
                userCanAddPublicDrive = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanAddPublicDrive));
                userCanSelfRegister = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanSelfRegister));
                useLoginAsDriveAuth = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UseLoginAsDriveAuthentication));
                forceLoginDriveAuth = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.ForceLoginAsDriveAuthentication));
                disconnectDrivesAtShutdown = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.DisconnectDrivesAtShutdown));
                logUserActions = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.LogUserActions));
                userCanChangeManagerSettings = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.UserCanChangeManagerSettings));
                defaultDomain = mysql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.DefaultDomain);
                masterPassword = mysql.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.MasterPassword);
                mysql.Close();
            }
            catch
            {
                MessageBox.Show("An error occured whilst trying to connect to the online-database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }


        #endregion
    }
}
