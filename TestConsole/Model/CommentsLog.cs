using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TestConsole.Model
{

    using System.Collections.Generic;
    using Helper.DbDataType;


    public partial class BudgetItemList
    {

        public System.Int32 ID { get; set; }
        public System.String SN { get; set; }
        public System.String Folio { get; set; }
        public System.String ProcessInstanceID { get; set; }
        public System.String ProcessInstanceName { get; set; }
        public System.String Company { get; set; }
        public System.String Creator { get; set; }
        public System.DateTime CreateDay { get; set; }
        public System.String BudgetID { get; set; }
        public System.String Members { get; set; }
        public System.String TotalAmout { get; set; }
        public System.String Msg { get; set; }
        public string TableName { get { return "BudgetItemList"; } }

    }

    public partial class BudgetItemList : Helper.IDicSerialize
    {

        public System.Collections.Generic.IDictionary<string, MyField> ToDic()
        {
            Dictionary<string, MyField> dic = new Dictionary<string, MyField>();
            dic["ID"] = new MyField(this.ID, typeof(System.Int32), true);
            dic["SN"] = new MyField(this.SN, typeof(System.String));
            dic["Folio"] = new MyField(this.Folio, typeof(System.String));
            dic["ProcessInstanceID"] = new MyField(this.ProcessInstanceID, typeof(System.String));
            dic["ProcessInstanceName"] = new MyField(this.ProcessInstanceName, typeof(System.String));
            dic["Company"] = new MyField(this.Company, typeof(System.String));
            dic["Creator"] = new MyField(this.Creator, typeof(System.String));
            dic["CreateDay"] = new MyField(this.CreateDay, typeof(System.DateTime));
            dic["BudgetID"] = new MyField(this.BudgetID, typeof(System.String));
            dic["Members"] = new MyField(this.Members, typeof(System.String));
            dic["TotalAmout"] = new MyField(this.TotalAmout, typeof(System.String));
            dic["Msg"] = new MyField(this.Msg, typeof(System.String));
            return dic;
        }

        public object ToObj(System.Collections.Generic.IDictionary<string, MyField> dic)
        {
            this.ID = (System.Int32)(((MyField)dic["ID"]).Data);
            this.SN = (System.String)(((MyField)dic["SN"]).Data);
            this.Folio = (System.String)(((MyField)dic["Folio"]).Data);
            this.ProcessInstanceID = (System.String)(((MyField)dic["ProcessInstanceID"]).Data);
            this.ProcessInstanceName = (System.String)(((MyField)dic["ProcessInstanceName"]).Data);
            this.Company = (System.String)(((MyField)dic["Company"]).Data);
            this.Creator = (System.String)(((MyField)dic["Creator"]).Data);
            this.CreateDay = (System.DateTime)(((MyField)dic["CreateDay"]).Data);
            this.BudgetID = (System.String)(((MyField)dic["BudgetID"]).Data);
            this.Members = (System.String)(((MyField)dic["Members"]).Data);
            this.TotalAmout = (System.String)(((MyField)dic["TotalAmout"]).Data);
            this.Msg = (System.String)(((MyField)dic["Msg"]).Data);
            return this;
        }
    }




}
