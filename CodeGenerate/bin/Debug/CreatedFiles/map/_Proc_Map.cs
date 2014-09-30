using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Proc : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcSetID"] = ProcSetID;
            dic["ExportID"] = ExportID;
            dic["MetaData"] = MetaData;
            dic["Priority"] = Priority;
            dic["ExpectedDuration"] = ExpectedDuration;
            dic["WorkID"] = WorkID;
            dic["LogLevel"] = LogLevel;
            dic["BusinessOwner"] = BusinessOwner;
            dic["TechnicalOwner"] = TechnicalOwner;
            dic["Language"] = Language;
            dic["Ver"] = Ver;
            dic["ChangeDate"] = ChangeDate; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            ProcSetID=GetVal<Int32>(dic, "ProcSetID");
            ExportID=GetVal<Int32>(dic, "ExportID");
            MetaData=GetVal<String>(dic, "MetaData");
            Priority=GetVal<Int32>(dic, "Priority");
            ExpectedDuration=GetVal<Int32>(dic, "ExpectedDuration");
            WorkID=GetVal<Int32>(dic, "WorkID");
            LogLevel=GetVal<Int32>(dic, "LogLevel");
            BusinessOwner=GetVal<String>(dic, "BusinessOwner");
            TechnicalOwner=GetVal<String>(dic, "TechnicalOwner");
            Language=GetVal<Int32>(dic, "Language");
            Ver=GetVal<Int32>(dic, "Ver");
            ChangeDate=GetVal<DateTime>(dic, "ChangeDate"); 
        }
    }
}
