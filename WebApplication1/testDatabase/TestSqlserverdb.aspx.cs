using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper;
using System.Data;
using System.Data.Common;
namespace WebApplication1.testDatabase
{
    public partial class TestSqlserverdb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string conn = "server=192.168.104.117;uid=wftsa;password=jd7nTF#wM;database=k2Sln";

            //object dt = db.ExecScalarInt(" select top 22 * from k2sln.dbo.TB_SNDA_Common_VirtualRole ");
            //Response.Write(dt);

            DbMap map = new DbMapSqlServer(conn);
        //    string sql = "select top 1 * from k2sln.dbo.TB_SNDA_Common_VirtualRole  where idx=27617";

            // IDataReader dr = map.db.GetReader(sql);
            // VirtualRole1 role = map.Get<VirtualRole1>(sql);
            // Response.Write(role.Proccode);

            //insert :
            //VirtualRole1 vr = new VirtualRole1()
            //{
            //    Proccode = "njhtext",
            //    RoleGroupCode = "njhtest",
            //    RoleGroupName = "njhtest",
            //    SpecialPersonid = "snda\\niejunhua",
            //};
            ////int rs = map.Insert<VirtualRole1>(vr);

            //// Response.Write(rs);
            //vr = new VirtualRole1();
            //vr = map.Get<VirtualRole1>(sql);
            //vr.Proccode = "njhprocvcode";
            //int idd = map.Update<VirtualRole1>(vr);
            //Response.Write(idd);

            SqlServerCore core = new SqlServerCore(conn);
            core.CreateAll("", "K2SLN");


        }
    }

    public class VirtualRole1 : BaseMap
    {
        public int IDx { get; set; }
        public string Proccode { get; set; }
        public string RoleGroupCode { get; set; }
        public string RoleGroupName { get; set; }
        public string SpecialClassCode { get; set; }
        public string SpecialClassname { get; set; }
        public string SpecialPersonname { get; set; }
        public string SpecialPersonid { get; set; }
        public int CheckOrder { get; set; }
        public int Type { get; set; }
        public double AuthMoney { get; set; }

        public VirtualRole1()
        {
           
        }

        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            dic["IDx"] = IDx;
            dic["Proccode"] = Proccode;
            dic["RoleGroupCode"] = RoleGroupCode;
            dic["RoleGroupName"] = RoleGroupName;
            dic["SpecialClassCode"] = SpecialClassCode;
            dic["SpecialClassname"] = SpecialClassname;
            dic["SpecialPersonname"] = SpecialPersonname;
            dic["SpecialPersonid"] = SpecialPersonid;
            dic["CheckOrder"] = CheckOrder;
            dic["Type"] = Type;
            dic["AuthMoney"] = AuthMoney;
            return dic;
        }

        public override void Deserialise(IDictionary<string, object> d)
        {
            IDx = GetVal<Int32>(d, "idx");
            Proccode = GetVal<string>(d, "proccode");
            RoleGroupCode = GetVal<string>(d, "RoleGroupCode");
            RoleGroupName = GetVal<string>(d, "RoleGroupName");
            SpecialClassCode = GetVal<string>(d, "SpecialClassCode");
            SpecialClassname = GetVal<string>(d, "SpecialClassname");
            SpecialPersonname = GetVal<string>(d, "SpecialPersonname");
            SpecialPersonid = GetVal<string>(d, "SpecialPersonid");

            CheckOrder = GetVal<Int32>(d, "checkorder");

            Type = GetVal<Int32>(d, "Type");

            AuthMoney = GetVal<double>(d, "AuthMoney");
        }
    }


}