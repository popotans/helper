using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
    public partial class user : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["idx"] = idx;
            dic["title"] = title;
            dic["pwd"] = pwd;
            dic["icon"] = icon;
            dic["indate"] = indate;
            dic["Status"] = Status;
            dic["Level"] = Level; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            idx=GetVal<Int32>(dic, "idx");
            title=GetVal<String>(dic, "title");
            pwd=GetVal<String>(dic, "pwd");
            icon=GetVal<String>(dic, "icon");
            indate=GetVal<DateTime>(dic, "indate");
            Status=GetVal<Int32>(dic, "Status");
            Level=GetVal<Int32>(dic, "Level"); 
        }
    }
}
