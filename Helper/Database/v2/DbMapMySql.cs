using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
namespace Helper
{
    public class DbMapMySql : DbMap
    {
        public DbMapMySql(string connStr)
        {
            this.db = new MySqlDB(connStr);
        }
        protected override string GeneralInsert<T>(T t, ref IDbDataParameter[] paramArr)
        {
            InitDbChar();
            IDictionary<string, object> dic = t.Serialize();
            string dBDatbelName = GetTableName(dic);
            string dbAutoField = GetAutoField(dic);
            string sqlColumn = "insert into " + _BeginChar + dBDatbelName + _EndChar + " (", sqlParam = "(";
            List<MySqlParameter> list = new List<MySqlParameter>();
            foreach (string item in dic.Keys)
            {
                if (item.StartsWith("_____")) continue;
                if (string.Compare(dbAutoField, item, true) == 0)
                {
                    continue;
                }

                sqlColumn += string.Format(_BeginChar + "{0}" + _EndChar + ",", item);
                sqlParam += string.Format(_ParameterChar + "{0},", item);
                MySqlParameter olp = new MySqlParameter(_ParameterChar + item, dic[item]);
                olp.MySqlDbType = Convert2DbType(dic[item]);
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
            InitDbChar();
            List<MySqlParameter> list = new List<MySqlParameter>();
            IDictionary<string, object> dic = t.Serialize();
            string dBDatbelName = dic["______TableName"].ToString();
            string sql = "update " + _BeginChar + dBDatbelName + _EndChar + " set ";
            string autoField = GetAutoField(dic);
            foreach (string item in dic.Keys)
            {
                if (item.StartsWith("_____")) continue;
                if (string.Compare(autoField, item, true) == 0)
                {
                    continue;
                }
                sql += string.Format(_BeginChar + "{0}" + _EndChar + "=" + _ParameterChar + "{0},", item);
                MySqlParameter olp = new MySqlParameter(_ParameterChar + item, dic[item]);
                olp.MySqlDbType = Convert2DbType(dic[item]);
                if (dic[item] == null) olp.Value = string.Empty;
                else if (dic[item].GetType() == typeof(DateTime))
                {
                    if (dic[item].ToString().StartsWith("0001")) olp.Value = new DateTime(1970, 1, 1);
                }
                list.Add(olp);
            }
            sql = sql.TrimEnd(',') + " where " + _BeginChar + autoField + _EndChar + "=" + dic[autoField];
            paraArr = list.ToArray();
            return sql;
        }

        protected override string GeneralUpdate<T>(T t, string where, ref IDbDataParameter[] paraArr)
        {
            InitDbChar();
            List<MySqlParameter> list = new List<MySqlParameter>();
            IDictionary<string, object> dic = t.Serialize();
            string sql = "update " + _BeginChar + GetTableName(dic) + _EndChar + " set ";
            string autoField = GetAutoField(dic);
            foreach (string item in dic.Keys)
            {
                if (item.StartsWith("_____")) continue;
                if (string.Compare(autoField, item, true) == 0)
                {
                    continue;
                }
                sql += string.Format(_BeginChar + "{0}" + _EndChar + "=" + _ParameterChar + "{0},", item);
                MySqlParameter olp = new MySqlParameter(_ParameterChar + item, dic[item]);
                olp.MySqlDbType = Convert2DbType(dic[item]);
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

        protected override string GeneralUpdate<T>(T t, string where, List<string> columns, ref IDbDataParameter[] paraArr)
        {
            InitDbChar();
            List<MySqlParameter> list = new List<MySqlParameter>();
            IDictionary<string, object> dic = t.Serialize();
            string sql = "update " + _BeginChar + GetTableName(dic) + _EndChar + " set ";
            string autoField = GetAutoField(dic);
            if (string.IsNullOrEmpty(where))
                where = _BeginChar + autoField + _EndChar + "=" + dic[autoField];
            foreach (string item in dic.Keys)
            {
                if (item.StartsWith("_____")) continue;
                if (!columns.Contains(item, StringComparer.CurrentCultureIgnoreCase)) continue;
                if (string.Compare(autoField, item, true) == 0)
                {
                    continue;
                }
                sql += string.Format(_BeginChar + "{0}" + _EndChar + "=" + _ParameterChar + "{0},", item);
                MySqlParameter olp = new MySqlParameter(_ParameterChar + item, dic[item]);
                olp.MySqlDbType = Convert2DbType(dic[item]);
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

        protected new MySqlDbType Convert2DbType(object val)
        {
            if (val == null)
            {
                return MySqlDbType.VarChar;
            }

            Type t = val.GetType();
            if (t == typeof(string)) return MySqlDbType.VarChar;
            if (t == typeof(decimal)) return MySqlDbType.Decimal;
            if (t == typeof(double)) return MySqlDbType.Double;
            if (t == typeof(float)) return MySqlDbType.Float;
            if (t == typeof(DateTime)) return MySqlDbType.Datetime;
            if (t == typeof(Int16)) return MySqlDbType.Int16;
            if (t == typeof(int)) return MySqlDbType.Int32;
            if (t == typeof(Int32)) return MySqlDbType.Int32;
            if (t == typeof(Int64)) return MySqlDbType.Int64;
            return MySqlDbType.VarChar;
        }

    }
}
