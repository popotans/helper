using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace Helper
{
    public class DbMapSqlServer : DbMap
    {
        public DbMapSqlServer(string connStr)
        {
            this.db = new SqlServerDB(connStr);
        }

        protected override string GeneralInsert<T>(T t, ref IDbDataParameter[] paramArr)
        {
            IDictionary<string, object> dic = t.Serialize();
            string sqlColumn = "insert into [" + GetTableName(dic) + "] (", sqlParam = "(";
            List<SqlParameter> list = new List<SqlParameter>();
            string autoField = GetAutoField(dic);
            foreach (string item in dic.Keys)
            {
                if (string.Compare(autoField, item, true) == 0)
                {
                    continue;
                }
                sqlColumn += string.Format("[{0}],", item);
                sqlParam += string.Format("@{0},", item);
                SqlParameter olp = new SqlParameter("@" + item, dic[item]);
                olp.SqlDbType = Convert2DbType(dic[item]);
                if (dic[item] == null) olp.Value = string.Empty;
                else if (dic[item].GetType() == typeof(DateTime))
                {
                    if (dic[item].ToString().StartsWith("0001")) olp.Value = new DateTime(1970, 1, 1);
                }

                list.Add(olp);
            }
            sqlColumn = sqlColumn.TrimEnd(',');
            sqlParam = sqlParam.TrimEnd(',');
            string sql = sqlColumn + ")values" + sqlParam + ")";
            paramArr = list.ToArray();
            return sql;
        }

        protected override string GeneralUpdate<T>(T t, ref IDbDataParameter[] paraArr)
        {
            
            List<SqlParameter> list = new List<SqlParameter>();
            IDictionary<string, object> dic = t.Serialize();
            string sql = "update [" + GetTableName(dic) + "] set ";
            string autoField = GetAutoField(dic);
            foreach (string item in dic.Keys)
            {
                if (string.Compare(autoField, item, true) == 0)
                {
                    continue;
                }
                sql += string.Format("[{0}]=@{0},", item);
                SqlParameter olp = new SqlParameter("@" + item, dic[item]);
                olp.SqlDbType = Convert2DbType(dic[item]);
                if (dic[item] == null) olp.Value = string.Empty;
                else if (dic[item].GetType() == typeof(DateTime))
                {
                    if (dic[item].ToString().StartsWith("0001")) olp.Value = new DateTime(1970, 1, 1);
                }
                list.Add(olp);
            }
            sql = sql.TrimEnd(',') + " where [" + autoField + "]=" + dic[autoField];
            paraArr = list.ToArray();
            return sql;
        }

        protected override string GeneralUpdate<T>(T t, string where, ref IDbDataParameter[] paraArr)
        {
            List<SqlParameter> list = new List<SqlParameter>();
            IDictionary<string, object> dic = t.Serialize();
            string sql = "update [" + GetTableName(dic) + "] set ";
            string autoField = GetAutoField(dic);
            foreach (string item in dic.Keys)
            {
                if (string.Compare(autoField, item, true) == 0)
                {
                    continue;
                }
                sql += string.Format("[{0}]=@{0},", item);
                SqlParameter olp = new SqlParameter("@" + item, dic[item]);
                olp.SqlDbType = Convert2DbType(dic[item]);
                if (dic[item] == null) olp.Value = string.Empty;
                else if (dic[item].GetType() == typeof(DateTime))
                {
                    if (dic[item].ToString().StartsWith("0001")) olp.Value = new DateTime(1970, 1, 1);
                }
                list.Add(olp);
            }
            sql = sql.TrimEnd(',') + " where " + where;
            paraArr = list.ToArray();
            return sql;
        }


        protected new SqlDbType Convert2DbType(object val)
        {
            if (val == null)
            {
                return SqlDbType.VarChar;
            }

            Type t = val.GetType();
            if (t == typeof(string)) return SqlDbType.VarChar;
            if (t == typeof(decimal)) return SqlDbType.Decimal;
            if (t == typeof(double)) return SqlDbType.Decimal;
            if (t == typeof(float)) return SqlDbType.Float;
            if (t == typeof(DateTime)) return SqlDbType.DateTime;
            if (t == typeof(Int16)) return SqlDbType.SmallInt;
            if (t == typeof(int)) return SqlDbType.Int;
            if (t == typeof(Int32)) return SqlDbType.Int;
            if (t == typeof(Int64)) return SqlDbType.BigInt;
            return SqlDbType.VarChar;
        }

    }
}
