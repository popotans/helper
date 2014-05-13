using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ader.TemplateEngine;
using System.IO;

namespace Helper
{
    public class AdtmplHelper
    {
        private static AdtmplHelper hp = new AdtmplHelper();
        private AdtmplHelper() { }
        public static AdtmplHelper Instance
        {
            get { return hp; }
        }

        public string Render(string filePath, Dictionary<string, object> values, string encoding, string writeTo)
        {
            TemplateManager tmr = TemplateManager.FromFile(AppDomain.CurrentDomain.BaseDirectory + (filePath));
            foreach (KeyValuePair<string, object> item in values)
            {
                tmr.SetValue(item.Key, item.Value);
            }
            string ts = tmr.Process();
            if (!string.IsNullOrEmpty(writeTo))
            {
                writeTo = writeTo.Replace("/", "\\");
                if (!writeTo.StartsWith("\\")) writeTo = "\\" + writeTo;
                
                string ___f = writeTo.Substring(0, writeTo.LastIndexOf("\\"));
                string folder = AppDomain.CurrentDomain.BaseDirectory + ___f;
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string file = AppDomain.CurrentDomain.BaseDirectory + writeTo;
                if (File.Exists(file)) File.Delete(file);
                Helper.IO.FileHelper.WriteFile(file, ts, encoding);
            }
            return ts;
        }
    }

    /*
     * example
     * 
     * 
     * 
     * 
     * **/



}