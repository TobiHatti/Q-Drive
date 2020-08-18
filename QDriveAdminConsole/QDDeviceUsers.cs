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

namespace QDriveAdminConsole
{
    public partial class QDDeviceUsers : SfForm
    {
        public string DeviceID = string.Empty;
        public WrapMySQLData DBData = null;

        public QDDeviceUsers()
        {
            InitializeComponent();
        }

        private void QDDeviceUsers_Load(object sender, EventArgs e)
        {
            using(WrapMySQL mysql = new WrapMySQL(DBData))
            {
                lbxDeviceUsers.DataSource = null;
                lbxDeviceUsers.Items.Clear();

                lbxDeviceUsers.DisplayMember = "UserDisplay";
                lbxDeviceUsers.ValueMember = "ID";
                lbxDeviceUsers.DataSource = mysql.CreateDataTable("SELECT CONCAT(Name, ' (', Username,', Last record: ', qd_conlog.LogTime, ')') AS UserDisplay, qd_users.ID FROM qd_users INNER JOIN qd_conlog ON qd_users.ID = qd_conlog.UserID WHERE qd_conlog.DeviceID = ? GROUP BY qd_users.ID ORDER BY qd_conlog.LogTime ASC", DeviceID);

                lblDeviceInfo.Text = mysql.ExecuteScalarACon<string>("SELECT CONCAT('Device: ', LogonName, ' @ ', DeviceName, ' (', MacAddress, ')') FROM qd_devices WHERE ID = ?", DeviceID);
            }
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void btnActionLogUser_Click(object sender, EventArgs e)
        {
            if (lbxDeviceUsers.SelectedIndex != -1)
            {
                QDActionBrowser actBrowser = new QDActionBrowser()
                {
                    SelectedObjectID = lbxDeviceUsers.SelectedValue.ToString(),
                    DBData = this.DBData
                };
                actBrowser.ShowDialog();
            }
        }

        private void btnActionLogDevice_Click(object sender, EventArgs e)
        {
            QDActionBrowser actBrowser = new QDActionBrowser()
            {
                SelectedObjectID = DeviceID,
                DBData = this.DBData
            };
            actBrowser.ShowDialog();
        }
    }
}
