using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Helper
{
    public class ClassHelper
    {
        /// <summary>
        ///类的字段 是否包含某个Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ContainsAttributebyField<T>(Type type, string FieldName) where T : Attribute
        {
            PropertyInfo[] piArr = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in piArr)
            {
                if (pi.Name == FieldName)
                {
                    object[] arr = pi.GetCustomAttributes(typeof(T), true);
                    if (arr.Length == 0) return null;
                    return (T)arr[0];
                }
            }
            return null;
        }

        public static T ContainsAttributebyMethod<T>(Type type, string methodName) where T : Attribute
        {
            MethodInfo mi = type.GetMethod(methodName);
            if (mi != null)
            {
                object[] arr = mi.GetCustomAttributes(typeof(T), true);
                if (arr.Length == 0) return null;
                return (T)arr[0];
            }
            return null;
        }

        /// <summary>
        /// 类是否包含某个Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static T ContainsAttribute<T>(Type type) where T : Attribute
        {
            object[] arr = type.GetCustomAttributes(typeof(T), true);
            if (arr.Length == 0) return null;
            return (T)arr[0];
        }
    }
}
