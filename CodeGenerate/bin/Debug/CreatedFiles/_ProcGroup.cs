using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _ProcGroup
    {
		
        
        public Int32 ProcSetID { get;set; }
        
        public String Group { get;set; }
        
        public Int32 Admin { get;set; }
        
        public Int32 Start { get;set; }
        
        public Int32 View { get;set; }
        
        public Int32 ViewPart { get;set; }
        
        public Int32 ServerEvent { get;set; } 
    }
}
