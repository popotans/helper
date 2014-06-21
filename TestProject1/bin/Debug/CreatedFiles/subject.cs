using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
	public partial class subject
    {
		
        [AutoIncrease]
        public Int32 Idx { get;set; }
        
        public String Title { get;set; }
        
        public String Guid { get;set; } 
    }
}
