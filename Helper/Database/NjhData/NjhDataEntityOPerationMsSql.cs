using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Helper.DbDataType;
using System.Data.SqlClient;
using System.Data.Common;
namespace Helper.Database
{
    internal class NjhDataEntityOPerationMsSql : IEntityOPeration
    {
        private readonly NjhDataSqlExecutor _executor;
        public NjhDataEntityOPerationMsSql(NjhDataSqlExecutor executor)
        {
            this._executor = executor;
        }

        public object Insert<T>(T t)
        {
            throw new NotImplementedException();
        }
        public void Update<T>(T t)
        {
            throw new NotImplementedException();
        }


        private string GetIdentityColumnName<T>(T t) where T : IDicSerialize
        {
            string rs = "idx";
            IDictionary<string, MyField> dic = t.ToDic();
            foreach (string key in dic.Keys)
            {
                if (dic[key] != null && dic[key].IsIdentity)
                {
                    rs = key;
                    break;
                }
            }
            return rs;
        }

        public object InsertV2<T>(T t) where T : IDicSerialize
        {
            IDictionary<string, MyField> dic = t.ToDic();
            string tbname = t.TableName;
            StringBuilder sb = new StringBuilder(" insert into  ");
            sb.AppendFormat(tbname);
            sb.AppendFormat("(");
            int dicCount = dic.Count;
            int count = 0;
            foreach (string key in dic.Keys)
            {
                count++;
                if (!_executor.IsInsertColumn(key, dic))
                {
                    continue;
                }
                if (count == dicCount)
                    sb.AppendFormat("[{0}]) values (", key);
                else
                    sb.AppendFormat("[{0}],", key);
            }
            count = 0;
            foreach (string key in dic.Keys)
            {
                count++;
                if (!_executor.IsInsertColumn(key, dic))
                {
                    continue;
                }
                if (count == dicCount)
                    sb.AppendFormat("@{0})", key);
                else
                    sb.AppendFormat("@{0},", key);
            }
            object last = 0;
            _executor.ExecuteNonQuery(sb.ToString(), CommandType.Text, CreateParameter(dic));
            last = _executor.ExecuteScalar(string.Format("SELECT  max({1}) from [{0}]", tbname, GetIdentityColumnName<T>(t)));
            return last;
        }
        public void UpdateV2<T>(T t) where T : IDicSerialize
        {
            IDictionary<string, MyField> dic = t.ToDic();
            string tbname = t.TableName;
            StringBuilder sb = new StringBuilder(" update  ");
            sb.AppendFormat("{0}{1}{2}", "[", tbname, "]");
            sb.AppendFormat(" set ");
            int dicCount = dic.Count;
            int count = 0;
            foreach (string key in dic.Keys)
            {
                count++;
                if (!_executor.IsInsertColumn(key, dic))
                {
                    continue;
                }
                if (count == dicCount)
                    sb.AppendFormat("[{0}]=@{1} ", key, key);
                else
                    sb.AppendFormat("[{0}]=@{1},", key, key);
            }
            string where = string.Empty;
            foreach (string key in dic.Keys)
            {
                if (dic[key].IsIdentity)
                {
                    where = key;
                    break;
                }
            }
            if (where.Length == 0) throw new ApplicationException("未发现更新条件！");
            if (typeof(int) == dic[where].Type)
                sb.AppendFormat(" where [{0}]={1} ", where, dic[where].Data);
            else if (typeof(string) == dic[where].Type)
                sb.AppendFormat(" where [{0}]='{1}' ", where, dic[where].Data.ToString().Replace("'", "").Replace("--", ""));
            _executor.ExecuteNonQuery(sb.ToString(), CommandType.Text, CreateParameter(dic));
        }

        private DbParameter[] CreateParameter(IDictionary<string, MyField> dic)
        {
            List<DbParameter> arr = new List<DbParameter>();
            foreach (string key in dic.Keys)
            {
                if (!_executor.IsInsertColumn(key, dic)) continue;
                arr.Add(CreateMysqlParameter(dic, key));
            }
            return arr.ToArray();
        }

        private DbParameter CreateMysqlParameter(IDictionary<string, MyField> dic, string key)
        {
            MyField val = dic[key];
            //if (val.Data == null) val = "";
            Type tp = val.Data.GetType();
            SqlDbType dbType = SqlDbType.VarChar;
            if (tp == typeof(int))
            {
                dbType = SqlDbType.Int;
            }
            else if (tp == typeof(bool))
            {
                if ((bool)val.Data) val.Data = 1;
                else val.Data = 0;
                dbType = SqlDbType.Int;
            }
            else if (tp == typeof(string))
            {
                dbType = SqlDbType.VarChar;
                if (val.Data == null) val.Data = string.Empty;
            }
            else if (tp == typeof(decimal))
            {
                dbType = SqlDbType.Decimal;
            }
            else if (tp == typeof(double))
            {
                dbType = SqlDbType.Decimal;
            }
            else if (tp == typeof(float))
            {
                dbType = SqlDbType.Decimal;
            }
            else if (tp == typeof(DateTime))
            {
                dbType = SqlDbType.DateTime;
            }
            else if (tp == typeof(Guid))
            {
                dbType = SqlDbType.VarChar;
            }

            SqlParameter sps = new SqlParameter(string.Format("@{0}", key), val.Data);
            sps.SqlDbType = dbType;
            return sps;
        }
    }
}
