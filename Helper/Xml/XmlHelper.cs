using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Reflection;
using System.IO;
namespace Helper.Xml
{
    public static class XmlHelper
    {
        /// <summary>
        /// 键值对必须包含，"key" "value"
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="csspath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDic(string FilePath, string csspath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);
            XmlNodeList xn = doc.SelectNodes(csspath);
            Dictionary<string, string> dics = new Dictionary<string, string>();
            foreach (XmlNode n in xn)
            {
                string key = n.Attributes["key"].Value;
                string val = n.Attributes["value"].Value;
                dics[key] = val;
            }
            return dics;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="csspath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDic(XmlDocument doc, string csspath)
        {
            XmlNodeList xn = doc.SelectNodes(csspath);
            Dictionary<string, string> dics = new Dictionary<string, string>();
            foreach (XmlNode n in xn)
            {
                string key = n.Attributes["key"].Value;
                string val = n.Attributes["value"].Value;
                dics[key] = val;
            }
            return dics;
        }


        /// <summary>
        /// 单个属性
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="csspath"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string GetAttr(string fileName, string csspath, string attr)
        {
            string rs = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNode xn = doc.SelectSingleNode(csspath);
            if (xn != null)
            {
                rs = xn.Attributes[attr].Value;
            }
            return rs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="csspath"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string GetAttr(XmlDocument doc, string csspath, string attr)
        {
            string rs = "";
            XmlNode xn = doc.SelectSingleNode(csspath);
            if (xn != null)
            {
                rs = xn.Attributes[attr].Value;
            }
            return rs;
        }

        #region $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$

        public static Dictionary<string, object> Save(string filename, Dictionary<string, object> dicList)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(filename, Encoding.GetEncoding("utf-8")))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.Formatting = Formatting.Indented;
                // 写入根元素   
                xmlWriter.WriteStartElement("______root");
                foreach (KeyValuePair<string, object> item in dicList)
                {
                    // 写入第一个元素   
                    xmlWriter.WriteStartElement(item.Key);
                    xmlWriter.WriteCData(item.Value.ToString());
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.Close();
            }
            return dicList;
        }

        public static List<Dictionary<string, object>> Save(string filename, List<Dictionary<string, object>> dicList)
        {
            return Save(filename, dicList, false);
        }

        public static List<Dictionary<string, object>> Save(string filename, List<Dictionary<string, object>> dicList, bool apppend)
        {
            if (apppend)
            {
                List<Dictionary<string, object>> preDic = ReadList(filename);
                dicList.InsertRange(0, preDic);
            }
            using (XmlTextWriter xmlWriter = new XmlTextWriter(filename, Encoding.GetEncoding("utf-8")))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.Formatting = Formatting.Indented;
                // 写入根元素   
                xmlWriter.WriteStartElement("______root");
                foreach (Dictionary<string, object> dicItem in dicList)
                {
                    xmlWriter.WriteStartElement("______config");
                    foreach (KeyValuePair<string, object> item in dicItem)
                    {
                        // 写入第一个元素   
                        xmlWriter.WriteStartElement(item.Key);
                        xmlWriter.WriteCData(item.Value.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.Close();
            }
            return dicList;
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Read(string file)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            using (XmlTextReader xmlReader = new XmlTextReader(file))
            {
                string key = string.Empty, txt = string.Empty;
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name != "______root")
                        {
                            key = xmlReader.Name;
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.Text)
                    {
                        if (!string.IsNullOrEmpty(key))
                        {
                            txt = xmlReader.Value;
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (xmlReader.Name == key)
                        {
                            dic.Add(key, txt);
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.CDATA)
                    {
                        txt = xmlReader.Value;
                    }
                }
            }
            return dic;
        }

        public static List<Dictionary<string, object>> ReadList(string file)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            using (XmlTextReader xmlReader = new XmlTextReader(file))
            {
                string key = string.Empty, txt = string.Empty;
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (xmlReader.Name != "______root" && xmlReader.Name != "______config")
                            {
                                key = xmlReader.Name;
                            }
                            if (xmlReader.Name == "______config")
                            {

                            }
                            break;
                        case XmlNodeType.CDATA:
                            txt = xmlReader.Value;
                            break;
                        case XmlNodeType.Text:
                            txt = xmlReader.Value;
                            break;
                        case XmlNodeType.EndElement:
                            if (xmlReader.Name == key && xmlReader.Name != "______root" && xmlReader.Name != "______config")
                            {
                                dic.Add(key, txt);
                            }

                            if (xmlReader.Name == "______config")
                            {
                                dicList.Add(dic);
                                dic = new Dictionary<string, object>();
                                key = string.Empty;
                                txt = string.Empty;
                            }
                            break;
                    }
                }
            }
            return dicList;
        }

        public static Dictionary<string, object> Save<T>(T t)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\config\\";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string filename = folder + typeof(T).FullName + ".config";
            List<PropertyInfo> proList = Helper.Database.DataMapper.GetProperties<T>();
            Dictionary<string, object> dic = new Dictionary<string, object>();

            foreach (PropertyInfo pi in proList)
            {
                if (!dic.ContainsKey(pi.Name))
                {
                    dic.Add(pi.Name, pi.GetValue(t, null));
                }
            }
            return Save(filename, dic);
        }

        public static void Save<T>(List<T> tlist)
        {
            Save<T>(tlist, true);
        }

        public static void Save<T>(List<T> tlist, bool append)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\config\\";
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string filename = folder + typeof(T).FullName + ".config";
            List<PropertyInfo> proList = Helper.Database.DataMapper.GetProperties<T>();
            List<Dictionary<string, object>> lll = new List<Dictionary<string, object>>();
            foreach (T t in tlist)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (PropertyInfo pi in proList)
                {
                    if (!dic.ContainsKey(pi.Name))
                    {
                        dic.Add(pi.Name, pi.GetValue(t, null));
                    }
                }
                lll.Add(dic);
            }
            Save(filename, lll, append);
        }

        public static T Read<T>()
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory + "\\app_data\\config\\";
            if (!Directory.Exists(folder)) return default(T);
            string filename = folder + typeof(T).FullName + ".config";
            if (!File.Exists(filename)) return default(T);

            T t = Activator.CreateInstance<T>();
            Dictionary<string, string> dic = Read(filename);
            List<PropertyInfo> proList = Helper.Database.DataMapper.GetProperties<T>();
            foreach (PropertyInfo pi in proList)
            {
                if (dic.ContainsKey(pi.Name))
                {
                    Type pitype = pi.PropertyType;
                    if (pitype == typeof(DateTime))
                        pi.SetValue(t, DateTime.Parse(dic[pi.Name]), null);
                    else if (pitype == typeof(Int32) || pitype == typeof(Int64) || pitype == typeof(Int16))
                    {
                        pi.SetValue(t, int.Parse(dic[pi.Name]), null);
                    }
                    else if (pitype == typeof(long))
                    {
                        pi.SetValue(t, long.Parse(dic[pi.Name]), null);
                    }
                    else if (pitype == typeof(decimal))
                    {
                        pi.SetValue(t, decimal.Parse(dic[pi.Name]), null);
                    }
                    else if (pitype == typeof(float))
                    {
                        pi.SetValue(t, float.Parse(dic[pi.Name]), null);
                    }
                    else
                        pi.SetValue(t, (dic[pi.Name]), null);
                }
            }
            return t;
        }


        #endregion



    }
}
