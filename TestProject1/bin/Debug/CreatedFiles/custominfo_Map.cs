using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
    public partial class custominfo : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["Idx"] = Idx;
            dic["Tag"] = Tag;
            dic["Title"] = Title;
            dic["Content"] = Content;
            dic["Indate"] = Indate;
            dic["guid"] = guid;
            dic["Mode"] = Mode;
            dic["UName"] = UName; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            Idx=GetVal<Int32>(dic, "Idx");
            Tag=GetVal<String>(dic, "Tag");
            Title=GetVal<String>(dic, "Title");
            Content=GetVal<String>(dic, "Content");
            Indate=GetVal<DateTime>(dic, "Indate");
            guid=GetVal<String>(dic, "guid");
            Mode=GetVal<Int32>(dic, "Mode");
            UName=GetVal<String>(dic, "UName"); 
        }
    }
}
