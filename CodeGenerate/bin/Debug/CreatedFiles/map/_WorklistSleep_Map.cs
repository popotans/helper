using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _WorklistSleep : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["HeaderID"] = HeaderID;
            dic["Status"] = Status;
            dic["AsyncID"] = AsyncID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            HeaderID=GetVal<Int32>(dic, "HeaderID");
            Status=GetVal<Int32>(dic, "Status");
            AsyncID=GetVal<Int32>(dic, "AsyncID"); 
        }
    }
}
