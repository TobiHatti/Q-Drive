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

        public QDAddPrivateDrive()
        {
            InitializeComponent();
            cbxDriveLetter.SelectedIndex = 2;
        }

        private void QDAddPrivateDrive_Load(object sender, EventArgs e)
        {
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txbDrivePath.Text) || string.IsNullOrEmpty(txbDisplayName.Text) || string.IsNullOrEmpty(txbUsername.Text) || string.IsNullOrEmpty(txbPassword.Text))
            {
                MessageBox.Show("Please fill out all required options.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
