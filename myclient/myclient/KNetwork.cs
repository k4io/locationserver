using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace myclient
{
    public class KNetwork
    {
        private TcpListener t_PeerServer;
        private TcpClient t_PeerClient;
        private Socket s_HostSocket;
        private Socket s_PeerSocket;
        public IPAddress ip_Peer;
        public const int PORT = 48001;
        public KNetwork()
        { if (!IPAddress.TryParse(Program.settings.IPAddress, out this.ip_Peer)) return; }
        public KNetwork(string ip_Peer)
        { if (!IPAddress.TryParse(ip_Peer, out this.ip_Peer)) throw new Exception("Invalid format for IP Address!"); }

        public void ConnectToPeer()
        { if (!ConnectToAddress()) HostChatServer(); }
        private bool ConnectToAddress()
        {
            Program.CSettings.Load();
            IPAddress.TryParse(Program.settings.IPAddress, out this.ip_Peer);
            byte[] helloBuffer = Encoding.ASCII.GetBytes("hs");
            t_PeerClient = new TcpClient(Program.settings.IPAddress, PORT);
            Program.txt.Invoke(new Action(() => Program.txt.Text += $"Attempting connection to {Program.settings.IPAddress}:{48001}...\n"));
            t_PeerClient.GetStream().Write(helloBuffer, 0, helloBuffer.Length);
            t_PeerClient.GetStream().Flush();
            Program.txt.Invoke(new Action(() => Program.txt.Text += $"Connection established!\n"));
            return true;
        }
        public void SendMessage(string msg)
        {
            if(!bSendMessage(msg))
                Program.txt.Invoke(new Action(() => Program.txt.Text += "Peer disconnected!\n"));
            else
                Program.txt.Invoke(new Action(() => Program.txt.Text += $"You > {msg}\n"));
        }
        private bool bSendMessage(string msg)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            try
            {
                t_PeerClient.GetStream().Write(bytes, 0, bytes.Length);
                t_PeerClient.GetStream().Flush();
                return true;
            } catch (Exception e) { return false; }
        }
        private bool HostChatServer()
        {

            return true;
        }
    }
}
