using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ProcInstDestQueue : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcInstID"] = ProcInstID;
            dic["QueueID"] = QueueID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            QueueID=GetVal<Int32>(dic, "QueueID"); 
        }
    }
}
