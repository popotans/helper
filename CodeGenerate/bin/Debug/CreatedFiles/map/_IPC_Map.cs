using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _IPC : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ID"] = ID;
            dic["Name"] = Name;
            dic["SrcProcID"] = SrcProcID;
            dic["SrcProcInstID"] = SrcProcInstID;
            dic["SrcEventInstID"] = SrcEventInstID;
            dic["DstProcInstID"] = DstProcInstID;
            dic["DstGuid"] = DstGuid;
            dic["DstServer"] = DstServer;
            dic["DstPort"] = DstPort;
            dic["DstConStr"] = DstConStr;
            dic["DstProc"] = DstProc;
            dic["DstFolio"] = DstFolio;
            dic["DstSync"] = DstSync;
            dic["OutFields"] = OutFields;
            dic["InFields"] = InFields;
            dic["Status"] = Status; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ID=GetVal<Int32>(dic, "ID");
            Name=GetVal<String>(dic, "Name");
            SrcProcID=GetVal<Int32>(dic, "SrcProcID");
            SrcProcInstID=GetVal<Int32>(dic, "SrcProcInstID");
            SrcEventInstID=GetVal<Int32>(dic, "SrcEventInstID");
            DstProcInstID=GetVal<Int32>(dic, "DstProcInstID");
            DstGuid=GetVal<Guid>(dic, "DstGuid");
            DstServer=GetVal<String>(dic, "DstServer");
            DstPort=GetVal<Int32>(dic, "DstPort");
            DstConStr=GetVal<String>(dic, "DstConStr");
            DstProc=GetVal<String>(dic, "DstProc");
            DstFolio=GetVal<String>(dic, "DstFolio");
            DstSync=GetVal<Int32>(dic, "DstSync");
            OutFields=GetVal<String>(dic, "OutFields");
            InFields=GetVal<String>(dic, "InFields");
            Status=GetVal<Int32>(dic, "Status"); 
        }
    }
}
