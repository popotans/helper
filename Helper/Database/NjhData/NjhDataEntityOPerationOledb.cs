using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using Helper.DbDataType;

namespace Helper.Database
{
    internal interface IEntityOPeration
    {
        object Insert<T>(T t);
        object InsertV2<T>(T t) where T : IDicSerialize;
        void Update<T>(T t);
        void UpdateV2<T>(T t) where T : IDicSerialize;
    }

    class OledbEntityOperation : IEntityOPeration
    {
        private readonly MyCompare _compare = new MyCompare();
        private readonly OleDbConnection conn;
        private readonly NjhDataSqlExecutor _executor;

        public OledbEntityOperation(NjhDataSqlExecutor factory)
        {
            this.conn = (OleDbConnection)factory.CreateConnection();
            this._executor = factory;
        }

        public object Insert<T>(T t)
        {
            StringBuilder sb = null;
            List<PropertyInfo> list = DataMapper.GetProperties<T>();

            string tbName = typeof(T).Name;
            #region  获取表中的所有列
            List<string> columns = new List<string>();
            columns = GetAccessTbColumns(conn, tbName);
            list = list.Where(x => columns.Contains(x.Name, _compare)).ToList();
            #endregion

            #region 判断需要过滤的字段
            //不需要插入数据库的列
            List<string> filteredColumns = ReflectionHelper.GetNotInsertedColumns<T>();
            //new List<string>();
            //filteredColumns.Add(ReflectionHelper.GetTypeField_IdentityKeys<T>());
            //返回过滤后的列列表
            if (filteredColumns.Count > 0)
                list = list.Where(x => !filteredColumns.Contains(x.Name, _compare)).ToList();
            #endregion

            int size = list.Count;
            sb = new StringBuilder(string.Format("insert into [{0}] (", tbName));
            for (int i = 0; i < size; i++)
            {
                if (filteredColumns.Contains(list[i].Name, _compare)) continue;
                if (i != size - 1)
                {
                    sb.AppendFormat("[{0}],", list[i].Name);
                }
                else
                {
                    sb.AppendFormat("[{0}]) values(", list[i].Name);
                }
            }
            object last = "0";
            PropertyInfo pi = null;
            for (int i = 0; i < size; i++)
            {
                pi = list[i];
                if (filteredColumns.Contains(list[i].Name, _compare)) continue;
                if (i != size - 1)
                {
                    sb.AppendFormat("@{0},", pi.Name);
                }
                else
                {
                    sb.AppendFormat("@{0})", pi.Name);
                }
            }
            _executor.ExecuteNonQuery(sb.ToString(), CommandType.Text, CreateParameter(list, filteredColumns, t, _compare));
            last = _executor.ExecuteScalar("SELECT  max(idx) from [" + tbName + "]");
            return last;
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
                if (!IsInsertColumn(key, dic))
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
                if (!IsInsertColumn(key, dic))
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
            last = _executor.ExecuteScalar("SELECT  max(idx) from [" + tbname + "]");
            return last;
        }

        public void Update<T>(T t)
        {
            string tbName = typeof(T).Name;
            List<PropertyInfo> list = DataMapper.GetProperties<T>();
            List<string> columns = GetAccessTbColumns(conn, tbName);
            List<string> filteredColumns = ReflectionHelper.GetIdentityColumn<T>();
            //new List<string>();
            //            filteredColumns.Add(ReflectionHelper.GetTypeField_IdentityKeys<T>());
            //过滤掉 表中没有的列
            list = list.Where(x => columns.Contains(x.Name, _compare)).ToList();


            int size = list.Count;
            int i = 0;
            string idx = string.Empty;
            StringBuilder sb = new StringBuilder(string.Format("update [{0}] set ", tbName));
            foreach (PropertyInfo pi in list)
            {
                if (filteredColumns.Contains(pi.Name, _compare))
                {
                    idx = string.Format(" where {0}={1}", filteredColumns[0], pi.GetValue(t, null));
                }
                else
                {
                    if (i != size - 1)
                        sb.AppendFormat("[{0}]=@{1},", pi.Name, pi.Name);
                    else
                        sb.AppendFormat("[{0}]=@{1} ", pi.Name, pi.Name);
                }
                i++;
            }
            if (string.IsNullOrEmpty(idx)) throw new ApplicationException("未发现合适的更新条件，为保护数据，终止了此次操作.....");
            //  sb = new StringBuilder(sb.ToString().TrimEnd(','));
            sb.AppendFormat(idx);
            _executor.ExecuteNonQuery(sb.ToString(), CreateParameter(list, filteredColumns, t, _compare));
        }

        public void UpdateV2<T>(T t) where T : IDicSerialize
        {
            IDictionary<string, MyField> dic = t.ToDic();
            string tbname = t.TableName;
            StringBuilder sb = new StringBuilder(" update  ");
            sb.AppendFormat("[{0}]", tbname);
            sb.AppendFormat(" set ");
            int dicCount = dic.Count;
            int count = 0;
            foreach (string key in dic.Keys)
            {
                count++;
                if (!IsInsertColumn(key, dic))
                {
                    continue;
                }
                if (count == dicCount)
                    sb.AppendFormat(" [{0}]=@{1} ", key, key);
                else
                    sb.AppendFormat(" [{0}]=@{1},", key, key);
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
            if (string.IsNullOrEmpty(where))
            {
                foreach (string key in dic.Keys)
                {
                    if (string.Compare(key, "idx", true) == 0)
                    {
                        where = key;
                        break;
                    }
                }
            }
            if (where.Length == 0) throw new ApplicationException("未发现更新条件！");
            if (typeof(int) == dic[where].Type)
                sb.AppendFormat(" where [{0}]={1} ", where, dic[where].Data);
            else if (typeof(string) == dic[where].Type)
                sb.AppendFormat(" where [{0}]='{1}' ", where, dic[where].Data.ToString().Replace("'", "").Replace("--", ""));

            _executor.ExecuteNonQuery(sb.ToString(), CommandType.Text, CreateParameter(dic));
        }

        private DbParameter[] CreateParameter<T>(List<PropertyInfo> list, T dbModel, MyCompare _compare)
        {
            List<string> filters = ReflectionHelper.GetNotInsertedColumns<T>();
            //new List<string>();
            //filters.Add(ReflectionHelper.GetTypeField_IdentityKeys<T>());

            List<DbParameter> arr = new List<DbParameter>();
            foreach (PropertyInfo pi in list)
            {
                if (filters.Contains(pi.Name, _compare)) continue;
                arr.Add(CreateOledbParameter(pi, dbModel));
            }
            return arr.ToArray();
        }

        private DbParameter[] CreateParameter<T>(List<PropertyInfo> list, List<string> notInsertedColumns, T dbModel, MyCompare _compare)
        {
            List<string> filters = notInsertedColumns;
            //new List<string>();
            //filters.Add(ReflectionHelper.GetTypeField_IdentityKeys<T>());

            List<DbParameter> arr = new List<DbParameter>();
            foreach (PropertyInfo pi in list)
            {
                if (filters.Contains(pi.Name, _compare)) continue;
                arr.Add(CreateOledbParameter(pi, dbModel));
            }
            return arr.ToArray();
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

        private DbParameter CreateOledbParameter(IDictionary<string, MyField> dic, string key)
        {
            MyField val = dic[key];
            //if (val.Data == null) val = "";
            Type tp = val.Data.GetType();
            OleDbType type = OleDbType.VarChar;
            if (tp == typeof(int))
            {
                type = OleDbType.Integer;
            }
            else if (tp == typeof(bool))
            {
                if ((bool)val.Data) val.Data = 1;
                else val.Data = 0;
                type = OleDbType.Integer;
            }
            else if (tp == typeof(string))
            {
                type = OleDbType.VarChar;
                if (val.Data == null) val.Data = string.Empty;
            }
            else if (tp == typeof(decimal))
            {
                type = OleDbType.Decimal;
            }
            else if (tp == typeof(double))
            {
                type = OleDbType.Decimal;
            }
            else if (tp == typeof(float))
            {
                type = OleDbType.Decimal;
            }
            else if (tp == typeof(DateTime))
            {
                type = OleDbType.Date;
            }
            else if (tp == typeof(Guid))
            {
                type = OleDbType.VarChar;
            }
            OleDbParameter sps = new OleDbParameter(string.Format("@{0}", key), val.Data);
            sps.OleDbType = type;
            return sps;
        }

        private DbParameter[] CreateParameter(IDictionary<string, MyField> dic)
        {
            List<DbParameter> arr = new List<DbParameter>();
            foreach (string key in dic.Keys)
            {
                if (!IsInsertColumn(key, dic)) continue;
                arr.Add(CreateOledbParameter(dic, key));
            }
            return arr.ToArray();
        }

        private List<string> GetAccessTbColumns(OleDbConnection conn, string tableName)
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

        private bool IsInsertColumn(string column)
        {
            if (column == "___TableName" || column.IndexOf("#identity") > 0)
                return false;
            else
                return true;
        }

        private bool IsInsertColumn(string column, IDictionary<string, MyField> dic)
        {
            //bool rs = true;
            //foreach (string item in dic.Keys)
            //{
            //    if (column == item)
            //    {
            //        if (dic[column].IsIdentity)
            //        {
            //            rs = false;
            //            break;
            //        }
            //    }
            //}
            //return rs;
            return _executor.IsInsertColumn(column, dic);
        }
    }
}
