using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ProcWork : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcID"] = ProcID;
            dic["WorkID"] = WorkID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcID=GetVal<Int32>(dic, "ProcID");
            WorkID=GetVal<Int32>(dic, "WorkID"); 
        }
    }
}
