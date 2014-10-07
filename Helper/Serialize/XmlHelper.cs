using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
namespace Helper.Serialize
{
    public class XmlHelper<T> where T : class
    {
        public static string ToString(T t)
        {
            string s = string.Empty;
            XmlSerializer xmls = new XmlSerializer(t.GetType());
            StringWriter sw = new StringWriter();
            xmls.Serialize(sw, t);
            s = sw.ToString();
            sw.Close();
            return s;
        }

        public static T ToObject(string xmlStr)
        {
            T t = default(T);
            XmlSerializer xmls = new XmlSerializer(typeof(T));
            StringReader sr = new StringReader(xmlStr);
            t = (T)xmls.Deserialize(sr);
            sr.Close();
            return t;
        }

        public static string Save(T t, string path)
        {
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(path))
            {
                doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                XmlElement root2 = doc.CreateElement("Configs");
                XmlElement root = doc.CreateElement("config");
                List<PropertyInfo> prolist = ReflectionHelper.GetProperties<T>();
                foreach (PropertyInfo pi in prolist)
                {
                    string name = pi.Name;
                    object value = pi.GetValue(t, null);
                    bool isXmlIdentity = false;
                    object[] att = pi.GetCustomAttributes(true);
                    if (pi.GetCustomAttributes(typeof(Helper.CustomAttribute.XmlIdentity), true).Length > 0) isXmlIdentity = true;
                    XmlElement ele = doc.CreateElement(name);
                    if (isXmlIdentity)
                    {
                        //XmlAttribute xmlatt = doc.CreateAttribute("identity");
                        //xmlatt.Value = "1";
                        //ele.Attributes.Append(xmlatt);
                        ele.SetAttribute("identity", "1");

                    }
                    ele.InnerText = value.ToString();
                    root.AppendChild(ele);
                }
                root2.AppendChild(root);
                doc.AppendChild(root2);
                Encoding encod = System.Text.Encoding.GetEncoding("utf-8");
                StreamWriter sw = new StreamWriter(path, false, encod);
                sw.WriteLine(doc.OuterXml);
                sw.Dispose();
            }
            else
            {
                doc.Load(path);
                XmlNode configs = doc.SelectSingleNode("/Configs");
                XmlElement root = doc.CreateElement("config");
                List<PropertyInfo> prolist = ReflectionHelper.GetProperties<T>();
                foreach (PropertyInfo pi in prolist)
                {
                    string name = pi.Name;
                    object value = pi.GetValue(t, null);
                    bool isXmlIdentity = false;
                    object[] att = pi.GetCustomAttributes(true);
                    if (pi.GetCustomAttributes(typeof(Helper.CustomAttribute.XmlIdentity), true).Length > 0) isXmlIdentity = true;
                    XmlElement ele = doc.CreateElement(name);
                    if (isXmlIdentity)
                    {
                        //XmlAttribute xmlatt = doc.CreateAttribute("identity");
                        //xmlatt.Value = "1";
                        //ele.Attributes.Append(xmlatt);
                        ele.SetAttribute("identity", "1");

                    }
                    ele.InnerText = value.ToString();
                    root.AppendChild(ele);
                }
                configs.AppendChild(root);
                doc.Save(path);
            }
            return doc.OuterXml;
        }
    }
}
