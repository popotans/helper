using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Code : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcID"] = ProcID;
            dic["CodeType"] = CodeType;
            dic["CodeName"] = CodeName;
            dic["Code"] = Code; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            ProcID=GetVal<Int32>(dic, "ProcID");
            CodeType=GetVal<Int32>(dic, "CodeType");
            CodeName=GetVal<String>(dic, "CodeName");
            Code=GetVal<String>(dic, "Code"); 
        }
    }
}
