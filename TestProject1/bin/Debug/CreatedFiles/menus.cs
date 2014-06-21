using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
	public partial class menus
    {
		
        [AutoIncrease]
        public Int32 Idx { get;set; }
        
        public Int32 ParentId { get;set; }
        
        public String Title { get;set; }
        
        public String Url { get;set; }
        
        public Int32 OrderIndex { get;set; }
        
        public String Target { get;set; }
        
        public String UName { get;set; } 
    }
}
