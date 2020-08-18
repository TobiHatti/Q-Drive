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

namespace QDriveAdminConsole
{
    public enum BrowserSort
    {
        AllEntries,
        SortedByDevice,
        SortedByUsers,
        SortedByActionType
    }

    public partial class QDActionBrowser :SfForm
    {
        public string SelectedObjectID = string.Empty;
        public WrapMySQLData DBData = null;

        private WrapMySQL mysql = null;

        public QDActionBrowser()
        {
            InitializeComponent();

            dgvActionBrowser.Columns.Add("ID", "ID");
            dgvActionBrowser.Columns.Add("DateTime", "Action Date & Time");
            dgvActionBrowser.Columns.Add("QDUser", "QD-User");
            dgvActionBrowser.Columns.Add("Device", "Device");
            dgvActionBrowser.Columns.Add("Action", "Action");

            dgvActionBrowser.Columns[0].Visible = false;
            dgvActionBrowser.Columns[1].Width = 200;
            dgvActionBrowser.Columns[2].Width = 200;
            dgvActionBrowser.Columns[3].Width = 200;
            dgvActionBrowser.Columns[4].Width = 200;


        }

        private void QDActionBrowser_Load(object sender, EventArgs e)
        {
            mysql = new WrapMySQL(DBData);

            UpdateDatagrid();
        }

        private void UpdateDatagrid()
        {
            BrowserSort sort;

            if (string.IsNullOrEmpty(SelectedObjectID)) sort = BrowserSort.AllEntries;
            else if (SelectedObjectID.StartsWith("ACT=")) sort = BrowserSort.SortedByActionType;
            else if (mysql.ExecuteScalarACon<int>("SELECT COUNT(*) FROM qd_users WHERE ID = ?", SelectedObjectID) != 0) sort = BrowserSort.SortedByUsers;
            else if (mysql.ExecuteScalarACon<int>("SELECT COUNT(*) FROM qd_devices WHERE ID = ?", SelectedObjectID) != 0) sort = BrowserSort.SortedByDevice;
            else return;

            string sqlQuery = "";

            switch(sort)
            {
                case BrowserSort.AllEntries:
                    sqlQuery = $"SELECT " +
                        $"*, " +
                        $"qd_conlog.ID AS MainID, " +
                        $"CONCAT(qd_users.Name, ' (', qd_users.Username, ')') AS UserDisplay, " +
                        $"CONCAT(qd_devices.LogonName, ' @ ', qd_devices.DeviceName) AS DeviceDisplay " +
                        $"FROM qd_conlog " +
                        $"INNER JOIN qd_users ON qd_conlog.UserID = qd_users.ID " +
                        $"INNER JOIN qd_devices ON qd_conlog.DeviceID = qd_devices.ID " +
                        $"ORDER BY LogTime DESC";
                    break;
                case BrowserSort.SortedByActionType:
                    sqlQuery = $"SELECT " +
                        $"*, " +
                        $"qd_conlog.ID AS MainID, " +
                        $"CONCAT(qd_users.Name, ' (', qd_users.Username, ')') AS UserDisplay, " +
                        $"CONCAT(qd_devices.LogonName, ' @ ', qd_devices.DeviceName) AS DeviceDisplay " +
                        $"FROM qd_conlog " +
                        $"INNER JOIN qd_users ON qd_conlog.UserID = qd_users.ID " +
                        $"INNER JOIN qd_devices ON qd_conlog.DeviceID = qd_devices.ID " +
                        $"WHERE qd_conlog.LogAction = ? " +
                        $"ORDER BY LogTime DESC";
                    break;
                case BrowserSort.SortedByDevice:
                    sqlQuery = $"SELECT " +
                        $"*, " +
                        $"qd_conlog.ID AS MainID, " +
                        $"CONCAT(qd_users.Name, ' (', qd_users.Username, ')') AS UserDisplay, " +
                        $"CONCAT(qd_devices.LogonName, ' @ ', qd_devices.DeviceName) AS DeviceDisplay " +
                        $"FROM qd_conlog " +
                        $"INNER JOIN qd_users ON qd_conlog.UserID = qd_users.ID " +
                        $"INNER JOIN qd_devices ON qd_conlog.DeviceID = qd_devices.ID " +
                        $"WHERE qd_devices.ID = ? " +
                        $"ORDER BY LogTime DESC";
                    break;
                case BrowserSort.SortedByUsers:
                    sqlQuery = $"SELECT " +
                        $"*, " +
                        $"qd_conlog.ID AS MainID, " +
                        $"CONCAT(qd_users.Name, ' (', qd_users.Username, ')') AS UserDisplay, " +
                        $"CONCAT(qd_devices.LogonName, ' @ ', qd_devices.DeviceName) AS DeviceDisplay " +
                        $"FROM qd_conlog " +
                        $"INNER JOIN qd_users ON qd_conlog.UserID = qd_users.ID " +
                        $"INNER JOIN qd_devices ON qd_conlog.DeviceID = qd_devices.ID " +
                        $"WHERE qd_users.ID = ? " +
                        $"ORDER BY LogTime DESC";
                    break;
            }

            dgvActionBrowser.Rows.Clear();

            mysql.Open();
            using (MySqlDataReader reader = (MySqlDataReader)mysql.ExecuteQuery(sqlQuery, SelectedObjectID))
            {
                while (reader.Read())
                {
                    dgvActionBrowser.Rows.Add(new string[] {
                        Convert.ToString(reader["MainID"]),
                        Convert.ToString(reader["LogTime"]),
                        Convert.ToString(reader["UserDisplay"]),
                        Convert.ToString(reader["DeviceDisplay"]),
                        Convert.ToString((QDLogAction)Convert.ToInt32(reader["LogAction"])),
                    });
                }
            }
            mysql.Close();
        }

        private void UpdateInfoData()
        {
            string conlogID = dgvActionBrowser.SelectedRows[0].Cells["ID"].Value.ToString();

            string sqlQuery = $"SELECT * " +
                $"FROM qd_conlog " +
                $"INNER JOIN qd_users ON qd_conlog.UserID = qd_users.ID " +
                $"INNER JOIN qd_devices ON qd_conlog.DeviceID = qd_devices.ID " +
                $"WHERE qd_conlog.ID = ?";

            
            using (WrapMySQL mysql = new WrapMySQL(DBData))
            {
                mysql.Open();
                using (MySqlDataReader reader = (MySqlDataReader)mysql.ExecuteQuery(sqlQuery, conlogID))
                {
                    while (reader.Read())
                    {
                        lblDateTime.Text = Convert.ToString(reader["LogTime"]);
                        lblActionType.Text = Convert.ToString((QDLogAction)Convert.ToInt32(reader["LogAction"]));
                        lblActionDescription.Text = QDLib.GetLogDescriptionFromAction((QDLogAction)Convert.ToInt32(reader["LogAction"]));

                        lblDisplayName.Text = Convert.ToString(reader["Name"]);
                        lblUsername.Text = Convert.ToString(reader["Username"]);
                        //lblAssignedDrives.Text = Convert.ToString(reader[""]);

                        lblDeviceName.Text = Convert.ToString(reader["DeviceName"]);
                        lblLogonName.Text = Convert.ToString(reader["LogonName"]);
                        lblMacAddress.Text = Convert.ToString(reader["MacAddress"]);
                        //lblUserCount.Text = Convert.ToString(reader[""]);
                    }
                }
                mysql.Close();
            }
        }

        private void dgvActionBrowser_SelectionChanged(object sender, EventArgs e)
        {
            UpdateInfoData();
        }
    }
}
