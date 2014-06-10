using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Web.Caching;
using Helper.DbDataType;

namespace Helper.Database
{

    public class DataMapper
    {
        private static object _lockhelper = new object();
        private static Dictionary<string, object[]> _typeCache = new Dictionary<string, object[]>();
        private static DataMapperStringComparer _StringComparer = new DataMapperStringComparer();


        private static void SetValue<T>(ref T t, string field, object value, List<PropertyInfo> _propertyList)
        {
            foreach (PropertyInfo p in _propertyList)
            {
                if (string.Compare(p.Name, field, true) == 0)
                {
                    //ISetValue setter = null;
                    //setter = GetterSetterFactory.CreatePropertySetterWrapper(p);
                    //setter.Set(t, value);
                    p.SetValue(t, value, null);
                    break;
                }
            }
        }
        public static T GetObject<T>(IDataReader reader)
        {
            List<PropertyInfo> _propertyList = GetProperties<T>();

            T t = Activator.CreateInstance<T>();
            string fieldName = "";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                fieldName = reader.GetName(i);
                SetValue(ref t, fieldName, reader.IsDBNull(i) ? null : reader[fieldName], _propertyList);
            }

            #region v2
            // List<string> _fieldNames = GetFieldNames(reader);
            //foreach (PropertyInfo p in _propertyList)
            //{
            //    if (!_fieldNames.Contains(p.Name, _StringComparer))
            //        continue;

            //    object fieldValue = GetRecValue(reader, p.Name);
            //    if (fieldValue != null)
            //    {
            //        #region v1 直接反射赋值
            //        //if (p.PropertyType == typeof(System.String))
            //        //{
            //        //    p.SetValue(order, fieldValue.ToString(), null);
            //        //}
            //        //else
            //        //{
            //        //    p.SetValue(order, fieldValue, null);
            //        //}
            //        #endregion
            //        #region v2 使用委托
            //        setter = GetterSetterFactory.CreatePropertySetterWrapper(p);
            //        setter.Set(t, fieldValue);
            //        #endregion
            //    }
            //}
            #endregion
            return t;
        }

        public static T GetObjectV2<T>(IDataReader reader) where T : IDicSerialize, new()
        {
            T tt = new T();
            Dictionary<string, MyField> dic = new Dictionary<string, MyField>();
            object _obj = null;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                object ritem = reader[i];
                Type __type = ritem.GetType();
                _obj = reader[reader.GetName(i)];
                if (_obj == null || _obj == DBNull.Value)
                {
                    if (__type == typeof(string))
                        _obj = string.Empty;
                    else if (__type == typeof(DateTime))
                        _obj = new DateTime(1970, 1, 1);
                }
                MyField mdv = new MyField(_obj, __type);
                dic.Add(reader.GetName(i), mdv);
            }

            return (T)tt.ToObj(dic);
        }

        public static T GetObjectV2<T>(DataTable dt, DataRow dr) where T : IDicSerialize, new()
        {
            if (dt.Rows.Count == 0) return default(T);
            T tt = new T();
            Dictionary<string, MyField> dic = new Dictionary<string, MyField>();
            object _obj = null;
            string _name = null;
            foreach (DataColumn c in dt.Columns)
            {
                _name = c.ColumnName;
                _obj = dr[_name];
                Type __type = dt.Columns[_name].DataType;
                if (_obj == null || _obj == DBNull.Value)
                {
                    if (__type == typeof(string))
                        _obj = string.Empty;
                    else if (__type == typeof(DateTime))
                        _obj = new DateTime(1970, 1, 1);
                }
                MyField mdv = new MyField(_obj, __type);
                dic.Add(_name, mdv);
            }
            return (T)tt.ToObj(dic);
        }

        public static T GetObject<T>(DataTable dt, DataRow dr)
        {
            List<PropertyInfo> _propertyList = GetProperties<T>();
            T t = Activator.CreateInstance<T>();

            List<string> _fieldNames = GetFieldNames(dt);
            foreach (string item in _fieldNames)
            {
                SetValue(ref t, item, dr[item] == DBNull.Value ? null : dr[item], _propertyList);
            }
            return t;
        }
        public static List<T> GetObjectList<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(GetObject<T>(dt, dr));
            }
            return list;
        }

        /// <summary>
        /// 获取类型和对应属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

            //if (_typeCache.ContainsKey(type.FullName))
            //{
            //    oProperties = _typeCache[type.FullName];
            //}
            //else
            //{
            //    oProperties = type.GetProperties();

            //    if (!_typeCache.ContainsKey(type.FullName))
            //        lock (_lockhelper)
            //            if (!_typeCache.ContainsKey(type.FullName))
            //                _typeCache[type.FullName] = oProperties;

            //    //lock (_lockhelper)
            //    //{
            //    //    if (!_typeCache.ContainsKey(type.FullName))
            //    //        _typeCache[type.FullName] = oProperties;
            //    //}

            //}

            //return list;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T Clone<T>(T source, T target)
        {
            if (target == null) target = default(T);
            List<PropertyInfo> props = GetProperties<T>();
            foreach (PropertyInfo pi in props)
            {
                if (pi.CanRead && pi.CanWrite)
                {
                    pi.SetValue(target, pi.GetValue(source, null), null);
                }
            }
            return target;
        }



        /// <summary>
        /// 清楚缓存
        /// </summary>
        public static void ClearTypeCache()
        {
            lock (_lockhelper)
                _typeCache.Clear();
        }

        //private static object GetRecValue(IDataReader dr, string name)
        //{
        //    IDataRecord rec = dr as IDataRecord;
        //    int idx = rec.GetOrdinal(name);
        //    if (idx >= 0)
        //    {
        //        if (rec.GetValue(idx) == DBNull.Value)
        //            return null;
        //        else
        //            return rec.GetValue(idx);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //private static object GetRecValue(System.Data.DataRow dr, string name)
        //{
        //    if (dr.Table.Columns.Contains(name))
        //        if (dr[name] != DBNull.Value) return dr[name];
        //        else return null;
        //    else return null;
        //}


        //public static List<string> GetFieldNames(IDataReader reader)
        //{
        //    DataTable dt = reader.GetSchemaTable();
        //    return dt.AsEnumerable().Select(row => { return (row[0] as string).ToLower(); }).ToList<string>();
        //}

        public static List<string> GetFieldNames(System.Data.DataTable dt)
        {
            List<string> l = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                l.Add(dc.ColumnName);
            }
            return l;
        }


        #region

        [AttributeUsage(AttributeTargets.Property)]
        public class KeyAttribute : Attribute
        {
        }

        private static IEnumerable<PropertyInfo> GetAllProperties(object entity)
        {
            return entity.GetType().GetProperties();
        }

        private static void BuildInsertParameters(object entityToInsert, StringBuilder sb)
        {
            var props = GetAllProperties(entityToInsert);

            for (var i = 0; i < props.Count(); i++)
            {
                var property = props.ElementAt(i);
                if (property.GetCustomAttributes(true).Where(a => a is KeyAttribute).Any()) continue;
                sb.Append(property.Name);
                if (i < props.Count() - 1)
                    sb.Append(", ");
            }
        }
        private static void BuildInsertValues(object entityToInsert, StringBuilder sb)
        {
            var props = GetAllProperties(entityToInsert);

            for (var i = 0; i < props.Count(); i++)
            {
                var property = props.ElementAt(i);
                if (property.GetCustomAttributes(true).Where(a => a is KeyAttribute).Any()) continue;
                sb.AppendFormat("@{0}", property.Name);
                if (i < props.Count() - 1)
                    sb.Append(", ");
            }
        }

        private static void BuildUpdateSet(object entityToUpdate, StringBuilder sb)
        {


        }

        #endregion
    }


    public class DataMapperStringComparer : IEqualityComparer<string>
    {

        public bool Equals(string x, string y)
        {
            return string.Compare(x, y, true) == 0;
        }

        public int GetHashCode(string obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }

    public interface ISetValue
    {
        void Set(object target, object val);
    }
    public class SetterWrapper<TTarget, TValue> : ISetValue
    {
        private Action<TTarget, TValue> _setter;

        public SetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.CanWrite == false)
                throw new NotSupportedException("属性不支持写操作。");

            MethodInfo m = propertyInfo.GetSetMethod(true);
            _setter = (Action<TTarget, TValue>)Delegate.CreateDelegate(typeof(Action<TTarget, TValue>), null, m);
        }

        public void SetValue(TTarget target, TValue val)
        {
            _setter(target, val);
        }

        public static ISetValue CreatePropertySetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            if (propertyInfo.CanWrite == false)
                throw new NotSupportedException("属性不支持写操作。");

            MethodInfo mi = propertyInfo.GetSetMethod(true);

            if (mi.GetParameters().Length > 1)
                throw new NotSupportedException("不支持构造索引器属性的委托。");

            Type instanceType = typeof(SetterWrapper<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            return (ISetValue)Activator.CreateInstance(instanceType, propertyInfo);
        }

        public void Set(object target, object val)
        {
            _setter((TTarget)target, (TValue)val);
        }
    }

    public static class GetterSetterFactory
    {
        public static ISetValue CreatePropertySetterWrapper(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");
            if (propertyInfo.CanRead == false)
                throw new InvalidOperationException("属性不支持读操作。");

            MethodInfo mi = propertyInfo.GetGetMethod(true);

            if (mi.GetParameters().Length > 0)
                throw new NotSupportedException("不支持构造索引器属性的委托。");

            Type instanceType = typeof(SetterWrapper<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
            return (ISetValue)Activator.CreateInstance(instanceType, propertyInfo);
        }
    }
}

