using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

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
    public static class QDInfo
    {
        public static string LocalCipherKey
        {
            get => $"Endev{QDLib.GetUniqueDeviceID()}QDSystem";
        }

        public static string GlobalCipherKey
        {
            get => "Endev2dsUSWTWRJLRQc85zDLNnn6wpFZ5qHCDGphJsYWBaFP9Sdmawhw3mMBBzKG9GSftQDSystem";
        }

        public static string QDVersion
        {
            get => typeof(QDInfo).Assembly.GetName().Version.ToString(3);
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
            public const string DefaultDomain = "DefaultDomain";
            public const string UseLoginAsDriveAuthentication = "UseLoginAsDriveAuthentication";
            public const string ForceLoginAsDriveAuthentication = "ForceLoginAsDriveAuthentication";
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
