using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ProcXml : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcID"] = ProcID;
            dic["Name"] = Name;
            dic["Hidden"] = Hidden;
            dic["Audit"] = Audit;
            dic["OnDemand"] = OnDemand;
            dic["MetaData"] = MetaData;
            dic["Schema"] = Schema;
            dic["Xsl"] = Xsl; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcID=GetVal<Int32>(dic, "ProcID");
            Name=GetVal<String>(dic, "Name");
            Hidden=GetVal<Int32>(dic, "Hidden");
            Audit=GetVal<Int32>(dic, "Audit");
            OnDemand=GetVal<Int32>(dic, "OnDemand");
            MetaData=GetVal<String>(dic, "MetaData");
            Schema=GetVal<String>(dic, "Schema");
            Xsl=GetVal<String>(dic, "Xsl"); 
        }
    }
}
