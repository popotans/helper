using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace Hjn.Model
{
    public partial class K2Users : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["UserID"] = UserID;
            dic["UserName"] = UserName;
            dic["UserEmail"] = UserEmail;
            dic["UserDescription"] = UserDescription;
            dic["ManagerID"] = ManagerID;
            dic["UserPassword"] = UserPassword; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            UserID=GetVal<Int32>(dic, "UserID");
            UserName=GetVal<String>(dic, "UserName");
            UserEmail=GetVal<String>(dic, "UserEmail");
            UserDescription=GetVal<String>(dic, "UserDescription");
            ManagerID=GetVal<Int32>(dic, "ManagerID");
            UserPassword=GetVal<String>(dic, "UserPassword"); 
        }
    }
}
