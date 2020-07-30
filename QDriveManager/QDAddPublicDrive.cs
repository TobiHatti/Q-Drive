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

namespace QDriveManager
{
    public partial class QDAddPublicDrive : SfForm
    {
        public string DBHost;
        public string DBName;
        public string DBUser;
        public string DBPass;

        public QDAddPublicDrive()
        {
            InitializeComponent();
        }

        private void QDAddPublicDrive_Load(object sender, EventArgs e)
        {
            ImageList imgList = new ImageList()
            {
                ImageSize = new Size(80, 80),
                ColorDepth = ColorDepth.Depth32Bit,

            };

            imgList.Images.Add("PublicUp", Properties.Resources.QDriveOnlinePublicUp);

            grvPublicDrives.SmallImageList = imgList;
            grvPublicDrives.GroupViewItems.Clear();

            using (WrapMySQL sql = new WrapMySQL(DBHost, DBName, DBUser, DBPass))
            {
                sql.Open();

                using (MySqlDataReader reader = sql.ExecuteQuery("SELECT * FROM qd_drives WHERE IsPublic = 1"))
                {
                    while(reader.Read())
                    {
                        grvPublicDrives.GroupViewItems.Add(
                            new Syncfusion.Windows.Forms.Tools.GroupViewItem(
                                $"({Convert.ToString(reader["DefaultDriveLetter"])}:\\) {Convert.ToString(reader["DefaultName"])}\r\n({Convert.ToString(reader["LocalPath"])})",
                                0
                            )
                        );
                    }
                }

                sql.Close();
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
}
