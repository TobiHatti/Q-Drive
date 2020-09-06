using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QDriveLib
{
    public partial class QDLoader : Form
    {

        public QDLoader()
        {
            InitializeComponent();

            pbxBackground.BackColor = Color.FromArgb(77, 216, 255);
            pbxLoader.Image = Properties.Resources.Load;
        }

        private void pbxBackground_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, 2, 2, this.Width - 4, this.Height - 4);
        }
    }
}
