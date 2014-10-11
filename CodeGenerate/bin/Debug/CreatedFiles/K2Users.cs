using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace Hjn.Model
{
	public partial class K2Users
    {
		
        [AutoIncrease,PrimaryKey]
        public Int32 UserID { get;set; }
        
        public String UserName { get;set; }
        
        public String UserEmail { get;set; }
        
        public String UserDescription { get;set; }
        
        public Int32 ManagerID { get;set; }
        
        public String UserPassword { get;set; } 
    }
}
