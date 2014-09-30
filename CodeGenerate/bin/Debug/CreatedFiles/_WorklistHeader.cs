using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _WorklistHeader
    {
		
        
        public Int32 ProcInstID { get;set; }
        
        public Int32 ID { get;set; }
        
        public Int32 ActID { get;set; }
        
        public Int32 EventID { get;set; }
        
        public Int32 ProcInstFieldID { get;set; }
        
        public Int32 ActInstID { get;set; }
        
        public Int32 AIPriority { get;set; }
        
        public Int32 AIExpectedDuration { get;set; }
        
        public DateTime AIStartDate { get;set; }
        
        public Int32 AISlots { get;set; }
        
        public Int32 Instances { get;set; } 
    }
}
