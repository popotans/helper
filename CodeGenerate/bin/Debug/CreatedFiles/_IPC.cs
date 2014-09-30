using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace HJN
{
	public partial class _IPC
    {
		
        
        public Int32 ID { get;set; }
        
        public String Name { get;set; }
        
        public Int32 SrcProcID { get;set; }
        
        public Int32 SrcProcInstID { get;set; }
        
        public Int32 SrcEventInstID { get;set; }
        
        public Int32 DstProcInstID { get;set; }
        
        public Guid DstGuid { get;set; }
        
        public String DstServer { get;set; }
        
        public Int32 DstPort { get;set; }
        
        public String DstConStr { get;set; }
        
        public String DstProc { get;set; }
        
        public String DstFolio { get;set; }
        
        public Int32 DstSync { get;set; }
        
        public String OutFields { get;set; }
        
        public String InFields { get;set; }
        
        public Int32 Status { get;set; } 
    }
}
