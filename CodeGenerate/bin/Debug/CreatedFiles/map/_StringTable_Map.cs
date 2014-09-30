using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _StringTable : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["Root"] = Root;
            dic["Name"] = Name;
            dic["Value"] = Value; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            Root=GetVal<String>(dic, "Root");
            Name=GetVal<String>(dic, "Name");
            Value=GetVal<String>(dic, "Value"); 
        }
    }
}
