using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
    public partial class menus : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["Idx"] = Idx;
            dic["ParentId"] = ParentId;
            dic["Title"] = Title;
            dic["Url"] = Url;
            dic["OrderIndex"] = OrderIndex;
            dic["Target"] = Target;
            dic["UName"] = UName; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            Idx=GetVal<Int32>(dic, "Idx");
            ParentId=GetVal<Int32>(dic, "ParentId");
            Title=GetVal<String>(dic, "Title");
            Url=GetVal<String>(dic, "Url");
            OrderIndex=GetVal<Int32>(dic, "OrderIndex");
            Target=GetVal<String>(dic, "Target");
            UName=GetVal<String>(dic, "UName"); 
        }
    }
}
