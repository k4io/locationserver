﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace locationserver
{
    public class Program
    {
        public static List<Socket> clients = new List<Socket>();
        public static List<Thread> threads = new List<Thread>();
        public static CSettings settings = CSettings.Load();
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(new IPEndPoint(IPAddress.Any, 43));
            server.Start();
            Console.WriteLine($"[{clients.Count}] server started on {server.LocalEndpoint}");
            while(true)
            {
                TcpClient s = server.AcceptTcpClient();
                new Thread(() => {
                    clients.Add(s.Client);
                    Thread.CurrentThread.Name = $"{s.Client.RemoteEndPoint}-Thread";
                    threads.Add(Thread.CurrentThread);
                    Console.WriteLine($"[{clients.Count}] {s.Client.RemoteEndPoint} connected.");
                    byte[] rec = new byte[1024];
                    while (true)
                    {
                        try
                        {
                            StreamWriter sw = new StreamWriter(s.GetStream());
                            StreamReader sr = new StreamReader(s.GetStream());
                            //rec = Encoding.ASCII.GetBytes(sr.ReadLine());
                            s.Client.Receive(rec);
                            sr.BaseStream.Flush();
                            Console.WriteLine($"[{clients.Count}] {s.Client.RemoteEndPoint} > {Encoding.ASCII.GetString(rec).Replace("\0", null)}.");
                            string msg = Encoding.ASCII.GetString(rec);
                            s.ReceiveTimeout = 1000;
                            s.SendTimeout = 1000;

                            if(msg.Contains("HTTP/1.1")) {
                                if (msg.Contains("GET"))
                                {
                                    string name = msg.Split('=')[1].Split(' ')[0];
                                    if (!File.Exists(name))
                                    {
                                        s.Client.Send(Encoding.ASCII.GetBytes("HTTP/1.1 404 Not Found\r\n" +
                                            "Content-Type: text/plain\r\n" +
                                            "\r\n"));
                                    }
                                    else
                                    {
                                        s.Client.Send(Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK\r\n" +
                                            $"Content-Type: text/plain\r\n" +
                                            $"\r\n" +
                                            $"{File.ReadAllText(name)}\r\n"));
                                    }
                                    continue;
                                }
                                else if (msg.Contains("POST"))
                                {
                                    /*"POST / HTTP/1.1\r\n" +
                                       $"Host: ({sSock.RemoteEndPoint.ToString()})\r\n" +
                                       $"Content-Length: {temp.Length}\r\nNULL\r\n" +
                                       $"name={name}&location={location}"*/
                                    string name = msg.Split('=')[1].Split('&')[0];
                                    string location = msg.Split('=')[2];
                                    File.WriteAllText(name, location);
                                    s.Client.Send(Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n" +
                                        "Content-Type: text/plain\r\n" +
                                        "\r\n"));
                                    continue;
                                }
                            } else if(msg.Contains("HTTP/1.0")) {
                                if(msg.Contains("GET"))
                                {
                                    string name = msg.Split('?')[1].Split(' ')[0];
                                    if(!File.Exists(name))
                                    {
                                        s.Client.Send(Encoding.ASCII.GetBytes($"HTTP/1.0 404 Not Found\r\n\n" +
                                                                                 $"Content-Type: text/plain\r\n\n" +
                                                                                  $"\r\n"));
                                        continue;
                                    } else {
                                        s.Client.Send(Encoding.ASCII.GetBytes($"HTTP/1.0 200 OK\r\n" +
                                            $"Content-Type: text/plain\r\n" +
                                            $"\r\n" +
                                            $"{File.ReadAllText(name)}\r\n"));
                                    }
                                    continue;
                                } else if(msg.Contains("POST")) {
                                    string name = msg.Split('/')[1].Split('H')[0].Split(' ')[0];
                                    string location = msg.Split('\n')[msg.Split('\n').Length - 1].Replace("\0", null);
                                    File.WriteAllText(name, location);
                                    s.Client.Send(Encoding.ASCII.GetBytes("HTTP/1.0 200 OK\r\n" +
                                        "Content-Type: text/plain\r\n" +
                                        "\r\n"));
                                    continue;
                                }
                            }

                            if (!msg.Contains("\r\n"))
                                msg += "\r\n";
                            if (msg.Contains("PUT"))
                            {
                                //Console.WriteLine(msg.Split('\n')[2].Split('\r')[0]);
                                try
                                {
                                    string name = msg.Split('/')[1].Split('\r')[0];
                                    string location = msg.Split('\n')[2].Split('\r')[0];
                                    File.WriteAllText(name, location);
                                }
                                catch (Exception ee)
                                {
                                    Console.WriteLine(ee);
                                }
                                s.Client.Send(Encoding.ASCII.GetBytes("HTTP/0.9 200 OK\r\nContent-Type: text/plain\r\n\r\n"));
                                /*sw.WriteLine(Encoding.ASCII.GetBytes("HTTP/0.9 200 OK\r\n" +
                                    "\nContent-Type: text/plain\r\n" +
                                    "\n\r\n"));*/
                                    sw.Flush();
                                Console.WriteLine("SENT REPLY");
                                continue;
                            }
                            else if (msg.Contains("GET")) {
                                string name = msg.Split(' ')[1].Split('<')[0].Replace("\r", null).Replace("\n", null).Replace("/", null).Replace("\0", null);
                                if (!File.Exists(name))
                                     s.Client.Send(Encoding.ASCII.GetBytes("HTTP/0.9 404 Not Found\r\n" +
                                        "\nContent-Type: text/plain\r\n" +
                                        "\n\r\n"));
                                else s.Client.Send(Encoding.ASCII.GetBytes("HTTP/0.9 200 OK\r\n" +
                                     "Content-Type: text/plain\r\n" +
                                     "\r\n" +
                                     $"{File.ReadAllText(name)}"));
                                sw.Flush();
                                continue;
                            }

                            if (msg.Contains(' '))
                            {
                                string fname = msg.Split(' ')[0];
                                if (File.Exists(fname))
                                    File.Delete(fname);
                                StreamWriter swl = File.CreateText(fname);
                                string txt = "";
                                foreach (string s2 in msg.Split(' '))
                                    if (s2 != fname) txt += s2.Split('\0')[0] + " ";
                                swl.WriteLine(txt);
                                swl.Close();
                                Thread.Sleep(150);
                                s.Client.Send(Encoding.ASCII.GetBytes("OK\r\n"));
                                Console.WriteLine($"[{clients.ToArray().Length}] Sent OK.");
                                //sw.WriteLine(Encoding.ASCII.GetBytes("OK\r\n"));
                                sw.Flush();
                                continue;
                            }
                            if (!File.Exists(msg.Split('\r')[0]))
                            {
                                sw.WriteLine("ERROR: no entries found\r\n");
                                sw.Flush();
                            }
                            else
                            {
                                string _s = File.ReadAllText(msg.Split('\r')[0]);
                                _s = _s.Split('\r')[0] + '\r';
                                Console.WriteLine("3");
                                sw.WriteLine(_s);
                                sw.Flush();
                            }
                        }
                        catch(Exception ee)
                        {
                            EndPoint dcEp = s.Client.RemoteEndPoint;
                            s.Client.Close();
                            threads.Remove(Thread.CurrentThread);
                            clients.Remove(s.Client);
                            Console.WriteLine($"[{clients.ToArray().Length}] {dcEp} disconnected.");
                            Thread.CurrentThread.Abort();
                        }
                    }
                }).Start();
            }
        }
    }
    public class CSettings : KSettings<CSettings>
    {
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
