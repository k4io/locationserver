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
    public static class Http09
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
            try
            {
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);

                sw.WriteLine($"GET /{name}");
                sw.Flush();
                //sSock.Send(req);
               // req = new byte[Program.szKilobyte];
                //sSock.Receive(req);
                string rec = sr.ReadToEnd();
                if (rec.Contains("Found"))
                {
                    Console.WriteLine($"no entries for {name} found!");
                    return true;
                }
                string loc = "";
                for (int i = 3; i < rec.Split('n').Length; i++)
                    if(rec.Split('n')[i] != String.Empty) loc += rec.Split('n')[i] + 'n';
                if (loc[loc.Length - 1] == 'n') loc = loc.Substring(0, loc.Length - 1);
                if (loc[loc.Length - 1] == ' ') loc = loc.Substring(0, loc.Length - 1);
                loc = loc.Replace("\0", null);
                loc = loc.Replace("\n", null);
                loc = loc.Replace("\r", null);

                Console.WriteLine($"{name} is {loc}\r\n");
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
                string req = $"PUT {name}\r\n" +
                    $"\r\n" +
                    $"{location}";
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);
                sw.WriteLine($"PUT /{name}");
                sw.WriteLine($"");
                sw.WriteLine($"{location}");
                sw.Flush();

                string rec = sr.ReadToEnd();
                if (rec.Contains("entries"))
                {
                    Console.WriteLine($"no entries for {name} found!");
                    return true;
                }
                else if (rec.Contains("OK"))
                    Console.WriteLine($"HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("period"))
                    tClient.EndConnect(_result);
                else throw e;
            }
            return true;
        }
    }
}
