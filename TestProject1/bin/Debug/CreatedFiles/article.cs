using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
	public partial class article
    {
		
        [AutoIncrease]
        public Int32 Idx { get;set; }
        
        public String Title { get;set; }
        
        public String Content { get;set; }
        
        public String Icon { get;set; }
        
        public Int32 Click { get;set; }
        
        public Int32 Istop { get;set; }
        
        public DateTime Indate { get;set; }
        
        public String Url { get;set; }
        
        public Int32 authorid { get;set; }
        
        public Int32 cid { get;set; }
        
        public String kwd { get;set; }
        
        public String desc { get;set; }
        
        public String SourceTxt { get;set; }
        
        public String SourceUrl { get;set; }
        
        public String AuthorNick { get;set; }
        
        public String guids { get;set; }
        
        public String img1 { get;set; }
        
        public String img2 { get;set; }
        
        public String img3 { get;set; }
        
        public String img4 { get;set; }
        
        public String img5 { get;set; }
        
        public String img6 { get;set; }
        
        public String img7 { get;set; }
        
        public String img8 { get;set; }
        
        public String img9 { get;set; }
        
        public String img10 { get;set; }
        
        public String Tags { get;set; }
        
        public String UName { get;set; }
        
        public String ContentMode { get;set; }
        
        public String Subject { get;set; } 
    }
}
