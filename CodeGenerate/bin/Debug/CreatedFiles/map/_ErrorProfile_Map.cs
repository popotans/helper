using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ErrorProfile : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["Name"] = Name;
            dic["ProcSetID"] = ProcSetID;
            dic["FromDate"] = FromDate;
            dic["ToDate"] = ToDate;
            dic["LineRule"] = LineRule;
            dic["PrecedingRule"] = PrecedingRule;
            dic["StartRule"] = StartRule;
            dic["DestinationRule"] = DestinationRule;
            dic["SucceedingRule"] = SucceedingRule;
            dic["EscalationRule"] = EscalationRule;
            dic["EscalationAction"] = EscalationAction;
            dic["ServerEvent"] = ServerEvent;
            dic["ClientEvent"] = ClientEvent;
            dic["IPC"] = IPC; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            Name=GetVal<String>(dic, "Name");
            ProcSetID=GetVal<Int32>(dic, "ProcSetID");
            FromDate=GetVal<DateTime>(dic, "FromDate");
            ToDate=GetVal<DateTime>(dic, "ToDate");
            LineRule=GetVal<Int32>(dic, "LineRule");
            PrecedingRule=GetVal<Int32>(dic, "PrecedingRule");
            StartRule=GetVal<Int32>(dic, "StartRule");
            DestinationRule=GetVal<Int32>(dic, "DestinationRule");
            SucceedingRule=GetVal<Int32>(dic, "SucceedingRule");
            EscalationRule=GetVal<Int32>(dic, "EscalationRule");
            EscalationAction=GetVal<Int32>(dic, "EscalationAction");
            ServerEvent=GetVal<Int32>(dic, "ServerEvent");
            ClientEvent=GetVal<Int32>(dic, "ClientEvent");
            IPC=GetVal<Int32>(dic, "IPC"); 
        }
    }
}
