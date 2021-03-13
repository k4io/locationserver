using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using static location.Program;
using System.IO;

namespace location.Protocols
{
    public static class Http10
    {
        public static TcpClient tClient;
        public static NetworkStream DataStream;
        public static Socket sSock;
        public static IAsyncResult _result;
        public static bool bConnectToServer()
        {
            CSettings.Load();
            IPEndPoint RemoteEndPoint = null;
            if (Program.s_WhoisServerAddress.Contains("ac"))
                RemoteEndPoint = new IPEndPoint(Dns.GetHostAddresses(Program.s_WhoisServerAddress)[0], Program.PORT);
            else
                RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Program.s_WhoisServerAddress), Program.PORT);

            tClient = new TcpClient();
            /*
            _result = tClient.BeginConnect(RemoteEndPoint.Address, RemoteEndPoint.Port, null, null);
            var success = _result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));
            if (!success)
            {
                Environment.Exit(-1);
                return false;
            }*/
            tClient.Connect(RemoteEndPoint);
            DataStream = tClient.GetStream();
            sSock = tClient.Client;
        
            return true;
        }
        public static bool bLookupName(string name)
        {
            if (!sSock.Connected)
                return false;
            
                /*GET<space>/?<name><space>HTTP/1.0<CR><LF>
                   <optional header lines><CR><LF>*/
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);
                //sSock.Send(req);
                sw.WriteLine($"GET /?{name} HTTP/1.0");
                sw.WriteLine($"");
                sw.Flush();

                string rec = sr.ReadLine();
                string rec1 = sr.ReadLine();
                string rec2 = sr.ReadLine();
                string loc = sr.ReadLine();
                if (rec.Contains("Found"))
                {
                    Console.WriteLine($"no entries for {name} found!");
                    return true;
                }
                Console.WriteLine($"{name} is in {loc.Replace("\0", null)}");
            
            return true;
        }
        public static bool bChangeLocation(string name, string location)
        {
            if (!sSock.Connected)
                return false;
            try
            {
                /*POST<space>/<name><space>HTTP/1.0<CR><LF>
                    Content-Length:<space><length><CR><LF>
                    <optional header lines><CR><LF>
                    <location>*/

                string req = $"POST /{name} HTTP/1.0<CR><LF>\n" +
                    $"Content-Length: {location.Length}<CR><LF>\n" +
                    $"NULL<CR><LF>\n" +
                    $"{location}";
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);
                sw.WriteLine($"POST /{name} HTTP/1.0");
                sw.WriteLine($"Content-Length: {location.Length - 1}");
                sw.WriteLine($"");
                sw.WriteLine($"{location}");
                sw.Flush();

                string rec = sr.ReadLine();
                if (rec.Contains("OK"))
                    Console.WriteLine($"updated {name}'s location.");
                else Console.WriteLine($"no entries for {name} found.");
            }
            catch { return false; }
            return true;
        }
    }
}
