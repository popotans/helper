using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
    public partial class files : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["Idx"] = Idx;
            dic["Path"] = Path;
            dic["Contenttype"] = Contenttype;
            dic["ActDate"] = ActDate;
            dic["Ext"] = Ext;
            dic["LocalName"] = LocalName;
            dic["FileSize"] = FileSize;
            dic["guids"] = guids;
            dic["UName"] = UName; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            Idx=GetVal<Int32>(dic, "Idx");
            Path=GetVal<String>(dic, "Path");
            Contenttype=GetVal<String>(dic, "Contenttype");
            ActDate=GetVal<DateTime>(dic, "ActDate");
            Ext=GetVal<String>(dic, "Ext");
            LocalName=GetVal<String>(dic, "LocalName");
            FileSize=GetVal<Int32>(dic, "FileSize");
            guids=GetVal<String>(dic, "guids");
            UName=GetVal<String>(dic, "UName"); 
        }
    }
}
