using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Reference : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ExportID"] = ExportID;
            dic["AssemblyID"] = AssemblyID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ExportID=GetVal<Int32>(dic, "ExportID");
            AssemblyID=GetVal<Int32>(dic, "AssemblyID"); 
        }
    }
}
