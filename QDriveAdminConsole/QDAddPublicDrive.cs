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

namespace QDriveAdminConsole
{
    public partial class QDAddPublicDrive : SfForm
    {
        public bool EditMode = false;

        public string DrivePath = "";
        public string DriveName = "";
        public string DriveLetter = "";
        public bool CanBeDeployed = true;

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

                chbCanBeDeployed.Checked = CanBeDeployed;

                this.Text = "Edit Public Drive";
                lblAddDriveTitle.Text = "Edit public drive template";
                btnSubmit.Text = "Update Drive";
            }
        }

        private void SubmitForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
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
            CanBeDeployed = chbCanBeDeployed.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
