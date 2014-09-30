using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class Article
    {
		
        
        public Int32 idx { get;set; }
        
        public String title { get;set; }
        
        public DateTime AddDate { get;set; }
        
        public DateTime updatetime { get;set; }
        
        public String content { get;set; }
        
        public Int32 stat { get;set; }
        
        public Int32 click { get;set; }
        
        public Int32 class { get;set; }
        
        public Int32 class2 { get;set; }
        
        public Int32 class3 { get;set; } 
    }
}
