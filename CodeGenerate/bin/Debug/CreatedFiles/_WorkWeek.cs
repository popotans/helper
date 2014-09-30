using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _WorkWeek
    {
		
        
        public Int32 WorkID { get;set; }
        
        public Int32 Day { get;set; }
        
        public Int32 StartHour { get;set; }
        
        public Int32 StartMinute { get;set; }
        
        public Int32 FinishHour { get;set; }
        
        public Int32 FinishMinute { get;set; } 
    }
}
