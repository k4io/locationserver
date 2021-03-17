using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace myclient
{
    public partial class frmMain : Form
    {
        public static KNetwork networktools = new KNetwork();
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Program.txt = rctMessages;
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            if (Program.settings.IPAddress == String.Empty) return;
            else networktools.ConnectToPeer();
        }
        private void cmdSet_Click(object sender, EventArgs e)
        {
            new KNetworkSettings().Show();
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            if (Program.settings.IPAddress == String.Empty) return;
            else networktools.ConnectToPeer();
            networktools.SendMessage(txtNewMsg.Text, txtLocation.Text, Program.settings.Protocol);
        }

        private void label2_Click(object sender, EventArgs e) { }

        /// <summary>
        /// Pressed when 'output log' checkbox check is changed.
        /// </summary>
        /// <param name="sender">Object sending action</param>
        /// <param name="e">Event arguments</param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Program.log = !Program.log;
            if (Program.log)
                log.init();
        }
    }
}
