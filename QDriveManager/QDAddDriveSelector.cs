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
