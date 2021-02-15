using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using static location.Program;

namespace location.Protocols
{
    public static class Whois
    {
        public static TcpClient tClient;
        public static NetworkStream DataStream;
        public static Socket sSock;
        public static bool bConnectToServer()
        {
            CSettings.Load();
            IPEndPoint RemoteEndPoint;
            if (Program.settings.IPAddress != string.Empty)
            {
                if (!Program.settings.IPAddress.Contains("ac"))
                    RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Program.settings.IPAddress), int.Parse(Program.settings.Port));
            } else
                if (!Program.s_WhoisServerAddress.Contains("ac"))
                    RemoteEndPoint = new IPEndPoint(IPAddress.Parse(Program.s_WhoisServerAddress), int.Parse(Program.settings.Port));
            tClient = new TcpClient();
            try
            {
                if (Program.s_WhoisServerAddress.Contains("ac"))
                    tClient.Connect("150.237.89.121", int.Parse(Program.settings.Port));
                else tClient.Connect(new IPEndPoint(IPAddress.Parse(Program.s_WhoisServerAddress), int.Parse(Program.settings.Port)));
                DataStream = tClient.GetStream();
                sSock = tClient.Client;
            } catch { throw; }
            return true;
        }
        public static bool bLookupName(string name)
        {
            if (!tClient.Connected)
                return false;
            try
            {
                StreamWriter sw = new StreamWriter(DataStream);
                sw.WriteLine(name);
                sw.Flush();
                byte[] buf = new byte[Program.szKilobyte];
                int req = tClient.Client.Receive(buf, SocketFlags.None);
                string rec = Encoding.ASCII.GetString(buf);
                if (rec.Contains("entries"))
                {
                    Console.WriteLine($"ERROR: no entries found");
                    return true;
                }
                Console.WriteLine($"{name} is {rec.Split('<')[0]}");
            } catch { return false; }
            return true;
        }
        public static bool bChangeLocation(string name, string location)
        {
            if (!sSock.Connected)
                return false;
            try
            {
                Thread.Sleep(100);
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);
                sw.WriteLine($"{name} {location}");
                sw.Flush();
                byte[] buf = new byte[Program.szKilobyte];
                string rec = sr.ReadToEnd();
                //int req = tClient.Client.Receive(buf, SocketFlags.None);
                //string rec = Encoding.ASCII.GetString(buf);
                if (rec.Contains("entries"))
                {
                    Console.WriteLine($"ERROR: no entries found");
                    return true;
                }
                else if (rec.Contains("OK"))
                    Console.WriteLine($"{name} location changed to be {location}");
            }
            catch { return false; }
            return true;
        }
    }
}
