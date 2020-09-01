using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QDrive
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
                Application.Run(new QDriveSetup());
            }
            catch
            {
                MessageBox.Show("Q-Drive encountered an unecpected error and needs to be shut down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
