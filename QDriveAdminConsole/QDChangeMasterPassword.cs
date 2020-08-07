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

namespace QDriveAdminConsole
{
    public partial class QDChangeMasterPassword : SfForm
    {
        public string MasterPassword;

        public QDChangeMasterPassword()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(txbOldPassword.Text != MasterPassword)
            {
                MessageBox.Show("Old password is not valid.", "Invalid password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!QDLib.ValidatePasswords(txbNewPassword.Text, txbConfirmNewPassword.Text)) return;

            MasterPassword = txbNewPassword.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
