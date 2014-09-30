using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Worklist : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["HeaderID"] = HeaderID;
            dic["ProcInstID"] = ProcInstID;
            dic["ActInstID"] = ActInstID;
            dic["ActInstDestID"] = ActInstDestID;
            dic["ActInstDestFieldID"] = ActInstDestFieldID;
            dic["EventInstID"] = EventInstID;
            dic["EIPriority"] = EIPriority;
            dic["EIExpectedDuration"] = EIExpectedDuration;
            dic["EIStartDate"] = EIStartDate;
            dic["User"] = User;
            dic["QueueID"] = QueueID;
            dic["Platform"] = Platform;
            dic["Status"] = Status;
            dic["Data"] = Data;
            dic["Verify"] = Verify; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            HeaderID=GetVal<Int32>(dic, "HeaderID");
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            ActInstID=GetVal<Int32>(dic, "ActInstID");
            ActInstDestID=GetVal<Int32>(dic, "ActInstDestID");
            ActInstDestFieldID=GetVal<Int32>(dic, "ActInstDestFieldID");
            EventInstID=GetVal<Int32>(dic, "EventInstID");
            EIPriority=GetVal<Int32>(dic, "EIPriority");
            EIExpectedDuration=GetVal<Int32>(dic, "EIExpectedDuration");
            EIStartDate=GetVal<DateTime>(dic, "EIStartDate");
            User=GetVal<String>(dic, "User");
            QueueID=GetVal<Int32>(dic, "QueueID");
            Platform=GetVal<String>(dic, "Platform");
            Status=GetVal<Int32>(dic, "Status");
            Data=GetVal<String>(dic, "Data");
            Verify=GetVal<Int32>(dic, "Verify"); 
        }
    }
}
