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

namespace QDriveManager
{
    public partial class QDAddPrivateDrive : SfForm
    {
        // Reserved for editing entry
        public string DBEntryID = null;

        public string DrivePath;
        public string DisplayName;
        public string Username;
        public string Password;
        public string Domain;
        public string DriveLetter;

        public bool ForceAutofill = false;

        public QDAddPrivateDrive()
        {
            InitializeComponent();
            cbxDriveLetter.SelectedIndex = 2;
        }

        private void QDAddPrivateDrive_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Username)) txbUsername.Text = Username;
            if (!string.IsNullOrEmpty(Password)) txbPassword.Text = Password;
            if (!string.IsNullOrEmpty(Domain)) txbDomain.Text = Domain;

            if (ForceAutofill && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Domain))
            {
                txbUsername.ReadOnly = true;
                txbPassword.ReadOnly = true;
                txbDomain.ReadOnly = true;

                txbUsername.Enabled = false;
                txbPassword.Enabled = false;
                txbDomain.Enabled = false;
            }

            if (DBEntryID != null)
            {
                txbDrivePath.Text = DrivePath;
                txbDisplayName.Text = DisplayName;
                txbUsername.Text = Username;
                txbPassword.Text = Password;
                txbDomain.Text = Domain;

                for (int i = 0; i < cbxDriveLetter.Items.Count; i++)
                    if (cbxDriveLetter.Items[i].ToString()[0].ToString() == DriveLetter)
                        cbxDriveLetter.SelectedIndex = i;

                this.Text = "Edit private drive";
                btnSubmit.Text = "Update Drive";
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
            if (string.IsNullOrEmpty(txbDisplayName.Text) || string.IsNullOrEmpty(txbUsername.Text) || string.IsNullOrEmpty(txbPassword.Text))
            {
                MessageBox.Show("Please fill out all required options.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txbDrivePath.Text) || !txbDrivePath.Text.StartsWith(@"\\"))
            {
                MessageBox.Show("Please enter a valid drive-path.", "Invalid Drive-Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DrivePath = txbDrivePath.Text;
            DriveLetter = cbxDriveLetter.Text[0].ToString();
            DisplayName = txbDisplayName.Text;
            Username = txbUsername.Text;
            Password = txbPassword.Text;
            Domain = txbDomain.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
