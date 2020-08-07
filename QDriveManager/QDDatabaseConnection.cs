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
    public partial class QDDatabaseConnection : SfForm
    {
        public WrapMySQLConDat dbConDat = null;

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

        private void btnTestConnection_Click(object sender, EventArgs e) => QDLib.TestConnection(new WrapMySQLConDat()
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
            dbConDat = new WrapMySQLConDat()
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
