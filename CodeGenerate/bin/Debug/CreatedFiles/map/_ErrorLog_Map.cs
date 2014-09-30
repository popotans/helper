using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ErrorLog : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcID"] = ProcID;
            dic["ProcInstID"] = ProcInstID;
            dic["State"] = State;
            dic["Context"] = Context;
            dic["ObjectID"] = ObjectID;
            dic["Descr"] = Descr;
            dic["Date"] = Date;
            dic["CodeID"] = CodeID;
            dic["ItemName"] = ItemName; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            ProcID=GetVal<Int32>(dic, "ProcID");
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            State=GetVal<Int32>(dic, "State");
            Context=GetVal<Int32>(dic, "Context");
            ObjectID=GetVal<Int32>(dic, "ObjectID");
            Descr=GetVal<String>(dic, "Descr");
            Date=GetVal<DateTime>(dic, "Date");
            CodeID=GetVal<Int32>(dic, "CodeID");
            ItemName=GetVal<String>(dic, "ItemName"); 
        }
    }
}
