using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _DestQueue
    {
		
        
        public Int32 ID { get;set; }
        
        public String Root { get;set; }
        
        public String Name { get;set; }
        
        public String Data { get;set; }
        
        public Int32 Interval { get;set; }
        
        public DateTime NextUpdate { get;set; }
        
        public Int32 ServerID { get;set; } 
    }
}
