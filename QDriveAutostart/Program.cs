using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QDriveAutostart
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try 
            { 
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new QDrive());
            }
            catch
            {
                MessageBox.Show("Q-Drive encountered an unecpected error and needs to be shut down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
