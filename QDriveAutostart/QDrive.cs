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

namespace QDriveAutostart
{
    public partial class QDrive : Form
    {
        private bool localConnection;
        private bool promptPassword;

        private string Username;
        private string Password;
        private string UserID;

        private WrapMySQLConDat dbData = new WrapMySQLConDat();

        private List<DriveViewItem> driveList = new List<DriveViewItem>();

        #region Page Layout and Initial Loading =================================================================[RF]=
        
        public QDrive()
        {
            InitializeComponent();
            pbxQDriveSplash.Image = Properties.Resources.QDriveSplash;
            lblVersionInfo.Text = QDInfo.QDVersion;

            ContextMenu ctmQDriveMenu = new ContextMenu();
            ctmQDriveMenu.MenuItems.Add("Open QD-Manager", nfiQDriveMenu_OpenManager_Click);
            ctmQDriveMenu.MenuItems.Add("Reconnect Drives", nfiQDriveMenu_Reconnect_Click);
            ctmQDriveMenu.MenuItems.Add("Log Off / Disconnect", nfiQDriveMenu_LogOff_Click);
            ctmQDriveMenu.MenuItems.Add("Quit Q-Drive", nfiQDriveMenu_Close_Click);

            nfiQDriveMenu.Visible = true;
            nfiQDriveMenu.ContextMenu = ctmQDriveMenu;
        }

        #endregion

        #region Login, Load and connect drives ==================================================================[RF]=

        private void QDrive_Shown(object sender, EventArgs e)
        {
            Thread.Sleep(1000);

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

            if (localConnection) QDLib.ConnectQDDrives("", "", dbData, true, driveList);
            else QDLib.ConnectQDDrives(UserID, Password, dbData, true, driveList);

            Thread.Sleep(1000);

            Hide(); 
        }

        private int LoadQDData()
        {
            // Load local Data

            using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile, true))
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
                using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile, true))
                {
                    sqlite.Open();

                    sqlite.Close();
                }
            }
            catch { return 3; }

            // Load Online Drives
            if (!localConnection)
            {
                try
                {
                    using (WrapMySQL mysql = new WrapMySQL(dbData))
                    {
                        mysql.Open();

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
            if (localConnection) QDLib.ConnectQDDrives("", "", dbData, true);
            else QDLib.ConnectQDDrives(UserID, Password, dbData, true);
        }

        private void nfiQDriveMenu_LogOff_Click(object sender, EventArgs e)
        {
            QDLib.DisconnectAllDrives(driveList);
        }

        private void nfiQDriveMenu_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
