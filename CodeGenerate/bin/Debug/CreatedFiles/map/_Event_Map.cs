using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Event : BaseMap
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
            dic["Priority"] = Priority;
            dic["ExpectedDuration"] = ExpectedDuration;
            dic["Pos"] = Pos;
            dic["Type"] = Type;
            dic["UseTran"] = UseTran;
            dic["CodeID"] = CodeID;
            dic["ExcepID"] = ExcepID; 
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
            Priority=GetVal<Int32>(dic, "Priority");
            ExpectedDuration=GetVal<Int32>(dic, "ExpectedDuration");
            Pos=GetVal<Int32>(dic, "Pos");
            Type=GetVal<Int32>(dic, "Type");
            UseTran=GetVal<Int32>(dic, "UseTran");
            CodeID=GetVal<Int32>(dic, "CodeID");
            ExcepID=GetVal<Int32>(dic, "ExcepID"); 
        }
    }
}
