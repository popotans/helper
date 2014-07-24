using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Helper.Database
{
    public class MyDbCommand
    {
        public static MyDbCommand Current { get; set; }
        public MyDbCommand(string sql, IDataParameter[] param)
        {
            this.Sql = sql;
            this.Params = param;
        }

        public string Sql { get; set; }
        public IDataParameter[] Params { get; set; }

    }
}
