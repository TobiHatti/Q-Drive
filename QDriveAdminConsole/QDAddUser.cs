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
using WrapSQL;

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
    public partial class QDAddUser : SfForm
    {
        public string EditID = null;

        public string DisplayName = "";
        public string Username = "";
        public string Password = "";

        public WrapMySQLData DBData = null;

        public QDAddUser()
        {
            InitializeComponent();
        }

        private void QDAddUser_Load(object sender, EventArgs e)
        {
            if(EditID != null)
            {
                using(WrapMySQL mysql = new WrapMySQL(DBData))
                {
                    mysql.Open();
                    Username = mysql.ExecuteScalar<string>("SELECT Username FROM qd_users WHERE ID = ?", EditID);
                    DisplayName = mysql.ExecuteScalar<string>("SELECT Name FROM qd_users WHERE ID = ?", EditID);
                    mysql.Close();

                    txbUsername.Text = Username;
                    txbDisplayName.Text = DisplayName;

                   
                }

                this.Text = "Edit User";
                btnSubmit.Text = "Save Changes";
                lblRegisterTitle.Text = "Update user-data";
                lblPassword.Text = "New Password";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e) => Submit();

        private void txbDisplayName_TextChanged(object sender, EventArgs e)
        {
            if(EditID == null) txbUsername.Text = txbDisplayName.Text.Replace(' ', '.').ToLower();
        }

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
            if (EditID == null || (EditID != null && !string.IsNullOrEmpty(txbPassword.Text) && !string.IsNullOrEmpty(txbConfirmPassword.Text)))
                if (!QDLib.ValidatePasswords(txbPassword.Text, txbConfirmPassword.Text)) return;

            if (string.IsNullOrEmpty(txbDisplayName.Text))
            {
                MessageBox.Show("Please enter a valid display-name.", "Invalid Display-Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txbUsername.Text))
            {
                MessageBox.Show("Please enter a valid username.", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (EditID == null || (EditID != null && Username != txbUsername.Text))
            {
                using (WrapMySQL mysql = new WrapMySQL(DBData))
                {
                    if (mysql.ExecuteScalarACon<int>("SELECT COUNT(*) FROM qd_users WHERE Username = ?", txbUsername.Text) != 0)
                    {
                        MessageBox.Show("Username already in use. Please choose another username", "Username already in use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            DisplayName = txbDisplayName.Text;
            Username = txbUsername.Text;

            if (EditID == null || (EditID != null && !string.IsNullOrEmpty(txbPassword.Text) && !string.IsNullOrEmpty(txbConfirmPassword.Text))) Password = txbPassword.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
