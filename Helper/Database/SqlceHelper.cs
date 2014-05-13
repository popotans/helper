using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using System.Data;
using System.Data.Common;
using System.Web;
namespace Helper.Database
{
    public class SqlceHelper
    {
        private string connstr { get; set; }
        public SqlceHelper()
        {
            connstr = string.Format("data source={0}", HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.ConnectionStrings["sqlcestr"].ConnectionString));
        }
        public SqlceHelper(string connstr)
        {
            this.connstr = connstr;
        }

        public SqlCeConnection CreateConn()
        {
            SqlCeConnection conn = new SqlCeConnection(connstr);
            conn.Open();
            return conn;
        }

        public IDataReader ExecuteReader(string sql, params SqlCeParameter[] sps)
        {
            SqlCeConnection conn = CreateConn();
            SqlCeCommand cmd = new SqlCeCommand(sql);
            cmd.Connection = conn;
            if (sps != null)
            {
                cmd.Parameters.AddRange(sps);
            }
            IDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }

        public SqlCeParameter AddInParameter(string name, object val, SqlDbType type)
        {
            SqlCeParameter sp = new SqlCeParameter(name, val);
            sp.SqlDbType = type;
            return sp;

        }

        public void ExecuteNonQuery(string sql, params SqlCeParameter[] sps)
        {
            SqlCeConnection conn = CreateConn();
            SqlCeCommand cmd = new SqlCeCommand(sql);
            cmd.Connection = conn;
            if (sps != null)
            {
                cmd.Parameters.AddRange(sps);
            }
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public string ExecuteScalar(string sql, params SqlCeParameter[] sps)
        {
            SqlCeConnection conn = CreateConn();
            SqlCeCommand cmd = new SqlCeCommand(sql);
            cmd.Connection = conn;
            if (sps != null)
            {
                cmd.Parameters.AddRange(sps);
            }
            string r = "";
            object l = cmd.ExecuteScalar();
            if (l != null) r = l.ToString();
            cmd.Connection.Close();
            return r;
        }

        public DataTable ExecuteDatatable(string sql, params SqlCeParameter[] sps)
        {
            DataTable dt = new DataTable();
            SqlCeDataAdapter adp = new SqlCeDataAdapter();
            SqlCeConnection conn = CreateConn();
            SqlCeCommand cmd = new SqlCeCommand(sql);
            if (sps != null) cmd.Parameters.AddRange(sps);
            cmd.Connection = conn;
            adp.SelectCommand = cmd;
            adp.Fill(dt);
            cmd.Connection.Close();
            return dt;
        }

        public DataTable Pageing(string table, string where, string orderby, int page, int pagesize, out int pagecount, out int recordcount)
        {
            pagecount = recordcount = 0;
            DataTable dt = null;
            string sqlcount = "select count(1) from " + table + "  " + where;
            string recordcountstr = ExecuteScalar(sqlcount);
            int.TryParse(recordcountstr, out recordcount);
            int _tmp = recordcount % pagesize;
            pagecount = _tmp == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
            string sql = "";
            string strw = string.Format(" {0} {1} {2} ", table, where, orderby);
            if (page == 1)
            {
                sql = string.Format(" select top {0} * from  {1}", pagesize.ToString(), strw);
                dt = ExecuteDatatable(sql);
            }
            else
            {
                int skips = (page - 1) * pagesize;
                string sqlminid = string.Format("select min([id]) from( select top {0} [id] from {1} ) t", skips.ToString(), strw);
                string minid = ExecuteScalar(sqlminid);
                sql = string.Format(" select top {0} * from  {1} {2} {3}", pagesize.ToString(), table, " where [id]< " + minid, orderby);
                dt = ExecuteDatatable(sql);
            }
            return dt;
        }

    }
}
