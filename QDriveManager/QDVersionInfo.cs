using QDriveLib;
using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QDriveManager
{
    public partial class QDVersionInfo : SfForm
    {
        public QDVersionInfo()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void QDUpdateCheck_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "Version " + QDInfo.QDVersion;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://endev.at/p/q-drive");
        }
    }
}
