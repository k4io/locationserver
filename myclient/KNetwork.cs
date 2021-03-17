using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.IO;

namespace myclient
{
    public class KNetwork
    {
        private TcpClient t_PeerClient;
        private NetworkStream nets;
        private Socket s_HostSocket;
        private Socket s_PeerSocket;
        public IPAddress ip_Peer;
        public int PORT = 4553;
        public KNetwork()
        { if (!IPAddress.TryParse(Program.settings.IPAddress, out this.ip_Peer)
                || !int.TryParse(Program.settings.Port, out PORT)) return; }
        public KNetwork(string ip_Peer)
        { if (!IPAddress.TryParse(ip_Peer, out this.ip_Peer)) throw new Exception("Invalid format for IP Address!"); }

        public void ConnectToPeer()
        { ConnectToAddress(); }
        private bool ConnectToAddress()
        {
            try
            {
                Program.CSettings.Load();
                //IPAddress.TryParse(Program.settings.IPAddress, out this.ip_Peer);
                t_PeerClient = new TcpClient();
                t_PeerClient.Connect(Program.settings.IPAddress, int.Parse(Program.settings.Port));
                nets = t_PeerClient.GetStream();
                s_PeerSocket = t_PeerClient.Client;
                nets.WriteTimeout = 1000;
                nets.ReadTimeout = 3000;
                return true;
            }
            catch(Exception e)
            {
                log.write(LogLevel.ERROR, $"Could not connect to {Program.settings.IPAddress}:{Program.settings.Port} ({e.Message} {e.StackTrace} {e.InnerException} {e.Source} {e.Data})");
                return false;
                //Program.txt.Invoke(new Action(() => Program.txt.Text += $"[FATAL] !\n"));
            }
        }
        public void SendMessage(string name, string location, Protocols protocol)
        {
            string request = "";
            if(location != String.Empty)
            {
                //format update location
                switch(protocol)
                {
                    case Protocols.WHOIS:
                        request =   $"{name} {location}";
                        break;
                    case Protocols.HTTP09:
                        request =   $"PUT /{name}\r\n" +
                                    $"\r\n" +
                                    $"{location}";
                        break;
                    case Protocols.HTTP10:
                        request =   $"POST /{name} HTTP/1.0\r\n" +
                                    $"Content-Length: {location.Length}\r\n" +
                                    $"\r\n" +
                                    $"{location}";
                        break;
                    case Protocols.HTTP11:
                        request = $"POST / HTTP/1.1\r\n" +
                                    $"Host: {Program.settings.IPAddress}\r\n" +
                                    $"Content-Length: {$"name={name}&location={location}".Length - 1}\r\n" +
                                    $"\r\n" +
                                    $"name={name}&location={location}";
                        break;
                }
            }
            else
            {
                switch (protocol)
                {
                    case Protocols.WHOIS:
                        request = $"{name}";
                        break;
                    case Protocols.HTTP09:
                        request = $"GET /{name}";
                        break;
                    case Protocols.HTTP10:
                        request = $"GET /?{name} HTTP/1.0\r\n\r\n";
                        break;
                    case Protocols.HTTP11:
                        request = $"GET /?name={name} HTTP/1.1\r\n" +
                                  $"Host: {Program.settings.IPAddress}\r\n" +
                                  $"\r\n";
                        break;
                }
            }

            if (!bSendMessage(request))
                log.write(LogLevel.INFO, $"Connection to server lost!");
            else
            {
                log.write(LogLevel.INFO, $"You > {request}");
                log.write(LogLevel.INFO, $"Recieved > " + new StreamReader(nets).ReadToEnd());
            }


            ConnectToAddress();
        }
        private bool bSendMessage(string msg)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            try
            {
                StreamWriter sw = new StreamWriter(nets);
                sw.WriteLine(msg);
                sw.Flush();
                return true;
            } catch (Exception e) { return false; }
        }
    }
}
