using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ProcInst : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcID"] = ProcID;
            dic["Status"] = Status;
            dic["StartDate"] = StartDate;
            dic["Originator"] = Originator;
            dic["Priority"] = Priority;
            dic["ExpectedDuration"] = ExpectedDuration;
            dic["Folio"] = Folio;
            dic["State"] = State;
            dic["ServerID"] = ServerID;
            dic["Version"] = Version;
            dic["Guid"] = Guid; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            ProcID=GetVal<Int32>(dic, "ProcID");
            Status=GetVal<Int32>(dic, "Status");
            StartDate=GetVal<DateTime>(dic, "StartDate");
            Originator=GetVal<String>(dic, "Originator");
            Priority=GetVal<Int32>(dic, "Priority");
            ExpectedDuration=GetVal<Int32>(dic, "ExpectedDuration");
            Folio=GetVal<String>(dic, "Folio");
            State=GetVal<String>(dic, "State");
            ServerID=GetVal<Int32>(dic, "ServerID");
            Version=GetVal<Int32>(dic, "Version");
            Guid=GetVal<Guid>(dic, "Guid"); 
        }
    }
}
