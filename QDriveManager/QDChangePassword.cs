using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using QDriveLib;
using Syncfusion.WinForms.Controls;

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

        private void btnSubmit_Click(object sender, EventArgs e) => Submit();

        private void SubmitForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Submit();
                e.Handled = e.SuppressKeyPress = true;
            }
        }

        private void Submit()
        {
            if (!NoOldPassword && OldPassword != txbOldPassword.Text)
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
