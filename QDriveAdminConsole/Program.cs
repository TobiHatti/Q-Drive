using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QDriveAdminConsole
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new QDriveAdminConsole());
            }
            catch
            {
                MessageBox.Show("Q-Drive encountered an unecpected error and needs to be shut down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
