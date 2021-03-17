using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace myclient
{
    public enum LogLevel
    {
        INFO  = 0,
        WARN  = 1,
        ERROR = 2,
        FATAL = 3
    }
    public enum Protocols
    {
        WHOIS  = 0,
        HTTP09 = 1,
        HTTP10 = 2,
        HTTP11 = 3
    }
    static class Program
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static RichTextBox txt;
        public static CSettings settings = CSettings.Load();
        public static bool log = false;
        public static string s_WhoisServerAddress = "127.0.0.1";
        public static int PORT = 43;
        public static bool gui = false;
        public static string ProtocolType = "WHOIS";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            CSettings.Load();
            if (args.Length == 0)
            {
                gui = true;
                ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            else
            {
                AllocConsole();

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

                            break;
                        }
                        if (largs.Contains("-p") && int.TryParse(args[args.ToList().IndexOf("-p") + 1], out PORT))
                        {
                            settings.Port = PORT.ToString();
                            largs.Remove(largs.Where(T => T == "-p").FirstOrDefault());
                            largs.Remove(s);
                        }
                        gIndex++;
                    }
                }

                Program.settings.Protocol = Protocols.WHOIS;
                if (args.Contains("-h1"))
                    Program.settings.Protocol = Protocols.HTTP11;
                if (args.Contains("-h0"))
                    Program.settings.Protocol = Protocols.HTTP10;
                if (args.Contains("-h9"))
                    Program.settings.Protocol = Protocols.HTTP09;
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

                //setup request

                KNetwork networktools = new KNetwork();

                networktools.SendMessage(largs[0], loc, Program.settings.Protocol);
            }
        }
        public class CSettings : KSettings<CSettings>
        {
            public string IPAddress = $"";
            public string Port = $"";
            public Protocols Protocol;
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
    public static class log
    {
        private static StreamWriter swlog;
        public static bool init()
        {
            //if (File.Exists("client.log")) File.Delete("client.log");
            StreamWriter sw = new StreamWriter("client.log");
            swlog = sw;
            return true;
        }
        public static bool write(LogLevel severity, string msg)
        {
            try
            {
                var culture = new CultureInfo("en-GB");

                string sout = "[" + DateTime.Now.ToString(culture) + "] ";
                switch (severity)
                {
                    case LogLevel.INFO:
                        sout += "<INFO> -   ";
                        break;
                    case LogLevel.WARN:
                        sout += "<WARNING>- ";
                        break;
                    case LogLevel.ERROR:
                        sout += "<ERROR> -  ";
                        break;
                    case LogLevel.FATAL:
                        sout += "<FATAL> -  ";
                        break;
                }
                sout += msg;
                if (Program.gui)
                    Program.txt.Invoke(new Action(() => Program.txt.Text += $"{sout}\r\n"));
                else
                    Console.WriteLine(sout);
                if (Program.log) { swlog.WriteLine(sout); swlog.Flush(); }
                return true;
            } catch(Exception e) { throw e; }
        }
    }
}
