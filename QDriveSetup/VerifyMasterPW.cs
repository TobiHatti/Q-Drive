using QDriveLib;
using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QDrive
{
    public partial class VerifyMasterPW : SfForm
    {
        public string DBHost;
        public string DBUser;
        public string DBPass;
        public string DBName;

        public VerifyMasterPW()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if(!QDLib.VerifyMasterPassword(txbMasterPassword.Text, DBHost, DBName, DBUser, DBPass)) 
            {
                MessageBox.Show("Master-Password is not valid. Please enter the corrent Master-Password, which has been set when the database was first initialised.", "Invalid Master-Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
