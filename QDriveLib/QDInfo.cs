using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace QDriveLib
{
    public static class QDInfo
    {
        public static string LocalCipherKey
        {
            get
            {
                return $"Endev{QDLib.GetMachineMac()}QDSystem";
            }
        }

        public static string ConfigFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"Endev", "Q-Drive", "qd.db");

        /// <summary>
        /// Setting-Values for online database
        /// </summary>
        public static class DBO
        {
            public const string UserCanToggleKeepLoggedIn = "UserCanToggleKeepLoggedIn";
            public const string UserCanAddPrivateDrive = "UserCanAddPrivateDrive";
            public const string UserCanAddPublicDrive = "UserCanAddPublicDrive";
            public const string UserCanSelfRegister = "UserCanSelfRegister";
            public const string MasterPassword = "MasterPassword";
        }

        /// <summary>
        /// Setting-Values for local database
        /// </summary>
        public static class DBL
        {
            public const string IsOnlineLinked = "IsOnlineLinked";
            public const string AlwaysPromptPassword = "AlwaysPromptPassword";
            public const string SetupSuccess = "SetupSuccess";
            public const string DBHost = "DBHost";
            public const string DBName = "DBName";
            public const string DBUsername = "DBUsername";
            public const string DBPassword = "DBPassword";
            public const string UserID = "UserID";
            public const string DefaultUsername = "DefaultUsername";
            public const string DefaultPassword = "DefaultPassword";
        }
    }
}
