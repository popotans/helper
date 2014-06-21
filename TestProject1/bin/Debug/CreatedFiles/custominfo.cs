using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
	public partial class custominfo
    {
		
        [AutoIncrease]
        public Int32 Idx { get;set; }
        
        public String Tag { get;set; }
        
        public String Title { get;set; }
        
        public String Content { get;set; }
        
        public DateTime Indate { get;set; }
        
        public String guid { get;set; }
        
        public Int32 Mode { get;set; }
        
        public String UName { get;set; } 
    }
}
