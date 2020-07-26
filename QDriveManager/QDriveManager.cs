using Syncfusion.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Schema;

namespace QDriveManager
{
    public partial class QDriveManager : SfForm
    {
        #region Page Layout and Initial Loading

        private List<Panel> panels = new List<Panel>();

        public QDriveManager(params string[] args)
        {
            InitializeComponent();

            panels.Add(pnlNotConfigured);
            panels.Add(pnlLogin);
            panels.Add(panel1);

            AlignPanels();
        }

        private void AlignPanels()
        {
            this.Width = 800;
            this.Height = 600;
            foreach (Panel panel in panels) panel.Dock = DockStyle.Fill;
        }

        #endregion

        private void QDriveManager_Load(object sender, EventArgs e)
        {
            panel1.BringToFront();

        }
    }
}
