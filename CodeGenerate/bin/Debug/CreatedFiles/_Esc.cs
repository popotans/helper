using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _Esc
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 ProcID { get;set; }
        
        public Int32 ActID { get;set; }
        
        public String Name { get;set; }
        
        public String Descr { get;set; }
        
        public String MetaData { get;set; }
        
        public Int32 RuleID { get;set; }
        
        public Int32 ActionID { get;set; } 
    }
}
