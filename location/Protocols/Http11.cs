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
    public static class Http11
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
            if (Program.s_WhoisServerAddress != string.Empty)
                RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Program.s_WhoisServerAddress), Program.PORT);
            tClient = new TcpClient();
            try
            {
                _result = tClient.BeginConnect(RemoteEndPoint.Address, RemoteEndPoint.Port, null, null);
                var success = _result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));
                if (!success)
                {
                    Environment.Exit(-1);
                    return false;
                }
                DataStream = tClient.GetStream();
                sSock = tClient.Client;
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
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);

                sw.WriteLine($"GET /?name={name} HTTP/1.1");
                sw.WriteLine($"Host: {sSock.RemoteEndPoint.ToString()}");
                sw.WriteLine($"NULL");
                sw.Flush();

                string rec = sr.ReadToEnd();
                if (rec.Contains("Found"))
                {
                    Console.WriteLine($"no entries for {name} found!");
                    return true;
                }
                Console.WriteLine(rec);
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
                string req = $"POST / HTTP/1.1<CR><LF>" +
                    $"Host: ({sSock.RemoteEndPoint.ToString()})<CR><LF>" +
                    $"Content-Length: {temp.Length}<CR><LF>NULL<CR><LF>" +
                    $"name={name}&location={location}";
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);
                sw.WriteLine(req);
                sw.Flush();

                string rec = sr.ReadLine();
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
