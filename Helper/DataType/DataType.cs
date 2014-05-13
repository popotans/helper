using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.DbDataType
{
    public class MyField
    {
        public MyField(object data, Type type)
            : this(data, type, false)
        {

        }
        public MyField(object data, Type type, bool isIdentity)
        {
            this.Type = type;

            if (type == typeof(DateTime))
            {
                if (data != null && data.ToString().IndexOf("0001") == 0)
                {
                    data = new DateTime(1970, 1, 1);
                }
                else if (data == null)
                {
                    data = new DateTime(1970, 1, 1);
                }
                else if (data.ToString().IndexOf("0000") == 0)
                {
                    data = new DateTime(1970, 1, 1);
                }
            }
            else if (typeof(string) == type)
            {
                if (data == null)
                    data = string.Empty;
            }
            this.Data = data;
            this.IsIdentity = isIdentity;
        }

        private object _data;
        /// <summary>
        /// 值
        /// </summary>
        public object Data
        {
            get
            {
                if (_data == null)
                {
                    if (Type == typeof(string)) _data = string.Empty;
                    else if (this.Type == typeof(int)) _data = -999999;
                }
                if (this.Type == typeof(DateTime))
                {
                    DateTime _dt = DateTime.Now;
                    if (_data == null) _data = new DateTime(1970, 1, 1);
                    else if (!DateTime.TryParse(_data.ToString(), out _dt))
                    {
                        _data = new DateTime(1970, 1, 1);
                    }
                }
                else if (this.Type == typeof(DBNull))
                {
                    _data = new DateTime(1970, 1, 1);
                }


                return _data;
            }
            set
            {
                if (value == null)
                {
                    if (Type == typeof(string)) value = string.Empty;
                    else if (Type == typeof(DateTime)) value = new DateTime(1970, 1, 1);
                }
                else if (value == DBNull.Value)
                {
                    if (Type == typeof(DateTime)) value = new DateTime(1970, 1, 1);
                }
                _data = value;
            }
        }
        /// <summary>
        /// 值的类型
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// 是否 自增
        /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// 是否 自增
        /// </summary>
        public bool IsPrimary { get; set; }
    }

    public class PageCount
    {
        public int PageTotal { get; set; }
        public int RecordTotal { get; set; }
    }

}
