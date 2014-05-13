using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
namespace Coder
{
    public class OledbHelper
    {
        public string ConnStr { get; set; }
        public OleDbConnection conn;
        public OledbHelper(string s)
        {
            this.ConnStr = s;
            Helper.Database.AccessHelper db = new Helper.Database.AccessHelper(ConnStr);
            conn = db.CreateConnection();
        }

        private OleDbConnection GetConn()
        {
            Helper.Database.AccessHelper db = new Helper.Database.AccessHelper(ConnStr);
            OleDbConnection conn = db.CreateConnection();
            return conn;
        }

        public List<string> GetTables()
        {
            DataTable dt = conn.GetSchema("Tables");
            List<string> tablesList = new List<string>();
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

        public  List<DbColumn> GetColums(string tbName)
        {
            DataTable dt =
           conn.GetSchema("columns", new string[] { null, null, tbName });
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
            return listColumns;
        }

    }
}
