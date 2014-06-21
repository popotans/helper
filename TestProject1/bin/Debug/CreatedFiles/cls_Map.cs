using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
    public partial class cls : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["Idx"] = Idx;
            dic["Title"] = Title;
            dic["ParentId"] = ParentId;
            dic["Disable"] = Disable;
            dic["OrderIdx"] = OrderIdx;
            dic["SiteID"] = SiteID;
            dic["Url"] = Url;
            dic["ContentMode"] = ContentMode;
            dic["UName"] = UName;
            dic["IsFinal"] = IsFinal; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            Idx=GetVal<Int32>(dic, "Idx");
            Title=GetVal<String>(dic, "Title");
            ParentId=GetVal<Int32>(dic, "ParentId");
            Disable=GetVal<Int32>(dic, "Disable");
            OrderIdx=GetVal<Int32>(dic, "OrderIdx");
            SiteID=GetVal<Int32>(dic, "SiteID");
            Url=GetVal<String>(dic, "Url");
            ContentMode=GetVal<String>(dic, "ContentMode");
            UName=GetVal<String>(dic, "UName");
            IsFinal=GetVal<Int32>(dic, "IsFinal"); 
        }
    }
}
