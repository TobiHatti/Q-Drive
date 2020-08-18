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
            if (QDLib.HashPassword(txbOldPassword.Text) != MasterPassword)
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
