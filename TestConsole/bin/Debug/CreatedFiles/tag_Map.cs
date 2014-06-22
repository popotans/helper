using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
    public partial class tag : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["Idx"] = Idx;
            dic["Title"] = Title;
            dic["UseCount"] = UseCount; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            Idx=GetVal<Int32>(dic, "Idx");
            Title=GetVal<String>(dic, "Title");
            UseCount=GetVal<Int32>(dic, "UseCount"); 
        }
    }
}
