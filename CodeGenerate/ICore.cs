using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Helper.Database;

namespace Coder
{
    public interface ICore
    {
        List<string> GetTables(string dbName);
        List<string> GetDatabases();
        List<DbColumn> GetDbColumns(string DbName, string tbName);
        List<DbColumn> GetDbColumns(List<string> tbNames);

        List<DbColumn> GetDbColumns(DataTable dt);

    }

    public abstract class BaseCore : ICore
    {
        protected NjhData db;
        public abstract List<string> GetTables(string dbName);
        public abstract List<DbColumn> GetDbColumns(string DbName, string tbName);
        public abstract List<DbColumn> GetDbColumns(List<string> tbNames);
        public virtual List<DbColumn> GetDbColumns(DataTable dt)
        {
            List<DbColumn> listColumns = new List<DbColumn>();
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

                    case "nvarchar": ColumnType = typeof(string); break;
                    case "varchar": ColumnType = typeof(string); break;
                    case "decimal": ColumnType = typeof(decimal); break;
                    case "datetime": ColumnType = typeof(DateTime); break;
                    case "bit": ColumnType = typeof(Int32); break;
                    case "float": ColumnType = typeof(float); break;
                    case "nchar": ColumnType = typeof(string); break;
                    case "ntext": ColumnType = typeof(string); break;
                    case "smallint": ColumnType = typeof(Int32); break;
                    case "text": ColumnType = typeof(Int32); break;
                    case "uniqueidentifier": ColumnType = typeof(Guid); break;
                    case "smallmoney": ColumnType = typeof(decimal); break;
                    case "date": ColumnType = typeof(DateTime); break;
                    case "money": ColumnType = typeof(decimal); break;
                    //case "tinyint": ColumnType = typeof(Int32); break;
                    //case "tinyint": ColumnType = typeof(Int32); break;
                    //case "tinyint": ColumnType = typeof(Int32); break;
                    //case "tinyint": ColumnType = typeof(Int32); break;

                    default: ColumnType = typeof(string); break;
                }

                listColumns.Add(new DbColumn
                {
                    ColumnName = column_Name,
                    ColumnType = ColumnType,
                    Description = description,
                    TableSchema = tableSchema
                });
            }
            return listColumns;
        }
        public abstract List<string> GetDatabases();
    }
}
