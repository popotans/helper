using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _ServerList
    {
		
        
        public Int32 ID { get;set; }
        
        public Int32 ProcInstID { get;set; }
        
        public Int32 ActInstDestID { get;set; }
        
        public Int32 EventInstID { get;set; }
        
        public Int32 ProcInstFieldID { get;set; }
        
        public Int32 ActInstDestFieldID { get;set; }
        
        public Int32 Verify { get;set; } 
    }
}
