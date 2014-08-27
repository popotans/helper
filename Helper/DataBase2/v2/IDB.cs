using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Helper
{
    public interface IDb
    {
        IDataReader GetReader(string sql, params IDataParameter[] p);
        IDataReader GetReader(DbConnection conn, string sql, params IDataParameter[] p);
        DataTable GetTable(string sql, params IDataParameter[] p);

        int ExecNonQuery(string sql, params IDataParameter[] p);
        int ExecNonQuery(DbConnection conn, string sql, params IDataParameter[] p);
        object ExecScalar(string sql, params IDataParameter[] p);
        object ExecScalar(DbConnection conn, string sql, params IDataParameter[] p);
        int ExecScalarInt(string sql, params IDataParameter[] p);
        int ExecScalarInt(DbConnection conn, string sql, params IDataParameter[] p);

        string ConnStr { get; set; }
        DbConnection GetConn();
        DbConnection GetConn(string connStr);

        IDbDataParameter GetParam(string name, object val);
        IDbDataParameter GetParam(string name, object val, object t);
        IDbDataParameter[] GetParams(List<string> names, List<object> vals, List<object> types);
        IDbDataParameter[] GetParams(Dictionary<string, object> dic, List<object> vals);
        IDbDataParameter[] GetParams(Dictionary<string, object> dic);
    }
}
