using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _Proc
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 ProcSetID { get;set; }
        
        public Int32 ExportID { get;set; }
        
        public String MetaData { get;set; }
        
        public Int32 Priority { get;set; }
        
        public Int32 ExpectedDuration { get;set; }
        
        public Int32 WorkID { get;set; }
        
        public Int32 LogLevel { get;set; }
        
        public String BusinessOwner { get;set; }
        
        public String TechnicalOwner { get;set; }
        
        public Int32 Language { get;set; }
        
        public Int32 Ver { get;set; }
        
        public DateTime ChangeDate { get;set; } 
    }
}
