using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace Helper
{
    public class AccessCore : BaseCore
    {
        private string _connStr;

        public AccessCore(string str)
        {
            this._connStr = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + str;
            this.db = new Helper.Database.NjhData(Helper.Database.DataType.Oledb, _connStr);
        }

        private OleDbConnection GetConn()
        {
            OleDbConnection conn = new OleDbConnection(this._connStr);
            conn.Open();
            return conn;
        }

        private DataTable GetTable(string sql)
        {
            return db.ExecuteDataTable(sql);
        }

        public override List<string> GetTables(string dbName)
        {
            OleDbConnection conn = GetConn();
            DataTable dt = conn.GetSchema("Tables");

            List<string> tablesList = new List<string>();
            conn.Close();
            foreach (DataRow dr in dt.Rows)
            {
                string table_name = dr["table_name"].ToString();
                string table_type = dr["table_type"].ToString();
                if (string.Compare(table_type, "table", true) == 0)
                {
                    tablesList.Add(table_name);
                }
            }
            return tablesList;
        }

        public override List<DbColumn> GetDbColumns(string DbName, string tableName)
        {
            OleDbConnection conn = GetConn();
            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName });

            dt = conn.GetSchema("columns", new string[] { null, null, tableName });
            List<DbColumn> listColumns = new List<DbColumn>();
            foreach (DataRow dr in dt.Rows)
            {
                string column_Name = dr["column_name"].ToString();
                string description = dr["description"].ToString();
                int dataType = int.Parse(dr["data_type"].ToString());
                Type ColumnType = typeof(string);
                switch (dataType)
                {
                    case 2: ColumnType = typeof(int); ; break;
                    case 3: ColumnType = typeof(int); break;
                    case 4: ColumnType = typeof(Single); break;
                    case 5: ColumnType = typeof(Double); break;
                    case 6: ColumnType = typeof(decimal); break;
                    case 7: ColumnType = typeof(DateTime); break;
                    case 11: ColumnType = typeof(bool); break;
                    case 17: ColumnType = typeof(byte); break;
                    case 72: ColumnType = typeof(string); break;
                    case 130: ColumnType = typeof(string); break;
                    case 131: ColumnType = typeof(decimal); break;
                    case 128: ColumnType = typeof(string); break;
                    default: ColumnType = typeof(string); break;
                }

                listColumns.Add(new DbColumn
                {
                    ColumnName = column_Name,
                    ColumnType = ColumnType,
                    Description = description
                });
            }
            //获取表的identitykeys
            if (listColumns.Count > 0)
            {
                string keys = "Idx";
                listColumns[0].IdentityKeys = keys;

                foreach (DbColumn item in listColumns)
                {
                    if (string.Compare(item.ColumnName, "idx", true) == 0)
                    {
                        item.IsAutoIncrement = true;
                    }
                }
            }

            return listColumns;
        }

        public override List<DbColumn> GetDbColumns(List<string> tbNames)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetDatabases()
        {
            throw new NotImplementedException();
        }

        public override void CreateAll(string np, string dbname)
        {
            if (string.IsNullOrEmpty(np)) np = "njh";
            List<string> tables = GetTables("");
            foreach (string table in tables)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("using System;\r\n");
                sb.Append("using System.Data;\r\n");
                sb.Append("using System.Configuration;\r\n");
                sb.Append("using System.Collections;\r\n");
                sb.Append("using System.Collections.Generic;\r\n");
                sb.Append("using Helper;\r\n");
                sb.Append("namespace " + np + "{\r\n");

                List<DbColumn> dbcolumns = GetDbColumns("", table);
                sb.Append(CreateOne(dbcolumns, table));
                sb.Append(" }\r\n");
                string folder = AppDomain.CurrentDomain.BaseDirectory + "\\CreatedFiles\\";
                Helper.IO.FileHelper.WriteFile(folder + table + ".cs", sb.ToString(), "utf-8");
            }
        }
    }
}
