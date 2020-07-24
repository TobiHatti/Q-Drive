using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QDriveLib
{
    public static class QDInfo
    {
        public const string VerifyKey = "DebugKey";

        public const string LocalDataFile = "loc.db";

        public static string ConfigFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"Endev", "Q-Drive", "qd.db");

        public static string DefaultLocalDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Endev", "Q-Drive");
    }
}
