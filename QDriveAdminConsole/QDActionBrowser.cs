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

// Q-Drive Network-Drive Manager
// Copyright(C) 2020 Tobias Hattinger

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<https://www.gnu.org/licenses/>.

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

            // Automatically causes Datagrid-Update
            cbxEntryLimit.SelectedIndex = 1;
        }

        private int listOffset = 0;
        private int listLimitSize = 0;

        private void UpdateDatagrid()
        {
            BrowserSort sort;

            if (string.IsNullOrEmpty(SelectedObjectID)) sort = BrowserSort.AllEntries;
            else if (SelectedObjectID.StartsWith("ACT=")) sort = BrowserSort.SortedByActionType;
            else if (mysql.ExecuteScalarACon<int>("SELECT COUNT(*) FROM qd_users WHERE ID = ?", SelectedObjectID) != 0) sort = BrowserSort.SortedByUsers;
            else if (mysql.ExecuteScalarACon<int>("SELECT COUNT(*) FROM qd_devices WHERE ID = ?", SelectedObjectID) != 0) sort = BrowserSort.SortedByDevice;
            else return;

            string limitString = "";
            switch(cbxEntryLimit.SelectedIndex)
            {
                case 0:
                    limitString = "LIMIT 50";
                    listLimitSize = 50;
                    break;
                case 1:
                    limitString = "LIMIT 100";
                    listLimitSize = 100;
                    break;
                case 2:
                    limitString = "LIMIT 250";
                    listLimitSize = 250;
                    break;
                case 3:
                    limitString = "LIMIT 500";
                    listLimitSize = 500;
                    break;
                case 4:
                    limitString = "LIMIT 1000";
                    listLimitSize = 1000;
                    break;
                case 5:
                    limitString = "LIMIT 5000";
                    listLimitSize = 5000;
                    break;
                default:
                    limitString = "";
                    listLimitSize = -1;
                    break;
            }

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
                        $"ORDER BY LogTime DESC " + limitString;
                    lblActionDescriptor.Text = "Showing all recorded actions.";
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
                        $"ORDER BY LogTime DESC " + limitString;
                    lblActionDescriptor.Text = $"Showing all recorded actions of type \"{(QDLogAction)Convert.ToInt32(SelectedObjectID.Replace("ACT=", ""))}\".";
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
                        $"ORDER BY LogTime DESC " + limitString;
                    lblActionDescriptor.Text = $"Showing all recorded actions of device \"{mysql.ExecuteScalarACon<string>("SELECT CONCAT(LogonName, ' @ ', DeviceName) FROM qd_devices WHERE ID = ?", SelectedObjectID)}\".";
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
                        $"ORDER BY LogTime DESC " + limitString;
                    lblActionDescriptor.Text = $"Showing all recorded actions of user \"{mysql.ExecuteScalarACon<string>("SELECT CONCAT(Name, ' (', Username, ')') FROM qd_users WHERE ID = ?", SelectedObjectID)}\".";
                    break;
            }

            dgvActionBrowser.Rows.Clear();

            int totalEntryCount = 0;

            if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return; }

            using (MySqlDataReader reader = (MySqlDataReader)mysql.ExecuteQuery(sqlQuery, SelectedObjectID.Replace("ACT=", "")))
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

            totalEntryCount = mysql.ExecuteScalar<int>("SELECT COUNT(*) FROM qd_conlog");

            mysql.Close();

            lblResultRange.Text = $"Showing entries {listOffset + 1} to {listOffset + dgvActionBrowser.Rows.Count} ({totalEntryCount} entries in total)";
        }

        private void UpdateInfoData()
        {
            if (dgvActionBrowser.Rows.Count <= 0) return;
            if (dgvActionBrowser.SelectedRows.Count <= 0) return;

            string conlogID = dgvActionBrowser.SelectedRows[0].Cells["ID"].Value.ToString();

            string sqlQuery = $"SELECT *, " +
                $"(SELECT COUNT(*) FROM qd_assigns WHERE qd_assigns.UserID = qd_conlog.UserID) AS AssignedDriveCount, " +
                $"(SELECT COUNT(*) FROM (SELECT * FROM qd_conlog GROUP BY qd_conlog.UserID) AS TMP WHERE TMP.DeviceID = qd_devices.ID) AS UserCount " +
                $"FROM qd_conlog " +
                $"INNER JOIN qd_users ON qd_conlog.UserID = qd_users.ID " +
                $"INNER JOIN qd_devices ON qd_conlog.DeviceID = qd_devices.ID " +
                $"WHERE qd_conlog.ID = ?";

            
            using (WrapMySQL mysql = new WrapMySQL(DBData))
            {
                if (!QDLib.ManagedDBOpen(mysql)) { QDLib.DBOpenFailed(); return; }

                using (MySqlDataReader reader = (MySqlDataReader)mysql.ExecuteQuery(sqlQuery, conlogID))
                {
                    while (reader.Read())
                    {
                        lblDateTime.Text = Convert.ToString(reader["LogTime"]);
                        lblActionType.Text = Convert.ToString((QDLogAction)Convert.ToInt32(reader["LogAction"]));
                        lblActionDescription.Text = QDLib.GetLogDescriptionFromAction((QDLogAction)Convert.ToInt32(reader["LogAction"]));

                        lblDisplayName.Text = Convert.ToString(reader["Name"]);
                        lblUsername.Text = Convert.ToString(reader["Username"]);
                        lblAssignedDrives.Text = Convert.ToString(reader["AssignedDriveCount"]);

                        lblDeviceName.Text = Convert.ToString(reader["DeviceName"]);
                        lblLogonName.Text = Convert.ToString(reader["LogonName"]);
                        lblMacAddress.Text = Convert.ToString(reader["MacAddress"]);
                        lblUserCount.Text = QDLib.UserCountAtDevice(Convert.ToString(reader["DeviceID"]), DBData).ToString();
                    }
                }
                mysql.Close();
            }
        }

        private void dgvActionBrowser_SelectionChanged(object sender, EventArgs e) => UpdateInfoData();

        private void btnRefresh_Click(object sender, EventArgs e) => UpdateDatagrid();

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void btnShowAllActions_Click(object sender, EventArgs e)
        {
            SelectedObjectID = "";
            UpdateDatagrid();
        }

        private void btnShowActionsCurrentType_Click(object sender, EventArgs e)
        {
            if (dgvActionBrowser.SelectedRows.Count > 0)
            {
                SelectedObjectID = "ACT=" + mysql.ExecuteScalarACon<string>("SELECT LogAction FROM qd_conlog WHERE ID = ?", dgvActionBrowser.SelectedRows[0].Cells["ID"].Value.ToString());
                UpdateDatagrid();
            }
        }

        private void btnShowActionsCurrentUser_Click(object sender, EventArgs e)
        {
            if (dgvActionBrowser.SelectedRows.Count > 0)
            {
                SelectedObjectID = mysql.ExecuteScalarACon<string>("SELECT UserID FROM qd_conlog WHERE ID = ?", dgvActionBrowser.SelectedRows[0].Cells["ID"].Value.ToString());
                UpdateDatagrid();
            }
        }

        private void btnShowActionsCurrentDevice_Click(object sender, EventArgs e)
        {
            if (dgvActionBrowser.SelectedRows.Count > 0)
            {
                SelectedObjectID = mysql.ExecuteScalarACon<string>("SELECT DeviceID FROM qd_conlog WHERE ID = ?", dgvActionBrowser.SelectedRows[0].Cells["ID"].Value.ToString());
                UpdateDatagrid();
            }
        }

        private void tmrSearchCooldown_Tick(object sender, EventArgs e)
        {
            string searchQuery = txbSearchbox.Text;

            tmrSearchCooldown.Stop();

            lbxSearchresult.DataSource = null;
            lbxSearchresult.Items.Clear();

            string sqlQuery = "SELECT CONCAT('User: ', qd_users.Name, ' (', qd_users.Username,')') AS Display, ID " +
                "FROM qd_users HAVING Display LIKE CONCAT('%', ?, '%') " +
                "UNION ALL " +
                "SELECT CONCAT('Device: ', qd_devices.LogonName, ' @ ', qd_devices.DeviceName) AS Display, ID " +
                "FROM qd_devices HAVING Display LIKE CONCAT('%', ?, '%')";

            lbxSearchresult.DisplayMember = "Display";
            lbxSearchresult.ValueMember = "ID";
            lbxSearchresult.DataSource = mysql.CreateDataTable(sqlQuery, searchQuery, searchQuery);
        }

        private void txbSearchbox_TextChanged(object sender, EventArgs e)
        {
            tmrSearchCooldown.Stop();
            tmrSearchCooldown.Start();
        }

        private void lbxSearchresult_DoubleClick(object sender, EventArgs e)
        {
            if(lbxSearchresult.SelectedIndex != -1)
            {
                SelectedObjectID = lbxSearchresult.SelectedValue.ToString();
                UpdateDatagrid();
            }
        }

        private void cbxEntryLimit_SelectedIndexChanged(object sender, EventArgs e) => UpdateDatagrid();
    }
}
