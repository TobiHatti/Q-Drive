using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
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
    }
}
