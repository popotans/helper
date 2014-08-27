using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper.Database;
using System.Data;

namespace Helper
{
    internal interface IModeCreate
    {
        List<string> GetTables(string dbName);
        List<string> GetDatabases();
        List<DbColumn> GetDbColumns(string DbName, string tbName);
        List<DbColumn> GetDbColumns(List<string> tbNames);

        List<DbColumn> GetDbColumns(DataTable dt);

    }


    public abstract class BaseModeCreate : IModeCreate
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
        public virtual void CreateAll(string np, string dbname)
        {
            if (string.IsNullOrEmpty(np)) np = "njh";
            List<string> tables = GetTables(dbname);
            foreach (string table in tables)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("using System;\r\n");
                sb.Append("using System.Data;\r\n");
                sb.Append("using System.Configuration;\r\n");
                sb.Append("using System.Collections;\r\n");
                sb.Append("using System.Collections.Generic;");

                sb.Append("using Helper;\r\n");
                sb.Append("namespace " + np + "{\r\n");

                List<DbColumn> dbcolumns = GetDbColumns(dbname, table);
                sb.Append(CreateOne(dbcolumns, table));

                sb.Append("}\r\n");
                string folder = AppDomain.CurrentDomain.BaseDirectory + "\\CreatedFiles\\";
                Helper.IO.FileHelper.WriteFile(folder + table + ".cs", sb.ToString(), "utf-8");
            }
        }

        public virtual void CreateAll(string np, string dbname, string tmpl)
        {
            string tmplMap = "";
            if (string.IsNullOrEmpty(tmpl))
            {
                //                tmpl = @"using System;
                //using System.Data;
                //using System.Configuration;
                //using System.Collections;
                //using System.Collections.Generic;
                //using Helper;
                //namespace @ns
                //{
                //	public partial class @tablename
                //    {
                //		@eb
                //        public @type @column { get;set; }@ee 
                //    }
                //
                //    public partial class @tablename : BaseMap
                //    {
                //        public override IDictionary<string, object> Serialize()
                //        {
                //            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
                //            @eb";
                //                tmpl += "dic[\"@column\"] = @column;@ee \r\n";
                //                tmpl += @"return dic;
                //        }
                //        public override void Deserialise(IDictionary<string, object> dic)
                //        {
                //            @eb";
                //                tmpl += "@column=GetVal<@type>(dic, \"@column\");@ee \r\n";
                //                tmpl += @";
                //        }
                //    }
                //}";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("using System;");
                sb.AppendLine("using System.Data;");
                sb.AppendLine("using System.Configuration;");
                sb.AppendLine("using System.Collections;");
                sb.AppendLine("using System.Collections.Generic;");
                sb.AppendLine("using Helper;");
                sb.AppendLine("namespace @ns");
                sb.AppendLine("{");
                sb.AppendLine("	public partial class @tablename");
                sb.AppendLine("    {");
                sb.AppendLine("		@eb");
                sb.AppendLine("        @attribute");
                sb.AppendLine("        public @type @column { get;set; }@ee ");
                sb.AppendLine("    }");
                sb.AppendLine("}");
                tmpl = sb.ToString();
                StringBuilder sb2 = new StringBuilder();
                sb2.AppendLine("using System;");
                sb2.AppendLine("using System.Data;");
                sb2.AppendLine("using System.Configuration;");
                sb2.AppendLine("using System.Collections;");
                sb2.AppendLine("using System.Collections.Generic;");
                sb2.AppendLine("using Helper;");
                sb2.AppendLine("namespace @ns");
                sb2.AppendLine("{");
                sb2.AppendLine("    public partial class @tablename : BaseMap");
                sb2.AppendLine("    {");
                sb2.AppendLine("        public override IDictionary<string, object> Serialize()");
                sb2.AppendLine("        {");
                sb2.AppendLine("            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);");
                sb2.AppendLine("            @eb");
                sb2.AppendLine("            dic[\"@column\"] = @column;@ee ");
                sb2.AppendLine("            return dic;");
                sb2.AppendLine("        }");
                sb2.AppendLine("        public override void Deserialise(IDictionary<string, object> dic)");
                sb2.AppendLine("        {");
                sb2.AppendLine("            @eb");
                sb2.AppendLine("            @column=GetVal<@type>(dic, \"@column\");@ee ");
                sb2.AppendLine("        }");
                sb2.AppendLine("    }");
                sb2.AppendLine("}");
                tmplMap = sb2.ToString();
            }
            if (string.IsNullOrEmpty(np)) np = "njh";
            List<string> tables = GetTables(dbname);
            foreach (string table in tables)
            {
                List<DbColumn> dbcolumns = GetDbColumns(dbname, table);
                string s = CreateOne(tmpl, dbcolumns, table, np);
                string folder = AppDomain.CurrentDomain.BaseDirectory + "\\CreatedFiles\\";
                Helper.IO.FileHelper.WriteFile(folder + table + ".cs", s, "utf-8");
                string sMap = CreateOne(tmplMap, dbcolumns, table, np);
                Helper.IO.FileHelper.WriteFile(folder + table + "_Map.cs", sMap, "utf-8");
            }
        }

        public virtual string CreateOne(string tmpl, List<DbColumn> dbcolumns, string table, string np)
        {
            tmpl = tmpl.Replace("@tablename", table);
            tmpl = tmpl.Replace("@ns", np);
            int i = 1;
            List<string> loopList = new List<string>();

            while (tmpl.IndexOf("@eb") >= 0)
            {
                string con = Helper.Str.StringHelper.SubStrContain(tmpl, "@eb", "@ee");
                loopList.Add(con);
                tmpl = tmpl.Replace(con, "@" + i.ToString());
                i++;
            }
            int i1 = 1;
            for (int qi = 0; qi < loopList.Count; qi++)
            {
                string item = loopList[qi];
                StringBuilder sb2 = new StringBuilder();
                foreach (DbColumn dc in dbcolumns)
                {
                    string cs = item;
                    cs = cs.Replace("@eb", "").Replace("@ee", "");
                    cs = cs.Replace("@type", dc.ColumnType.ToString().Replace("System.", ""));
                    cs = cs.Replace("@column", dc.ColumnName);
                    string attribute = "";
                    if (dc.IsAutoIncrement)
                    {
                        attribute = "AutoIncrease,";
                    }
                    if (dc.IsPrimaryKey)
                    {
                        attribute += "PrimaryKey,";
                    }
                    attribute = attribute.TrimEnd(',');
                    attribute = "[" + attribute + "]";
                    if (attribute == "[]") attribute = "";
                    cs = cs.Replace("@attribute", attribute);
                    sb2.Append(cs);
                }
                tmpl = tmpl.Replace("@" + (qi + 1), sb2.ToString());
            }
            return tmpl;
        }

        public virtual string CreateOne(List<DbColumn> dbcolumns, string table)
        {
            string idx = "";
            StringBuilder sb = new StringBuilder();
            //main
            sb.Append(" public partial class " + table + " {\r\n");
            foreach (DbColumn dc in dbcolumns)
            {
                if (dc.IsAutoIncrement) idx = dc.ColumnName;
                sb.Append("     public " + dc.ColumnType.ToString().Replace("System.", "") + " " + dc.ColumnName + " {get;set;} \r\n");
            }
            sb.Append(" }\r\n");
            sb.Append(" \r\n");
            sb.Append(" public partial class " + table + " : BaseMap {\r\n");

            //sb.Append(" public " + table + " (){\r\n");
            //sb.Append("     DbAutoField = \"" + idx + "\";\r\n");
            //sb.Append("     DbTableName = \"" + table + "\";\r\n}\r\n");

            sb.Append("\tpublic override IDictionary<string, object> Serialize() {\r\n");
            sb.Append("\t\tDictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);\r\n");
            foreach (DbColumn dc in dbcolumns)
            {
                sb.Append("\t\tdic[\"" + dc.ColumnName + "\"] = " + dc.ColumnName + ";\r\n");
            }
            //2014.05009
            sb.Append("\t\tdic[\"______TableName\"] =\"" + table + "\";\r\n");
            sb.Append("\t\tdic[\"______AutoField\"] =\"idx\";\r\n");

            sb.Append("\t\treturn dic;\r\n\t\t}\r\n");


            sb.Append("\tpublic override void Deserialise(IDictionary<string, object> dic){\r\n");
            foreach (DbColumn dc in dbcolumns)
            {
                sb.Append("\t\t" + dc.ColumnName + " = GetVal<" + dc.ColumnType.ToString().Replace("System.", "") + ">(dic, \"" + dc.ColumnName + "\");\r\n");
            }
            sb.Append("\t\t}\r\n");

            sb.Append("\t}\r\n");
            return sb.ToString();
        }
    }
}
