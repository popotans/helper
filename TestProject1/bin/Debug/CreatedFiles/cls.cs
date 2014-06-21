using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
	public partial class cls
    {
		
        [AutoIncrease]
        public Int32 Idx { get;set; }
        
        public String Title { get;set; }
        
        public Int32 ParentId { get;set; }
        
        public Int32 Disable { get;set; }
        
        public Int32 OrderIdx { get;set; }
        
        public Int32 SiteID { get;set; }
        
        public String Url { get;set; }
        
        public String ContentMode { get;set; }
        
        public String UName { get;set; }
        
        public Int32 IsFinal { get;set; } 
    }
}
