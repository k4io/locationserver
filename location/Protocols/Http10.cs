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
            tClient = new TcpClient();
            try
            {
                _result = tClient.BeginConnect(Program.s_WhoisServerAddress, int.Parse(Program.settings.Port), null, null);
                var success = _result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                if (!success)
                {
                    Environment.Exit(-1);
                    return false;
                }
                DataStream = tClient.GetStream();
                sSock = tClient.Client;
                DataStream.ReadTimeout = 1000;
                DataStream.WriteTimeout = 1000;

                return true;
            } catch (Exception e) { throw e; }
        }
        public static bool bLookupName(string name)
        {
            if (!sSock.Connected)
                return false;
            try
            {
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
                Console.WriteLine($"{name} is {loc.Replace("\0", null)}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("period"))
                    tClient.EndConnect(_result);
                else throw e;
            }
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
            catch(Exception e)
            {
                if (e.Message.Contains("period"))
                    tClient.EndConnect(_result);
                else throw e;
            }
            return true;
        }
    }
}
