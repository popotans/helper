using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace njh
{
    public partial class Test
    {

        [AutoIncrease, PrimaryKey]
        public Int32 ID { get; set; }
        public Int32 BatID { get; set; }
        public Int32 ActID { get; set; }
    }

    public partial class Test : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            dic["ID"] = ID; dic["BatID"] = BatID; dic["ActID"] = ActID;
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            ID = GetVal<Int32>(dic, "ID"); BatID = GetVal<Int32>(dic, "BatID"); ActID = GetVal<Int32>(dic, "ActID");
            ;
        }
    }
}