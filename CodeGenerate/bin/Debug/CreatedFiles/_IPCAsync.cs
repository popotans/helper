using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _IPCAsync
    {
		
        
        public Int32 ItemID { get;set; }
        
        public Int32 Type { get;set; }
        
        public DateTime Date { get;set; }
        
        public Int32 ServerID { get;set; }
        
        public Int32 Retries { get;set; } 
    }
}
