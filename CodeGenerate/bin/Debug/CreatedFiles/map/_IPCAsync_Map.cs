using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _IPCAsync : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ItemID"] = ItemID;
            dic["Type"] = Type;
            dic["Date"] = Date;
            dic["ServerID"] = ServerID;
            dic["Retries"] = Retries; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ItemID=GetVal<Int32>(dic, "ItemID");
            Type=GetVal<Int32>(dic, "Type");
            Date=GetVal<DateTime>(dic, "Date");
            ServerID=GetVal<Int32>(dic, "ServerID");
            Retries=GetVal<Int32>(dic, "Retries"); 
        }
    }
}
