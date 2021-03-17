using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace myserver
{
    public partial class mainForm : Form
    {
        public Thread serverThread = null;
        public bool serverstart = false;
        public mainForm()
        {
            InitializeComponent();
        }

        private void cmdStopStart_Click(object sender, EventArgs e)
        {
            serverstart = !serverstart;
            if (serverstart)
            {
                lblstatus.Text = "Status: ON";
                cmdStopStart.Text = "Stop server";
            }
            else
            {
                lblstatus.Text = "Status: Off";
                cmdStopStart.Text = "Start server";
            }

            if(serverstart)
            {
                Thread w = new Thread(() => { Program.settings.server.Init(); });
                if (serverThread == null)
                {
                    serverThread = w;
                }
                serverThread.Start();
            } 
            else if(serverThread != null)
            {
                serverThread.Abort();
            }
        }

        private void checkLog_CheckedChanged(object sender, EventArgs e)
        {
            Program.log = !Program.log;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            Program.txt = richTextBox1;
        }
    }
}
