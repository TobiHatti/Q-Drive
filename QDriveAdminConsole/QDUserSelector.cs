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

namespace QDriveAdminConsole
{
    public partial class QDUserSelector : SfForm
    {
        public List<string> userIDs = new List<string>();
        public WrapMySQLConDat dbData = null;

        private WrapMySQL mysql = null;

        public QDUserSelector()
        {
            InitializeComponent();
        }

        private void QDUserSelector_Load(object sender, EventArgs e)
        {
            mysql = new WrapMySQL(dbData);

            string sqlQuery = "";

            if (userIDs == null || userIDs.Count == 0)
            {
                sqlQuery = "SELECT CONCAT(Name, ' (', Username, ')') AS UserData, ID FROM qd_users";
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT CONCAT(Name, ' (', Username, ')') AS UserData, ID FROM qd_users WHERE ID IN (");

                for(int i = 0; i < userIDs.Count; i++)
                {
                    if (i == 0) sb.Append($"'{userIDs[i]}'");
                    else sb.Append($", '{userIDs[i]}'");
                }

                sb.Append(")");

                sqlQuery = sb.ToString();


            }

            lbxAllUsers.DisplayMember = "UserData";
            lbxAllUsers.ValueMember = "ID";
            lbxAllUsers.DataSource = mysql.FillDataTable(sqlQuery);

        }

        private void lbxAllUsers_DoubleClick(object sender, EventArgs e)
        {
            if(lbxAllUsers.SelectedIndex != -1)
            {
                lbxSelectedUsers.Items.Add(new ListItem(lbxAllUsers.SelectedItem.ToString(), lbxAllUsers.SelectedValue.ToString()));
                userIDs.Add(lbxAllUsers.SelectedValue.ToString());
            }
        }

        private void lbxSelectedUsers_DoubleClick(object sender, EventArgs e)
        {
            if (lbxSelectedUsers.SelectedIndex != -1)
            {
                lbxSelectedUsers.Items.RemoveAt(lbxSelectedUsers.SelectedIndex);
                userIDs.Remove((lbxSelectedUsers.SelectedItem as ListItem).Value);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }        
    }

    public class ListItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ListItem(string pName, string pValue)
        {
            Name = pName;
            Value = pValue;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
