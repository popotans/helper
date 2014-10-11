using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace Hjn.Model
{
	public partial class K2Groups
    {
		
        [AutoIncrease,PrimaryKey]
        public Int32 GroupID { get;set; }
        
        public String GroupName { get;set; }
        
        public String GroupDescription { get;set; } 
    }
}
