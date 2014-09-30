using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _ErrorProfile
    {
		
        
        public Int32 ID { get;set; }
        
        public String Name { get;set; }
        
        public Int32 ProcSetID { get;set; }
        
        public DateTime FromDate { get;set; }
        
        public DateTime ToDate { get;set; }
        
        public Int32 LineRule { get;set; }
        
        public Int32 PrecedingRule { get;set; }
        
        public Int32 StartRule { get;set; }
        
        public Int32 DestinationRule { get;set; }
        
        public Int32 SucceedingRule { get;set; }
        
        public Int32 EscalationRule { get;set; }
        
        public Int32 EscalationAction { get;set; }
        
        public Int32 ServerEvent { get;set; }
        
        public Int32 ClientEvent { get;set; }
        
        public Int32 IPC { get;set; } 
    }
}
