using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _ActXml
    {
		
        
        public Int32 ActID { get;set; }
        
        public String Name { get;set; }
        
        public Int32 Hidden { get;set; }
        
        public Int32 Audit { get;set; }
        
        public Int32 OnDemand { get;set; }
        
        public String MetaData { get;set; }
        
        public String Schema { get;set; }
        
        public String Xsl { get;set; } 
    }
}
