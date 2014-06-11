using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace TestProject1
{
    public partial class TB_SNDA_Common_VirtualRole
    {

        public Int32 IDX { get; set; }
        public String ProcCode { get; set; }
        public String RoleGroupCode { get; set; }
        public String RoleGroupName { get; set; }
        public String SpecialClassCode { get; set; }
        public String SpecialClassName { get; set; }
        public String SpecialPersonName { get; set; }
        public String SpecialPersonId { get; set; }
        public Int32 CheckOrder { get; set; }
        public String Type { get; set; }
        public String AuthMoney { get; set; }
    }

    public partial class TB_SNDA_Common_VirtualRole : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            dic["IDX"] = IDX; dic["ProcCode"] = ProcCode; dic["RoleGroupCode"] = RoleGroupCode; dic["RoleGroupName"] = RoleGroupName; dic["SpecialClassCode"] = SpecialClassCode; dic["SpecialClassName"] = SpecialClassName; dic["SpecialPersonName"] = SpecialPersonName; dic["SpecialPersonId"] = SpecialPersonId; dic["CheckOrder"] = CheckOrder; dic["Type"] = Type; dic["AuthMoney"] = AuthMoney;
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            IDX = GetVal<Int32>(dic, "IDX"); ProcCode = GetVal<String>(dic, "ProcCode"); RoleGroupCode = GetVal<String>(dic, "RoleGroupCode"); RoleGroupName = GetVal<String>(dic, "RoleGroupName"); SpecialClassCode = GetVal<String>(dic, "SpecialClassCode"); SpecialClassName = GetVal<String>(dic, "SpecialClassName"); SpecialPersonName = GetVal<String>(dic, "SpecialPersonName"); SpecialPersonId = GetVal<String>(dic, "SpecialPersonId"); CheckOrder = GetVal<Int32>(dic, "CheckOrder"); Type = GetVal<String>(dic, "Type"); AuthMoney = GetVal<String>(dic, "AuthMoney");
            ;
        }
    }
}