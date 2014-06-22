using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
    public partial class tagarticle : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["Idx"] = Idx;
            dic["ArticleId"] = ArticleId;
            dic["TagId"] = TagId;
            dic["Title"] = Title; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            Idx=GetVal<Int32>(dic, "Idx");
            ArticleId=GetVal<Int32>(dic, "ArticleId");
            TagId=GetVal<Int32>(dic, "TagId");
            Title=GetVal<String>(dic, "Title"); 
        }
    }
}
