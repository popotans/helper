using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Threading;
using System.Reflection;
using Helper.DbDataType;
namespace Helper.Database
{
    #region DbContext
    public class DbContext
    {
        public DbContext()
        {

        }
        public DbContext(string connstr)
        {
            this.ConnStr = connstr;
        }
        public DbContext(string connstr, AbstractDbProvider provider)
        {
            this.ConnStr = connstr;
            this.DbProvider = provider;
        }
        public string ConnStr { get; set; }
        internal AbstractDbProvider DbProvider { get; set; }
    }
    #endregion

    #region DataType
    public enum DataType
    {
        Oledb = 0,
        Mssql,
        Mysql,
        Sqllite
    }
    #endregion

    public class NjhData
    {
        public DbContext DbContext { get; internal set; }
        private NjhDataSqlExecutor _da { get; set; }
        private AbstractDbProvider _dbProvider { get; set; }
        private static object _locker = new object();
        private static NjhData _njhData;
        public static NjhData Instance
        {
            get
            {
                if (_njhData == null)
                {
                    lock (_locker)
                    {
                        if (_njhData == null)
                        {
                            _njhData = new NjhData();
                        }
                    }
                }
                return _njhData;
            }
        }

        #region init
        private NjhData()
        {

        }

        public NjhData(DataType dbtype, string connstr)
        {
            InitDb(dbtype, connstr);
        }

        public void InitDb(DataType dbtype, string connstr)
        {
            this.DbContext = new DbContext(connstr);
            switch (dbtype)
            {
                case DataType.Oledb:
                    connstr = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + connstr;
                    DbContext.ConnStr = connstr;
                    DbContext.DbProvider = new OleDbProvider();
                    break;
                case DataType.Mssql:
                    DbContext.DbProvider = new SqlProvider();
                    break;
                case DataType.Mysql:
                    DbContext.DbProvider = new MySqlProvider();
                    break;
                default:
                    throw new ArgumentException("未支持的provider:" + dbtype.ToString(), "provider");
            }
            if (_da == null) _da = new NjhDataSqlExecutor(this.DbContext);
        }
        public void InitAccess(string connstr)
        {
            InitDb(DataType.Oledb, connstr);
        }
        public void InitMySql(string connstr)
        {
            InitDb(DataType.Mysql, connstr);
        }
        public void InitMsSql(string connstr)
        {
            InitDb(DataType.Mssql, connstr);
        }
        #endregion

        #region 事务处理
        /// <summary>
        /// open事务
        /// </summary>
        public IDbTransaction BeginTransaction()
        {
            return _da.BeginTransaction();
        }

        #endregion

        #region Execute方法,执行操作的方法

        /// <summary>
        /// 执行指定的 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        public int ExecuteNonQuery(string strCmd, params IDbDataParameter[] parameters)
        {
            return ExecuteNonQuery(strCmd, CommandType.Text, parameters);
        }

        /// <summary>
        /// 执行指定的 Transact-SQL 语句并返回受影响的行数。
        /// </summary>
        public int ExecuteNonQuery(string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return ExecuteNonQuery(DbContext.ConnStr, strCmd, CommandType.Text, parameters);
        }

        public int ExecuteNonQuery(string connStr, string sql, CommandType type, params IDbDataParameter[] parameters)
        {
            return _da.ExecuteNonQuery(connStr, sql, type, parameters);
        }

        /// <summary>
        /// 执行事物
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="strCmd"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(IDbTransaction trans, string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return _da.ExecteNonQuery(trans, strCmd, type, parameters);
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        public object ExecuteScalar(string strCmd, params IDbDataParameter[] parameters)
        {
            return ExecuteScalar(strCmd, CommandType.Text, parameters);
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        public object ExecuteScalar(string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return ExecuteScalar(DbContext.ConnStr, strCmd, type, parameters);
        }

        public object ExecuteScalar(string connStr, string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return _da.ExecuteScalar(connStr, strCmd, type, parameters);
        }

        public T ExecuteScalar<T>(string strCmd, T defaultval, params IDbDataParameter[] parameters)
        {
            object o = ExecuteScalar(strCmd);
            if (o != null)
                return (T)Convert.ChangeType(o, typeof(T));

            return defaultval;
        }

        /// <summary>
        /// 执行查询，并返回一个DataTable对象
        /// </summary>
        public DataTable ExecuteDataTable(string strCmd, params IDbDataParameter[] parameters)
        {
            return ExecuteDataTable(strCmd, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行查询，并返回一个DataTable对象
        /// </summary>
        public DataTable ExecuteDataTable(string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return ExecuteDataTable(DbContext.ConnStr, strCmd, type, parameters);
        }

        public DataTable ExecuteDataTable(string connStr, string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return _da.ExecuteDataTable(connStr, strCmd, type, parameters);
        }

        /// <summary>
        /// 执行查询，生成一个DataReader返回数据(此函数内部不会调用OpenConnection和CloseConnection，所以必须在调用OpenConnection方法后使用)
        /// </summary>
        public IDataReader ExecuteReader(string strCmd, params IDbDataParameter[] parameters)
        {
            return ExecuteReader(strCmd, CommandType.Text, parameters);
        }
        /// <summary>
        /// 执行查询，生成一个DataReader返回数据(此函数内部不会调用OpenConnection和CloseConnection，所以必须在调用OpenConnection方法后使用)
        /// </summary>
        public IDataReader ExecuteReader(string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return ExecuteReader(DbContext.ConnStr, strCmd, type, parameters);
        }
        public IDataReader ExecuteReader(string connnstr, string strCmd, CommandType type, params IDbDataParameter[] parameters)
        {
            return _da.ExecuteReader(connnstr, strCmd, type, parameters);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool Exists(string sql, params IDbDataParameter[] parameters)
        {
            return Exists(DbContext.ConnStr, sql, parameters);
        }
        public bool Exists(string connstr, string sql, params IDbDataParameter[] parameters)
        {
            return ExecuteDataTable(connstr, sql, CommandType.Text, parameters).Rows.Count > 0;
        }

        #endregion

        #region GetEntity

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetEntity<T>(string sql, params IDbDataParameter[] parameters) where T : IDicSerialize, new()
        {
            return GetEntityV2<T>(sql, parameters);
        }

        public T GetEntity<T>(string connstr, string sql, params IDbDataParameter[] parameters) where T : IDicSerialize, new()
        {
            T t = default(T);
            DataTable dt = ExecuteDataTable(connstr, sql, CommandType.Text, parameters);
            if (dt.Rows.Count == 0) return default(T);
            t = DataMapper.GetObjectV2<T>(dt, dt.Rows[0]);
            return t;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetEntityV2<T>(string sql, params IDbDataParameter[] parameters) where T : IDicSerialize, new()
        {
            return GetEntity<T>(DbContext.ConnStr, sql, parameters);
        }

        //public T GetEntityV2<T>(System.Linq.Expressions.Expression<Func<T, bool>> exrp, params IDbDataParameter[] parameters) where T : IDicSerialize, new()
        //{
        //    string condition = exrp.Body.ToString().Replace("&&", "and").Replace("||", "or");
        //    string parateter = string.Empty;
        //    if (exrp.Parameters.Count > 0)
        //    {
        //        parateter = exrp.Parameters[0].Name;
        //    }
        //    else
        //        throw new ApplicationException("No Lambda Parameter");

        //    System.Text.StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat(" select * from {0} as {2} where {1}", typeof(T).Name, condition, parateter);
        //    T t = default(T);
        //    using (IDataReader dr = ExecuteReader(sb.ToString(), parameters))
        //    {
        //        if (dr.Read())
        //            t = DataMapper.GetObjectV2<T>(dr);
        //    }
        //    return t;
        //}


        /// <summary>
        /// 查询实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<T> GetEntityList<T>(DataTable dt) where T : IDicSerialize, new()
        {
            List<T> list = new List<T>();
            if (dt.Rows.Count == 0) return new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = DataMapper.GetObjectV2<T>(dt, dr);
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        ///  查询实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetEntityList<T>(string sql, params IDbDataParameter[] parameters) where T : IDicSerialize, new()
        {
            return GetEntityList<T>(DbContext.ConnStr, sql, parameters);
        }

        public List<T> GetEntityList<T>(string constr, string sql, params IDbDataParameter[] parameters) where T : IDicSerialize, new()
        {
            DataTable dt = ExecuteDataTable(constr, sql, CommandType.Text, parameters);
            return GetEntityList<T>(dt);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetEntityListV2<T>(string sql, params IDbDataParameter[] parameters) where T : IDicSerialize, new()
        {
            return GetEntityList<T>(sql, parameters);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbModel"></param>
        /// <returns></returns>
        public object Insert<T>(T dbModel) where T : IDicSerialize
        {
            return InsertV2<T>(dbModel);
        }

        public object InsertV2<T>(T t) where T : IDicSerialize
        {
            object ooo = _da.InsertV2(t);
            return ooo;
        }

        public void Update<T>(T t) where T : IDicSerialize
        {
            _da.UpdateV2<T>(t);
        }
        public void UpdateV2<T>(T t) where T : IDicSerialize
        {
            _da.UpdateV2<T>(t);

        }

        //public void Update(IDictionary<string, MyField> dic)
        //{
        //    _da.Update(dic);
        //}

        public void Delete<T>(int[] arrIndex)
        {
            _da.Delete<T>(arrIndex);
        }
        public void Delete<T>(int arrIndex)
        {
            _da.Delete<T>(arrIndex);
        }
        public void Delete<T>(string condition)
        {
            _da.Delete<T>(condition);
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
        public PageModel GetPageModel<T>(string fields, string table, string strWhere, string orderkey, string orderType, int currentPage, int pageSize) where T : IDicSerialize, new()
        {
            PageModel pm = new PageModel();
            pm.Pagesize = pageSize;
            pm.Page = currentPage;
            string sql = string.Format(" select count(1) from  [{0}]", table);
            if (!string.IsNullOrEmpty(strWhere)) { sql = string.Format("{0} where  {1}", sql, strWhere); }
            object obj = ExecuteScalar(sql);
            if (obj != null) { pm.TotalRecord = int.Parse(obj.ToString()); }
            DataTable dt = _da.Paging(fields, table, strWhere, orderkey, orderType, currentPage, pageSize);
            List<T> list = GetEntityList<T>(dt);
            pm.List = list;
            //  pm.List = Paging(fields, table, strWhere, orderkey, orderType, currentPage, pageSize);
            return pm;
        }
        #endregion


        #region 参数处理

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public IDbDataParameter CreateParameter(string name, object value, DbType type)
        {
            return _da.CreateParameter(name, value, type);
        }

        #endregion
    }

    //NjhDataMySqlProvider.cs
}


