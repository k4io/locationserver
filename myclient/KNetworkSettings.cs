using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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
            comboProtocol.SelectedIndex = (int)Program.settings.Protocol;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            /*IPAddress outip;
            if(IPAddress.TryParse(txtIP.Text, out outip))
                Program.settings.IPAddress = txtIP.Text;
            else Program.settings.IPAddress = Dns.GetHostAddresses(txtIP.Text)[0].ToString();*/
            Program.settings.IPAddress = txtIP.Text;
            Program.settings.Port = txtPort.Text;

            //Protocol to use
            switch(comboProtocol.SelectedIndex)
            {
                case 0:
                    Program.settings.Protocol = Protocols.WHOIS;
                    break;
                case 1:
                    Program.settings.Protocol = Protocols.HTTP09;
                    break;
                case 2:
                    Program.settings.Protocol = Protocols.HTTP10;
                    break;
                case 3:
                    Program.settings.Protocol = Protocols.HTTP11;
                    break;
            }
            Program.settings.Save();
            log.write(LogLevel.INFO, $"Saved network settings! ({Program.settings.IPAddress}:{Program.settings.Port = txtPort.Text} @ {Program.settings.Protocol})");
        }
    }
}
