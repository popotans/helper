using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class Article : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["idx"] = idx;
            dic["title"] = title;
            dic["AddDate"] = AddDate;
            dic["updatetime"] = updatetime;
            dic["content"] = content;
            dic["stat"] = stat;
            dic["click"] = click;
            dic["class"] = class;
            dic["class2"] = class2;
            dic["class3"] = class3; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            idx=GetVal<Int32>(dic, "idx");
            title=GetVal<String>(dic, "title");
            AddDate=GetVal<DateTime>(dic, "AddDate");
            updatetime=GetVal<DateTime>(dic, "updatetime");
            content=GetVal<String>(dic, "content");
            stat=GetVal<Int32>(dic, "stat");
            click=GetVal<Int32>(dic, "click");
            class=GetVal<Int32>(dic, "class");
            class2=GetVal<Int32>(dic, "class2");
            class3=GetVal<Int32>(dic, "class3"); 
        }
    }
}
