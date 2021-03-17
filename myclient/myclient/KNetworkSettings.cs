using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myclient;

namespace myclient
{
    public partial class KNetworkSettings : Form
    {
        public KNetworkSettings()
        {
            InitializeComponent();
        }

        private void KNetworkSettings_Load(object sender, EventArgs e)
        {
            txtIP.Text = Program.settings.IPAddress;
            txtPort.Text = Program.settings.Port;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            Program.settings.IPAddress = txtIP.Text;
            Program.settings.Port = txtPort.Text;
            Program.settings.Save();
        }
    }
}
