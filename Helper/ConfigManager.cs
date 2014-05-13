using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;

namespace Helper
{
    /// <summary>
    /// 配置文件管理类
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// 读取键值对 配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <returns></returns>
        public static NameValueCollection GetConfigSettings(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ApplicationException("config file path is not valid!");
            NameValueCollection nvc = new NameValueCollection();
            XmlReader reader = XmlReader.Create(path);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.XmlDeclaration:
                        break;
                    case XmlNodeType.Attribute:
                        break;
                    case XmlNodeType.CDATA:
                        break;
                    case XmlNodeType.Element:
                        // Console.WriteLine(reader.LocalName);
                        if (reader.HasAttributes && reader.AttributeCount == 2 && !string.IsNullOrEmpty(reader.GetAttribute("key")))
                        {
                            // Console.WriteLine(reader.GetAttribute("key") + "==" + reader.GetAttribute("value"));
                            nvc.Add(reader.GetAttribute("key"), reader.GetAttribute("value"));
                        }

                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.Whitespace:
                        break;
                }
            }

            return nvc;
        }

    }
}
