using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Helper.DbDataType;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Helper.Database
{
    class NjhDataEntityOPerationMysql : IEntityOPeration
    {
        private readonly NjhDataSqlExecutor _executor;
        public NjhDataEntityOPerationMysql(NjhDataSqlExecutor executor)
        {
            this._executor = executor;
        }

        public object Insert<T>(T t)
        {
            throw new NotImplementedException();
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
                    sb.AppendFormat("`{0}`) values (", key);
                else
                    sb.AppendFormat("`{0}`,", key);
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
                    sb.AppendFormat("?{0})", key);
                else
                    sb.AppendFormat("?{0},", key);
            }
            object last = 0;
            _executor.ExecuteNonQuery(sb.ToString(), CommandType.Text, CreateParameter(dic));
            last = _executor.ExecuteScalar("SELECT  max(idx) from `" + tbname + "`");
            return last;
        }

        public void Update<T>(T t)
        {
            throw new NotImplementedException();
        }

        public void UpdateV2<T>(T t) where T : IDicSerialize
        {
            IDictionary<string, MyField> dic = t.ToDic();
            string tbname = t.TableName;
            StringBuilder sb = new StringBuilder(" update  `");
            sb.AppendFormat(tbname);
            sb.AppendFormat("` set ");
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
                    sb.AppendFormat(" `{0}`=?{1} ", key, key);
                else
                    sb.AppendFormat(" `{0}`=?{1},", key, key);
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
                sb.AppendFormat(" where `{0}`={1} ", where, dic[where].Data);
            else if (typeof(string) == dic[where].Type)
                sb.AppendFormat(" where `{0}`='{1}' ", where, dic[where].Data.ToString().Replace("'", "").Replace("--", ""));
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
            MySqlDbType dbType = MySqlDbType.VarChar;
            if (tp == typeof(int))
            {
                dbType = MySqlDbType.Int64;
            }
            else if (tp == typeof(bool))
            {
                if ((bool)val.Data) val.Data = 1;
                else val.Data = 0;
                dbType = MySqlDbType.Int16;
            }
            else if (tp == typeof(string))
            {
                dbType = MySqlDbType.VarChar;
                if (val.Data == null) val.Data = string.Empty;
            }
            else if (tp == typeof(decimal))
            {
                dbType = MySqlDbType.Decimal;
            }
            else if (tp == typeof(double))
            {
                dbType = MySqlDbType.Double;
            }
            else if (tp == typeof(float))
            {
                dbType = MySqlDbType.Decimal;
            }
            else if (tp == typeof(DateTime))
            {
                dbType = MySqlDbType.Datetime;
            }
            else if (tp == typeof(Guid))
            {
                dbType = MySqlDbType.VarChar;
            }
            MySqlParameter sps = new MySqlParameter(string.Format("?{0}", key), val.Data);
            sps.MySqlDbType = dbType;
            return sps;
        }
    }
}
