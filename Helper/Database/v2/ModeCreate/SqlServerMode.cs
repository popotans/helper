using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Helper.Database;

namespace Helper
{
    public class DbColumn
    {
        public string ColumnName { get; set; }
        public Type ColumnType { get; set; }
        public string Description { get; set; }
        public bool IsAutoIncrement { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string TableSchema { get; set; }

        public string IdentityKeys { get; set; }
    }

    public class SqlServerModeCreate : BaseModeCreate
    {
        public SqlServerModeCreate(string str)
        {
            this.db = new Helper.Database.NjhData(Helper.Database.DataType.Mssql, str);
        }

        public override List<string> GetTables(string dbName)
        {
            List<string> list = new List<string>() { };
            if (string.IsNullOrEmpty(dbName)) { return list; }
            string sql = "SELECT * FROM " + dbName + "..SysObjects Where XType='U' ORDER BY Name";
            DataTable dt = db.ExecuteDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }

            return list;
        }

        public override List<string> GetDatabases()
        {
            List<string> list = new List<string>() { "" };
            string sql = "SELECT Name FROM Master..SysDatabases ORDER BY Name ";
            DataTable dt = db.ExecuteDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }
            return list;
        }
        private string _tbName { get; set; }
        public override List<DbColumn> GetDbColumns(string dbName, string tbName)
        {
            this._tbName = tbName;
            DataTable dt = db.ExecuteDataTable("select column_name,data_type,'' as [description],Table_Schema from " + dbName + ".information_schema.columns where table_name = '" + tbName + "'");
            return GetDbColumns(dt);

        }

        /// <summary>
        /// 获取表中的自增列
        /// </summary>
        /// <returns></returns>
        private List<string> GetSqlServerIdentityColumns(string tableName)
        {
            string sql = @"Select so.name Table_name,                   --表名字
       sc.name Iden_Column_name,             --自增字段名字
       ident_current(so.name) curr_value,    --自增字段当前值
       ident_incr(so.name) incr_value,       --自增字段增长值
       ident_seed(so.name) seed_value        --自增字段种子值
  from sysobjects so 
Inner Join syscolumns sc  on so.id = sc.id and columnproperty(sc.id, sc.name, 'IsIdentity') = 1
Where upper(so.name) = upper('{0}')";
            sql = string.Format(sql, tableName);
            DataTable dt = db.ExecuteDataTable(sql);
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string clm = dr[1].ToString();
                if (!list.Contains(clm))
                    list.Add(clm);
            }
            return list;
        }

        private List<string> GetSqlServerPrimarykeyColumns(string tbName)
        {
            string sql = "SELECT   name FROM SysColumns WHERE id=Object_Id('" + tbName + "') and colid in (select keyno from sysindexkeys where id=Object_Id('" + tbName + "'))";
            DataTable dt = db.ExecuteDataTable(sql);
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string clm = dr[0].ToString();
                if (!list.Contains(clm))
                    list.Add(clm);
            }
            return list;
        }


        public override List<DbColumn> GetDbColumns(List<string> tbName)
        {
            return null;
        }

        public override List<DbColumn> GetDbColumns(DataTable dt)
        {
            List<DbColumn> listColumns = new List<DbColumn>();

            List<string> columnsStrList = GetSqlServerIdentityColumns(this._tbName);
            List<string> columnPrmaryKey = GetSqlServerPrimarykeyColumns(_tbName);
            foreach (DataRow dr in dt.Rows)
            {
                string column_Name = dr["column_name"].ToString();
                string description = dr["description"].ToString();
                string dataType = (dr["data_type"].ToString());
                string tableSchema = dr["Table_Schema"].ToString();
                if (dataType == "hierarchyid" || dataType == "varbinary" || dataType == "geography") continue;


                Type ColumnType = typeof(string);
                switch (dataType)
                {
                    case "bigint": ColumnType = typeof(Int32); break;
                    case "int": ColumnType = typeof(Int32); break;
                    case "tinyint": ColumnType = typeof(Int32); break;
                    case "smallint": ColumnType = typeof(Int32); break;

                    case "nvarchar": ColumnType = typeof(string); break;
                    case "varchar": ColumnType = typeof(string); break;
                    case "decimal": ColumnType = typeof(decimal); break;
                    case "numeric": ColumnType = typeof(decimal); break;
                    case "real": ColumnType = typeof(decimal); break;
                    case "smallmoney": ColumnType = typeof(decimal); break;
                    case "datetime": ColumnType = typeof(DateTime); break;
                    case "smalldatetime": ColumnType = typeof(DateTime); break;
                    // case "timestamp": ColumnType = typeof(DateTime); break;


                    case "bit": ColumnType = typeof(Int32); break;
                    case "float": ColumnType = typeof(float); break;
                    case "nchar": ColumnType = typeof(string); break;
                    case "char": ColumnType = typeof(string); break;
                    case "ntext": ColumnType = typeof(string); break;

                    case "text": ColumnType = typeof(Int32); break;
                    case "uniqueidentifier": ColumnType = typeof(Guid); break;

                    case "date": ColumnType = typeof(DateTime); break;
                    case "money": ColumnType = typeof(decimal); break;
                    //case "tinyint": ColumnType = typeof(Int32); break;
                    //case "tinyint": ColumnType = typeof(Int32); break;
                    //case "tinyint": ColumnType = typeof(Int32); break;
                    //case "tinyint": ColumnType = typeof(Int32); break;

                    default: ColumnType = typeof(string); break;
                }
                DbColumn column = new DbColumn
                {
                    ColumnName = column_Name,
                    ColumnType = ColumnType,
                    Description = description,
                    TableSchema = tableSchema
                };

                foreach (string item in columnsStrList)
                {
                    if (item == column_Name)
                    {
                        column.IsAutoIncrement = true;
                        break;
                    }
                }

                foreach (string item in columnPrmaryKey)
                {
                    if (item == column_Name)
                    {
                        column.IsPrimaryKey = true;
                        break;
                    }
                }
                listColumns.Add(column);
            }
            return listColumns;
            //  return base.GetDbColumns(dt);
        }



        //public override void CreateAll(string np, string dbname, string tmpl)
        //{
        //    if (string.IsNullOrEmpty(np)) np = "njh";
        //    List<string> tables = GetTables(dbname);
        //    foreach (string table in tables)
        //    {
        //        List<DbColumn> dbcolumns = GetDbColumns(dbname, table);
        //        string s = CreateOne(tmpl, dbcolumns, table, np);
        //        string folder = AppDomain.CurrentDomain.BaseDirectory + "\\CreatedFiles\\";
        //        Helper.IO.FileHelper.WriteFile(folder + table + ".cs", s, "utf-8");
        //    }
        //}

    }
}
