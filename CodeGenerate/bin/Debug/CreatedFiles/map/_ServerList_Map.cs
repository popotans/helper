using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ServerList : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["ProcInstID"] = ProcInstID;
            dic["ActInstDestID"] = ActInstDestID;
            dic["EventInstID"] = EventInstID;
            dic["ProcInstFieldID"] = ProcInstFieldID;
            dic["ActInstDestFieldID"] = ActInstDestFieldID;
            dic["Verify"] = Verify; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            ActInstDestID=GetVal<Int32>(dic, "ActInstDestID");
            EventInstID=GetVal<Int32>(dic, "EventInstID");
            ProcInstFieldID=GetVal<Int32>(dic, "ProcInstFieldID");
            ActInstDestFieldID=GetVal<Int32>(dic, "ActInstDestFieldID");
            Verify=GetVal<Int32>(dic, "Verify"); 
        }
    }
}
