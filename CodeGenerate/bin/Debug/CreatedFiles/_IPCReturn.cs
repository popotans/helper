using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _IPCReturn
    {
		
        
        public Int32 DstProcInstID { get;set; }
        
        public Guid DstGuid { get;set; }
        
        public Int32 DstProcID { get;set; }
        
        public String DstFolio { get;set; }
        
        public String DstUser { get;set; }
        
        public String SrcServer { get;set; }
        
        public Int32 SrcPort { get;set; }
        
        public Int32 SrcProcInstID { get;set; }
        
        public Int32 SrcEventInstID { get;set; }
        
        public String InFields { get;set; }
        
        public Int32 Status { get;set; } 
    }
}
