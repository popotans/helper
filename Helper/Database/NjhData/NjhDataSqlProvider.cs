using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Helper.Database
{
    internal class SqlProvider : AbstractDbProvider
    {
        public override IDbConnection CreateConnection()
        {
            return new SqlConnection();
        }

        public override IDbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        public override IDbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public override IDbDataParameter CreateParameter()
        {
            return new SqlParameter();
        }

    }
}
