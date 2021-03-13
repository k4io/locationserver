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
        public static IAsyncResult _result;
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
                _result = tClient.BeginConnect(Program.s_WhoisServerAddress, int.Parse(Program.settings.Port), null, null);

                var success = _result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));
                if (!success)
                {
                    Environment.Exit(-1);
                    return false;
                }

                DataStream = tClient.GetStream();
                sSock = tClient.Client;
            } catch { throw; }
            return true;
        }
        public static bool bLookupName(string name)
        {
            if (!tClient.Connected)
                return false;
            //try
            //{
            StreamWriter sw = new StreamWriter(DataStream);
            StreamReader sr = new StreamReader(DataStream);
            string _s = "" + name + "";
            sw.WriteLine(_s);
            sw.Flush();
            byte[] buf = new byte[Program.szKilobyte];
            string rec = sr.ReadLine();
            if (rec.Contains("entries"))
            {
                Console.WriteLine($"ERROR: no entries found\r\n");
                return true;
            }
            //Console.WriteLine(rec);
            string location = rec.Split('\0')[0];
            //if (location.Split('\0')[0].Contains(' '))
            //    location = location.Split('\0')[0].Split(' ')[0];
            Console.WriteLine($"{name} is {location}");
            //} catch(Exception e) 
            //{ 
            //    throw e; 
            //    return false; 
            //}
            Environment.Exit(-1);
            tClient.EndConnect(_result);
            return true;
        }
        public static bool bChangeLocation(string name, string location)
        {
            if (!sSock.Connected)
                return false;
            try
            {
                //Thread.Sleep(100);
                StreamWriter sw = new StreamWriter(DataStream);
                StreamReader sr = new StreamReader(DataStream);
                if (location[location.Length-1] == ' ') 
                    location = location.Substring(0, location.Length - 1);
                sw.WriteLine($"{name} {location}");
                sw.Flush();
                byte[] buf = new byte[Program.szKilobyte];
                string rec = sr.ReadLine();
                //int req = tClient.Client.Receive(buf, SocketFlags.None);
                //string rec = Encoding.ASCII.GetString(buf);
                if (rec.Contains("entries"))
                {
                    Console.WriteLine($"ERROR: no entries found");
                    return true;
                }
                else if (rec.Contains("OK"))
                    Console.WriteLine($"{name} location changed to be {location}\r\n");
                //Environment.Exit(-1);
                tClient.EndConnect(_result);
            }
            catch { return false; }
            return true;
        }
    }
}
