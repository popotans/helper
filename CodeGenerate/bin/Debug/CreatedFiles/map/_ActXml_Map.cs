using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ActXml : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ActID"] = ActID;
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
            
            ActID=GetVal<Int32>(dic, "ActID");
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
