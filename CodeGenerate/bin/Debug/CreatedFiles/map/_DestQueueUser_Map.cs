using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _DestQueueUser : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["QueueID"] = QueueID;
            dic["User"] = User; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            QueueID=GetVal<Int32>(dic, "QueueID");
            User=GetVal<String>(dic, "User"); 
        }
    }
}
