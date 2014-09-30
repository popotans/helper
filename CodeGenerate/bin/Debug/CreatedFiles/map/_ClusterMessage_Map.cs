using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ClusterMessage : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ServerID"] = ServerID;
            dic["Message"] = Message; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ServerID=GetVal<Int32>(dic, "ServerID");
            Message=GetVal<String>(dic, "Message"); 
        }
    }
}
