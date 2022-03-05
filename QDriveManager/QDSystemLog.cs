using QDriveLib;
using System;
using System.Windows.Forms;

namespace QDriveManager
{
    public partial class QDSystemLog : Form
    {
        private string lastMsg = "";

        public QDSystemLog()
        {
            InitializeComponent();
        }

        private void QDSystemLog_Load(object sender, EventArgs e)
        {
            UpdateTxb();
        }

        private void UpdateTxb()
        {
            if (lastMsg != QDLog.sessionLog[QDLog.sessionLog.Count - 1])
            {
                txbLog.Text = "Log Output\r\n=================================\r\n";

                for (int i = QDLog.sessionLog.Count - 1; i >= 0; i--)
                {
                    txbLog.Text += QDLog.sessionLog[i];
                }

                lastMsg = QDLog.sessionLog[QDLog.sessionLog.Count - 1];
            }
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            UpdateTxb();
        }
    }
}
