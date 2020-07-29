using MySql.Data.MySqlClient;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace QDriveLib
{
    public static class QDLib
    {
        public static bool VerifyMasterPassword(string pPassword, string pDBHost, string pDBName, string pDBUser, string pDBPassword)
        {
            bool masterPasswordValid = false;

            try
            {
                using (WrapMySQL sql = new WrapMySQL(pDBHost, pDBName, pDBUser, pDBPassword))
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

        public static string GetMachineMac()
        {
            var macAddr =
           (
               from nic in NetworkInterface.GetAllNetworkInterfaces()
               where nic.OperationalStatus == OperationalStatus.Up
               select nic.GetPhysicalAddress().ToString()
           ).FirstOrDefault();

            return macAddr;
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

        public static int ConnectQDDrives(string pUserID, string pUserPassword, string dbHost, string dbName, string dbUser, string dbPassword, bool pDisconnectFirst = true)
        {
            // Disconnect all current drives
            if (pDisconnectFirst) DisconnectAllDrives();

            // Connect online-drives (online-synced)
            if (!string.IsNullOrEmpty(pUserID))
            {
                try
                {
                    using(WrapMySQL sql = new WrapMySQL(dbHost, dbName, dbUser, dbPassword))
                    {
                        sql.Open();
                        // Connect local network drives
                        using (MySqlDataReader reader = sql.ExecuteQuery("SELECT * FROM qd_drives INNER JOIN qd_assigns ON qd_drives.ID = qd_assigns.DriveID INNER JOIN qd_users ON qd_assigns.UserID = qd_users.ID WHERE qd_assigns.UserID = ?", pUserID))
                        {
                            while(reader.Read())
                            {
                                try
                                {
                                    ConnectDrive(
                                        Convert.ToChar(reader["CustomDriveLetter"]),
                                        Convert.ToString(reader["LocalPath"]),
                                        Cipher.Decrypt(Convert.ToString(reader["DUsername"]), pUserPassword),
                                        Cipher.Decrypt(Convert.ToString(reader["DPassword"]), pUserPassword),
                                        Convert.ToString(reader["CustomDriveName"]),
                                        Cipher.Decrypt(Convert.ToString(reader["DDomain"]), pUserPassword)
                                    );
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

                using (WrapSQLite sqlite = new WrapSQLite(QDInfo.ConfigFile, true))
                {
                    sqlite.Open();
                    // Connect local network drives
                    using (SQLiteDataReader reader = sqlite.ExecuteQuery("SELECT * FROM qd_drives"))
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                ConnectDrive(
                                    Convert.ToChar(reader["DriveLetter"]),
                                    Convert.ToString(reader["LocalPath"]),
                                    Cipher.Decrypt(Convert.ToString(reader["Username"]), QDInfo.LocalCipherKey),
                                    Cipher.Decrypt(Convert.ToString(reader["Password"]), QDInfo.LocalCipherKey),
                                    Convert.ToString(reader["DriveName"]),
                                    Cipher.Decrypt(Convert.ToString(reader["Domain"]), QDInfo.LocalCipherKey)
                                );
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

            return 0;
        }

        public static void DisconnectAllDrives()
        {
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
    }
}
