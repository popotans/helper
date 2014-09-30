using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Async : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcInstID"] = ProcInstID;
            dic["ItemID"] = ItemID;
            dic["Type"] = Type;
            dic["Date"] = Date;
            dic["ServerID"] = ServerID;
            dic["Status"] = Status; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            ItemID=GetVal<Int32>(dic, "ItemID");
            Type=GetVal<Int32>(dic, "Type");
            Date=GetVal<DateTime>(dic, "Date");
            ServerID=GetVal<Int32>(dic, "ServerID");
            Status=GetVal<Int32>(dic, "Status"); 
        }
    }
}
