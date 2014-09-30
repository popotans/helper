using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _ProcInst
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 ProcID { get;set; }
        
        public Int32 Status { get;set; }
        
        public DateTime StartDate { get;set; }
        
        public String Originator { get;set; }
        
        public Int32 Priority { get;set; }
        
        public Int32 ExpectedDuration { get;set; }
        
        public String Folio { get;set; }
        
        public String State { get;set; }
        
        public Int32 ServerID { get;set; }
        
        public Int32 Version { get;set; }
        
        public Guid Guid { get;set; } 
    }
}
