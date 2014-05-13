using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.Database
{
    /// <summary>
    /// 列Attribute
    /// </summary>
    public class DbColumnAttribute : Attribute
    {
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimary { get; set; }
        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsAutoIncrement { get; set; }

        public DbColumnAttribute()
        {

        }
        public DbColumnAttribute(bool isPrimary, bool isAutoIncrement)
        {
            this.IsAutoIncrement = isAutoIncrement;
            this.IsPrimary = isPrimary;
        }

    }

    //public class Test
    //{
    //    [DbColumn(IsPrimary = true, IsAutoIncrement = true)]
    //    public int IDx { get; set; }
    //    [DbColumn(true, true)]
    //    public string Name { get; set; }
    //}
  
}
