using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _DestQueue : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["Root"] = Root;
            dic["Name"] = Name;
            dic["Data"] = Data;
            dic["Interval"] = Interval;
            dic["NextUpdate"] = NextUpdate;
            dic["ServerID"] = ServerID; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            Root=GetVal<String>(dic, "Root");
            Name=GetVal<String>(dic, "Name");
            Data=GetVal<String>(dic, "Data");
            Interval=GetVal<Int32>(dic, "Interval");
            NextUpdate=GetVal<DateTime>(dic, "NextUpdate");
            ServerID=GetVal<Int32>(dic, "ServerID"); 
        }
    }
}
