using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
	public partial class files
    {
		
        [AutoIncrease]
        public Int32 Idx { get;set; }
        
        public String Path { get;set; }
        
        public String Contenttype { get;set; }
        
        public DateTime ActDate { get;set; }
        
        public String Ext { get;set; }
        
        public String LocalName { get;set; }
        
        public Int32 FileSize { get;set; }
        
        public String guids { get;set; }
        
        public String UName { get;set; } 
    }
}
