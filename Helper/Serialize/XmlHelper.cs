using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
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
    }
}
