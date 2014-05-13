using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace Helper.Database
{
    internal class OleDbProvider : AbstractDbProvider
    {
        public override IDbConnection CreateConnection()
        {
            return new OleDbConnection();
        }

        public override IDbCommand CreateCommand()
        {
            return new OleDbCommand();
        }

        public override IDbDataAdapter CreateDataAdapter()
        {
            return new OleDbDataAdapter();
        }

        public override IDbDataParameter CreateParameter()
        {
            return new OleDbParameter();
        }


    }
}
