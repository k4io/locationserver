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
            tClient = new TcpClient();
            try
            {
                tClient.Connect(s_WhoisServerAddress, PORT);
                DataStream = tClient.GetStream();
                //DataStream.ReadTimeout = 1000;
                //DataStream.WriteTimeout = 1000;
                sSock = tClient.Client;
            }
            catch(Exception e)
            {
                if (e.Message.Contains("period"))
                    tClient.EndConnect(_result);
                else throw e;
            }
            return true;
        }
        public static bool bLookupName(string name)
        {
            if (!sSock.Connected)
                return false;
            
                /*GET<space>/?name=<name><space>HTTP/1.1<CR><LF>
                        Host:<space><hostname><CR><LF>
                        <optional header lines><CR><LF>*/
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);

                sw.WriteLine($"GET /?name={name} HTTP/1.1");
                if (s_WhoisServerAddress == "127.0.0.1")
                    sw.WriteLine($"Host: localhost");
                else sw.WriteLine($"Host: {s_WhoisServerAddress}");
                sw.WriteLine($"");
                sw.Flush();

                string rec = sr.ReadToEnd();
                if (rec.Contains("Found"))
                {
                    Console.WriteLine($"no entries for {name} found!");
                    return true;
                }
                //Console.WriteLine(rec);
                string loc = rec.Split('\n')[3].Split('\r')[0];
                Console.WriteLine($"{name} is {loc.Replace("\0", null)}");
            
            return true;
        }
        public static bool bChangeLocation(string name, string location)
        {
            if (!sSock.Connected)
                return false;
            try
            {
                string temp = $"name={name}&location={location}";
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);
                sw.WriteLine($"POST / HTTP/1.1");
                if(s_WhoisServerAddress == "127.0.0.1")
                    sw.WriteLine($"Host: localhost");
                else sw.WriteLine($"Host: {s_WhoisServerAddress}");
                sw.WriteLine($"Content-Length: {temp.Length - 1}");
                sw.WriteLine($"");
                sw.WriteLine($"name={name}&location={location}");
                sw.Flush();

                string rec = sr.ReadToEnd();
                if (rec.Contains("entries"))
                {
                    Console.WriteLine($"no entries for {name} found!");
                    return true;
                }
                else if (rec.Contains("OK"))
                    Console.WriteLine($"updated {name}'s location.");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("period"))
                {
                    Console.WriteLine("Server timed out!");
                    tClient.EndConnect(_result);
                }
                else throw e;
            }
            return true;
        }
    }
}
