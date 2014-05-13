using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Helper.Database
{
    internal class MySqlProvider : AbstractDbProvider
    {
        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

        public override IDbCommand CreateCommand()
        {
            return new MySqlCommand();
        }

        public override IDbDataAdapter CreateDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        public override IDbDataParameter CreateParameter()
        {
            return new MySqlParameter();
        }
    }
}
