using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Helper
{
    public class MyLog
    {
        private static readonly MyLog log = new MyLog();

        private MyLog() { }
        public static MyLog Current
        {
            get
            {
                return log;
            }
        }

        public void Write(string message)
        {
            WriteFile(message);
        }

        public void WriteLine(string message)
        {
            WriteFile(message);
        }

        public void WriteLine(object o)
        {
            string rs = string.Empty;
            if (o is Exception)
            {
                rs = o.ToString() + " " + ((Exception)o).Message + " " + ((Exception)o).StackTrace;
            }
            else
            {
                rs = o.ToString();
            }
            WriteFile(rs);
        }

        public void WriteLine(object o, string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                WriteLine(o);
                return;
            }
            else
            {
                string rs = string.Empty;
                if (o is Exception)
                {
                    rs = o.ToString() + " " + ((Exception)o).Message + " " + ((Exception)o).StackTrace;
                }
                else
                {
                    rs = o.ToString();
                }
                WriteFile(rs, category);
            }
        }

        private void WriteFile(string message)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory + "\\logs\\";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string file = folder + DateTime.Now.ToString("yyyyMMdd") + ".log";
            StreamWriter sw = new StreamWriter(file, true);
            sw.WriteLine(message);
            sw.Close();
            sw.Dispose();
        }
        private void WriteFile(string message, string category)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory + "\\logs\\" + category + "\\";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string file = folder + DateTime.Now.ToString("yyyyMMdd") + ".log";
            StreamWriter sw = new StreamWriter(file, true);
            sw.WriteLine(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]"));
            sw.WriteLine(message);
            sw.WriteLine(" - - - - - - - - - - - - -");
            sw.Close();
            sw.Dispose();
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this.WriteLine(string.Format(format, args), "error");
        }

        public void InfoFormat(string format, params object[] args)
        {
            this.WriteLine(string.Format(format, args), "info");
        }

    }
}
