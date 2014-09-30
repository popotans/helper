using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _WorklistRemove
    {
		
        
        public Int32 WorklistID { get;set; }
        
        public String User { get;set; }
        
        public DateTime Date { get;set; }
        
        public String RowVer { get;set; } 
    }
}
