using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _ProcSet
    {
		
        
        public Int32 ID { get;set; }
        
        public String Name { get;set; }
        
        public String FullName { get;set; }
        
        public String Folder { get;set; }
        
        public String Descr { get;set; }
        
        public Int32 ProcVerID { get;set; } 
    }
}
