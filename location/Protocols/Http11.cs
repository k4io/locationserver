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
    public static class Http11
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
                /*GET<space>/?name=<name><space>HTTP/1.1<CR><LF>
                        Host:<space><hostname><CR><LF>
                        <optional header lines><CR><LF>*/
                byte[] req = Encoding.ASCII.GetBytes($"GET /?name={name} HTTP/1.1<CR><LF>\n" +
                    $"Host: ({sSock.RemoteEndPoint.ToString()})<CR><LF>\n" +
                    $"NULL<CR><LF>");
                sSock.Send(req);
                req = new byte[Program.szKilobyte];
                sSock.Receive(req);
                string rec = Encoding.ASCII.GetString(req);
                if (rec.Contains("Found"))
                {
                    Console.WriteLine($"no entries for {name} found!");
                    return true;
                }
                string loc = rec.Split('<')[rec.Split('<').Length - 3].Split('>')[1];
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
                /*POST<space>/<space>HTTP/1.1<CR><LF>
                    Host:<space><hostname><CR><LF>
                    Content-Length:<space><length><CR><LF>
                    <optional header lines><CR><LF>
                    name=<name>&location=<location>
                    */
                string temp = $"name={name}&location={location}"; // I wrote this after i saw it had to be not the location like before haha funny one stop pulling tricks
                byte[] req = Encoding.ASCII.GetBytes($"POST / HTTP/1.1<CR><LF>" +
                    $"Host: ({sSock.RemoteEndPoint.ToString()})<CR><LF>" +
                    $"Content-Length: {temp.Length}<CR><LF>NULL<CR><LF>" +
                    $"name={name}&location={location}");
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
                    Console.WriteLine($"updated {name}'s location.");
            }
            catch { return false; }
            return true;
        }
    }
}
