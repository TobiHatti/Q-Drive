using System;
using System.Collections.Generic;
using System.IO;

namespace QDriveLib
{
    public static class QDLog
    {
        public static List<string> sessionLog = new List<string>();

        public static void Log(string logMessage, bool isError)
        {
            if (!string.IsNullOrWhiteSpace(logMessage))
            {
                string logFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Endev", "Q-Drive", "Log", "QDLog" + DateTime.Today.ToString("yyyyMMdd") + ".log");

                if (!Directory.Exists(Path.GetDirectoryName(logFilename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(logFilename));

                string logMsgFormated = String.Empty;

                if (isError)
                    logMsgFormated = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "][ERROR] " + logMessage;
                else
                    logMsgFormated = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "][INFO] " + logMessage;

                File.AppendAllText(logFilename, logMsgFormated);

                sessionLog.Add(logMsgFormated);
                if (sessionLog.Count > 100) sessionLog.RemoveAt(0);
            }
        }
    }
}
