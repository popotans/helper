using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _WorklistHeader : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcInstID"] = ProcInstID;
            dic["ID"] = ID;
            dic["ActID"] = ActID;
            dic["EventID"] = EventID;
            dic["ProcInstFieldID"] = ProcInstFieldID;
            dic["ActInstID"] = ActInstID;
            dic["AIPriority"] = AIPriority;
            dic["AIExpectedDuration"] = AIExpectedDuration;
            dic["AIStartDate"] = AIStartDate;
            dic["AISlots"] = AISlots;
            dic["Instances"] = Instances; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            ID=GetVal<Int32>(dic, "ID");
            ActID=GetVal<Int32>(dic, "ActID");
            EventID=GetVal<Int32>(dic, "EventID");
            ProcInstFieldID=GetVal<Int32>(dic, "ProcInstFieldID");
            ActInstID=GetVal<Int32>(dic, "ActInstID");
            AIPriority=GetVal<Int32>(dic, "AIPriority");
            AIExpectedDuration=GetVal<Int32>(dic, "AIExpectedDuration");
            AIStartDate=GetVal<DateTime>(dic, "AIStartDate");
            AISlots=GetVal<Int32>(dic, "AISlots");
            Instances=GetVal<Int32>(dic, "Instances"); 
        }
    }
}
