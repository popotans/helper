using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using Helper.Database;
namespace Helper
{
    public class ReflectionHelper
    {
        static Hashtable ht = new Hashtable();
        static Dictionary<string, object> dic = new Dictionary<string, object>();

        public static object GetTypeField<T>(string name)
        {
            object obj = null;
            if (dic.TryGetValue(typeof(T).ToString(), out obj))
            {

            }
            else
            {
                FieldInfo fieldInfo = typeof(T).GetField(name);
                if (fieldInfo != null)
                {
                    obj = fieldInfo.GetValue(null);
                    dic.Add(typeof(T).ToString(), obj);
                }
            }
            return obj;
        }
        public static string GetTypeField_IdentityKeys<T>()
        {
            object oop = GetTypeField<T>("IdentitKeys");
            return oop == null ? string.Empty : oop.ToString();
        }
        /// <summary>
        /// 获取不需要插入的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<string> GetNotInsertedColumns<T>()
        {
            return CacheHelper.GetCachedObject<List<string>>(string.Format("GetNotInsertedColumns{0}", typeof(T).ToString()), 1800, delegate()
            {
                List<string> list = new List<string>();
                PropertyInfo[] properties = DataMapper.GetProperties<T>().ToArray();
                object[] arr = null;
                foreach (PropertyInfo pi in properties)
                {
                    arr = pi.GetCustomAttributes(typeof(DbColumnAttribute), false);
                    if (arr == null || arr.Length == 0) continue;
                    foreach (object obj in arr)
                    {
                        if (((DbColumnAttribute)obj).IsAutoIncrement) list.Add(pi.Name);
                    }
                }
                return list;
            });
        }

        public static List<string> GetIdentityColumn<T>()
        {
            return CacheHelper.GetCachedObject<List<string>>(string.Format("GetIdentityColumnT{0}", typeof(T).ToString()), 1800, delegate()
            {
                List<string> list = new List<string>();
                PropertyInfo[] properties = DataMapper.GetProperties<T>().ToArray();
                object[] arr = null;
                foreach (PropertyInfo pi in properties)
                {
                    arr = pi.GetCustomAttributes(typeof(DbColumnAttribute), false);
                    if (arr == null || arr.Length == 0) continue;
                    foreach (object obj in arr)
                    {
                        if (((DbColumnAttribute)obj).IsPrimary) list.Add(pi.Name);
                    }
                }

                return list;
            });
        }

        /// <summary>
        /// 获取类型和对应属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<PropertyInfo> GetProperties(Type type)
        {
            object[] oProperties = null;

            return CacheHelper.GetCachedObject<List<PropertyInfo>>(type.FullName, 1800, delegate()
            {
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
            });

        }

        public static List<PropertyInfo> GetProperties<T>()
        {
            Type type = typeof(T);
            object[] oProperties = null;

            return CacheHelper.GetCachedObject<List<PropertyInfo>>(type.FullName, 1800, delegate()
            {
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
            });
        }



    }
}
