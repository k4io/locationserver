using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.IO;

namespace myclient
{
    static class Program
    {
        public static RichTextBox txt;
        public static CSettings settings = CSettings.Load();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            CSettings.Load();
        }
        public class CSettings : KSettings<CSettings>
        {
            public string IPAddress = $"";
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
