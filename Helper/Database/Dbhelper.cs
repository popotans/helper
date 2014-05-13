using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Helper.Database
{
    public enum DbProviderType
    {
        Mssql,
        MySql,
        Oledb
    }

    /// <summary>
    ///
    /// </summary>
    public class DbHelper
    {
        /*  private static string DbProviderName = System.Configuration.ConfigurationManager.AppSettings["DbHelperProvider"];
          private static string DbConnectionString = System.Configuration.ConfigurationManager.AppSettings["DbHelperConnectionString"];
         * mysql 驱动节点配置
         ///<system.data>
    ///<DbProviderFactories>
    /// <add name="MySQL Data Provider" invariant="MySql.Data.MySqlClient" description=".Net Framework Data Provider for MySQL" type="MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data, Version=5.1.5.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d" />
    /// </DbProviderFactories>
    /// </system.data>
         * */
        public string DbProviderName
        {
            get;
            set;
        }
        public string DbConnectionString
        {
            get;
            set;
        }

        #region Constructor
        public DbHelper()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings["connstr"] == null)
                throw new Excep.HelperException("database connectionstring of key \"connstr\" can't be null.");
            DbConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ToString();
            DbProviderName = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ProviderName;
            if (string.IsNullOrEmpty(DbConnectionString) || string.IsNullOrEmpty(DbProviderName))
                throw new Excep.HelperException("database connectionstring can't be null.");
        }
        public DbHelper(string connectionString)
        {
            DbConnectionString = connectionString;
            DbProviderName = "System.Data.SqlClient";
            if (string.IsNullOrEmpty(DbConnectionString) || string.IsNullOrEmpty(DbProviderName))
                throw new Excep.HelperException("database connectionstring can't be null.");
        }

        public DbHelper(string connectionString, string providername)
        {
            DbConnectionString = connectionString;
            DbProviderName = providername;
            if (string.IsNullOrEmpty(DbConnectionString) || string.IsNullOrEmpty(DbProviderName))
                throw new Excep.HelperException("database connectionstring can't be null.");
        }
        public DbHelper(string connectionString, DbProviderType providerType)
        {
            string providername = "System.Data.SqlClient";
            if (providerType == DbProviderType.MySql)
            {
                providername = "MySql.Data.MySqlClient";
            }
            else if (DbProviderType.Oledb == providerType)
            {
                providername = "System.Data.OleDb";
            }

            DbConnectionString = connectionString;
            DbProviderName = providername;
            if (string.IsNullOrEmpty(DbConnectionString) || string.IsNullOrEmpty(DbProviderName))
                throw new Excep.HelperException("database connectionstring can't be null.");
        }

        //public DbHelper(string WebConfigConnStkey)
        //{
        //    if (System.Configuration.ConfigurationManager.ConnectionStrings[WebConfigConnStkey] == null)
        //        throw new Excep.HelperException("database connectionstring of key \"" + WebConfigConnStkey + "\" can't be null.");
        //    DbConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[WebConfigConnStkey].ToString();
        //    DbProviderName = System.Configuration.ConfigurationManager.ConnectionStrings[WebConfigConnStkey].ProviderName;
        //    if (string.IsNullOrEmpty(DbConnectionString) || string.IsNullOrEmpty(DbProviderName))
        //        throw new Excep.HelperException("database connectionstring or DbProviderName can't be null.");
        //}
        #endregion

        #region CreateConnection
        public DbConnection CreateConnection()
        {
            //DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbProviderName);
            //DbConnection dbconn = dbfactory.CreateConnection();
            DbConnection dbconn = new SqlConnection();
            if (DbProviderName == "System.Data.SqlClient") { }
            else if (DbProviderName == "MySql.Data.MySqlClient") { dbconn = new MySql.Data.MySqlClient.MySqlConnection(); }
            else if (DbProviderName == "System.Data.OleDb") { dbconn = new System.Data.OleDb.OleDbConnection(); }
            dbconn.ConnectionString = DbConnectionString;
            return dbconn;
        }
        #endregion

        #region GetCommand
        public DbCommand GetStoredProcedureCommond(string storedProcedure)
        {
            DbConnection connection = CreateConnection();
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = storedProcedure;
            dbCommand.CommandType = CommandType.StoredProcedure;
            return dbCommand;
        }
        public DbCommand GetSqlStringCommond(string sqlQuery)
        {
            DbConnection connection = CreateConnection();
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = sqlQuery;
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }
        #endregion

        #region 增加参数
        public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
        {
            foreach (DbParameter dbParameter in dbParameterCollection)
            {
                cmd.Parameters.Add(dbParameter);
            }
        }
        public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Size = size;
            dbParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(dbParameter);
        }
        public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter);
        }
        public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(dbParameter);
        }
        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            return cmd.Parameters[parameterName];
        }

        #endregion

        #region 执行查询
        public DataSet ExecuteDataSet(DbCommand cmd)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            dbDataAdapter.Fill(ds);
            return ds;
        }
        public DataTable ExecuteDataTable(DbCommand cmd)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dbDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public DataTable ExecuteDataTable(string sql)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbProviderName);
            DbCommand cmd = GetSqlStringCommond(sql);

            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dbDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            cmd.Connection.Open();
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
        public DbDataReader ExecuteReader(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            cmd.Connection.Open();
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
        public int ExecuteNonQuery(DbCommand cmd)
        {
            cmd.Connection.Open();
            int ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return ret;
        }
        public int ExecuteNonQuery(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            cmd.Connection.Open();
            int ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return ret;
        }
        public object ExecuteScalar(DbCommand cmd)
        {
            cmd.Connection.Open();
            object ret = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return ret;
        }
        public object ExecuteScalar(string sql)
        {
            DbCommand cmd = GetSqlStringCommond(sql);
            cmd.Connection.Open();
            object ret = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return ret;
        }


        public bool Exists(string sql)
        {
            object obj = ExecuteScalar(sql);
            return obj != null;
        }

        /// <summary>
        /// 生成参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public SqlParameter CreateParameter(string key, object val, SqlDbType type)
        {
            SqlParameter sps = new SqlParameter(key, val);
            sps.SqlDbType = type;
            return sps;
        }

        #endregion

        #region 执行事务
        public DataSet ExecuteDataSet(DbCommand cmd, Trans t)
        {
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataSet ds = new DataSet();
            dbDataAdapter.Fill(ds);
            return ds;
        }
        public DataTable ExecuteDataTable(DbCommand cmd, Trans t)
        {
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbProviderName);
            DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = cmd;
            DataTable dataTable = new DataTable();
            dbDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public DbDataReader ExecuteReader(DbCommand cmd, Trans t)
        {
            cmd.Connection.Close();
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            DbDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            return reader;
        }
        public int ExecuteNonQuery(DbCommand cmd, Trans t)
        {
            cmd.Connection.Close();
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            int ret = cmd.ExecuteNonQuery();
            return ret;
        }
        public object ExecuteScalar(DbCommand cmd, Trans t)
        {
            cmd.Connection.Close();
            cmd.Connection = t.DbConnection;
            cmd.Transaction = t.DbTrans;
            object ret = cmd.ExecuteScalar();
            return ret;
        }
        #endregion



        #region  执行分页


        public DataTable MySqlPaging(string strfields, string strTableWhere, string strorderby, int pno, int pagesize, out int pagecount, out int recordcount)
        {
            pagecount = recordcount = 0;
            if (pno < 1) pno = 1;

            DataTable dt = new DataTable();
            string sqlCount = "select count(1) " + strTableWhere;
            recordcount = int.Parse(ExecuteScalar(sqlCount).ToString());

            double f = (double)recordcount / (double)pagesize;
            pagecount = (int)Math.Ceiling(f);

            if (pno > pagecount) pno = pagecount;

            int idxFrom = (pno - 1) * pagesize;
            string sqlPage = "select " + strfields + strTableWhere + strorderby + " limit " + idxFrom + ", " + pagesize + "";
            dt = ExecuteDataTable(sqlPage);
            return dt;

        }

        public DataTable SqlLitePaging(string strfields, string strTableWhere, string strorderby, int pno, int pagesize, out int pagecount, out int recordcount)
        {
            return MySqlPaging(strfields, strTableWhere, strorderby, pno, pagesize, out pagecount, out recordcount);
        }
        #endregion
    }

    #region TransClass
    public class Trans : IDisposable
    {
        private DbConnection conn;
        private DbTransaction dbTrans;
        public DbConnection DbConnection
        {
            get { return this.conn; }
        }
        public DbTransaction DbTrans
        {
            get { return this.dbTrans; }
        }
        public Trans()
        {
            conn = new DbHelper().CreateConnection();
            conn.Open();
            dbTrans = conn.BeginTransaction();
        }
        public void Commit()
        {
            dbTrans.Commit();
            this.Colse();
        }
        public void RollBack()
        {
            dbTrans.Rollback();
            this.Colse();
        }
        public void Dispose()
        {
            this.Colse();
        }
        public void Colse()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
    #endregion

    #region 调用示例
    /*
     1)直接执行sql语句


DbHelper db = new DbHelper();
DbCommand cmd = db.GetSqlStringCommond("insert t1 (id)values('haha')");
db.ExecuteNonQuery(cmd);


2)执行存储过程

DbHelper db = new DbHelper();
DbCommand cmd = db.GetStoredProcCommond("t1_insert");
db.AddInParameter(cmd, "@id", DbType.String, "heihei");
db.ExecuteNonQuery(cmd);


3)返回DataSet

DbHelper db = new DbHelper();
DbCommand cmd = db.GetSqlStringCommond("select * from t1");
DataSet ds = db.ExecuteDataSet(cmd);


4)返回DataTable

DbHelper db = new DbHelper();
DbCommand cmd = db.GetSqlStringCommond("t1_findall");
DataTable dt = db.ExecuteDataTable(cmd);


5)输入参数/输出参数/返回值的使用(比较重要哦)

DbHelper db = new DbHelper();
DbCommand cmd = db.GetStoredProcCommond("t2_insert");
db.AddInParameter(cmd, "@timeticks", DbType.Int64, DateTime.Now.Ticks);
db.AddOutParameter(cmd, "@outString", DbType.String, 20);
db.AddReturnParameter(cmd, "@returnValue", DbType.Int32);
db.ExecuteNonQuery(cmd);
string s = db.GetParameter(cmd, "@outString").Value as string;//out parameter
int r = Convert.ToInt32(db.GetParameter(cmd, "@returnValue").Value);//return value


6)DataReader使用

DbHelper db = new DbHelper();
DbCommand cmd = db.GetStoredProcCommond("t2_insert");
db.AddInParameter(cmd, "@timeticks", DbType.Int64, DateTime.Now.Ticks);
db.AddOutParameter(cmd, "@outString", DbType.String, 20);
db.AddReturnParameter(cmd, "@returnValue", DbType.Int32);
using (DbDataReader reader = db.ExecuteReader(cmd))
{
dt.Load(reader);
}
string s = db.GetParameter(cmd, "@outString").Value as string;//out parameter
int r = Convert.ToInt32(db.GetParameter(cmd, "@returnValue").Value);//return value

7)事务的使用.(项目中需要将基本的数据库操作组合成一个完整的业务流时,代码级的事务是必不可少的哦)

public void DoBusiness()
{
using (Trans t = new Trans())
{
try
{
D1(t);
throw new Exception();//如果有异常,会回滚滴
            D2(t);
t.Commit();
}
catch
{
t.RollBack();
}
}
}
public void D1(Trans t)
{
DbHelper db = new DbHelper();
DbCommand cmd = db.GetStoredProcCommond("t2_insert");
db.AddInParameter(cmd, "@timeticks", DbType.Int64, DateTime.Now.Ticks);
db.AddOutParameter(cmd, "@outString", DbType.String, 20);
db.AddReturnParameter(cmd, "@returnValue", DbType.Int32);
if (t == null)
db.ExecuteNonQuery(cmd);
else
db.ExecuteNonQuery(cmd, t);
string s = db.GetParameter(cmd, "@outString").Value as string;//out parameter
    int r = Convert.ToInt32(db.GetParameter(cmd, "@returnValue").Value);//return value
}
public void D2(Trans t)
{
DbHelper db = new DbHelper();
DbCommand cmd = db.GetSqlStringCommond("insert t1 (id)values('..')");
if (t == null)
db.ExecuteNonQuery(cmd);
else
db.ExecuteNonQuery(cmd, t);
}

以上我们好像没有指定数据库连接字符串,大家如果看下DbHelper的代码,就知道要使用它必须在config中配置两个参数,如下:

<appSettings>
<add key="DbHelperProvider" value="System.Data.SqlClient"/>
<add key="DbHelperConnectionString" value="Data Source=(local);Initial Catalog=DbHelperTest;Persist Security Info=True;User ID=sa;Password=sa"/>
</appSettings> 

     */
    #endregion
}
