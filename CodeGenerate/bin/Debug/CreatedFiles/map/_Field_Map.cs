using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Field : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcInstID"] = ProcInstID;
            dic["ID"] = ID;
            dic["Data"] = Data;
            dic["Xml"] = Xml; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcInstID=GetVal<Int32>(dic, "ProcInstID");
            ID=GetVal<Int32>(dic, "ID");
            Data=GetVal<String>(dic, "Data");
            Xml=GetVal<String>(dic, "Xml"); 
        }
    }
}
