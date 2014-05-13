using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
namespace Helper.Database
{
    public class JsonStorage
    {
        private static object _Locker = new object();
        private static JsonStorage _instance = null;

        public static JsonStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_Locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new JsonStorage();
                        }
                    }
                }
                return _instance;
            }
        }
        private JsonStorage() { }

        public T Read<T>() where T : class,new()
        {
            T t = default(T);
            string folder = string.Format("{0}\\app_data\\config", AppDomain.CurrentDomain.BaseDirectory);
            if (!System.IO.Directory.Exists(folder)) return default(T);
            string path = string.Format("{0}\\{1}.config", folder, typeof(T).ToString());
            if (!System.IO.File.Exists(path)) return default(T);
            string txt = Helper.IO.FileHelper.ReadFile(path, "utf-8");
            if (!string.IsNullOrEmpty(txt))
            {
                t = Helper.Serialize.JsonHelper.ToObject<T>(txt);
            }
            t = ReadObjfromJson<T>(t);
            return t;
        }

        public List<T> ReadAll<T>() where T : class,new()
        {
            List<T> t = null;
            string folder = string.Format("{0}\\app_data\\config", AppDomain.CurrentDomain.BaseDirectory);
            if (!System.IO.Directory.Exists(folder)) return null;
            string path = string.Format("{0}\\{1}List.config", folder, typeof(T).ToString());
            if (!System.IO.File.Exists(path)) return null;
            string txt = Helper.IO.FileHelper.ReadFile(path, "utf-8");
            t = Helper.Serialize.JsonHelper.ToObject<List<T>>(txt);
            if (t != null)
            {
                List<PropertyInfo> proList = Helper.Database.DataMapper.GetProperties<T>();
                foreach (T _t in t)
                {
                    foreach (PropertyInfo pi in proList)
                    {
                        object obj = pi.GetValue(_t, null);
                        string rs = obj == null ? string.Empty : obj.ToString();
                        rs = JsonDecode(rs);
                        pi.SetValue(_t, rs, null);
                    }
                }
            }
            return t;
        }

        public static List<PropertyInfo> GetProperties(Type type)
        {

            object[] oProperties = null;

            List<PropertyInfo> list = new List<PropertyInfo>();
            oProperties = type.GetProperties();
            foreach (PropertyInfo pi in oProperties)
            {
                // 排除entityFrameWork 对象
                //  if (pi.PropertyType == typeof(System.Data.EntityKey)) continue;
                //  if (pi.PropertyType == typeof(System.Data.EntityState)) continue;
                if (pi.CanRead && pi.CanWrite)
                    list.Add(pi);
            }
            return list;

        }

        private string BuildJsonStr<T>(T t)
        {
            Type type = t.GetType();
            List<PropertyInfo> proList = GetProperties(type);
            StringBuilder sb = new StringBuilder("{\r");
            foreach (PropertyInfo pi in proList)
            {
                object obj = pi.GetValue(t, null);
                string rs = obj == null ? string.Empty : obj.ToString();
                rs = JsonEncode(rs);

                sb.AppendFormat("\"{0}\":\"{1}\",\r", pi.Name, rs);
            }
            sb.Append("}");
            return sb.ToString();
        }

        private T ReadObjfromJson<T>(T t)
        {
            List<PropertyInfo> proList = Helper.Database.DataMapper.GetProperties<T>();
            foreach (PropertyInfo pi in proList)
            {
                object obj = pi.GetValue(t, null);
                string rs = obj == null ? string.Empty : obj.ToString();
                rs = JsonDecode(rs);
                pi.SetValue(t, rs, null);
            }
            return t;
        }

        public string Save<T>(T t)
        {
            string folder = string.Format("{0}\\app_data\\config", AppDomain.CurrentDomain.BaseDirectory);
            if (!System.IO.Directory.Exists(folder)) System.IO.Directory.CreateDirectory(folder);
            string path = string.Format("{0}\\{1}.config", folder, typeof(T).ToString());

            string _rs = BuildJsonStr(t);
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(_rs);
            }
            return _rs;
        }

        public string Save<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder("[\r");
            foreach (T obj in list)
            {
                sb.AppendFormat("{0},", BuildJsonStr<T>(obj));
            }
            sb.AppendFormat("]");
            string folder = string.Format("{0}\\app_data\\config", AppDomain.CurrentDomain.BaseDirectory);
            if (!System.IO.Directory.Exists(folder)) System.IO.Directory.CreateDirectory(folder);
            string path = string.Format("{0}\\{1}List.config", folder, typeof(T).ToString());
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(sb.ToString());
            }
            return sb.ToString();
        }




        string JsonEncode(string rs)
        {
            if (!string.IsNullOrEmpty(rs))
            {
                rs = rs.Replace("\"", "&quot;");
                //rs = rs.Replace("", "");
                //rs = rs.Replace("", "");
                //rs = rs.Replace("", "");
                //rs = rs.Replace("", "");
                //rs = rs.Replace("", "");
            }
            return rs;
        }

        string JsonDecode(string rs)
        {
            if (!string.IsNullOrEmpty(rs))
            {
                rs = rs.Replace("&quot;", "\"");
                //rs = rs.Replace("", "");
                //rs = rs.Replace("", "");
                //rs = rs.Replace("", "");
                //rs = rs.Replace("", "");
                //rs = rs.Replace("", "");
            }
            return rs;
        }
    }
}
