using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _Act
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 ProcID { get;set; }
        
        public String Name { get;set; }
        
        public String Descr { get;set; }
        
        public String MetaData { get;set; }
        
        public Int32 Priority { get;set; }
        
        public Int32 ExpectedDuration { get;set; }
        
        public Int32 WorkID { get;set; }
        
        public Int32 Slots { get;set; }
        
        public Int32 UseTran { get;set; }
        
        public Int32 PrecRuleID { get;set; }
        
        public Int32 StartRuleID { get;set; }
        
        public Int32 DestRuleID { get;set; }
        
        public Int32 SucRuleID { get;set; }
        
        public Int32 ExcepID { get;set; } 
    }
}
