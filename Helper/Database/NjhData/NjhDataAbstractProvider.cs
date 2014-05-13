using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Helper.Database
{
    /// <summary>
    /// 抽象的数据访问提供者，提供数据访问的核心对象
    /// </summary>
    abstract public class AbstractDbProvider
    {
        public string ConnStr { get; set; }
        public abstract IDbConnection CreateConnection();

        public abstract IDbCommand CreateCommand();

        public abstract IDbDataAdapter CreateDataAdapter();

        public abstract IDbDataParameter CreateParameter();

    }
}
