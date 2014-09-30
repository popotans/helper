using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Line : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcID"] = ProcID;
            dic["Name"] = Name;
            dic["Descr"] = Descr;
            dic["MetaData"] = MetaData;
            dic["StartID"] = StartID;
            dic["FinishID"] = FinishID;
            dic["LineRuleID"] = LineRuleID;
            dic["ExcepID"] = ExcepID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            ProcID=GetVal<Int32>(dic, "ProcID");
            Name=GetVal<String>(dic, "Name");
            Descr=GetVal<String>(dic, "Descr");
            MetaData=GetVal<String>(dic, "MetaData");
            StartID=GetVal<Int32>(dic, "StartID");
            FinishID=GetVal<Int32>(dic, "FinishID");
            LineRuleID=GetVal<Int32>(dic, "LineRuleID");
            ExcepID=GetVal<Int32>(dic, "ExcepID"); 
        }
    }
}
