using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using static location.Program;

namespace location.Protocols
{
    public static class Http09
    {
        public static TcpClient tClient;
        public static Socket sSock;
        public static bool bConnectToServer()
        {
            CSettings.Load();
            IPEndPoint RemoteEndPoint = null;
            if (Program.s_WhoisServerAddress.Contains("ac"))
                RemoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(Program.s_WhoisServerAddress)[0], Program.PORT);
            if (Program.settings.IPAddress != string.Empty)
                RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Program.settings.IPAddress), Program.PORT);
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(RemoteEndPoint);
                sSock = client.Client;
            }
            catch { return false; }
            return true;
        }
        public static bool bLookupName(string name)
        {
            if (!sSock.Connected)
                return false;
            try
            {
                byte[] req = Encoding.ASCII.GetBytes($"GET {name}<CR><LF>");
                sSock.Send(req);
                req = new byte[Program.szKilobyte];
                sSock.Receive(req);
                string rec = Encoding.ASCII.GetString(req);
                if (rec.Contains("Found"))
                {
                    Console.WriteLine($"no entries for {name} found!");
                    return true;
                }
                string loc = rec.Split(' ')[1].Split('<')[0];
                Console.WriteLine($"{name} is in {loc.Replace("\0", null)}");
            }
            catch { return false; }
            return true;
        }
        public static bool bChangeLocation(string name, string location)
        {
            if (!sSock.Connected)
                return false;
            try
            {
                byte[] req = Encoding.ASCII.GetBytes($"PUT {name} <CR><LF>" +
                    $"\n<CR><LF>" +
                    $"\n{location}<CR><LF>");
                sSock.Send(req);
                req = new byte[Program.szKilobyte];
                sSock.Receive(req);
                string rec = Encoding.ASCII.GetString(req);
                if (rec.Contains("entries"))
                {
                    Console.WriteLine($"no entries for {name} found!");
                    return true;
                }
                else if (rec.Contains("OK"))
                    Console.WriteLine($"HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n");
            }
            catch { return false; }
            return true;
        }
    }
}
