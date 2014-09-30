﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
    public partial class _ProcData : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["ProcID"] = ProcID;
            dic["Name"] = Name;
            dic["Hidden"] = Hidden;
            dic["Audit"] = Audit;
            dic["OnDemand"] = OnDemand;
            dic["Type"] = Type;
            dic["Value"] = Value;
            dic["MetaData"] = MetaData; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            ProcID=GetVal<Int32>(dic, "ProcID");
            Name=GetVal<String>(dic, "Name");
            Hidden=GetVal<Int32>(dic, "Hidden");
            Audit=GetVal<Int32>(dic, "Audit");
            OnDemand=GetVal<Int32>(dic, "OnDemand");
            Type=GetVal<Int32>(dic, "Type");
            Value=GetVal<String>(dic, "Value");
            MetaData=GetVal<String>(dic, "MetaData"); 
        }
    }
}
