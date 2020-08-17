using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
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

namespace QDriveLib
{
    public static class QDLib
    {
        public static void AlignPanels(Form form, List<Panel> panels, int width, int height)
        {
            form.Width = width;
            form.Height = height;
            foreach (Panel panel in panels) panel.Dock = DockStyle.Fill;
        }

        public static bool ValidatePasswords(string pPW1, string pPW2)
        {
            bool passwordsValid = true;

            if (pPW1 != pPW2)
            {
                MessageBox.Show("Passwords are not identical.", "Password invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                passwordsValid = false;
            }
            else if (string.IsNullOrEmpty(pPW1))
            {
                MessageBox.Show("Please enter a password.", "Password invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                passwordsValid = false;
            }

            return passwordsValid;
        }

        

        public static bool VerifyMasterPassword(string pPassword, WrapMySQLData pDBData)
        {
            bool masterPasswordValid = false;

            try
            {
                using (WrapMySQL sql = new WrapMySQL(pDBData))
                {
                    string passwordHash = sql.ExecuteScalarACon<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.MasterPassword);

                    if (passwordHash == HashPassword(pPassword)) masterPasswordValid = true;
                    else masterPasswordValid = false;
                }
            }
            catch
            {
                masterPasswordValid = false;
            }
            return masterPasswordValid;
        }

        public static bool RunQDriveSetup()
        {
            string qdSetupProgram = "QDriveSetup.exe";

            if (File.Exists(qdSetupProgram))
            {
                Process.Start(qdSetupProgram);
                return true;
            }
            else MessageBox.Show($"Could not start setup!\r\n\r\nThe setup could not be started > The file \"{qdSetupProgram}\" could not be found. If this error remains, please try to re-install the program.", "Could not start setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        public static bool IsQDConfigured()
        {
            if (File.Exists(QDInfo.ConfigFile))
            {
                try
                {
                    using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile))
                    {
                        object result = sqlite.ExecuteScalarACon("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBL.SetupSuccess);

                        if (result != null && Convert.ToBoolean(Convert.ToInt16(result))) return true;
                        else return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            else return false;
        }

        public static List<DriveViewItem> CreateDriveList(bool pIsLocalConnection, string pUserID, string pUserPassword, WrapMySQLData pDBConDat)
        {
            List<DriveViewItem> driveList = new List<DriveViewItem>();

            using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile))
            {
                sqlite.Open();
                using (SQLiteDataReader reader = (SQLiteDataReader)sqlite.ExecuteQuery("SELECT * FROM qd_drives"))
                {
                    while (reader.Read())
                    {
                        driveList.Add(new DriveViewItem(
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
            }

            if (!pIsLocalConnection)
            {
                try
                {
                    using (WrapMySQL mysql = new WrapMySQL(pDBConDat))
                    {
                        mysql.Open();
                        using (MySqlDataReader reader = (MySqlDataReader)mysql.ExecuteQuery("SELECT *, qd_assigns.ID as AID, qd_drives.ID AS DID FROM qd_drives INNER JOIN qd_assigns ON qd_drives.ID = qd_assigns.DriveID WHERE qd_assigns.UserID = ?", pUserID))
                        {
                            while (reader.Read())
                            {
                                driveList.Add(new DriveViewItem(
                                Convert.ToString(reader["AID"]),
                                Convert.ToString(reader["CustomDriveName"]),
                                Convert.ToString(reader["LocalPath"]),
                                Convert.ToString(reader["CustomDriveLetter"]),
                                false,
                                Convert.ToBoolean(Convert.ToInt16(reader["IsPublic"])),
                                Cipher.Decrypt(Convert.ToString(reader["DUsername"]), pUserPassword),
                                Cipher.Decrypt(Convert.ToString(reader["DPassword"]), pUserPassword),
                                Cipher.Decrypt(Convert.ToString(reader["DDomain"]), pUserPassword),
                                Convert.ToString(reader["DID"])
                            ));
                            }
                        }
                        mysql.Close();
                    }
                }
                catch { }
            }

            driveList.Sort();

            return driveList;
        }


        private static string uniqueID = null;

        public static string GetUniqueDeviceID()
        {
            if (uniqueID == null)
            {
                if(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\QDrive", "QDSysID", null) == null)
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("QDrive");
                    key.SetValue("QDSysID", Guid.NewGuid());
                    key.Close();
                }

                uniqueID = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("QDrive", true).GetValue("QDSysID").ToString();
            }

            return uniqueID;
        }

        public static string HashPassword(string pPassword)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pPassword));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static int ConnectQDDrives(string pUserID, string pUserPassword, WrapMySQLData pDBData, bool pLogUserData, bool pDisconnectFirst = true, List<DriveViewItem> drives = null, bool ConnectOnlyIfNotAvailable = false)
        {
            // Disconnect all current drives
            if (pDisconnectFirst)
            {
                DisconnectAllDrives(drives);
                if(!string.IsNullOrEmpty(pUserID)) LogUserConnection(pUserID, QDLogAction.QDDrivesDisconnect, pDBData, pLogUserData);
            }

            // Connect online-drives (online-synced)
            if (!string.IsNullOrEmpty(pUserID))
            {
                try
                {
                    using(WrapMySQL sql = new WrapMySQL(pDBData))
                    {
                        sql.Open();
                        // Connect local network drives
                        using (MySqlDataReader reader = (MySqlDataReader)sql.ExecuteQuery("SELECT * FROM qd_drives INNER JOIN qd_assigns ON qd_drives.ID = qd_assigns.DriveID INNER JOIN qd_users ON qd_assigns.UserID = qd_users.ID WHERE qd_assigns.UserID = ?", pUserID))
                        {
                            while(reader.Read())
                            {
                                try
                                {
                                    if(!ConnectOnlyIfNotAvailable || (ConnectOnlyIfNotAvailable && !Directory.Exists($@"{Convert.ToChar(reader["CustomDriveLetter"])}:\")))
                                    {
                                        //MessageBox.Show("Try to connect " + Convert.ToString(reader["CustomDriveName"]));
                                        
                                        ConnectDrive(
                                            Convert.ToChar(reader["CustomDriveLetter"]),
                                            Convert.ToString(reader["LocalPath"]),
                                            Cipher.Decrypt(Convert.ToString(reader["DUsername"]), pUserPassword),
                                            Cipher.Decrypt(Convert.ToString(reader["DPassword"]), pUserPassword),
                                            Convert.ToString(reader["CustomDriveName"]),
                                            Cipher.Decrypt(Convert.ToString(reader["DDomain"]), pUserPassword)
                                        );
                                    }
                                }
                                catch
                                {
                                    return 5;
                                }
                            }
                        }

                        sql.Close();

                        // Conenct remote network drives
                        // TODO
                    }
                }
                catch
                {
                    return 4;
                }
            }


            // Connect Private drives (not online-synced)
            try
            {
                if (!File.Exists(QDInfo.ConfigFile)) return 1;

                using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile))
                {
                    sqlite.Open();
                    // Connect local network drives
                    using (SQLiteDataReader reader = (SQLiteDataReader)sqlite.ExecuteQuery("SELECT * FROM qd_drives"))
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                if (!ConnectOnlyIfNotAvailable || (ConnectOnlyIfNotAvailable && Directory.Exists($@"{Convert.ToChar(reader["CustomDriveLetter"])}:\")))
                                {
                                    //MessageBox.Show("Try to connect " + Convert.ToString(reader["CustomDriveName"]));

                                    ConnectDrive(
                                        Convert.ToChar(reader["DriveLetter"]),
                                        Convert.ToString(reader["LocalPath"]),
                                        Cipher.Decrypt(Convert.ToString(reader["Username"]), QDInfo.LocalCipherKey),
                                        Cipher.Decrypt(Convert.ToString(reader["Password"]), QDInfo.LocalCipherKey),
                                        Convert.ToString(reader["DriveName"]),
                                        Cipher.Decrypt(Convert.ToString(reader["Domain"]), QDInfo.LocalCipherKey)
                                    );
                                }
                            }
                            catch
                            {
                                return 3;
                            }
                        }
                    }
                    sqlite.Close();

                    // Conenct remote network drives
                    // TODO
                }
            }
            catch
            {
                return 2;
            }

            if (!string.IsNullOrEmpty(pUserID)) LogUserConnection(pUserID, QDLogAction.QDDrivesConnect, pDBData, pLogUserData);

            return 0;
        }

        public static bool TestConnection(WrapMySQLData pOnlineDBConDat, bool messageOnSuccess = true)
        {
            bool success = false;

            using (WrapMySQL sql = new WrapMySQL(pOnlineDBConDat))
            {
                try
                {
                    sql.Open();
                    success = true;
                }
                catch { success = false; }
                finally { sql.Close(); }
            }

            if (success)
            {
                if(messageOnSuccess) MessageBox.Show("Connection to the database established successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Could not connect to the database.", "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return success;
        }

        public static bool VerifyPassword(bool pIsLocalConnection, string pUsername, string pPassword, out string pUserID, WrapMySQLData pDBData)
        {
            pUserID = "";

            bool passwordValid = false;

            if (pIsLocalConnection)
            {
                bool errorEncountered = false;
                using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile))
                {
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
                }

                if (errorEncountered) MessageBox.Show("An error occured whilst trying to authenticate the user.", "Authentication error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (WrapMySQL mysql = new WrapMySQL(pDBData))
                {
                    mysql.Open();
                    using (MySqlDataReader reader = (MySqlDataReader)mysql.ExecuteQuery("SELECT * FROM qd_users WHERE Username = ? AND Password = ?", pUsername, QDLib.HashPassword(pPassword)))
                    {
                        while (reader.Read())
                        {
                            pUserID = Convert.ToString(reader["ID"]);
                            passwordValid = true;
                        }
                    }
                    mysql.Close();
                }
            }

            return passwordValid;
        }

        public static void DisconnectAllDrives(List<DriveViewItem> drives = null)
        {
            if (drives != null)
            {
                foreach (DriveViewItem drive in drives)
                {
                    ProcessStartInfo psireg = new ProcessStartInfo()
                    {
                        FileName = "cmd.exe",
                        Arguments = $@"/c reg delete HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\MountPoints2\{drive.DrivePath.Replace('\\', '#')} /f /va",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    Process.Start(psireg);
                }
            }

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/c net use * /delete /yes",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(psi);
        }

        public static void ConnectDrive(char pDriveLetter, string pPath, string pUsername, string pPassword, string pName, string pDomain = null)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = $@"/c reg add HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\MountPoints2\{pPath.Replace('\\', '#')} /v _LabelFromReg /t REG_SZ /f /d ""{pName}""",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(psi);

            if (pDomain == null)
            {
                psi = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $@"/c net use {pDriveLetter}: {pPath} /user:{pUsername} {pPassword}",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
            }
            else
            {
                psi = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $@"/c net use {pDriveLetter}: {pPath} /user:{pDomain}\{pUsername} {pPassword}",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
            }


            Process.Start(psi);
        }

        private static string GetMACAddress()
        {
            return (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
        }

        public static void LogUserConnection(string pUserID, QDLogAction pLogAction, WrapMySQLData pDBData, bool pLogUserActionAllowed)
        {
            if (!pLogUserActionAllowed) return;

            try
            {
                using (WrapMySQL mysql = new WrapMySQL(pDBData))
                {
                    string deviceID;
                    string deviceMac = GetMACAddress();

                    mysql.Open();
                    mysql.TransactionBegin();

                    try
                    {
                        // Add device to DB if it doesn't exist. Get device ID
                        if (mysql.ExecuteScalar<int>("SELECT COUNT(*) FROM qd_devices WHERE MacAddress = ? AND LogonName = ? AND DeviceName = ?", deviceMac, Environment.UserName, Environment.MachineName) == 0)
                        {
                            deviceID = Guid.NewGuid().ToString();
                            mysql.ExecuteNonQuery("INSERT INTO qd_devices (ID, MacAddress, LogonName, DeviceName) VALUES (?,?,?,?)",
                                deviceID,
                                deviceMac,
                                Environment.UserName,
                                Environment.MachineName
                            );
                        }
                        else
                        {
                            deviceID = mysql.ExecuteScalar<string>("SELECT ID FROM qd_devices WHERE MacAddress = ? AND LogonName = ? AND DeviceName = ?", deviceMac, Environment.UserName, Environment.MachineName);
                        }

                        mysql.ExecuteNonQuery("INSERT INTO qd_conlog (ID, UserID, DeviceID, LogTime, LogAction) VALUES (?,?,?, NOW() ,?)",
                            Guid.NewGuid(),
                            pUserID,
                            deviceID,
                            pLogAction
                        );

                        mysql.TransactionCommit();
                    }
                    catch
                    {
                        mysql.TransactionRollback();
                    }

                    mysql.Close();
                }
            }
            catch { }
        }

    }

    public enum QDLogAction
    {
        UserRegistered,             // -> User registers
        UserLoggedIn,               // -> User loggs into manager
        UserLoggedInAutoStart,      // -> User loggs in via autostart
        UserLoggedOut,              // -> User loggs out via manager
        UserDrivelistUpdated,       // -> User manually updates manager drivelist

        UserChangedPassword,        // -> User changed password
        UserCreactedBackup,         // -> User created backup file
        UserLoadedBackup,           // -> User loaded backup file
        UserDisabledAutostart,      // -> User disabled QD-Autostart
        UserEnabledAutostart,       // -> User enabled QD-Autostart
        UserResetLocalDatabase,     // -> User reset local database

        DrivePrivateAdded,          // -> User added a private drive
        DrivePublicAdded,           // -> User added a public drive
        DrivePrivateEdited,         // -> User edited a private drive
        DrivePublicEdited,          // -> User edited a public drive
        DriveRemoved,               // -> User removed a drive

        QDDrivesDisconnect,         // -> QD Disconnected all netdrives 
        QDDrivesConnect,            // -> QD Connected all netdrives
        QDDrivelistUpdated,         // -> QD updated drivelist in manager

        QDSystemAutostartFinished,  // -> QD-Autostart has finished
        QDSystemAppClosed,          // -> QD-Autostart app has been closed by user
        QDSystemDeviceShutdown      // x-> QD-Autostart app has been closed by system (shutdown)
        
    }

    public class DriveViewItem : IComparable
    {
        public string ID { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string DrivePath { get; set; } = string.Empty;
        public string DriveLetter { get; set; } = string.Empty;
        public bool IsLocalDrive { get; set; } = false;
        public bool IsPublicDrive { get; set; } = false;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string ParentDriveID { get; set; } = string.Empty;


        public DriveViewItem(string pID, string pDisplayName, string pDrivePath, string pDriveLetter, bool pIsLocalDrive, bool pIsPublicDrive, string pUsername, string pPassword, string pDomain, string pParentDriveID = "")
        {
            ID = pID;
            DisplayName = pDisplayName;
            DrivePath = pDrivePath;
            DriveLetter = pDriveLetter;
            IsLocalDrive = pIsLocalDrive;
            IsPublicDrive = pIsPublicDrive;
            Username = pUsername;
            Password = pPassword;
            Domain = pDomain;

            ParentDriveID = pParentDriveID;
        }

        public int CompareTo(object obj)
        {
            if (Convert.ToInt32(Convert.ToChar((obj as DriveViewItem).DriveLetter)) > Convert.ToInt32(Convert.ToChar(DriveLetter))) return -1;
            else return 1;
        }
    }

   
}
