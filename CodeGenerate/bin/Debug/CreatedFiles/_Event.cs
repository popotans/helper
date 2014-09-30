using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _Event
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 ProcID { get;set; }
        
        public Int32 ActID { get;set; }
        
        public String Name { get;set; }
        
        public String Descr { get;set; }
        
        public String MetaData { get;set; }
        
        public Int32 Priority { get;set; }
        
        public Int32 ExpectedDuration { get;set; }
        
        public Int32 Pos { get;set; }
        
        public Int32 Type { get;set; }
        
        public Int32 UseTran { get;set; }
        
        public Int32 CodeID { get;set; }
        
        public Int32 ExcepID { get;set; } 
    }
}
