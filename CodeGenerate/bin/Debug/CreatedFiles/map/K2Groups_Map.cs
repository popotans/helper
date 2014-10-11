using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace Hjn.Model
{
    public partial class K2Groups : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["GroupID"] = GroupID;
            dic["GroupName"] = GroupName;
            dic["GroupDescription"] = GroupDescription; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            GroupID=GetVal<Int32>(dic, "GroupID");
            GroupName=GetVal<String>(dic, "GroupName");
            GroupDescription=GetVal<String>(dic, "GroupDescription"); 
        }
    }
}
