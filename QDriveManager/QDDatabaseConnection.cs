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

namespace QDriveManager
{
    public partial class QDDatabaseConnection : SfForm
    {
        public WrapMySQLData dbConDat = null;

        public QDDatabaseConnection()
        {
            InitializeComponent();
        }

        private void QDDatabaseConnection_Load(object sender, EventArgs e)
        {
            if(dbConDat != null)
            {
                txbHostname.Text = dbConDat.Hostname;
                txbName.Text = dbConDat.Database;
                txbUsername.Text = dbConDat.Username;
                txbPassword.Text = dbConDat.Password;
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e) => QDLib.TestConnection(new WrapMySQLData()
        {
            Hostname = txbHostname.Text,
            Database = txbName.Text,
            Username = txbUsername.Text,
            Password = txbPassword.Text
        });

        private void btnSubmit_Click(object sender, EventArgs e) => Submit();

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SubmitForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }

        private void Submit()
        {
            dbConDat = new WrapMySQLData()
            {
                Hostname = txbHostname.Text,
                Database = txbName.Text,
                Username = txbUsername.Text,
                Password = txbPassword.Text
            };

            if (QDLib.TestConnection(dbConDat, false))
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

    }
}
