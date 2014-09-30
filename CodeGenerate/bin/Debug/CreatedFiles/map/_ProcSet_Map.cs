using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ProcSet : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["Name"] = Name;
            dic["FullName"] = FullName;
            dic["Folder"] = Folder;
            dic["Descr"] = Descr;
            dic["ProcVerID"] = ProcVerID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            Name=GetVal<String>(dic, "Name");
            FullName=GetVal<String>(dic, "FullName");
            Folder=GetVal<String>(dic, "Folder");
            Descr=GetVal<String>(dic, "Descr");
            ProcVerID=GetVal<Int32>(dic, "ProcVerID"); 
        }
    }
}
