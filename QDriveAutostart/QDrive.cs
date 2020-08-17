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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

namespace QDriveAutostart
{
    public partial class QDrive : Form
    {
        private bool localConnection;
        private bool promptPassword;

        private bool disconnectAtShutdown = false;
        private bool logUserActions = false;

        private string Username;
        private string Password;
        private string UserID;

        private WrapMySQLData dbData = new WrapMySQLData();

        private List<DriveViewItem> driveList = new List<DriveViewItem>();

        #region Page Layout and Initial Loading =================================================================[RF]=
        
        public QDrive()
        {
            InitializeComponent();
            lblVersionInfo.Text = QDInfo.QDVersion;

            ContextMenu ctmQDriveMenu = new ContextMenu();
            ctmQDriveMenu.MenuItems.Add("Open QD-Manager", nfiQDriveMenu_OpenManager_Click);
            ctmQDriveMenu.MenuItems.Add("Reconnect Drives", nfiQDriveMenu_Reconnect_Click);
            ctmQDriveMenu.MenuItems.Add("Log Off / Disconnect", nfiQDriveMenu_LogOff_Click);
            ctmQDriveMenu.MenuItems.Add("Quit Q-Drive", nfiQDriveMenu_Close_Click);

            nfiQDriveMenu.Visible = true;
            nfiQDriveMenu.ContextMenu = ctmQDriveMenu;

            this.Location = new Point(
                Screen.FromControl(this).Bounds.Width - this.Width,
                Screen.FromControl(this).Bounds.Height - this.Height - 50
            );
        }

        #endregion

        #region Login, Load and connect drives ==================================================================[RF]=

        private void QDrive_Load(object sender, EventArgs e)
        {
            // Check if the setup has been completed yet
            if (!QDLib.IsQDConfigured())
            {
                QDLib.RunQDriveSetup();
                this.Close();
            }

            LoadQDData();

            if (promptPassword)
            {
                QDriveManager.QDriveManager managerLogin = new QDriveManager.QDriveManager() { AutostartLogin = true };

                if (managerLogin.ShowDialog() != DialogResult.OK) this.Close();

                Username = managerLogin.uUsername;
                Password = managerLogin.uPassword;
                UserID = managerLogin.userID;
            }

            driveList = QDLib.CreateDriveList(localConnection, UserID, Password, dbData);

            if (localConnection)
            {
                pbxQDriveSplash.Image = Properties.Resources.QDSplashLocal;
                QDLib.ConnectQDDrives("", "", dbData, logUserActions, true, driveList);
            }
            else
            {
                pbxQDriveSplash.Image = Properties.Resources.QDSplashOnline;
                QDLib.ConnectQDDrives(UserID, Password, dbData, logUserActions, true, driveList);

                if(!localConnection) QDLib.LogUserConnection(UserID, QDLogAction.QDSystemAutostartFinished, dbData, logUserActions);
            }
        }

        private void QDrive_Shown(object sender, EventArgs e) => tmrQDSplash.Start();

        private void tmrQDSplash_Tick(object sender, EventArgs e) => Hide();

        private int LoadQDData()
        {
            // Load local Data

            using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile))
            {

                sqlite.Open();

                localConnection = !Convert.ToBoolean(sqlite.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.IsOnlineLinked));
                promptPassword = Convert.ToBoolean(sqlite.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.AlwaysPromptPassword));

                Username = sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultUsername);
                Password = sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DefaultPassword);

                if (!string.IsNullOrEmpty(Password)) Password = Cipher.Decrypt(Password, QDInfo.LocalCipherKey);

                dbData.Hostname = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBHost), QDInfo.LocalCipherKey);
                dbData.Username = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBUsername), QDInfo.LocalCipherKey);
                dbData.Password = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBPassword), QDInfo.LocalCipherKey);
                dbData.Database = Cipher.Decrypt(sqlite.ExecuteScalar<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.DBName), QDInfo.LocalCipherKey);

                sqlite.Close();
            }

            try
            {
                using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile))
                {
                    sqlite.Open();

                    sqlite.Close();
                }
            }
            catch { return 3; }

            // Load Online Data
            if (!localConnection)
            {
                try
                {
                    using (WrapMySQL mysql = new WrapMySQL(dbData))
                    {
                        mysql.Open();

                        disconnectAtShutdown = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.DisconnectDrivesAtShutdown));
                        logUserActions = Convert.ToBoolean(mysql.ExecuteScalar<short>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.LogUserActions));

                        mysql.Close();
                    }
                }
                catch { return 2; }
            }

            if (!promptPassword) QDLib.VerifyPassword(localConnection, Username, Password, out UserID, dbData);

            return 0;
        }

        #endregion

        #region NortifyIcon Events ==============================================================================[RF]=

        private void nfiQDriveMenu_DoubleClick(object sender, EventArgs e) => OpenManager();

        private void nfiQDriveMenu_OpenManager_Click(object sender, EventArgs e) => OpenManager();

        private void OpenManager()
        {
            QDriveManager.QDriveManager managerLogin = new QDriveManager.QDriveManager();
            managerLogin.Show();
        }

        private void nfiQDriveMenu_Reconnect_Click(object sender, EventArgs e)
        {
            if (localConnection) QDLib.ConnectQDDrives("", "", dbData, logUserActions, true);
            else QDLib.ConnectQDDrives(UserID, Password, dbData, logUserActions, true);
        }

        private void nfiQDriveMenu_LogOff_Click(object sender, EventArgs e)
        {
            QDLib.DisconnectAllDrives(driveList);
            if (!localConnection)
            {
                QDLib.LogUserConnection(UserID, QDLogAction.UserLoggedOut, dbData, logUserActions);
                QDLib.LogUserConnection(UserID, QDLogAction.QDDrivesDisconnect, dbData, logUserActions);
            }
        }

        private void nfiQDriveMenu_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        #endregion

        private void tmrDriveCheck_Tick(object sender, EventArgs e)
        {
            if (localConnection) QDLib.ConnectQDDrives("", "", dbData, logUserActions, false, null, true);
            else
            {
                if(string.IsNullOrEmpty(UserID)) LoadQDData();
                QDLib.ConnectQDDrives(UserID, Password, dbData, logUserActions, false, null, true);
            }
        }

        private void QDrive_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!localConnection && disconnectAtShutdown)
            {
                QDLib.DisconnectAllDrives();
                if (!localConnection)
                {
                    QDLib.LogUserConnection(UserID, QDLogAction.QDDrivesDisconnect, dbData, logUserActions);
                    QDLib.LogUserConnection(UserID, QDLogAction.QDSystemAppClosed, dbData, logUserActions);
                }
            }
        }
    }
}
