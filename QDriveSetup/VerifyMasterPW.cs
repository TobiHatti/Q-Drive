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

namespace QDrive
{
    public partial class VerifyMasterPW : SfForm
    {
        public WrapMySQLConDat DBData;

        public VerifyMasterPW()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnVerify_Click(object sender, EventArgs e) => Submit();

        private void VerifyMasterPW_Load(object sender, EventArgs e)
        {
            txbMasterPassword.Focus();
        }

        private void SubmitForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }

        private void Submit()
        {
            if (!QDLib.VerifyMasterPassword(txbMasterPassword.Text, DBData))
            {
                MessageBox.Show("Master-Password is not valid. Please enter the corrent Master-Password, which has been set when the database was first initialised.", "Invalid Master-Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
