using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace Hjn.Model
{
    public partial class Employee : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["Account"] = Account;
            dic["Email"] = Email;
            dic["RoleName"] = RoleName;
            dic["MgrAccount"] = MgrAccount; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            Account=GetVal<String>(dic, "Account");
            Email=GetVal<String>(dic, "Email");
            RoleName=GetVal<String>(dic, "RoleName");
            MgrAccount=GetVal<String>(dic, "MgrAccount"); 
        }
    }
}
