using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace Hjn.Model
{
	public partial class Employee
    {
		
        
        public String Account { get;set; }
        
        public String Email { get;set; }
        
        public String RoleName { get;set; }
        
        public String MgrAccount { get;set; } 
    }
}
