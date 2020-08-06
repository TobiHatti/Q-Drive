using QDriveLib;
using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QDriveAdminConsole
{
    public partial class QDriveAdminConsole : SfForm
    {
        private readonly List<Panel> panels = new List<Panel>();

        public QDriveAdminConsole()
        {
            InitializeComponent();

            this.Style.Border = new Pen(Color.FromArgb(77, 216, 255), 2);
            this.Style.InactiveBorder = new Pen(Color.FromArgb(77, 216, 255), 2);

            panels.Add(pnlLogin);
            panels.Add(pnlSettings);
            panels.Add(pnlLoading);
            panels.Add(pnlLocal);

            QDLib.AlignPanels(this, panels, 470, 440);

            pbxQDLogo.Image = Properties.Resources.QDriveProgamFavicon;
            pbxQDLogoLocal.Image = Properties.Resources.QDriveProgamFavicon;
            pbxQDLogoLoading.Image = Properties.Resources.QDriveProgamFavicon;

            pnlSettings.BringToFront();
            txbMasterPassword.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }
    }
}
