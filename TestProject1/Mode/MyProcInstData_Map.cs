using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace TestProject1
{
    public partial class MyProcInstData : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcInstID"] = ProcInstID;
            dic["ID"] = ID;
            dic["Name"] = Name;
            dic["Value"] = Value; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            ID=GetVal<Int32>(dic, "ID");
            Name=GetVal<String>(dic, "Name");
            Value=GetVal<String>(dic, "Value"); 
        }
    }
}
