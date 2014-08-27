using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace njh
{
    public partial class list
    {
        public String classname { get; set; }
        public String href { get; set; }
        public Int32 idx { get; set; }
        public String title { get; set; }
    }

    public partial class list : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            dic["classname"] = classname;
            dic["href"] = href;
            dic["idx"] = idx;
            dic["title"] = title;
            dic["______TableName"] = "list";
            dic["______AutoField"] = "idx";
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            classname = GetVal<String>(dic, "classname");
            href = GetVal<String>(dic, "href");
            idx = GetVal<Int32>(dic, "idx");
            title = GetVal<String>(dic, "title");
        }
    }
}
