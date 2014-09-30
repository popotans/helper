using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Esc : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcID"] = ProcID;
            dic["ActID"] = ActID;
            dic["Name"] = Name;
            dic["Descr"] = Descr;
            dic["MetaData"] = MetaData;
            dic["RuleID"] = RuleID;
            dic["ActionID"] = ActionID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            ProcID=GetVal<Int32>(dic, "ProcID");
            ActID=GetVal<Int32>(dic, "ActID");
            Name=GetVal<String>(dic, "Name");
            Descr=GetVal<String>(dic, "Descr");
            MetaData=GetVal<String>(dic, "MetaData");
            RuleID=GetVal<Int32>(dic, "RuleID");
            ActionID=GetVal<Int32>(dic, "ActionID"); 
        }
    }
}
