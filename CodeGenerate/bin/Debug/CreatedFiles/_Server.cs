using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _Server
    {
		
        
        public Int32 ID { get;set; }
        
        public String Name { get;set; }
        
        public Int32 Port { get;set; }
        
        public String LicenseKey { get;set; }
        
        public Int32 Processor { get;set; }
        
        public String ClusterConStr { get;set; }
        
        public Int32 Running { get;set; }
        
        public DateTime LastUpdate { get;set; } 
    }
}
