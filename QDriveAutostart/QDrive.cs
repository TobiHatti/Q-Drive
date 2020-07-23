using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QDriveAutostart
{
    public partial class QDrive : Form
    {
        public QDrive()
        {
            InitializeComponent();
            pbxQDriveSplash.Image = Image.FromFile(@"F:\Desktop\QDriveSplash.png");
        }

        private void bgwNetDriveConnector_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Application.Exit();
        }

        private void bgwNetDriveConnector_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblCurrentOperation.Text = e.UserState.ToString();
            pgbNetDriveProgress.Value = e.ProgressPercentage;
        }
    }
}
