using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _WorklistRemove : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["WorklistID"] = WorklistID;
            dic["User"] = User;
            dic["Date"] = Date;
            dic["RowVer"] = RowVer; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            WorklistID=GetVal<Int32>(dic, "WorklistID");
            User=GetVal<String>(dic, "User");
            Date=GetVal<DateTime>(dic, "Date");
            RowVer=GetVal<String>(dic, "RowVer"); 
        }
    }
}
