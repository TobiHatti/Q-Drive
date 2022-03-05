using QDriveLib;
using Syncfusion.WinForms.Controls;
using System;

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

namespace QDriveManager
{
    public partial class QDSystemLog : SfForm
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

                if (QDLog.sessionLog.Count > 0)
                {
                    for (int i = QDLog.sessionLog.Count - 1; i >= 0; i--)
                    {
                        txbLog.Text += QDLog.sessionLog[i].Replace(System.Environment.NewLine, String.Empty) + "\r\n";
                    }

                    lastMsg = QDLog.sessionLog[QDLog.sessionLog.Count - 1];
                }
            }
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            UpdateTxb();
        }
    }
}
