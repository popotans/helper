using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
namespace Helper
{
    public abstract class DbMap
    {
        internal string _BeginChar { get; set; }
        internal string _EndChar { get; set; }
        internal string _ParameterChar { get; set; }

        public IDb DbContext
        {
            get;
            set;
        }

        #region protected

        protected string GetTableName(IDictionary<string, object> dic)
        {
            return dic["______TableName"].ToString();
        }

        protected string GetAutoField(IDictionary<string, object> dic)
        {
            return dic["______AutoField"].ToString();
        }

        private string GetDbSpecChara(IDb db)
        {
            if (db.GetType() == typeof(AccessDB) || db.GetType() == typeof(SqlServerDB))
            {
                return "[";
            }
            else if (db.GetType() == typeof(MySqlDB))
            {
                return "`";
            }
            return "";
        }

        private string GetDbSpecCharb(IDb db)
        {
            if (db.GetType() == typeof(AccessDB) || db.GetType() == typeof(SqlServerDB))
            {
                _ParameterChar = "@";
                return "]";
            }
            else if (db.GetType() == typeof(MySqlDB))
            {
                _ParameterChar = "?";
                return "`";
            }
            return "";
        }

        protected void InitDbChar()
        {
            this._BeginChar = GetDbSpecChara(DbContext);
            this._EndChar = GetDbSpecCharb(DbContext);
        }

        protected virtual string GeneralInsert<T>(T t, ref IDbDataParameter[] paramArr) where T : BaseMap, new()
        {
            InitDbChar();
            IDictionary<string, object> dic = t.Serialize();
            string dBDatbelName = GetTableName(dic);
            string dbAutoField = GetAutoField(dic);
            string sqlColumn = "insert into " + _BeginChar + dBDatbelName + _EndChar + " (", sqlParam = "(";
            List<OleDbParameter> list = new List<OleDbParameter>();
            foreach (string item in dic.Keys)
            {
                if (item.StartsWith("_____")) continue;
                if (string.Compare(dbAutoField, item, true) == 0)
                {
                    continue;
                }

                sqlColumn += string.Format(_BeginChar + "{0}" + _EndChar + ",", item);
                sqlParam += string.Format(_ParameterChar + "{0},", item);
                OleDbParameter olp = new OleDbParameter(_ParameterChar + item, dic[item]);
                olp.OleDbType = Convert2DbType(dic[item]);
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

        protected virtual string GeneralUpdate<T>(T t, ref IDbDataParameter[] paraArr) where T : BaseMap, new()
        {
            InitDbChar();
            List<OleDbParameter> list = new List<OleDbParameter>();
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
                OleDbParameter olp = new OleDbParameter(_ParameterChar + item, dic[item]);
                olp.OleDbType = Convert2DbType(dic[item]);
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

        protected virtual string GeneralUpdate<T>(T t, string where, ref IDbDataParameter[] paraArr) where T : BaseMap, new()
        {
            InitDbChar();
            List<OleDbParameter> list = new List<OleDbParameter>();
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
                OleDbParameter olp = new OleDbParameter(_ParameterChar + item, dic[item]);
                olp.OleDbType = Convert2DbType(dic[item]);
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

        protected virtual string GeneralUpdate<T>(T t, string where, List<string> columns, ref IDbDataParameter[] paraArr) where T : BaseMap, new()
        {
            InitDbChar();
            List<OleDbParameter> list = new List<OleDbParameter>();
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
                OleDbParameter olp = new OleDbParameter(_ParameterChar + item, dic[item]);
                olp.OleDbType = Convert2DbType(dic[item]);
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

        protected OleDbType Convert2DbType(object val)
        {
            if (val == null)
            {
                return OleDbType.VarChar;
            }

            Type t = val.GetType();
            if (t == typeof(string)) return OleDbType.VarChar;
            if (t == typeof(decimal)) return OleDbType.Decimal;
            if (t == typeof(double)) return OleDbType.Double;
            if (t == typeof(float)) return OleDbType.Double;
            if (t == typeof(DateTime)) return OleDbType.Date;
            if (t == typeof(Int16)) return OleDbType.SmallInt;
            if (t == typeof(int)) return OleDbType.Integer;
            if (t == typeof(Int32)) return OleDbType.Integer;
            if (t == typeof(Int64)) return OleDbType.BigInt;
            return OleDbType.VarChar;
        }

        protected bool ContainsWithNoCase(List<string> list, string compareto)
        {
            if (string.IsNullOrEmpty(compareto)) return false;
            return list.Contains(compareto, StringComparer.CurrentCultureIgnoreCase);
        }

        protected virtual Dictionary<string, object> Reader2Dict(IDataReader reader)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            string name = "";
            for (int i = 0; i < reader.FieldCount; i++)
            {
                name = reader.GetName(i);
                dict[name] = reader[i];
            }
            return dict;
        }

        protected virtual Dictionary<string, object> Row2Dict(DataRow row)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            string name = "";
            foreach (DataColumn dc in row.Table.Columns)
            {
                name = dc.ColumnName;
                dict[name] = row[dc];
            }
            return dict;
        }

        #region T
        public virtual T Get<T>(IDataReader reader) where T : BaseMap, new()
        {
            using (reader)
            {
                if (reader.Read())
                {
                    Dictionary<string, object> dic = Reader2Dict(reader); ;
                    T t = Activator.CreateInstance<T>();
                    t.Deserialise(dic);
                    return t;
                }
            }
            return default(T);
        }

        public virtual T Get<T>(string sql, params IDbDataParameter[] param) where T : BaseMap, new()
        {
            IDataReader reader = null;
            using (reader = DbContext.GetReader(sql, param))
            {
                if (reader.Read())
                {
                    Dictionary<string, object> dic = Reader2Dict(reader); ;
                    T t = new T();// Activator.CreateInstance<T>();
                    t.Deserialise(dic);
                    return t;
                }
                reader.Close();
                reader.Dispose();
            }
            return default(T);
        }

        public List<T> GetList<T>(string sql, params IDbDataParameter[] sps) where T : BaseMap, new()
        {
            List<T> list = new List<T>();
            using (IDataReader reader = DbContext.GetReader(sql, sps))
            {
                while (reader.Read())
                {
                    Dictionary<string, object> dic = this.Reader2Dict(reader);
                    T t = new T();// Activator.CreateInstance<T>();
                    t.Deserialise(dic);
                    list.Add(t);
                }
                reader.Close();
                reader.Dispose();
            }
            return list;
        }

        public virtual int Insert<T>(T t) where T : BaseMap, new()
        {
            IDbDataParameter[] paramArr = null;
            string sql = GeneralInsert<T>(t, ref paramArr);

            return DbContext.ExecNonQuery(sql, paramArr);
        }

        public virtual int Update<T>(T t) where T : BaseMap, new()
        {
            IDbDataParameter[] paramArr = null;
            string sql = GeneralUpdate<T>(t, ref paramArr);
            return DbContext.ExecNonQuery(sql, paramArr);
        }

        public virtual int Update<T>(T t, string whereStr) where T : BaseMap, new()
        {
            IDbDataParameter[] paramArr = null;
            string sql = GeneralUpdate<T>(t, whereStr, ref paramArr);
            return DbContext.ExecNonQuery(sql, paramArr);
        }

        public virtual int Update<T>(T t, string whereStr, List<string> columns) where T : BaseMap, new()
        {
            IDbDataParameter[] paramArr = null;
            string sql = GeneralUpdate<T>(t, whereStr, columns, ref paramArr);
            Console.WriteLine(sql);
            Columns.Clear();
            return DbContext.ExecNonQuery(sql, paramArr);
        }

        public DbMap Include(string columnNames)
        {
            if (!Columns.Contains(columnNames))
                Columns.Add(columnNames);
            return this;
        }
        public List<string> Columns = new List<string>();

        #endregion

        #endregion


    }

}