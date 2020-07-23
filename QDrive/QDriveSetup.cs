using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QDrive
{
    public enum ConnectionType
    {
        LocalConnection,
        OnlineConnection
    }

    public partial class QDriveSetup : SfForm
    {
        private List<Panel> panels = new List<Panel>();
        
        public QDriveSetup()
        {
            InitializeComponent();

            panels.Add(pnlS1ConnectionType);
            panels.Add(pnlS2LocalConnection);
            panels.Add(pnlS2OnlineConnection);

            AlignPanels();
        }

        private void AlignPanels()
        {
            this.Width = 680;
            this.Height = 500;
            foreach(Panel panel in panels) panel.Dock = DockStyle.Fill;
        }

        private void QDriveSetup_Load(object sender, EventArgs e) => pnlS1ConnectionType.BringToFront();

        #region STEP-1-Connection-Type =======================================================

        // Step 1: Connection Type
        private ConnectionType connectionType;

        private void btnNextS1_Click(object sender, EventArgs e)
        {
            if (rbnConnectionTypeLocal.Checked)
            {
                connectionType = ConnectionType.LocalConnection;
                pnlS2LocalConnection.BringToFront();
            }
            else
            {
                connectionType = ConnectionType.OnlineConnection;
                pnlS2OnlineConnection.BringToFront();
            }
        }

        #endregion

        #region STEP-2-Local-Connection ======================================================

        private void btnPrevS2Local_Click(object sender, EventArgs e) => pnlS1ConnectionType.BringToFront();

        private void btnNextS2Local_Click(object sender, EventArgs e)
        {

        }

        private void btnBrowseS2Local_Click(object sender, EventArgs e)
        {
            if(fbdSelectLocalConfigFile.ShowDialog() == DialogResult.OK)
                txbS2ConfigLocation.Text = fbdSelectLocalConfigFile.SelectedPath;
        }

        #endregion

        #region STEP-2-Online-Connection =====================================================

        private void btnPrevS2Online_Click(object sender, EventArgs e) => pnlS1ConnectionType.BringToFront();

        private void btnNextS2Online_Click(object sender, EventArgs e)
        {

        }


        #endregion

        
    }
}
