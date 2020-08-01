﻿using MySql.Data.MySqlClient;
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

        public static bool VerifyMasterPassword(string pPassword, WrapMySQLConDat pDBData)
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

        public static int ConnectQDDrives(string pUserID, string pUserPassword, WrapMySQLConDat pDBData, bool pDisconnectFirst = true, List<DriveViewItem> drives = null)
        {
            // Disconnect all current drives
            if (pDisconnectFirst) DisconnectAllDrives(drives);

            // Connect online-drives (online-synced)
            if (!string.IsNullOrEmpty(pUserID))
            {
                try
                {
                    using(WrapMySQL sql = new WrapMySQL(pDBData))
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
