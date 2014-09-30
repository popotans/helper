using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Assembly : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["Name"] = Name;
            dic["Date"] = Date;
            dic["Assembly"] = Assembly; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            Name=GetVal<String>(dic, "Name");
            Date=GetVal<DateTime>(dic, "Date");
            Assembly=GetVal<String>(dic, "Assembly"); 
        }
    }
}
