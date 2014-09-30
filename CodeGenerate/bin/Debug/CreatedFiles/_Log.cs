using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _Log
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 ProcInstID { get;set; }
        
        public String Data { get;set; }
        
        public Int32 ServerID { get;set; } 
    }
}
