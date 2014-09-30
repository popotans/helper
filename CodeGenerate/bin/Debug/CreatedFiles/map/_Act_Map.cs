using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Act : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcID"] = ProcID;
            dic["Name"] = Name;
            dic["Descr"] = Descr;
            dic["MetaData"] = MetaData;
            dic["Priority"] = Priority;
            dic["ExpectedDuration"] = ExpectedDuration;
            dic["WorkID"] = WorkID;
            dic["Slots"] = Slots;
            dic["UseTran"] = UseTran;
            dic["PrecRuleID"] = PrecRuleID;
            dic["StartRuleID"] = StartRuleID;
            dic["DestRuleID"] = DestRuleID;
            dic["SucRuleID"] = SucRuleID;
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
            Priority=GetVal<Int32>(dic, "Priority");
            ExpectedDuration=GetVal<Int32>(dic, "ExpectedDuration");
            WorkID=GetVal<Int32>(dic, "WorkID");
            Slots=GetVal<Int32>(dic, "Slots");
            UseTran=GetVal<Int32>(dic, "UseTran");
            PrecRuleID=GetVal<Int32>(dic, "PrecRuleID");
            StartRuleID=GetVal<Int32>(dic, "StartRuleID");
            DestRuleID=GetVal<Int32>(dic, "DestRuleID");
            SucRuleID=GetVal<Int32>(dic, "SucRuleID");
            ExcepID=GetVal<Int32>(dic, "ExcepID"); 
        }
    }
}
