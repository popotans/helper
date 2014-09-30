using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _ErrorLog
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 ProcID { get;set; }
        
        public Int32 ProcInstID { get;set; }
        
        public Int32 State { get;set; }
        
        public Int32 Context { get;set; }
        
        public Int32 ObjectID { get;set; }
        
        public String Descr { get;set; }
        
        public DateTime Date { get;set; }
        
        public Int32 CodeID { get;set; }
        
        public String ItemName { get;set; } 
    }
}
