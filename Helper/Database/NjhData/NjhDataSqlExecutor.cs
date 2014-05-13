using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using System.Reflection;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Helper.DbDataType;

namespace Helper.Database
{
    internal class NjhDataSqlExecutor
    {
        internal string ConnectionString { get; private set; }

        private IEntityOPeration operation;

        /// <summary>
        /// 最后执行的一条SQL语句，仅用来Debug
        /// </summary>
        static public string LastSQL { get; private set; }

        private AbstractDbProvider _dbProvider;
        internal DbContext DbContext;

        public NjhDataSqlExecutor(DbContext context)
        {
            this.DbContext = context;
            this._dbProvider = context.DbProvider;
            this.ConnectionString = context.ConnStr;
        }

        #region 打开/关闭数据库连接

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        public IDbConnection CreateConnection()
        {
            IDbConnection _conn = null;
            if (_conn == null)
            {
                _conn = DbContext.DbProvider.CreateConnection();
                _conn.ConnectionString = DbContext.ConnStr;
                _conn.Open();
            }
            else if (_conn.State != ConnectionState.Open)
            {
                _conn.ConnectionString = DbContext.ConnStr;
                _conn.Open();
            }
            return _conn;
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnection(IDbConnection conn)
        {
            conn.Close();
        }

        #endregion


        #region 事务处理

        /// <summary>
        /// 开启事务(内部会调用OpenConnect)
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            IDbConnection con = _dbProvider.CreateConnection();
            con.ConnectionString = DbContext.ConnStr;
            con.Open();
            return con.BeginTransaction();
        }

        /// <summary>
        /// 提交事务(内部会调用CloseConnect)
        /// </summary>
        public void CommitTransaction(IDbTransaction tran)
        {
            tran.Commit();
            tran.Connection.Close();
        }

        /// <summary>
        /// 回滚事务(内部会调用CloseConnect)
        /// </summary>
        public void RollbackTransaction(IDbTransaction tran)
        {
            tran.Commit();
            tran.Connection.Close();
        }

        #endregion


        #region execute方法,执行操作的方法

        /// <summary>
        /// ExecuteNonQuery 
        /// 执行指定的 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        /// <param name="strCmd"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return ExecuteNonQuery(this.ConnectionString, strCmd, type, parameters);
        }

        public int ExecuteNonQuery(string strCmd, params IDbDataParameter[] parameters)
        {
            return ExecuteNonQuery(this.ConnectionString, strCmd, CommandType.Text, parameters);
        }
        /// <summary>
        /// ExecteNonQuery 执行事务
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        public int ExecteNonQuery(IDbTransaction trans, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = CreateCommand(trans, cmdText, cmdType, commandParameters);
            return cmd.ExecuteNonQuery();
        }
        public int ExecuteNonQuery(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            int i = 0;
            IDbCommand cmd = CreateCommand(connectionString, cmdText, cmdType, commandParameters);
            try
            {
                {
                    i = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection != null && cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
            return i;
        }

        #region  ExecuteScalar
        /// <summary>
        /// ExecuteScalar
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="strCmd"></param>
        /// <returns></returns>
        public object ExecuteScalar(string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return ExecuteScalar(DbContext.ConnStr, strCmd, type, parameters);
        }
        public object ExecuteScalar(string strCmd, params IDbDataParameter[] parameters)
        {
            return ExecuteScalar(DbContext.ConnStr, strCmd, CommandType.Text, parameters);
        }
        public object ExecuteScalar(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            object val = null;
            IDbCommand cmd = CreateCommand(connectionString, cmdText, cmdType, commandParameters);
            try
            {
                {
                    val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd.Connection != null && cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
            return val;
        }
        #endregion

        /// <summary>
        ///ExecuteReader 执行查询，生成一个DataReader返回数据(此函数内部不会调用OpenConnection和CloseConnection)
        /// </summary>
        /// <param name="strCmd"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return ExecuteReader(DbContext.ConnStr, strCmd, type, parameters);
        }
        public IDataReader ExecuteReader(string strCmd, params IDbDataParameter[] parameters)
        {
            return ExecuteReader(DbContext.ConnStr, strCmd, CommandType.Text, parameters);
        }
        public IDataReader ExecuteReader(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            IDataReader dr = null;
            IDbCommand cmd = CreateCommand(connectionString, cmdText, cmdType, commandParameters);
            try
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch (Exception)
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                throw;
            }
        }


        /// <summary>
        ///ExecuteDataTable
        ///执行查询，并返回一个DataTable对象
        /// </summary>
        /// <param name="strCmd"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return ExecuteDataTable(DbContext.ConnStr, strCmd, type, parameters);
        }
        public DataTable ExecuteDataTable(string strCmd, params IDbDataParameter[] parameters)
        {
            return ExecuteDataTable(DbContext.ConnStr, strCmd, CommandType.Text, parameters);
        }
        public DataTable ExecuteDataTable(string connectionString, string cmdText, CommandType cmdType, params IDbDataParameter[] commandParameters)
        {
            IDbCommand cmd = CreateCommand(connectionString, cmdText, cmdType, commandParameters);
            try 
            {
                IDbDataAdapter adapter = _dbProvider.CreateDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dt = new DataSet();
                adapter.Fill(dt);
                cmd.Connection.Close();
                DataTable dtt = dt.Tables[0];
                int rows = dtt.Rows.Count;
                return dtt;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Connection.Dispose();
            }
        }


        #endregion


        #region 私有方法

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public IDbDataParameter CreateParameter(string name, object value, DbType type)
        {
            IDbDataParameter parameter = _dbProvider.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = type;

            return parameter;
        }

        #endregion

        #region new
        private IDbCommand CreateCommandAll(string sql, CommandType type, IDbDataParameter[] parameters)
        {
            IDbCommand cmd = _dbProvider.CreateCommand();
            cmd.CommandType = type;
            cmd.Connection = this.CreateConnection();
            //  cmd.Transaction = this._tran;
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (IDbDataParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
            return cmd;
        }
        private IDbCommand CreateCommand(IDbTransaction trans, string sql, CommandType type, params IDbDataParameter[] parameters)
        {
            IDbCommand cmd = _dbProvider.CreateCommand();
            cmd.CommandType = type;
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (IDbDataParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
            IDbConnection connection = trans.Connection;
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            cmd.Transaction = trans;
            return cmd;

        }
        private IDbCommand CreateCommand(string connectionString, string sql, CommandType type, params IDbDataParameter[] parameters)
        {
            IDbCommand cmd = _dbProvider.CreateCommand();
            cmd.CommandType = type;
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (IDbDataParameter param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
            IDbConnection connection = _dbProvider.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
            cmd.Connection = connection;
            return cmd;
        }



        #endregion

        MyCompare _compare = new MyCompare();

        #region  operate entity v2
        public object InsertV2<T>(T t) where T : IDicSerialize
        {
            object inserted = 0;
            if (_dbProvider is OleDbProvider)
            {
                operation = new OledbEntityOperation(this);
                inserted = operation.InsertV2(t);
            }
            else if (_dbProvider is MySqlProvider)
            {
                operation = new NjhDataEntityOPerationMysql(this);
                inserted = operation.InsertV2(t);
            }
            else if (_dbProvider is SqlProvider)
            {
                operation = new NjhDataEntityOPerationMsSql(this);
                inserted = operation.InsertV2(t);
            }
            return inserted;
        }
        public void UpdateV2<T>(T t) where T : IDicSerialize
        {
            if (_dbProvider is OleDbProvider)
            {
                operation = new OledbEntityOperation(this);
                operation.UpdateV2<T>(t);
            }
            else if (_dbProvider is MySqlProvider)
            {
                operation = new NjhDataEntityOPerationMysql(this);
                operation.UpdateV2<T>(t);
            }
            else if (_dbProvider is SqlProvider)
            {
                operation = new NjhDataEntityOPerationMsSql(this);
                operation.UpdateV2<T>(t);
            }
        }

        #endregion


        #region operate entity
        public object Insert<T>(T dbModel)
        {
            StringBuilder sb = null;
            List<PropertyInfo> list = DataMapper.GetProperties<T>();


            List<string> filteredColumns = null; filteredColumns = new List<string>();
            string tbName = typeof(T).Name;
            #region  获取表中的所有列
            List<string> columns = new List<string>();
            if (_dbProvider is OleDbProvider)
                columns = GetAccessTbColumns((OleDbConnection)CreateConnection(), tbName);

            list = list.Where(x => columns.Contains(x.Name, _compare)).ToList();
            #endregion


            #region 判断需要过滤的字段

            if (_dbProvider is SqlProvider)
            {
                string schema = ExecuteScalar(string.Format("select top 1 [table_schema] from information_schema.columns where TABLE_NAME='{0}'", tbName)).ToString();
                string fulelTbName = string.Format("{0}.{1}", schema, tbName);
                sb = new StringBuilder(string.Format("insert into {0} (", fulelTbName));
                string sql = string.Format("select [name] from sys.columns WHERE object_id=OBJECT_ID('{0}') AND is_identity=1", fulelTbName);
                using (IDataReader dr = ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        if (filteredColumns == null) filteredColumns = new List<string>();
                        filteredColumns.Add(dr[0].ToString());
                    }
                }
            }
            else if (_dbProvider is OleDbProvider)
            {
                sb = new StringBuilder(string.Format("insert into {0} (", tbName));
                filteredColumns.Add(ReflectionHelper.GetTypeField_IdentityKeys<T>());
            }
            else if (_dbProvider is MySqlProvider)
            {
                sb = new StringBuilder(string.Format("insert into {0} (", string.Format("{0}", tbName)));
            }
            #endregion

            int size = list.Count;
            for (int i = 0; i < size; i++)
            {
                if (filteredColumns.Contains(list[i].Name, _compare)) continue;

                if (i != size - 1)
                {
                    sb.AppendFormat("{0},", list[i].Name);
                }
                else
                {
                    sb.AppendFormat("{0}) values(", list[i].Name);
                }
            }

            object last = "0";
            if (_dbProvider is MySqlProvider)
            {
                PropertyInfo pi = null;
                for (int i = 0; i < size; i++)
                {
                    pi = list[i];
                    if (filteredColumns.Contains(list[i].Name, _compare)) continue;
                    if (i != size - 1)
                    {
                        sb.AppendFormat("?{0},", pi.Name);
                    }
                    else
                    {
                        sb.AppendFormat("?{0})", pi.Name);
                    }
                }
                last = ExecuteScalar("SELECT  LAST_INSERT_ID()");
            }
            else if (_dbProvider is SqlProvider)
            {
                PropertyInfo pi = null;
                for (int i = 0; i < size; i++)
                {
                    pi = list[i];
                    if (filteredColumns.Contains(list[i].Name)) continue;
                    if (i != size - 1)
                    {
                        sb.AppendFormat("@{0},", pi.Name);
                    }
                    else
                    {
                        sb.AppendFormat("@{0})", pi.Name);
                    }
                }
                last = ExecuteScalar("SELECT  @@identity");
            }
            else if (_dbProvider is OleDbProvider)
            {
                //PropertyInfo pi = null;
                //for (int i = 0; i < size; i++)
                //{
                //    pi = list[i];
                //    if (filteredColumns.Contains(list[i].Name, _compare)) continue;
                //    if (i != size - 1)
                //    {
                //        sb.AppendFormat("@{0},", pi.Name);
                //    }
                //    else
                //    {
                //        sb.AppendFormat("@{0})", pi.Name);
                //    }
                //}
                //ExecuteNonQuery(sb.ToString(), CommandType.Text, CreateParameter(list, dbModel));
                //last = ExecuteScalar("SELECT  max(idx) from " + tbName);
                operation = new OledbEntityOperation(this);
                last = operation.Insert<T>(dbModel);

            }

            //   HttpContext.Current.Response.Write(sb.ToString());

            return last;
        }



        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Update<T>(T t)
        {
            List<PropertyInfo> list = DataMapper.GetProperties<T>();
            List<string> filteredColumns = new List<string>();
            if (_dbProvider is OleDbProvider)
            {
                filteredColumns.Add(ReflectionHelper.GetTypeField_IdentityKeys<T>());
            }
            else if (_dbProvider is SqlProvider)
            {

            }
            else if (_dbProvider is MySqlProvider)
            {

            }


            StringBuilder sb = new StringBuilder(string.Format("update {0} set ", typeof(T).Name));
            int size = list.Count;
            int i = 0;

            string idx = string.Empty;
            if (_dbProvider is MySqlProvider)
            {
                foreach (PropertyInfo pi in list)
                {
                    if (i != size - 1)
                        sb.AppendFormat("{0}=?{1},", pi.Name, pi.Name);
                    else
                        sb.AppendFormat("{0}=?{1} ", pi.Name, pi.Name);
                    i++;

                    if ((filteredColumns.Contains(pi.Name)))
                    {
                        idx = string.Format(" where idx={0}", pi.GetValue(t, null));
                    }
                }
            }
            else if (_dbProvider is OleDbProvider)
            {
                operation = new OledbEntityOperation(this);
                operation.Update<T>(t);
            }
            //if (string.IsNullOrEmpty(idx)) throw new ApplicationException("未发现合适的更新条件，为保护数据，终止了此次操作.....");
            ////  sb = new StringBuilder(sb.ToString().TrimEnd(','));
            //sb.AppendFormat(idx);
            //ExecuteNonQuery(sb.ToString(), CreateParameter(list, dbModel));

        }

        //public void UpdateV2(IDictionary<string, MyField> dic)
        //{
        //    if (_dbProvider is OleDbProvider)
        //    {
        //        operation = new OledbEntityOperation(this);
        //        operation.Update(dic);
        //    }
        //}

        public void Delete<T>(int Idx)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("delete from {0} where {2}={1}", typeof(T).Name, Idx, ReflectionHelper.GetTypeField_IdentityKeys<T>());
            ExecuteNonQuery(sb.ToString());
        }
        public void Delete<T>(string condition)
        {
            ExecuteNonQuery(string.Format("delete from {0} where {1}", typeof(T).Name, condition));
        }

        public void Delete<T>(int[] arrIdx)
        {
            StringBuilder sb = new StringBuilder();

            if (arrIdx.Length == 1)
                sb.AppendFormat("delete from {0} where {2}={1}", typeof(T).Name, arrIdx[0], ReflectionHelper.GetTypeField_IdentityKeys<T>());
            else
            {
                sb.AppendFormat("delete from {0} where {1} in (", typeof(T).Name, ReflectionHelper.GetTypeField_IdentityKeys<T>());
                for (int i = 0; i < arrIdx.Length; i++)
                {
                    if (i != arrIdx.Length - 1)
                    {
                        sb.AppendFormat("{0},", i);
                    }
                    else
                    {
                        sb.AppendFormat("{0})", i);
                    }
                }
            }
            ExecuteNonQuery(sb.ToString());
        }
        #endregion

        #region CreateParameter

        private DbParameter[] CreateParameter<T>(List<PropertyInfo> list, T dbModel)
        {
            int size = list.Count;
            StringBuilder sb = new StringBuilder();
            List<DbParameter> arr = new List<DbParameter>();
            if (_dbProvider is MySqlProvider)
            {
                foreach (PropertyInfo pi in list)
                {
                    arr.Add(CreateMyDbParameter(pi, dbModel));
                }
            }
            else if (_dbProvider is SqlProvider)
            {
                foreach (PropertyInfo pi in list)
                {
                    arr.Add(CreateSqlParameter(pi, dbModel));
                }
            }
            else if (_dbProvider is OleDbProvider)
            {
                List<string> filters = new List<string>();
                filters.Add(ReflectionHelper.GetTypeField_IdentityKeys<T>());
                foreach (PropertyInfo pi in list)
                {
                    if (filters.Contains(pi.Name, _compare)) continue;
                    arr.Add(CreateOledbParameter(pi, dbModel));
                }
            }
            return arr.ToArray();
        }

        private DbParameter CreateMyDbParameter<T>(PropertyInfo pi, T dbModel)
        {
            MySqlParameter sps = new MySqlParameter(string.Format("?{0}", pi.Name), pi.GetValue(dbModel, null));
            MySqlDbType type = MySqlDbType.String;
            if (pi.PropertyType == typeof(int))
            {
                type = MySqlDbType.Int32;
            }
            else if (pi.PropertyType == typeof(bool))
            {
                type = MySqlDbType.Bit;
            }
            else if (pi.PropertyType == typeof(string))
            {
                type = MySqlDbType.String;
            }
            else if (pi.PropertyType == typeof(decimal))
            {
                type = MySqlDbType.Decimal;
            }
            else if (pi.PropertyType == typeof(double))
            {
                type = MySqlDbType.Double;
            }
            else if (pi.PropertyType == typeof(float))
            {
                type = MySqlDbType.Float;
            }
            else if (pi.PropertyType == typeof(DateTime))
            {
                type = MySqlDbType.Datetime;
            }

            sps.MySqlDbType = type;
            return sps;
        }
        private DbParameter CreateSqlParameter<T>(PropertyInfo pi, T model)
        {
            SqlParameter sps = new SqlParameter(string.Format("@{0}", pi.Name), pi.GetValue(model, null));
            SqlDbType type = SqlDbType.VarChar;
            if (pi.PropertyType == typeof(int))
            {
                type = SqlDbType.Int;
            }
            else if (pi.PropertyType == typeof(bool))
            {
                type = SqlDbType.Bit;
            }
            else if (pi.PropertyType == typeof(string))
            {
                type = SqlDbType.VarChar;
            }
            else if (pi.PropertyType == typeof(decimal))
            {
                type = SqlDbType.Decimal;
            }
            else if (pi.PropertyType == typeof(double))
            {
                type = SqlDbType.Decimal;
            }
            else if (pi.PropertyType == typeof(float))
            {
                type = SqlDbType.Float;
            }
            else if (pi.PropertyType == typeof(DateTime))
            {
                type = SqlDbType.DateTime;
            }
            else if (pi.PropertyType == typeof(Guid))
            {
                type = SqlDbType.UniqueIdentifier;
            }

            sps.SqlDbType = type;
            return sps;
        }
        private DbParameter CreateOledbParameter<T>(PropertyInfo pi, T model)
        {
            object val = pi.GetValue(model, null);

            OleDbType type = OleDbType.VarChar;
            if (pi.PropertyType == typeof(int))
            {
                type = OleDbType.Integer;
            }
            else if (pi.PropertyType == typeof(bool))
            {
                if ((bool)val) val = 1;
                else val = 0;
                type = OleDbType.Integer;
            }
            else if (pi.PropertyType == typeof(string))
            {
                type = OleDbType.VarChar;
                if (val == null) val = string.Empty;
            }
            else if (pi.PropertyType == typeof(decimal))
            {
                type = OleDbType.Decimal;
            }
            else if (pi.PropertyType == typeof(double))
            {
                type = OleDbType.Decimal;
            }
            else if (pi.PropertyType == typeof(float))
            {
                type = OleDbType.Decimal;
            }
            else if (pi.PropertyType == typeof(DateTime))
            {
                type = OleDbType.Date;
            }
            else if (pi.PropertyType == typeof(Guid))
            {
                type = OleDbType.VarChar;
            }
            OleDbParameter sps = new OleDbParameter(string.Format("@{0}", pi.Name), val);
            sps.OleDbType = type;
            return sps;
        }

        public DataTable Paging(string fields, string table, string strWhere, string orderkey, string orderType, int currentPage, int pageSize)
        {
            if (_dbProvider is OleDbProvider)
            {
                return PagingOledb(fields, table, strWhere, orderkey, orderType, currentPage, pageSize);
            }
            return null;
        }

        private DataTable PagingOledb(string fields, string table, string strWhere, string orderkey, string orderType, int currentPage, int pageSize)
        {

            StringBuilder strSql = new StringBuilder();
            if (currentPage != 1)
            {
                int topNum = pageSize * (currentPage - 1);
                strSql.AppendFormat("select top " + pageSize + " " + fields + " from [" + table + "] ");
                if (string.Compare(orderType, "desc", true) == 0)
                    strSql.Append(" where " + orderkey + " <( select min(" + orderkey + ") from( select top " + topNum + " " + orderkey + " from  [" + table + "]");
                else
                    strSql.Append(" where " + orderkey + " >( select max(" + orderkey + ") from( select top " + topNum + " " + orderkey + " from  [" + table + "]");
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
                strSql.AppendFormat("select top {0} {1} from  [{2}]", pageSize, fields, table);
                if (!string.IsNullOrEmpty(strWhere))
                {
                    strSql.AppendFormat(" where {0}", strWhere);
                }
                strSql.AppendFormat(" order by {0} {1}", orderkey, orderType);
            }

            return ExecuteDataTable(strSql.ToString());
        }
        #endregion

        internal bool IsInsertColumn(string column, IDictionary<string, MyField> dic)
        {
            bool rs = true;
            foreach (string item in dic.Keys)
            {
                if (column == item)
                {
                    if (dic[column].IsIdentity)
                    {
                        rs = false;
                        break;
                    }
                }
            }
            return rs;
        }

        #region   获取表的列


        List<string> GetAccessTbColumns(OleDbConnection conn, string tableName)
        {
            List<string> list = new List<string>();
            DataTable dt = null;
            using (conn)
            {
                dt = conn.GetSchema("columns", new string[] { null, null, tableName });
                foreach (DataRow dr in dt.Rows)
                {
                    string column_Name = dr["column_name"].ToString();
                    list.Add(column_Name);
                }
            }

            return list;
        }
        #endregion
    }

    class MyCompare : IEqualityComparer<string>
    {

        public bool Equals(string x, string y)
        {
            return string.Compare(x, y, true) == 0;
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
