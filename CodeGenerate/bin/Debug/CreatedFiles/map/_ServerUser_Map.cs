using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ServerUser : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["User"] = User;
            dic["Admin"] = Admin;
            dic["Export"] = Export; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            User=GetVal<String>(dic, "User");
            Admin=GetVal<Int32>(dic, "Admin");
            Export=GetVal<Int32>(dic, "Export"); 
        }
    }
}
