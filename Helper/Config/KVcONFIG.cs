using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
namespace Helper.Config
{
    public class KVConfig : Singleton<KVConfig>
    {
        private KVConfig() { }


        public T Read<T>(string path) where T : class,new()
        {
            T t = Activator.CreateInstance<T>();
            PropertyInfo[] pisarr = typeof(T).GetProperties();
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Root;
            var arr = root.Elements();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var a in arr)
            {
                string name = a.Name.LocalName;
                string val = a.Value;
                if (!dic.ContainsKey(name))
                    dic.Add(name, val);
            }
            foreach (PropertyInfo pi in pisarr)
            {
                if (dic.ContainsKey(pi.Name))
                {
                    object val = dic[pi.Name];
                    if (pi.PropertyType == typeof(int))
                    {
                        val = int.Parse(val.ToString());
                    }
                    else if (pi.PropertyType == typeof(decimal))
                    {
                        val = decimal.Parse(val.ToString());
                    }
                    else if (pi.PropertyType == typeof(double))
                    {
                        val = double.Parse(val.ToString());
                    }
                    else if (pi.PropertyType == typeof(float))
                    {
                        val = float.Parse(val.ToString());
                    }
                    else if (pi.PropertyType == typeof(long))
                    {
                        val = long.Parse(val.ToString());
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        val = DateTime.Parse(val.ToString());
                    }

                    pi.SetValue(t, val, null);
                }
            }

            return t;
        }

        public void Save<T>(string path, T t) where T : class,new()
        {
            XDocument xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            XElement root = new XElement("configuration");

            PropertyInfo[] pisarr = typeof(T).GetProperties();
            foreach (PropertyInfo pi in pisarr)
            {
                XElement ele = new XElement(pi.Name);

                object valObj = pi.GetValue(t, null);
                if (valObj == null) valObj = "";
                if (valObj.ToString() != "")
                {
                    if (pi.PropertyType == typeof(DateTime))
                        valObj = DateTime.Parse(valObj.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                }

                XCData cdata = new XCData(valObj.ToString());
                ele.Add(cdata);
                root.Add(ele);
            }
            if (File.Exists(path))
                File.Delete(path);
            root.Save(path, SaveOptions.None);
        }
    }
}
