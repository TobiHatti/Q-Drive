using MySql.Data.MySqlClient;
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
    public partial class QDAddPublicDrive : SfForm
    {
        // Reserved for editing entry
        public string DBEntryID = null;

        public WrapMySQLData DBData;

        public string DriveID;
        public string CustomDriveName;
        public string CustomDriveLetter;
        public string Username;
        public string Password;
        public string Domain;

        public bool ForceAutofill = false;

        public QDAddPublicDrive()
        {
            InitializeComponent();
            pbxNoDrivesFound.Image = Properties.Resources.QDriveNoDrivesAvailable;
        }

        private void QDAddPublicDrive_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList()
            {
                ImageSize = new Size(60, 60),
                ColorDepth = ColorDepth.Depth32Bit,

            };

            imgList.Images.Add("PublicUp", Properties.Resources.QDriveOnlinePublicUp);

            grvPublicDrives.SmallImageList = imgList;
            grvPublicDrives.GroupViewItems.Clear();

            using (WrapMySQL sql = new WrapMySQL(DBData))
            {
                sql.Open();

                using (MySqlDataReader reader = (MySqlDataReader)sql.ExecuteQuery("SELECT * FROM qd_drives WHERE IsPublic = 1 ORDER BY DefaultDriveLetter ASC"))
                {
                    while(reader.Read())
                    {
                        grvPublicDrives.GroupViewItems.Add(
                            new GroupViewItemEx(
                                $"({Convert.ToString(reader["DefaultDriveLetter"])}:\\) {Convert.ToString(reader["DefaultName"])}\r\n({Convert.ToString(reader["LocalPath"])})",
                                new DriveViewItem(
                                    Convert.ToString(reader["ID"]),
                                    Convert.ToString(reader["DefaultName"]),
                                    Convert.ToString(reader["LocalPath"]),
                                    Convert.ToString(reader["DefaultDriveLetter"]),
                                    false,
                                    true,
                                    "",
                                    "",
                                    ""
                                ),
                                0
                            )
                        );
                    }
                }

                sql.Close();
            }

            if (grvPublicDrives.GroupViewItems.Count == 0)
            {
                pbxNoDrivesFound.Visible = true;
                btnSubmit.Enabled = false;
            }
            else pbxNoDrivesFound.Visible = false;

            if (!string.IsNullOrEmpty(Username)) txbUsername.Text = Username;
            if (!string.IsNullOrEmpty(Password)) txbPassword.Text = Password;
            if (!string.IsNullOrEmpty(Domain)) txbDomainName.Text = Domain;

            if (ForceAutofill && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Domain))
            {
                txbUsername.ReadOnly = true;
                txbPassword.ReadOnly = true;
                txbDomainName.ReadOnly = true;

                txbUsername.Enabled = false;
                txbPassword.Enabled = false;
                txbDomainName.Enabled = false;
            }

            // Set values for edit mode
            if (DBEntryID != null)
            {
                txbDisplayName.Text = CustomDriveName;
                txbUsername.Text = Username;
                txbPassword.Text = Password;
                txbDomainName.Text = Domain;

                for (int i = 0; i < cbxDriveLetter.Items.Count; i++)
                    if (cbxDriveLetter.Items[i].ToString()[0].ToString() == CustomDriveLetter)
                        cbxDriveLetter.SelectedIndex = i;

                for(int i = 0; i < grvPublicDrives.GroupViewItems.Count; i++)
                {
                    if ((grvPublicDrives.GroupViewItems[i] as GroupViewItemEx).Drive.ID == DriveID)
                    {
                        grvPublicDrives.SelectedItem = i;
                        txbDrivePath.Text = (grvPublicDrives.GroupViewItems[i] as GroupViewItemEx).Drive.DrivePath;
                    }
                }

                this.Text = "Edit public drive";
                btnSubmit.Text = "Update Drive";
            }
        }
        
        private void grvPublicDrives_Click(object sender, EventArgs e)
        {
            if (grvPublicDrives.SelectedItem != -1)
            {
                DriveViewItem drive = (grvPublicDrives.GroupViewItems[grvPublicDrives.SelectedItem] as GroupViewItemEx).Drive;

                txbDrivePath.Text = drive.DrivePath;
                txbDisplayName.Text = drive.DisplayName;

                DriveID = drive.ID;

                for (int i = 0; i < cbxDriveLetter.Items.Count; i++)
                    if (cbxDriveLetter.Items[i].ToString()[0].ToString() == drive.DriveLetter)
                        cbxDriveLetter.SelectedIndex = i;
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
                Submit();
        }

        private void Submit()
        {
            if (string.IsNullOrEmpty(txbDisplayName.Text)) { MessageBox.Show("Please enter a Display-Name.", "Missing Value", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrEmpty(txbUsername.Text)) { MessageBox.Show("Please enter a Username.", "Missing Value", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrEmpty(txbPassword.Text)) { MessageBox.Show("Please enter a Password.", "Missing Value", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            CustomDriveName = txbDisplayName.Text;
            CustomDriveLetter = cbxDriveLetter.Text[0].ToString();
            Username = txbUsername.Text;
            Password = txbPassword.Text;
            Domain = txbDomainName.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
