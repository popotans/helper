using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Helper
{
    public class MySqlModeCreate : BaseModeCreate
    {
        public MySqlModeCreate(string str)
        {
            this.db = new Helper.Database.NjhData(Helper.Database.DataType.Mysql, str);
        }

        public override List<string> GetTables(string dbName)
        {
            List<string> list = new List<string>() { };
            if (string.IsNullOrEmpty(dbName)) { return list; }
            string sql = "use " + dbName + "; show tables;";
            DataTable dt = db.ExecuteDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }

            return list;
        }

        public override List<DbColumn> GetDbColumns(string DbName, string tbName)
        {
            string sql = "select column_name,Data_Type,''`description`,table_Schema,`EXTRA` from information_schema.columns where table_name='" + tbName + "' AND table_Schema='" + DbName + "';";
            DataTable dt = db.ExecuteDataTable(sql);
            return GetDbColumns(dt);
        }

        public override List<DbColumn> GetDbColumns(DataTable dt)
        {
            List<DbColumn> listColumns = new List<DbColumn>();
            foreach (DataRow dr in dt.Rows)
            {
                string column_Name = dr["column_name"].ToString();
                string description = dr["description"].ToString();
                string dataType = (dr["data_type"].ToString());
                string tableSchema = dr["Table_Schema"].ToString();
                string extra = dr["extra"].ToString();
                if (dataType == "hierarchyid" || dataType == "varbinary" || dataType == "geography") continue;

                Type ColumnType = typeof(string);
                switch (dataType)
                {
                    case "bigint": ColumnType = typeof(Int32); break;
                    case "int": ColumnType = typeof(Int32); break;
                    case "tinyint": ColumnType = typeof(Int32); break;
                    case "smallint": ColumnType = typeof(Int32); break;

                    case "varchar": ColumnType = typeof(string); break;
                    case "char": ColumnType = typeof(string); break;
                    case "decimal": ColumnType = typeof(double); break;
                    case "enum": ColumnType = typeof(string); break;
                    case "longtext": ColumnType = typeof(string); break;
                    case "datetime": ColumnType = typeof(DateTime); break;
                    //  case "bit": ColumnType = typeof(Int32); break;
                    // case "float": ColumnType = typeof(float); break;
                    //  case "nchar": ColumnType = typeof(string); break;
                    // case "ntext": ColumnType = typeof(string); break;
                    case "text": ColumnType = typeof(string); break;
                    case "timestamp": ColumnType = typeof(long); break;
                    //    case "smallmoney": ColumnType = typeof(decimal); break;
                    //    case "date": ColumnType = typeof(DateTime); break;
                    //      case "money": ColumnType = typeof(decimal); break;
                    default: ColumnType = typeof(string); break;
                }

                bool isIdentity = extra == "auto_increment";
                listColumns.Add(new DbColumn
                {
                    ColumnName = column_Name,
                    ColumnType = ColumnType,
                    Description = description,
                 //   TableSchema = tableSchema,
                    IsAutoIncrement = isIdentity
                });
            }
            return listColumns;
        }

        public override List<DbColumn> GetDbColumns(List<string> tbNames)
        {
            return new List<DbColumn>();
        }

        public override List<string> GetDatabases()
        {
            List<string> list = new List<string>() { "" };
            string sql = "show databases; ";
            DataTable dt = db.ExecuteDataTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }
            return list;
        }

        //public override void CreateAll(string np,string dbname)
        //{
        //    if (string.IsNullOrEmpty(np)) np = "njh";
        //    List<string> tables = GetTables(dbname);
        //    foreach (string table in tables)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("using System;\r\n");
        //        sb.Append("using System.Data;\r\n");
        //        sb.Append("using System.Configuration;\r\n");
        //        sb.Append("using System.Collections;\r\n");
        //        sb.Append("using System.Collections.Generic;\r\n");

        //        sb.Append("using Helper;\r\n");
        //        sb.Append("namespace " + np + "{\r\n");

        //        List<DbColumn> dbcolumns = GetDbColumns(dbname, table);
        //        sb.Append(CreateOne(dbcolumns, table));

        //        sb.Append("}\r\n");
        //        string folder = AppDomain.CurrentDomain.BaseDirectory + "\\CreatedFiles\\";
        //        Helper.IO.FileHelper.WriteFile(folder + table + ".cs", sb.ToString(), "utf-8");
        //    }
        //}
    }
}
