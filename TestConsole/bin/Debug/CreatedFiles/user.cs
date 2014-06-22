using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
	public partial class user
    {
		
        [AutoIncrease]
        public Int32 idx { get;set; }
        
        public String title { get;set; }
        
        public String pwd { get;set; }
        
        public String icon { get;set; }
        
        public DateTime indate { get;set; }
        
        public Int32 Status { get;set; }
        
        public Int32 Level { get;set; } 
    }
}
