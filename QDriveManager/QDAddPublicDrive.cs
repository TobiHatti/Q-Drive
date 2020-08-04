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

namespace QDriveManager
{
    public partial class QDAddPublicDrive : SfForm
    {
        // Reserved for editing entry
        public string DBEntryID = null;

        public WrapMySQLConDat DBData;

        public string DriveID;
        public string CustomDriveName;
        public string CustomDriveLetter;
        public string Username;
        public string Password;
        public string Domain;

        public QDAddPublicDrive()
        {
            InitializeComponent();
            pbxNoDrivesFound.Image = Properties.Resources.QDriveNoDrivesAvailable;
        }

        private void QDAddPublicDrive_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList()
            {
                ImageSize = new Size(80, 80),
                ColorDepth = ColorDepth.Depth32Bit,

            };

            imgList.Images.Add("PublicUp", Properties.Resources.QDriveOnlinePublicUp);

            grvPublicDrives.SmallImageList = imgList;
            grvPublicDrives.GroupViewItems.Clear();

            using (WrapMySQL sql = new WrapMySQL(DBData))
            {
                sql.Open();

                using (MySqlDataReader reader = sql.ExecuteQuery("SELECT * FROM qd_drives WHERE IsPublic = 1"))
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


        private void btnSubmit_Click(object sender, EventArgs e)
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
