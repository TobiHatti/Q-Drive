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

namespace QDriveAdminConsole
{
    public partial class QDAddUser : SfForm
    {
        public string EditID = null;

        public string DisplayName = "";
        public string Username = "";
        public string Password = "";

        public WrapMySQLConDat DBData = null;

        public QDAddUser()
        {
            InitializeComponent();
        }

        private void QDAddUser_Load(object sender, EventArgs e)
        {
            if(EditID != null)
            {
                using(WrapMySQL mysql = new WrapMySQL(DBData))
                {
                    mysql.Open();

                    Username = mysql.ExecuteScalar<string>("SELECT Username FROM qd_users WHERE ID = ?", EditID);
                    DisplayName = mysql.ExecuteScalar<string>("SELECT Name FROM qd_users WHERE ID = ?", EditID);

                    txbUsername.Text = Username;
                    txbDisplayName.Text = DisplayName;

                    mysql.Close();
                }

                this.Text = "Edit User";
                btnSubmit.Text = "Save Changes";
                lblRegisterTitle.Text = "Update user-data";
                lblPassword.Text = "New Password";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e) => Submit();

        private void txbDisplayName_TextChanged(object sender, EventArgs e)
        {
            if(EditID == null) txbUsername.Text = txbDisplayName.Text.Replace(' ', '.').ToLower();
        }

        private void SubmitForm(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }

        private void Submit()
        {
            if (EditID == null || (EditID != null && !string.IsNullOrEmpty(txbPassword.Text) && !string.IsNullOrEmpty(txbConfirmPassword.Text)))
                if (!QDLib.ValidatePasswords(txbPassword.Text, txbConfirmPassword.Text)) return;

            if (string.IsNullOrEmpty(txbDisplayName.Text))
            {
                MessageBox.Show("Please enter a valid display-name.", "Invalid Display-Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txbUsername.Text))
            {
                MessageBox.Show("Please enter a valid username.", "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (EditID == null || (EditID != null && Username != txbUsername.Text))
            {
                using (WrapMySQL mysql = new WrapMySQL(DBData))
                {
                    if (mysql.ExecuteScalarACon<int>("SELECT COUNT(*) FROM qd_users WHERE Username = ?", txbUsername.Text) != 0)
                    {
                        MessageBox.Show("Username already in use. Please choose another username", "Username already in use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            DisplayName = txbDisplayName.Text;
            Username = txbUsername.Text;

            if (EditID == null || (EditID != null && !string.IsNullOrEmpty(txbPassword.Text) && !string.IsNullOrEmpty(txbConfirmPassword.Text))) Password = txbPassword.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
