using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _WorkWeek : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["WorkID"] = WorkID;
            dic["Day"] = Day;
            dic["StartHour"] = StartHour;
            dic["StartMinute"] = StartMinute;
            dic["FinishHour"] = FinishHour;
            dic["FinishMinute"] = FinishMinute; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            WorkID=GetVal<Int32>(dic, "WorkID");
            Day=GetVal<Int32>(dic, "Day");
            StartHour=GetVal<Int32>(dic, "StartHour");
            StartMinute=GetVal<Int32>(dic, "StartMinute");
            FinishHour=GetVal<Int32>(dic, "FinishHour");
            FinishMinute=GetVal<Int32>(dic, "FinishMinute"); 
        }
    }
}
