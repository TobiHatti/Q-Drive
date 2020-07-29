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

        public QDAddDriveSelector()
        {
            InitializeComponent();
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
