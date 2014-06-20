using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Helper.Database.v2
{
    public class SqlCommand
    {
        public static SqlCommand Current { get; set; }
        public SqlCommand(string sql, IDataParameter[] param)
        {
            this.Sql = sql;
            this.Params = param;
        }

        public string Sql { get; set; }
        public IDataParameter[] Params { get; set; }

    }
}
