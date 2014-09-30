using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _Line
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 ProcID { get;set; }
        
        public String Name { get;set; }
        
        public String Descr { get;set; }
        
        public String MetaData { get;set; }
        
        public Int32 StartID { get;set; }
        
        public Int32 FinishID { get;set; }
        
        public Int32 LineRuleID { get;set; }
        
        public Int32 ExcepID { get;set; } 
    }
}
