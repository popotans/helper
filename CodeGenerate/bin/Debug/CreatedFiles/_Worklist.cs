using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _Worklist
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 HeaderID { get;set; }
        
        public Int32 ProcInstID { get;set; }
        
        public Int32 ActInstID { get;set; }
        
        public Int32 ActInstDestID { get;set; }
        
        public Int32 ActInstDestFieldID { get;set; }
        
        public Int32 EventInstID { get;set; }
        
        public Int32 EIPriority { get;set; }
        
        public Int32 EIExpectedDuration { get;set; }
        
        public DateTime EIStartDate { get;set; }
        
        public String User { get;set; }
        
        public Int32 QueueID { get;set; }
        
        public String Platform { get;set; }
        
        public Int32 Status { get;set; }
        
        public String Data { get;set; }
        
        public Int32 Verify { get;set; } 
    }
}
