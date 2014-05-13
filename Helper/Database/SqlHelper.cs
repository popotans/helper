using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Helper.DbDataType;

namespace Helper.Database
{
    public class SqlHelperContext
    {
        public string ConnStr { get; set; }
        public SqlConnection Conn { get; set; }
    }


    /// <summary>
    /// 数据库的通用访问代码
    /// 此类为抽象类，不允许实例化，在应用时直接调用即可
    /// </summary>
    public class SqlHelper
    {
        //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
        private string ConnectionStringLocalTransaction = "";

        public SqlHelper(string connstr)
        {
            this.ConnectionStringLocalTransaction = connstr;
        }

        public SqlConnection CreateConnection(string connstr)
        {
            SqlConnection conn = new SqlConnection(connstr);
            return conn;
        }

        // 哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数。
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        ///执行一个不需要返回值的SqlCommand命令，通过指定专用的连接字符串。
        /// 使用参数数组形式提供参数列表 
        /// </summary>
        /// <remarks>
        /// 使用示例：
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                //清空SqlCommand中的参数列表
                cmd.Parameters.Clear();
                return val;
            }
        }
        public int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(this.ConnectionStringLocalTransaction, CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        ///执行一条不返回结果的SqlCommand，通过一个已经存在的数据库连接 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个现有的数据库连接</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行一条不返回结果的SqlCommand，通过一个已经存在的数据库事物处理 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">一个存在的 sql 事物处理</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// 执行一条返回结果集的SqlCommand命令，通过专用的连接字符串。
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个包含结果的SqlDataReader</returns>
        public SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // 在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                throw;
            }
            finally
            {
                try
                {
                    conn.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] commandParameters)
        {
            CommandType cmdType = CommandType.Text;
            string connectionString = this.ConnectionStringLocalTransaction;
            return ExecuteReader(connectionString, cmdType, cmdText, commandParameters);
        }

        public DataTable ExecuteDataTable(SqlConnection conn, string sql)
        {
            // 在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
            //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
            //关闭数据库连接，并通过throw再次引发捕捉到的异常。
            DataTable dt = new DataTable();
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlDataAdapter adap = new SqlDataAdapter(sql, conn);
                adap.Fill(dt);
            }
            catch
            {
                throw;
            }
            finally { conn.Close(); }
            return dt;
        }

        public DataTable ExecuteDataTable(string sql)
        {
            SqlConnection conn = new SqlConnection(ConnectionStringLocalTransaction);
            return ExecuteDataTable(conn, sql);

        }

        /// <summary>
        /// 执行一条返回第一条记录第一列的SqlCommand命令，通过专用的连接字符串。 
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例：  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            object obj = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                obj = ExecuteScalar(connection, cmdType, cmdText, commandParameters);
            }
            return obj;
        }

        /// <summary>
        /// 执行一条返回第一条记录第一列的SqlCommand命令，通过已经存在的数据库连接。
        /// 使用参数数组提供参数
        /// </summary>
        /// <remarks>
        /// 使用示例： 
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">一个已经存在的数据库连接</param>
        /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
        public object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
            return val;
        }

        public object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(this.ConnectionStringLocalTransaction, CommandType.Text, cmdText, commandParameters);
        }


        public bool Exists(SqlConnection conn, string sql)
        {
            return null != ExecuteScalar(conn, CommandType.Text, sql);
        }
        public bool Exists(string sql)
        {
            SqlConnection conn = new SqlConnection(this.ConnectionStringLocalTransaction);
            return Exists(conn, sql);
        }

        public PageCount GetPageCount(string sql, int pageSize)
        {
            object obj = ExecuteScalar(sql);
            int record = int.Parse(obj.ToString());
            int pagecount = (int)Math.Ceiling(record * 1.0 / pageSize);
            PageCount pm = new PageCount();
            pm.PageTotal = pagecount;
            pm.RecordTotal = record;
            return pm;
        }


        public DataTable ExecutePager(SqlConnection conn, int pageIndex, int pageSize, string fields, string Tablename, string whereString, string orderString, out int pageCount, out int recordCount, ref string outSql)
        {
            string table = string.Format(" {0} att  ", Tablename);

            #region calc total record
            string ss = string.Format(" select count(1) from {0} {1}", table, whereString);

            SqlCommand cmdCount = new SqlCommand(ss, conn);
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


            int begin = (pageIndex - 1) * pageSize + 1;
            int end = begin + pageSize;
            string sql = string.Format(@"select * from 
 (select  ROW_NUMBER() over( order by {0}  ) row, {3} from {1}  {2}  
 ) 
t where row between {4} and {5} ", orderString, table, whereString, fields, begin, end);
            DataTable dt = ExecuteDataTable(conn, sql);
            return dt;
        }

        public DataTable ExecutePager(SqlConnection conn, int pageIndex, int pageSize, string fields, string Tablename, string whereString, string orderString, string uniqueKey, Type uniqueKeyType, out int pageCount, out int recordCount, ref string outSql)
        {
            string sql = "";
            DataTable dt = null;
            try
            {
                string table = string.Format(" {0} att  ", Tablename);

                #region calc total record
                string ss = string.Format(" select count(1) from {0} {1}", table, whereString);

                SqlCommand cmdCount = new SqlCommand(ss, conn);
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

                SqlCommand cmd;
                if (pageIndex == 1)//第一页
                {
                    sql = string.Format("select top {0} {1} from {2} {3} order by {4} ", pageSize, fields, table, whereString, orderString);
                    cmd = new SqlCommand(sql, conn);
                }
                else if (pageIndex > pageCount)//超出总页数
                {
                    sql = string.Format("select top {0} {1} from {2} {3} order by {4} ", pageSize, fields, table, "where 1=2", orderString);
                    cmd = new SqlCommand(sql, conn);
                }
                else
                {
                    int begin = pageSize * pageIndex;
                    int skipped = begin - pageSize;
                    string sqlRecordIds = string.Format("select top {0} {1} from {2} {3} order by {4} ", begin, uniqueKey, table, whereString, orderString);
                    string recordIDs = string.Empty;
                    #region calc recordids ,这里可根据当前页在总页数中的比重进一步优化
                    cmd = new SqlCommand(sqlRecordIds, conn);
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
                    cmd = new SqlCommand(sql, conn);
                }
                outSql = sql;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
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
        /// 缓存参数数组
        /// </summary>
        /// <param name="cacheKey">参数缓存的键值</param>
        /// <param name="cmdParms">被缓存的参数列表</param>
        public void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// 获取被缓存的参数
        /// </summary>
        /// <param name="cacheKey">用于查找参数的KEY值</param>
        /// <returns>返回缓存的参数数组</returns>
        public SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            //新建一个参数的克隆列表
            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            //通过循环为克隆参数列表赋值
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                //使用clone方法复制参数列表中的参数
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">SqlCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            //判断数据库连接状态
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            //判断是否需要事物处理
            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
