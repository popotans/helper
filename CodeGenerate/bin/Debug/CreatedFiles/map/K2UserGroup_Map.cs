using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace Hjn.Model
{
    public partial class K2UserGroup : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["UserGroupID"] = UserGroupID;
            dic["GroupID"] = GroupID;
            dic["UserID"] = UserID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            UserGroupID=GetVal<Int32>(dic, "UserGroupID");
            GroupID=GetVal<Int32>(dic, "GroupID");
            UserID=GetVal<Int32>(dic, "UserID"); 
        }
    }
}
