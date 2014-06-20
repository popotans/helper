using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace TestProject1
{
    public class TableAttribute : Attribute
    {
        public string TableName { get; set; }

        public TableAttribute()
        {

        }
    }

    [Table(TableName = "_ProcInstDataAudit")]
    public partial class _ProcInstDataAudit
    {
       
        public Int32 ProcInstID { get; set; }
        public String Name { get; set; }
        public String User { get; set; }
        public String Location { get; set; }
        public String Value { get; set; }
        public DateTime Date { get; set; }
    }

    public partial class _ProcInstDataAudit : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            dic["ProcInstID"] = ProcInstID; dic["Name"] = Name; dic["User"] = User; dic["Location"] = Location; dic["Value"] = Value; dic["Date"] = Date;
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            ProcInstID = GetVal<Int32>(dic, "ProcInstID"); Name = GetVal<String>(dic, "Name"); User = GetVal<String>(dic, "User"); Location = GetVal<String>(dic, "Location"); Value = GetVal<String>(dic, "Value"); Date = GetVal<DateTime>(dic, "Date");
            ;
        }
    }
}