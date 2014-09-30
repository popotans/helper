using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ProcGroup : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcSetID"] = ProcSetID;
            dic["Group"] = Group;
            dic["Admin"] = Admin;
            dic["Start"] = Start;
            dic["View"] = View;
            dic["ViewPart"] = ViewPart;
            dic["ServerEvent"] = ServerEvent; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcSetID=GetVal<Int32>(dic, "ProcSetID");
            Group=GetVal<String>(dic, "Group");
            Admin=GetVal<Int32>(dic, "Admin");
            Start=GetVal<Int32>(dic, "Start");
            View=GetVal<Int32>(dic, "View");
            ViewPart=GetVal<Int32>(dic, "ViewPart");
            ServerEvent=GetVal<Int32>(dic, "ServerEvent"); 
        }
    }
}
