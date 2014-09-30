using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _IPCReturn : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["DstProcInstID"] = DstProcInstID;
            dic["DstGuid"] = DstGuid;
            dic["DstProcID"] = DstProcID;
            dic["DstFolio"] = DstFolio;
            dic["DstUser"] = DstUser;
            dic["SrcServer"] = SrcServer;
            dic["SrcPort"] = SrcPort;
            dic["SrcProcInstID"] = SrcProcInstID;
            dic["SrcEventInstID"] = SrcEventInstID;
            dic["InFields"] = InFields;
            dic["Status"] = Status; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            DstProcInstID=GetVal<Int32>(dic, "DstProcInstID");
            DstGuid=GetVal<Guid>(dic, "DstGuid");
            DstProcID=GetVal<Int32>(dic, "DstProcID");
            DstFolio=GetVal<String>(dic, "DstFolio");
            DstUser=GetVal<String>(dic, "DstUser");
            SrcServer=GetVal<String>(dic, "SrcServer");
            SrcPort=GetVal<Int32>(dic, "SrcPort");
            SrcProcInstID=GetVal<Int32>(dic, "SrcProcInstID");
            SrcEventInstID=GetVal<Int32>(dic, "SrcEventInstID");
            InFields=GetVal<String>(dic, "InFields");
            Status=GetVal<Int32>(dic, "Status"); 
        }
    }
}
