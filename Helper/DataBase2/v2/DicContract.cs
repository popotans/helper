using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{

    /// <summary>
    /// 进行字典序列化的接口，方便实现键值对的映射关系和重建
    /// </summary>
    public interface IDicSerializable
    {
        /// <summary>
        /// 从数据到字典
        /// </summary>
        /// <returns></returns>
        IDictionary<string, object> Serialize();

        /// <summary>
        /// 从字典到数据
        /// </summary>
        /// <param name="dicts">字典</param>
        /// <param name="scenario">应用场景</param>
        void Deserialise(IDictionary<string, object> dicts);
    }

    public abstract class BaseMap : IDicSerializable
    {
        public abstract IDictionary<string, object> Serialize();

        public abstract void Deserialise(IDictionary<string, object> dicts);

        //public string DbTableName { get; set; }
        //public string DbAutoField { get; set; }

        protected virtual T GetVal<T>(IDictionary<string, object> dic, string key)
        {
            if (dic[key] == null || dic[key].ToString() == "NULL" || dic[key] == DBNull.Value)
                return default(T);
            return (T)Convert.ChangeType(dic[key], typeof(T));
        }

        protected virtual void SetVal(IDictionary<string, object> dic, object val)
        {

        }
    }



}
