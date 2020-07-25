using System;
using System.Collections.Generic;
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
                    string cipher = sql.ExecuteScalarACon<string>("SELECT QDValue FROM qd_info WHERE QDKey = 'VerificationKey'");

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
    }
}
