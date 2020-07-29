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
                    string cipher = sql.ExecuteScalarACon<string>("SELECT QDValue FROM qd_info WHERE QDKey = ?", QDInfo.DBO.VerificationKey);

                    try
                    {
                        string decrypt = Cipher.Decrypt(cipher, pPassword);
                        if (decrypt == QDInfo.VerifyKey) masterPasswordValid = true;
                    }
                    catch { }
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

        public static int ConnectQDDrives(string pUserID, bool pDisconnectFirst = true)
        {
            try
            {
                if (!File.Exists(QDInfo.ConfigFile)) return 1;

                using (WrapSQLite sql = new WrapSQLite(QDInfo.ConfigFile, true))
                {
                    using (SQLiteDataReader reader = sql.ExecuteQuery("SELECT * FROM qd_drives WHERE LocalNetwork = 1 UserID = ?", pUserID))
                    {
                        if (pDisconnectFirst) DisconnectAllDrives();

                        while (reader.Read())
                        {
                            try
                            {
                                ConnectDrive(
                                    Convert.ToChar(reader["DriveLetter"]),
                                    Convert.ToString(reader["Path"]),
                                    Convert.ToString(reader["Username"]),
                                    Cipher.Decrypt(Convert.ToString(reader["Password"]), QDInfo.LocalCipherKey),
                                    Convert.ToString(reader["DriveName"]),
                                    Convert.ToString(reader["Domain"])
                                );
                            }
                            catch
                            {
                                return 3;
                            }
                        }
                    }
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
