using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _Server : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["Name"] = Name;
            dic["Port"] = Port;
            dic["LicenseKey"] = LicenseKey;
            dic["Processor"] = Processor;
            dic["ClusterConStr"] = ClusterConStr;
            dic["Running"] = Running;
            dic["LastUpdate"] = LastUpdate; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            Name=GetVal<String>(dic, "Name");
            Port=GetVal<Int32>(dic, "Port");
            LicenseKey=GetVal<String>(dic, "LicenseKey");
            Processor=GetVal<Int32>(dic, "Processor");
            ClusterConStr=GetVal<String>(dic, "ClusterConStr");
            Running=GetVal<Int32>(dic, "Running");
            LastUpdate=GetVal<DateTime>(dic, "LastUpdate"); 
        }
    }
}
