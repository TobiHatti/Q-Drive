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
    public partial class QDAddPublicDrive : SfForm
    {
        public bool EditMode = false;

        public string DrivePath = "";
        public string DriveName = "";
        public string DriveLetter = "";
        public bool CanBeDeployed = false;

        public QDAddPublicDrive()
        {
            InitializeComponent();
            cbxDriveLetter.SelectedIndex = 2;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e) => Submit();

        private void QDAddPublicDrive_Load(object sender, EventArgs e)
        {
            if(EditMode)
            {
                txbDrivePath.Text = DrivePath;
                txbDisplayName.Text = DriveName;

                for (int i = 0; i < cbxDriveLetter.Items.Count; i++)
                    if (cbxDriveLetter.Items[i].ToString()[0].ToString() == DriveLetter)
                        cbxDriveLetter.SelectedIndex = i;

                this.Text = "Edit Public Drive";
                lblAddDriveTitle.Text = "Edit public drive template";
                btnSubmit.Text = "Update Drive";
            }
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
            if (string.IsNullOrEmpty(txbDrivePath.Text) || !txbDrivePath.Text.StartsWith(@"\\"))
            {
                MessageBox.Show("Please enter a valid drive-path.", "Invalid Drive-Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txbDisplayName.Text))
            {
                MessageBox.Show("Please enter a display-name.", "Invalid Drive-Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DrivePath = txbDrivePath.Text;
            DriveName = txbDisplayName.Text;
            DriveLetter = cbxDriveLetter.Text[0].ToString();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
