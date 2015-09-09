using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Helper.Serialize
{
    public class XmlSerialize
    {

        public static string Serialize<T>(T t)
        {
            string s = string.Empty;
            XmlSerializer xmls = new XmlSerializer(t.GetType());
            StringWriter sw = new StringWriter();
            xmls.Serialize(sw, t);
            s = sw.ToString();
            sw.Close();
            return s;
        }

        public static T DeSerialize<T>(string xmlStr)
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
