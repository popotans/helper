using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
namespace Helper
{
    public class DbMapSqlServer : DbMap
    {
        public DbMapSqlServer(string connStr)
        {
            this.DbContext = new SqlServerDB(connStr);
        }

        protected override string GeneralInsert<T>(T t, ref IDbDataParameter[] paramArr)
        {
            InitDbChar();
            IDictionary<string, object> dic = t.Serialize();
            string dBDatbelName = GetTableName(dic);
            string dbAutoField = GetAutoField(dic);
            string sqlColumn = "insert into " + _BeginChar + dBDatbelName + _EndChar + " (", sqlParam = "(";
            List<SqlParameter> list = new List<SqlParameter>();
            foreach (string item in dic.Keys)
            {
                if (item.StartsWith("_____")) continue;
                if (string.Compare(dbAutoField, item, true) == 0)
                {
                    continue;
                }

                sqlColumn += string.Format(_BeginChar + "{0}" + _EndChar + ",", item);
                sqlParam += string.Format(_ParameterChar + "{0},", item);
                SqlParameter olp = new SqlParameter(_ParameterChar + item, dic[item]);
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
            InitDbChar();
            List<SqlParameter> list = new List<SqlParameter>();
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
                SqlParameter olp = new SqlParameter(_ParameterChar + item, dic[item]);
                olp.SqlDbType = Convert2DbType(dic[item]);
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
            List<SqlParameter> list = new List<SqlParameter>();
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
                SqlParameter olp = new SqlParameter(_ParameterChar + item, dic[item]);
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

        protected override string GeneralUpdate<T>(T t, string where, List<string> columns, ref IDbDataParameter[] paraArr)
        {
            InitDbChar();
            List<SqlParameter> list = new List<SqlParameter>();
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
                SqlParameter olp = new SqlParameter(_ParameterChar + item, dic[item]);
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

        #region db reflection
        public override string GeneralInsertRef<T>(T t, ref IDbDataParameter[] paramArr)
        {
            InitDbChar();
            Type type = typeof(T);
            string dBDatbelName = type.Name; ;
            PropertyInfo[] piArr = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string sqlColumn = string.Format("insert into {0}{1}{2} (", _BeginChar, dBDatbelName, _EndChar);
            string sqlParam = "(";
            List<SqlParameter> list = new List<SqlParameter>();
            foreach (PropertyInfo pi in piArr)
            {
                if (pi.GetCustomAttributes(typeof(AutoIncreaseAttribute), true).Length > 0)
                {
                    continue;
                }
                sqlColumn += string.Format("{0}{1}{2},", _BeginChar, pi.Name, _EndChar);
                sqlParam += string.Format("{0}{1},", _ParameterChar, pi.Name);

                object val = GetDefaultValue(pi.PropertyType, pi.GetValue(t, null));
                SqlParameter olp = new SqlParameter(string.Format("{0}{1}", _ParameterChar, pi.Name), val);
                olp.SqlDbType = Convert2DbType(val);
                list.Add(olp);
            }

            sqlColumn = sqlColumn.TrimEnd(',');
            sqlParam = sqlParam.TrimEnd(',');
            string sql = sqlColumn + ")values" + sqlParam + ")";
            paramArr = list.ToArray();
            return sql;
        }

        public override string GeneralUpdateRef<T>(T t, ref IDbDataParameter[] paraArr)
        {
            InitDbChar();
            string where = "";
            Type type = typeof(T);
            List<SqlParameter> list = new List<SqlParameter>();
            string dBDatbelName = type.Name;
            string sql = string.Format("update {0}{1}{2} set ", _BeginChar, dBDatbelName, _EndChar);
            PropertyInfo[] piArr = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in piArr)
            {
                if (pi.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length > 0)
                {
                    string __fff = GetDefaultValue(pi.PropertyType, pi.GetValue(t, null)).ToString();
                    if (pi.PropertyType == typeof(string)) __fff = string.Format("'{0}'", __fff);
                    where = string.Format(" where {0}{1}{2}={3} ", _BeginChar, pi.Name, _EndChar, __fff);
                }
                if (pi.GetCustomAttributes(typeof(AutoIncreaseAttribute), true).Length == 0)
                {
                    sql += string.Format("{0}{1}{2}={3}{1},", _BeginChar, pi.Name, _EndChar, _ParameterChar);

                    object val = GetDefaultValue(pi.PropertyType, pi.GetValue(t, null));
                    SqlParameter olp = new SqlParameter(string.Format("{0}{1}", _ParameterChar, pi.Name), val);
                    olp.SqlDbType = Convert2DbType(val);
                    list.Add(olp);
                }
            }
            if (string.IsNullOrEmpty(where))
            {
                throw new ArgumentException("lost primarykey");
            }
            sql = sql.TrimEnd(',') + where;
            paraArr = list.ToArray();
            return sql;
        }
        public override string GeneralUpdateRef<T>(T t, List<string> columns, ref IDbDataParameter[] paraArr)
        {
            if (columns.Count == 0) throw new ArgumentException("no columns");
            InitDbChar();
            string where = "";
            Type type = typeof(T);
            List<SqlParameter> list = new List<SqlParameter>();
            string dBDatbelName = type.Name;
            string sql = string.Format("update {0}{1}{2} set ", _BeginChar, dBDatbelName, _EndChar);
            PropertyInfo[] piArr = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in piArr)
            {
                if (pi.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length > 0)
                {
                    string __fff = GetDefaultValue(pi.PropertyType, pi.GetValue(t, null)).ToString();
                    if (pi.PropertyType == typeof(string)) __fff = string.Format("'{0}'", __fff);
                    where = string.Format(" where {0}{1}{2}={3} ", _BeginChar, pi.Name, _EndChar, __fff);
                }

                if (pi.GetCustomAttributes(typeof(AutoIncreaseAttribute), true).Length == 0)
                {
                    if (!columns.Contains(pi.Name)) continue;

                    sql += string.Format("{0}{1}{2}={3}{1},", _BeginChar, pi.Name, _EndChar, _ParameterChar);

                    object val = GetDefaultValue(pi.PropertyType, pi.GetValue(t, null));
                    SqlParameter olp = new SqlParameter(string.Format("{0}{1}", _ParameterChar, pi.Name), val);
                    olp.SqlDbType = Convert2DbType(val);
                    list.Add(olp);
                }
            }
            if (string.IsNullOrEmpty(where))
            {
                throw new ArgumentException("lost primarykey");
            }
            sql = sql.TrimEnd(',') + where;
            paraArr = list.ToArray();
            return sql;
        }
        public override string GeneralDelete<T>(T t, ref IDbDataParameter[] paraArr)
        {
            InitDbChar();
            string where = "";
            Type type = typeof(T);
            List<SqlParameter> list = new List<SqlParameter>();
            string dBDatbelName = type.Name;
            string sql = string.Format("delete * from  {0}{1}{2} ", _BeginChar, dBDatbelName, _EndChar);
            PropertyInfo[] piArr = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in piArr)
            {
                if (pi.GetCustomAttributes(typeof(PrimaryKeyAttribute), true).Length > 0)
                {
                    string __fff = GetDefaultValue(pi.PropertyType, pi.GetValue(t, null)).ToString();
                    if (pi.PropertyType == typeof(string)) __fff = string.Format("'{0}'", __fff);
                    where = string.Format(" where {0}{1}{2}={3} ", _BeginChar, pi.Name, _EndChar, __fff);
                }
                if (string.IsNullOrEmpty(where) && pi.GetCustomAttributes(typeof(AutoIncreaseAttribute), true).Length == 0)
                {
                    object val = GetDefaultValue(pi.PropertyType, pi.GetValue(t, null));
                    SqlParameter olp = new SqlParameter(string.Format("{0}{1}", _ParameterChar, pi.Name), val);
                    olp.SqlDbType = Convert2DbType(val);
                    list.Add(olp);
                }
            }
            if (string.IsNullOrEmpty(where))
            {
                throw new ArgumentException("lost primarykey");
            }
            sql = sql.TrimEnd(',') + where;
            paraArr = list.ToArray();
            return sql;
        }
        #endregion

    }
}
