using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using location.Protocols;
using System.Net;

namespace location
{
    public static class Program
    {
        public const int szKilobyte = 1024;
        public static string s_WhoisServerAddress = "127.0.0.1";
        public static int PORT = 43;
        public static string ProtocolType = "WHOIS";
        public static CSettings settings = CSettings.Load();
        static void Main(string[] args)
        {
            List<string> largs = args.ToList<string>();
            //try
            //{
            if (args.Length > 2)
            {
                int hIndex = 0, pIndex = 0, gIndex = 0;
                IPAddress nIp = IPAddress.Any;
                foreach (string s in args)
                {
                    if (s == "localhost")
                    {
                        hIndex = gIndex;
                        settings.IPAddress = IPAddress.Parse("127.0.0.1").ToString();
                        s_WhoisServerAddress = IPAddress.Parse("127.0.0.1").ToString();
                        largs.Remove(largs.Where(T => T == "-h").FirstOrDefault());
                        largs.Remove(s);
                        continue;
                    }
                    if (s == "-h")
                        s_WhoisServerAddress = args[args.ToList().IndexOf("-h") + 1];
                        //s_WhoisServerAddress = args[gIndex+1];
                    if (s == "-p")
                        pIndex = gIndex + 1;
                    foreach (string ss in largs)
                        args.ToList().Remove(ss);
                    if (IPAddress.TryParse(args[hIndex], out nIp))
                    {
                        if (!args[hIndex].Contains('.'))
                        {
                            continue; 
                        }
                        settings.IPAddress = args[hIndex - 1];
                        s_WhoisServerAddress = args[hIndex - 1];
                        largs.Remove(largs.Where(T => T == "-h").FirstOrDefault());
                        if (largs[0].Contains('.'))
                            largs.RemoveAt(0);
                        if (largs.Contains("-p") && int.TryParse(args[pIndex], out PORT))
                        {
                            settings.Port = args[pIndex];
                            largs.Remove(largs.Where(T => T == "-p").FirstOrDefault());
                            largs.Remove(s);
                        }
                        break;
                    }
                    gIndex++;
                }
            }


            if (args.Contains("-h1"))
                ProtocolType = "h1";
            if (args.Contains("-h0"))
                ProtocolType = "h0";
            if (args.Contains("-h9"))
                ProtocolType = "h9";
            largs.Remove("-h1");
            largs.Remove("-h0");
            largs.Remove("-h9");
            largs.Remove("-h");
            largs.Remove("-p");
            largs.Remove(settings.Port);
            largs.Remove(s_WhoisServerAddress);

            string loc = "";
            if (largs.Count >= 2)
                for (int i = 1; i < largs.Count; i++)
                    loc += (largs[i] + ' ');
            foreach (string s in loc.Split(' '))
                largs.Remove(s);
            //if (loc != "")
            //    largs.Add(loc);
            switch (largs.Count)
            {
                case 1:
                    switch (ProtocolType)
                    {
                        case "WHOIS":
                            if (Whois.bConnectToServer())
                                Whois.bLookupName(largs[0]);
                            break;
                        case "h9":
                            if (Http09.bConnectToServer())
                                Http09.bLookupName(largs[0]);
                            break;
                        case "h0":
                            if (Http10.bConnectToServer())
                                Http10.bLookupName(largs[0]);
                            break;
                        case "h1":
                            if (Http11.bConnectToServer())
                                Http11.bLookupName(largs[0]);
                            break;
                    }
                    break;
                case 2:
                    switch (ProtocolType)
                    {
                        case "WHOIS":
                            if (Whois.bConnectToServer())
                                Whois.bChangeLocation(largs[0], loc);
                            break;
                        case "h9":
                            if (Http09.bConnectToServer())
                                Http09.bChangeLocation(largs[0], loc);
                            break;
                        case "h0":
                            if (Http10.bConnectToServer())
                                Http10.bChangeLocation(largs[0], loc);
                            break;
                        case "h1":
                            if (Http11.bConnectToServer())
                                Http11.bChangeLocation(largs[0], loc);
                            break;
                    }
                    break;
            }
            //}
            //catch (Exception er)
            //{
            //    throw er;
            //    Console.WriteLine($"Error: {er.Message}");
            //    return;
            //}
            Console.ReadLine();
        }
        public class CSettings : KSettings<CSettings>
        {
            public string IPAddress = $"0.0.0.0";
            public string Port = $"";
        }
        public class KSettings<T> where T : new()
        {
            private const string DEFAULT = "settings.json";
            public static void Save(T pSettings, string fname = DEFAULT)
            { File.WriteAllText(fname, (new JavaScriptSerializer()).Serialize(pSettings)); }

            public void Save(string fname = DEFAULT)
            { File.WriteAllText(fname, (new JavaScriptSerializer()).Serialize(this)); }
            public static T Load(string fname = DEFAULT)
            {
                T t = new T();
                if (File.Exists(fname))
                    t = (new JavaScriptSerializer()).Deserialize<T>(File.ReadAllText(fname));
                return t;
            }
        }
    }
}