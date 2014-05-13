using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
namespace Helper.Database
{
    public class AccessHelper
    {

        //  protected OleDbCommand comm = new OleDbCommand();
        public string dbpath = "";

        public AccessHelper()
        {
            dbpath = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["accessstr"]);
        }
        /// <summary>
        /// 传入绝对路径
        /// </summary>
        /// <param name="path"></param>
        public AccessHelper(string path)
        {
            this.dbpath = path;
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        public OleDbConnection CreateConnection()
        {
            OleDbConnection conn = null;
            conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + dbpath;
            try
            {
                conn.Open();
            }
            catch (Exception e)
            { throw new Exception(e.Message); }

            return conn;
        }

        public OleDbConnection CreateConnection(string dbpath)
        {
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + dbpath;
            try
            {
                conn.Open();
            }
            catch (Exception e)
            { throw new Exception(e.Message); }

            return conn;
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        private void closeConnection(OleDbConnection conn)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sqlstr"></param>
        public void ExecuteNonQuery(string sqlstr)
        {
            OleDbCommand comm = new OleDbCommand();
            OleDbConnection conn = CreateConnection();
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                comm.Connection = conn;
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            { closeConnection(conn); }
        }


        public int ExecuteNonQuery(string sql, OleDbParameter[] ops)
        {
            int i = 0;
            OleDbCommand comm = new OleDbCommand();
            OleDbConnection conn = CreateConnection();
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = sql;
                comm.Connection = conn;

                if (ops != null)
                {
                    foreach (OleDbParameter p in ops)
                    {
                        comm.Parameters.Add(p);
                    }
                }
                i = comm.ExecuteNonQuery();
                comm.Parameters.Clear();
                return i;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            { closeConnection(conn); }
        }

        public void ExecuteNonQuery(OleDbConnection conn, string sql, OleDbParameter[] ops)
        {
            OleDbCommand comm = new OleDbCommand();
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = sql;
                comm.Connection = conn;
                if (ops != null)
                {
                    comm.Parameters.AddRange(ops);
                }
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                conn.Close();
                throw new Exception(e.Message);
            }
        }
        public void ExecuteNonQuery(OleDbTransaction trans, string sql, OleDbParameter[] ops)
        {
            OleDbCommand comm = new OleDbCommand();
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = sql;
                comm.Connection = trans.Connection;
                if (ops != null)
                {
                    comm.Parameters.AddRange(ops);
                }
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                trans.Connection.Close();
                throw new Exception(e.Message);
            }
        }


        public OleDbParameter BuildInParameter(string name, object value, OleDbType type)
        {
            OleDbParameter p = new OleDbParameter(name, value);
            p.OleDbType = type;
            return p;
        }

        public IDataReader ExecuteReader(string sql, OleDbParameter[] ops)
        {
            OleDbCommand comm = new OleDbCommand();
            OleDbConnection conn = CreateConnection();
            OleDbDataReader dr = null;
            try
            {
                comm.Connection = conn;
                comm.CommandText = sql;
                comm.CommandType = CommandType.Text;
                if (ops != null)
                {
                    comm.Parameters.AddRange(ops);
                }
                dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                try
                {
                    dr.Close();
                    closeConnection(conn);
                }
                catch { }
            }
            return dr;
        }

        public string ExecuteScalar(string sqlstr)
        {
            string s = string.Empty;
            OleDbCommand cmd = new OleDbCommand(sqlstr);
            cmd.CommandType = CommandType.Text;
            OleDbConnection conn = CreateConnection();
            cmd.Connection = conn;
            object obj = cmd.ExecuteScalar();
            if (null != obj) s = obj.ToString();
            cmd.Connection.Close();
            return s;
        }
        public string ExecuteScalar(string sqlstr, OleDbParameter[] ops)
        {
            string s = null;
            OleDbCommand cmd = new OleDbCommand(sqlstr);
            cmd.CommandType = CommandType.Text;
            OleDbConnection conn = CreateConnection();
            cmd.Connection = conn;
            if (ops != null && ops.Length > 0)
            {
                cmd.Parameters.AddRange(ops);
            }
            object obj = cmd.ExecuteScalar();
            if (null != obj && obj != DBNull.Value) s = obj.ToString();
            cmd.Parameters.Clear();
            cmd.Connection.Close();
            return s;
        }


        public string ExecuteScalar(OleDbConnection conn, string sqlstr)
        {
            string s = string.Empty;
            OleDbCommand cmd = new OleDbCommand(sqlstr);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            object obj = cmd.ExecuteScalar();
            if (null != obj) s = obj.ToString();
            return s;
        }
        public int ExecuteSql(string sql, OleDbParameter[] ops)
        {
            return ExecuteNonQuery(sql, ops);
        }

        #region  常用的
        public bool Exists(string sql, OleDbParameter[] ops)
        {
            return !string.IsNullOrEmpty(ExecuteScalar(sql, ops));
        }

        /// <summary>
        /// 返回指定sql语句的datatable
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sqlstr)
        {
            OleDbCommand comm = new OleDbCommand();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection conn = CreateConnection();
            try
            {
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection(conn);
            }
            return dt;
        }

        #endregion



        /// <summary>
        /// 返回指定sql语句的OleDbDataReader对象，使用时请注意关闭这个对象。
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string sqlstr)
        {
            OleDbCommand comm = new OleDbCommand();
            OleDbConnection conn = CreateConnection();
            OleDbDataReader dr = null;
            try
            {
                comm.Connection = conn;
                comm.CommandText = sqlstr;
                comm.CommandType = CommandType.Text;
                dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                try
                {
                    dr.Close();
                    closeConnection(conn);
                }
                catch { }
            }
            return dr;
        }

        public DbDataReader ExecuteReader(OleDbConnection conn, string sqlstr)
        {
            OleDbCommand comm = new OleDbCommand();
            OleDbDataReader dr = null;
            try
            {
                comm.Connection = conn;
                comm.CommandText = sqlstr;
                comm.CommandType = CommandType.Text;
                dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
            }
            return dr;
        }
        public DataTable ExecuteDataTable(string sql, OleDbParameter[] ops)
        {
            OleDbCommand comm = new OleDbCommand();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter();
            OleDbConnection conn = CreateConnection();
            try
            {
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = sql;
                if (ops != null)
                {
                    comm.Parameters.AddRange(ops);
                }
                da.SelectCommand = comm;
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection(conn);
            }
            return dt;
        }

        public DataSet Query(string sql, OleDbParameter[] ops)
        {
            DataTable dt = ExecuteDataTable(sql, ops);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt); return ds;

        }

        public DataSet Query(string sql)
        {
            DataTable dt = ExecuteDataTable(sql);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt); return ds;

        }

        public DataTable ExecuteDataTable(OleDbConnection conn, string sql, OleDbParameter[] ops)
        {
            OleDbCommand comm = new OleDbCommand();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = sql;
                if (ops != null)
                {
                    comm.Parameters.AddRange(ops);
                }
                da.SelectCommand = comm;
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {

            }
            return dt;
        }

        /// <summary>
        /// 内置where和order
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="table"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderType"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="pagecount"></param>
        /// <param name="recordcount"></param>
        /// <returns></returns>
        public DataTable Paging(string fields, string table, string strWhere, string orderType, int pageSize, int currentPage, out int pagecount, out int recordcount)
        {
            return Paging(fields, table, strWhere, "id", orderType, pageSize, currentPage, out pagecount, out recordcount);
        }

        /// <summary>
        /// 获取完整分页模型
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="table"></param>
        /// <param name="strWhere"></param>
        /// <param name="orderkey"></param>
        /// <param name="orderType"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageModel GetPageModel(string fields, string table, string strWhere, string orderkey, string orderType, int currentPage, int pageSize)
        {
            PageModel pm = new PageModel();
            pm.Pagesize = pageSize;
            pm.Page = currentPage;
            string sql = string.Format(" select count(1) from  {0}", table);
            if (!string.IsNullOrEmpty(strWhere)) { sql = string.Format("{0} where  {1}", sql, strWhere); }
            object obj = ExecuteScalar(sql);
            if (obj != null) { pm.TotalRecord = int.Parse(obj.ToString()); }
            pm.List = Paging(fields, table, strWhere, orderkey, orderType, currentPage, pageSize);
            return pm;
        }


        public DataTable Paging(string fields, string table, string strWhere, string orderkey, string orderType, int currentPage, int pageSize)
        {
            StringBuilder strSql = new StringBuilder();
            if (currentPage != 1)
            {
                int topNum = pageSize * (currentPage - 1);
                strSql.AppendFormat("select top " + pageSize + " " + fields + " from " + table + " ");
                if (string.Compare(orderType, "desc", true) == 0)
                    strSql.Append(" where " + orderkey + " <( select min(" + orderkey + ") from( select top " + topNum + " " + orderkey + " from  " + table + "");
                else
                    strSql.Append(" where " + orderkey + " >( select max(" + orderkey + ") from( select top " + topNum + " " + orderkey + " from  " + table + "");
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.Append(" where " + strWhere);
                }
                strSql.AppendFormat(" order by {0} {1}))", orderkey, orderType);
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.AppendFormat(" and {0}", strWhere);
                }
                //5%1+a+s+p+x
                strSql.AppendFormat(" order by  {0} {1}", orderkey, orderType);
            }
            else if (currentPage == 1)
            {
                strSql.AppendFormat("select top {0} {1} from  {2}", pageSize, fields, table);
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.AppendFormat(" where {0}", strWhere);
                }
                strSql.AppendFormat(" order by {0} {1}", orderkey, orderType);
            }

            return ExecuteDataTable(strSql.ToString());
        }

        /// <summary>
        /// 获得查询分页数据,必须包含数字类型自增ID
        /// 内置where和order
        /// </summary>
        public DataTable Paging(string fields, string table, string strWhere, string orderkey, string orderType, int pageSize, int currentPage, out int pagecount, out int recordcount)
        {
            if (currentPage < 1) currentPage = 1;
            pagecount = recordcount = 0;
            string sql = " select count(1) from  " + table;
            if (!string.IsNullOrEmpty(strWhere)) { sql += " where  " + strWhere; }
            object obj = ExecuteScalar(sql);
            if (obj != null) { recordcount = int.Parse(obj.ToString()); } else { recordcount = 0; }
            pagecount = (int)Math.Ceiling((double)recordcount / (double)pageSize);
            StringBuilder strSql = new StringBuilder();
            //select top 10 * from t  where order by id desc 
            /*
             where id< ( select min(id) from ( select top20 * from t where order by id desc) )
             * order by id desc 
             */

            if (currentPage != 1)
            {
                int topNum = pageSize * (currentPage - 1);
                strSql.AppendFormat("select top " + pageSize + " " + fields + " from " + table + " ");
                if (string.Compare(orderType, "desc", true) == 0)
                    strSql.Append(" where " + orderkey + " <( select min(" + orderkey + ") from( select top " + topNum + " " + orderkey + " from  " + table + "");
                else
                    strSql.Append(" where " + orderkey + " >( select max(" + orderkey + ") from( select top " + topNum + " " + orderkey + " from  " + table + "");
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.Append(" where " + strWhere);
                }
                strSql.AppendFormat(" order by {0} {1}))", orderkey, orderType);
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.AppendFormat(" and {0}", strWhere);
                }
                //5%1+a+s+p+x
                strSql.AppendFormat(" order by  {0} {1}", orderkey, orderType);
            }
            else if (currentPage == 1)
            {
                strSql.AppendFormat("select top {0} {1} from  {2}", pageSize, fields, table);
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.AppendFormat(" where {0}", strWhere);
                }
                strSql.AppendFormat(" order by {0} {1}", orderkey, orderType);
            }

            return ExecuteDataTable(strSql.ToString());
        }

        /// <summary>
        /// 获得查询分页数据,三次top反转法,
        /// 内置where和order
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="pagesize"></param>
        /// <param name="fields"></param>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <param name="recordcount"></param>
        /// <param name="pagecount"></param>
        /// <returns></returns>
        public DataTable Paging(int pno, int pagesize, string fields, string table,
            string where, string orderby,
            out int recordcount, out int pagecount)
        {
            where = " where  " + where;
            orderby = " order by  " + orderby;
            if (string.IsNullOrEmpty(fields)) fields = " * ";
            recordcount = pagecount = 0;
            DataTable dt = new DataTable();

            //计算总记录数
            string sql = "select count(1) as c from " + table + "  " + where + "  " + " ";

            string _recordcount = ExecuteScalar(sql);
            //  HttpContext.Current.Response.Write(sql);
            //HttpContext.Current.Response.End();
            int.TryParse(_recordcount, out recordcount);
            //计算页数
            int _tmp = recordcount % pagesize;
            int pages = _tmp == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
            pagecount = pages;
            string execSql = "";
            if (pno == 1)
            {
                execSql = " select top " + pagesize + " " + fields + " from  " + table + " " + where + "  " + orderby + " desc";
            }
            else
            {
                int index = (pno) * pagesize;
                execSql = " select top " + index + "  " + fields + " from  " + table + " " + where + "  " + orderby + " desc";
                execSql = "select top " + pagesize + " * from  (" + execSql + ") as t1 " + orderby + " asc ";
                execSql = " select  * from (" + execSql + ")as t2 " + orderby + " desc";
            }
            dt = ExecuteDataTable(execSql);
            return dt;
        }
        /// <summary>
        /// 分线查询，直接传入sql，采用总记录数减去前几页数量计算
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="pagesize"></param>
        /// <param name="sql"></param>
        /// <param name="recordcount"></param>
        /// <param name="pagecount"></param>
        /// <returns></returns>
        public DataTable Paging(int pno, int pagesize, string sql, out int recordcount, out int pagecount)
        {
            recordcount = pagecount = 0;
            string sqlrecordcount = "select count(1) from (" + sql + ") as t1";
            string _recordcount = ExecuteScalar(sqlrecordcount);
            int.TryParse(_recordcount, out recordcount);
            //计算页数
            int _tmp = recordcount % pagesize;
            int pages = _tmp == 0 ? recordcount / pagesize : recordcount / pagesize + 1;
            pagecount = pages;
            string execSql = "";
            DataTable dt = null;
            if (pno == 1)
            {
                execSql = " select top " + pagesize + " * from  (" + sql + ") as t1";
                dt = ExecuteDataTable(execSql);
            }
            else
            {
                // int index = recordcount - (pno - 1) * pagesize;
                execSql = " select top " + pno * pagesize + " * from (" + sql + ") as t1";
                DataTable _dt = ExecuteDataTable(execSql);
                dt = new DataTable();
                foreach (DataColumn dc in _dt.Columns)
                {
                    DataColumn d = new DataColumn()
                    {
                        DataType = dc.DataType,
                        ColumnName = dc.ColumnName
                    };
                    dt.Columns.Add(d);
                }
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    if (i <= (pno - 1) * pagesize) continue;
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dr[dc.ColumnName] = _dt.Rows[i][dc.ColumnName];
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }

        /// <summary>
        /// 这个不建议使用了效率不行
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <param name="queryViewOrTable"></param>
        /// <param name="whereString"></param>
        /// <param name="orderString"></param>
        /// <param name="uniqueKey"></param>
        /// <param name="uniqueKeyType"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <param name="outSql"></param>
        /// <returns></returns>
        public DataTable ExecutePager(int pageIndex, int pageSize, string fields, string queryViewOrTable, string whereString, string orderString, string uniqueKey, Type uniqueKeyType, out int pageCount, out int recordCount, ref string outSql)
        {
            string sql = "";
            OleDbConnection conn = CreateConnection();
            DataTable dt = null;
            try
            {
                string table = string.Format(" ({0})  ", queryViewOrTable);

                #region calc total record
                string ss = string.Format(" select count(1) from {0} {1}", table, whereString);

                OleDbCommand cmdCount = new OleDbCommand(ss, conn);
                //总记录
                recordCount = Convert.ToInt32(cmdCount.ExecuteScalar());
                #endregion

                if ((recordCount % pageSize) > 0)
                    pageCount = recordCount / pageSize + 1;
                else
                    pageCount = recordCount / pageSize;

                if (pageIndex < 1) pageIndex = 1;
                if (pageSize < 1) pageSize = 10;
                if (string.IsNullOrEmpty(fields)) fields = "*";
                if (string.IsNullOrEmpty(orderString)) orderString = " ID desc";

                OleDbCommand cmd;
                if (pageIndex == 1)//第一页
                {
                    sql = string.Format("select top {0} {1} from {2} {3} order by {4} ", pageSize, fields, table, whereString, orderString);
                    cmd = new OleDbCommand(sql, conn);
                }
                else if (pageIndex > pageCount)//超出总页数
                {
                    sql = string.Format("select top {0} {1} from {2} {3} order by {4} ", pageSize, fields, table, "where 1=2", orderString);
                    cmd = new OleDbCommand(sql, conn);
                }
                else
                {
                    int begin = pageSize * pageIndex;
                    int skipped = begin - pageSize;
                    string sqlRecordIds = string.Format("select top {0} {1} from {2} {3} order by {4} ", begin, uniqueKey, table, whereString, orderString);
                    string recordIDs = string.Empty;
                    #region calc recordids ,这里可根据当前页在总页数中的比重进一步优化
                    cmd = new OleDbCommand(sqlRecordIds, conn);
                    StringBuilder result = new StringBuilder();
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (skipped < 1)
                            {
                                if (uniqueKeyType == typeof(int))
                                    result.AppendFormat(",{0}", dr.GetInt32(0).ToString());
                                else if (uniqueKeyType == typeof(string))
                                    result.AppendFormat(",'{0}'", dr[0].ToString());
                            }
                            skipped--;
                        }
                        if (!dr.IsClosed) { dr.Close(); }
                    }
                    string s = result.ToString();
                    recordIDs = s.Length > 0 ? s.Substring(1) : s;
                    #endregion
                    sql = string.Format("select {0} from {1} where {4} in ({2}) order by {3} ", fields, table, recordIDs, orderString, uniqueKey);
                    cmd = new OleDbCommand(sql, conn);
                }
                outSql = sql;
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                dataAdapter.Fill(dt);
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
            return dt;
        }
        /// <summary>
        /// 效率有问题不建议使用了
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <param name="queryViewOrTable"></param>
        /// <param name="whereString"></param>
        /// <param name="orderString"></param>
        /// <param name="uniqueKey"></param>
        /// <param name="uniqueKeyType"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <param name="outSql"></param>
        /// <returns></returns>
        public DataTable ExecutePager(OleDbConnection conn, int pageIndex, int pageSize, string fields, string queryViewOrTable, string whereString, string orderString, string uniqueKey, Type uniqueKeyType, out int pageCount, out int recordCount, ref string outSql)
        {
            string sql = "";
            DataTable dt = null;
            try
            {
                string table = string.Format(" ({0})  ", queryViewOrTable);

                #region calc total record
                string ss = string.Format(" select count(1) from {0} {1}", table, whereString);

                OleDbCommand cmdCount = new OleDbCommand(ss, conn);
                //总记录
                recordCount = Convert.ToInt32(cmdCount.ExecuteScalar());
                #endregion

                if ((recordCount % pageSize) > 0)
                    pageCount = recordCount / pageSize + 1;
                else
                    pageCount = recordCount / pageSize;

                if (pageIndex < 1) pageIndex = 1;
                if (pageSize < 1) pageSize = 10;
                if (string.IsNullOrEmpty(fields)) fields = "*";
                if (string.IsNullOrEmpty(orderString)) orderString = " ID desc";

                OleDbCommand cmd;
                if (pageIndex == 1)//第一页
                {
                    sql = string.Format("select top {0} {1} from {2} {3} order by {4} ", pageSize, fields, table, whereString, orderString);
                    cmd = new OleDbCommand(sql, conn);
                }
                else if (pageIndex > pageCount)//超出总页数
                {
                    sql = string.Format("select top {0} {1} from {2} {3} order by {4} ", pageSize, fields, table, "where 1=2", orderString);
                    cmd = new OleDbCommand(sql, conn);
                }
                else
                {
                    int begin = pageSize * pageIndex;
                    int skipped = begin - pageSize;
                    string sqlRecordIds = string.Format("select top {0} {1} from {2} {3} order by {4} ", begin, uniqueKey, table, whereString, orderString);
                    string recordIDs = string.Empty;
                    #region calc recordids ,这里可根据当前页在总页数中的比重进一步优化
                    cmd = new OleDbCommand(sqlRecordIds, conn);
                    StringBuilder result = new StringBuilder();
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (skipped < 1)
                            {
                                if (uniqueKeyType == typeof(int))
                                    result.AppendFormat(",{0}", dr.GetInt32(0).ToString());
                                else if (uniqueKeyType == typeof(string))
                                    result.AppendFormat(",'{0}'", dr[0].ToString());
                            }
                            skipped--;
                        }
                        if (!dr.IsClosed) { dr.Close(); }
                    }
                    string s = result.ToString();
                    recordIDs = s.Length > 0 ? s.Substring(1) : s;
                    #endregion
                    sql = string.Format("select {0} from {1} where {4} in ({2}) order by {3} ", fields, table, recordIDs, orderString, uniqueKey);
                    cmd = new OleDbCommand(sql, conn);
                }
                outSql = sql;
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
                dt = new DataTable();
                dataAdapter.Fill(dt);
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally { conn.Close(); }
            return dt;
        }




        #region static method
        public static string GetConnStr(string dbPath)
        {
            return string.Format(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source={0}", dbPath);
        }

        public static OleDbConnection GetConnection(string dbpath)
        {
            return new OleDbConnection(GetConnStr(dbpath));
        }
        public static DataTable ExecuteDataTable(string dbPath, string sql, params OleDbParameter[] ops)
        {
            DataTable dt = new DataTable();
            OleDbConnection conn = GetConnection(dbPath);
            try
            {
                OleDbCommand comm = new OleDbCommand();
                comm.Connection = conn;
                comm.CommandType = CommandType.Text;
                comm.CommandText = sql;
                if (ops != null)
                {
                    comm.Parameters.AddRange(ops);
                }
                OleDbDataAdapter da = new OleDbDataAdapter();
                da.SelectCommand = comm;
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
               // System.Web.HttpContext.Current.Response.Write(conn.State);
            }
            return dt;
        }

        public int ExecuteNonQuery(string dbPath, string sql, params OleDbParameter[] ops)
        {
            int rs = -1;
            OleDbCommand comm = new OleDbCommand();
            OleDbConnection conn = GetConnection(dbPath);
            try
            {
                comm.CommandType = CommandType.Text;
                comm.CommandText = sql;
                comm.Connection = conn;
                if (ops != null) comm.Parameters.AddRange(ops);
                if (comm.Connection.State != ConnectionState.Open)
                    comm.Connection.Open();
                rs = comm.ExecuteNonQuery();
                if (comm.Connection.State != ConnectionState.Closed)
                    comm.Connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (comm.Connection.State != ConnectionState.Closed)
                    comm.Connection.Close();
            }

            return rs;
        }
        public static string ExecuteScalar(string dbPath, string sql, params OleDbParameter[] ops)
        {
            string s = string.Empty;
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.CommandType = CommandType.Text;

            OleDbConnection conn = GetConnection(dbPath);
            cmd.Connection = conn;
            try
            {
                if (ops != null) cmd.Parameters.AddRange(ops);
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                object obj = cmd.ExecuteScalar();
                if (null != obj && obj != DBNull.Value) s = obj.ToString();
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
            return s;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="dbPath">数据库绝对路径</param>
        /// <param name="fields">要插曲的字段 * </param>
        /// <param name="table">tablename</param>
        /// <param name="strWhere">id=1 and name='njh'</param>
        /// <param name="orderkey">idx</param>
        /// <param name="orderType">desc </param>
        /// <param name="pageSize">20</param>
        /// <param name="currentPage">1</param>
        /// <param name="pagecount"></param>
        /// <param name="recordcount"></param>
        /// <returns></returns>
        public static DataTable Paging(string dbPath, string fields, string table, string strWhere, string orderkey, string orderType, int pageSize, int currentPage, out int pagecount, out int recordcount)
        {
            if (currentPage < 1) currentPage = 1;
            pagecount = recordcount = 0;

            string sql = string.Format(" select count(1) from  {0} {1}", table, string.IsNullOrEmpty(strWhere) ? strWhere : string.Format(" where ", strWhere));
            string obj = ExecuteScalar(dbPath, sql);

            if (!string.IsNullOrEmpty(obj)) { recordcount = int.Parse(obj.ToString()); } else { recordcount = 0; }
            pagecount = (int)Math.Ceiling((double)recordcount / (double)pageSize);
            StringBuilder strSql = new StringBuilder();
            //select top 10 * from t  where order by id desc 
            /*
             where id< ( select min(id) from ( select top20 * from t where order by id desc) )
             * order by id desc 
             */

            if (currentPage != 1)
            {
                int topNum = pageSize * (currentPage - 1);
                strSql.AppendFormat("select top {0} {1} from {2}", pageSize, fields, table);
                if (string.Compare(orderType, "desc", true) == 0)
                    strSql.AppendFormat(" where {0} <( select min({1}) from( select top {2} {3} from {4} ", orderkey, orderkey, topNum, orderkey, table);
                else
                    strSql.AppendFormat(" where {0} >( select max({1}) from( select top {2} {3} from {4} ", orderkey, orderkey, topNum, orderkey, table);
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.AppendFormat(" where {0} ", strWhere);
                }
                strSql.AppendFormat(" order by {0} {1}))", orderkey, orderType);
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.AppendFormat(" and {0}", strWhere);
                }
                //5%1+a+s+p+x
                strSql.AppendFormat(" order by  {0} {1}", orderkey, orderType);
            }
            else if (currentPage == 1)
            {
                strSql.AppendFormat("select top {0} {1} from  {2}", pageSize, fields, table);
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.AppendFormat(" where {0}", strWhere);
                }
                strSql.AppendFormat(" order by {0} {1}", orderkey, orderType);
            }

            return ExecuteDataTable(dbPath, strSql.ToString());
        }


        public static PageModel Paging(string dbPath, string fields, string table, string strWhere, string orderkey, string orderType, int pageSize, int currentPage)
        {
            int pagecount = 0, recordcount = 0;
            DataTable dt = Paging(dbPath, fields, table, strWhere, orderkey, orderType, pageSize, currentPage, out pagecount, out recordcount);

            PageModel pm = new PageModel();
            pm.Page = currentPage;
            pm.TotalRecord = recordcount;
            pm.Pagesize = pageSize;
            pm.List = dt;
            return pm;
        }

        #endregion


    }
}
