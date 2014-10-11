using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace Hjn.Model
{
	public partial class K2UserGroup
    {
		
        [AutoIncrease,PrimaryKey]
        public Int32 UserGroupID { get;set; }
        
        public Int32 GroupID { get;set; }
        
        public Int32 UserID { get;set; } 
    }
}
