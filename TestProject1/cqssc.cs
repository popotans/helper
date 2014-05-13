using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;using Helper;
namespace Xinshijie{
 public partial class cqssc {
     public Int32 Idx {get;set;} 
     public Int32 pdate {get;set;} 
     public Int32 pnum {get;set;} 
     public String Nums {get;set;} 
     public Int32 CpType {get;set;} 
     public DateTime kjshijian {get;set;} 
 }
 
 public partial class cqssc : BaseMap {
	public override IDictionary<string, object> Serialize() {
		Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
		dic["Idx"] = Idx;
		dic["pdate"] = pdate;
		dic["pnum"] = pnum;
		dic["Nums"] = Nums;
		dic["CpType"] = CpType;
		dic["kjshijian"] = kjshijian;
		dic["______TableName"] ="cqssc";
		dic["______AutoField"] ="idx";
		return dic;
		}
	public override void Deserialise(IDictionary<string, object> dic){
		Idx = GetVal<Int32>(dic, "Idx");
		pdate = GetVal<Int32>(dic, "pdate");
		pnum = GetVal<Int32>(dic, "pnum");
		Nums = GetVal<String>(dic, "Nums");
		CpType = GetVal<Int32>(dic, "CpType");
		kjshijian = GetVal<DateTime>(dic, "kjshijian");
		}
	}
}
