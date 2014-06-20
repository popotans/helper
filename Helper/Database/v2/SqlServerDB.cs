using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace Helper
{
    public class SqlServerDB : IDb
    {
        // public AccessDB() { }
        public SqlServerDB(string connStr)
        {
            this.ConnStr = connStr;
        }
        public System.Data.IDataReader GetReader(string sql, params System.Data.IDataParameter[] p)
        {
            SqlConnection conn = GetConn();
            SqlCommand cmd = new SqlCommand(sql);
            if (p != null)
                cmd.Parameters.AddRange(p);
            cmd.Connection = conn;
            conn.Open();
            IDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            //   cmd.Connection.Close();
            return dr;
        }

        public IDataReader GetReader(DbConnection conn, string sql, params IDataParameter[] p)
        {
            SqlCommand cmd = new SqlCommand(sql);
            if (p != null)
                cmd.Parameters.AddRange(p);
            cmd.Connection = (SqlConnection)conn;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            IDataReader dr = cmd.ExecuteReader();
            //   cmd.Connection.Close();
            return dr;
        }

        public System.Data.DataTable GetTable(string sql, params System.Data.IDataParameter[] p)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConn())
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                if (p != null)
                    cmd.Parameters.AddRange(p);
                SqlDataAdapter sdap = new SqlDataAdapter(cmd);
                sdap.Fill(dt);
            }
            return dt;
        }

        public int ExecNonQuery(string sql, params System.Data.IDataParameter[] p)
        {
            SqlConnection conn = GetConn();
            SqlCommand cmd = new SqlCommand(sql);
            if (p != null)
                cmd.Parameters.AddRange(p);
            cmd.Connection = conn;
            conn.Open();
            int i = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return i;
        }

        public string ConnStr
        {
            get
           ;
            set;
        }

        public SqlConnection GetConn()
        {
            return new SqlConnection(this.ConnStr);
        }

        public IDbDataParameter GetParam(string name, object val, object t)
        {
            SqlParameter p = new SqlParameter(name, val);
            p.SqlDbType = (SqlDbType)t;
            return p;
        }

        public IDbDataParameter[] GetParams(List<string> names, List<object> vals, List<object> t)
        {
            IDbDataParameter[] arr = new SqlParameter[names.Count];
            //  IDbDataParameter[] psa=new OleDbParameter[]();
            if (names.Count != vals.Count && names.Count != t.Count) throw new ApplicationException("参数不匹配");
            for (int i = 0; i < names.Count; i++)
            {
                arr[i] = GetParam(names[i], vals[i], t[i]);
            }
            return arr;
        }

        public IDbDataParameter[] GetParams(Dictionary<string, object> dic, List<object> vals)
        {
            IDbDataParameter[] arr = new SqlParameter[dic.Count];
            int i = 0;
            foreach (KeyValuePair<string, object> item in dic)
            {
                arr[i] = GetParam(item.Key, vals[i], item.Value);
                i++;
            }
            return arr;
        }


        public object ExecScalar(string sql, params IDataParameter[] p)
        {
            SqlConnection conn = GetConn();
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = conn;
            conn.Open();
            object obj = cmd.ExecuteScalar();
            cmd.Connection.Close();
            if (obj != null && obj != DBNull.Value) return obj;
            return null;
        }
        public int ExecScalarInt(string sql, params IDataParameter[] p)
        {
            object obj = ExecScalar(sql, p);
            if (obj == null) return -1;
            return int.Parse(obj.ToString());
        }

        public int ExecNonQuery(DbConnection conn, string sql, params IDataParameter[] p)
        {
            //  HttpContext.Current.Response.End();
            DbCommand cmd = new SqlCommand(sql);
            if (p != null)
                cmd.Parameters.AddRange(p);
            cmd.Connection = conn;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            int i = cmd.ExecuteNonQuery();
            //  cmd.Connection.Close();

            return i;
        }

        public int ExecScalarInt(DbConnection conn, string sql, params IDataParameter[] p)
        {
            object obj = ExecScalar(conn, sql, p);
            if (obj == null) return -1;
            return int.Parse(obj.ToString());
        }


        public object ExecScalar(DbConnection conn, string sql, params IDataParameter[] p)
        {
            DbCommand cmd = new SqlCommand(sql);
            cmd.Connection = conn;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            object obj = cmd.ExecuteScalar();
            //cmd.Connection.Close();
            if (obj != null && obj != DBNull.Value) return obj;
            return null;
        }


        DbConnection IDb.GetConn()
        {
            return GetConn();
        }

        public DbConnection GetConn(string connStr)
        {
            return new SqlConnection(connStr);
        }

        //public SqlDbType ConvertType(string type)
        //{
        //    if (CompareNoCase(type, "int")) return OleDbType.Integer;
        //    else if (CompareNoCase(type, "bigint")) return OleDbType.BigInt;
        //    else if (CompareNoCase(type, "string")) return OleDbType.VarChar;
        //    else if (CompareNoCase(type, "double")) return OleDbType.Double;
        //    else if (CompareNoCase(type, "float")) return OleDbType.Double;
        //    else if (CompareNoCase(type, "decimal")) return OleDbType.Decimal;
        //    else if (CompareNoCase(type, "date")) return OleDbType.Date;

        //    return OleDbType.VarChar;
        //}

        private bool CompareNoCase(string s1, string s2)
        {
            return string.Compare(s1, s2, true) == 0;
        }

        private object GetPropertyValue<T>(T t, string propertyname)
        {
            System.Reflection.PropertyInfo[] arrPi = typeof(T).GetProperties();
            Type type = null;
            foreach (PropertyInfo pi in arrPi)
            {
                if (CompareNoCase(pi.Name, propertyname))
                {
                    type = pi.GetType();
                    return pi.GetValue(t, null);
                }
            }
            if (type != null)
            {
                if (type == typeof(string)) { return ""; }
                if (type == typeof(DateTime)) { return new DateTime(1970, 1, 1); }
            }
            return "";
        }


        public IDbDataParameter[] GetParams(Dictionary<string, object> dic)
        {
            IDbDataParameter[] arr = new SqlParameter[dic.Count];
            int i = 0;
            foreach (KeyValuePair<string, object> item in dic)
            {
                arr[i] = GetParam(item.Key, item.Value);
                i++;
            }
            return arr;
        }

        public IDbDataParameter GetParam(string name, object val)
        {
            return new SqlParameter(name, val);
        }
    }
}
