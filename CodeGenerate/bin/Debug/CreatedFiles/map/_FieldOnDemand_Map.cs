using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _FieldOnDemand : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcInstID"] = ProcInstID;
            dic["ID"] = ID;
            dic["Binary"] = Binary;
            dic["String"] = String; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            ID=GetVal<Int32>(dic, "ID");
            Binary=GetVal<String>(dic, "Binary");
            String=GetVal<String>(dic, "String"); 
        }
    }
}
