using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using QDriveLib;
using Syncfusion.WinForms.Controls;

namespace QDriveManager
{
    public partial class QDChangePassword : SfForm
    {
        public string OldPassword = null;
        public string NewPassword = null;

        public bool NoOldPassword = false;

        public QDChangePassword()
        {
            InitializeComponent();
        }

        private void QDChangePassword_Load(object sender, EventArgs e)
        {
            if (NoOldPassword)
            {
                txbOldPassword.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(!NoOldPassword && OldPassword != txbOldPassword.Text)
            {
                MessageBox.Show("Old password is not valid.", "Invalid password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!QDLib.ValidatePasswords(txbNewPassword.Text, txbConfirmNewPassword.Text)) return;

            NewPassword = txbNewPassword.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
