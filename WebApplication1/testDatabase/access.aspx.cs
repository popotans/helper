using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper.Database;
using System.Data;
using Helper;
namespace WebApplication1.testDatabase
{
    public partial class access : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string dbPath = Server.MapPath("/App_Data/nq.mdb");

            DataTable dt = AccessHelper.ExecuteDataTable(dbPath, "select * from comclass");

          //  Response.Write(dt.Rows.Count);


            PageModel pm = AccessHelper.Paging(dbPath, "*", "comclass", "", "idx", "desc", 3, 3);

            foreach (DataRow dr in pm.DataTable.Rows)
            {
                Response.Write(dr["idx"].ToString() + "<br/>");
            }
        }
    }
}