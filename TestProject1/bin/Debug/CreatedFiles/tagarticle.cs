using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
	public partial class tagarticle
    {
		
        [AutoIncrease]
        public Int32 Idx { get;set; }
        
        public Int32 ArticleId { get;set; }
        
        public Int32 TagId { get;set; }
        
        public String Title { get;set; } 
    }
}
