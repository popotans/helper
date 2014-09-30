using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ProcRef : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcID"] = ProcID;
            dic["Name"] = Name;
            dic["FullName"] = FullName;
            dic["Gac"] = Gac; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcID=GetVal<Int32>(dic, "ProcID");
            Name=GetVal<String>(dic, "Name");
            FullName=GetVal<String>(dic, "FullName");
            Gac=GetVal<Int32>(dic, "Gac"); 
        }
    }
}
