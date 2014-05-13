using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;

namespace Helper.Config
{
    public static class CustomConfigHandler
    {
        private static XmlDocument _doc;
        private static string _path = AppDomain.CurrentDomain.BaseDirectory + "\\custom.config";
        static CustomConfigHandler()
        {

        }

        public static Dictionary<string, string> appSettings
        {
            get
            {
                Dictionary<string, string> dic = null;
                object obj = HttpRuntime.Cache.Get("CustomConfigHandler");
                if (obj == null)
                {
                    dic = new Dictionary<string, string>();
                    _doc = new XmlDocument();
                    _doc.Load(_path);

                    XmlNodeList xmls = _doc.SelectNodes("/configuration/appSettings/add");
                    foreach (XmlNode xn in xmls)
                    {
                        string attrkey = xn.Attributes["key"].Value;
                        if (!dic.ContainsKey(attrkey))
                        {
                            dic.Add(attrkey, xn.Attributes["value"].Value);
                        }
                    }
                    HttpRuntime.Cache.Insert("CustomConfigHandler", dic, new System.Web.Caching.CacheDependency(_path));
                }
                else
                {
                    dic = obj as Dictionary<string, string>;
                }
                return dic;
            }
        }


        public static void SaveappSetting(string key, string value)
        {
            if (!appSettings.ContainsKey(key)) return;
            if (appSettings[key] == value) return;

            XmlNodeList xmls = _doc.SelectNodes("/configuration/appSettings/add");
            foreach (XmlNode node in xmls)
            {
                if (node.Attributes["key"].Value == key)
                {
                    node.Attributes["value"].Value = value;
                    break;
                }
            }
            _doc.Save(_path);
        }


    }
}
