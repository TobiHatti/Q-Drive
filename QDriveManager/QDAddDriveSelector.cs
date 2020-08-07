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
    public partial class QDAddDriveSelector : SfForm
    {
        public int SelectedOption = 0;
        public bool CanAddPrivateDrive = false;
        public bool CanAddPublicDrive = false;

        public QDAddDriveSelector()
        {
            InitializeComponent();
        }

        private void QDAddDriveSelector_Load(object sender, EventArgs e)
        {
            if(!CanAddPrivateDrive)
            {
                rbnPrivateAccountLinked.Enabled = false;
                rbnPrivateDeviceLinked.Enabled = false;

                lblPrivateDriveAccountLinked.Enabled = false;
                lblPrivateDriveDeviceLinked.Enabled = false;

                lblPrivateDriveAccountLinked.Text += "\r\n(To enable this option, contact your network administrator.)";
                lblPrivateDriveDeviceLinked.Text += "\r\n(To enable this option, contact your network administrator.)";
            }

            if(!CanAddPublicDrive)
            {
                rbnPublicDrive.Enabled = false;
                lblPublicDrive.Enabled = false;
                lblPublicDrive.Text += "\r\n(To enable this option, contact your network administrator.)";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (rbnPublicDrive.Checked) SelectedOption = 1;
            else if (rbnPrivateAccountLinked.Checked) SelectedOption = 2;
            else SelectedOption = 3;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        
    }
}
