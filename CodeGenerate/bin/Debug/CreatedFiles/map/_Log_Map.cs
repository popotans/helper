using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Log : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcInstID"] = ProcInstID;
            dic["Data"] = Data;
            dic["ServerID"] = ServerID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            Data=GetVal<String>(dic, "Data");
            ServerID=GetVal<Int32>(dic, "ServerID"); 
        }
    }
}
