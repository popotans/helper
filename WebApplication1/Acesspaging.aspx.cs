using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace WebApplication1
{
    public partial class Acesspaging : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Helper.Database.AccessHelper db = new Helper.Database.AccessHelper("c:\\iwms.mdb");



            Helper.PageModel pm = db.GetPageModel("*", "iwms_news", "", "articleId", "asc", 10, 20);
            Response.Write(pm.Page + "," + pm.Nextpage + "," + pm.Prevpage + "," + pm.TotalRecord + "," + pm.TotalPage + "," + pm.Pagesize + "<br/>");

            DataTable dt = pm.List as DataTable;

            foreach (DataRow dr in dt.Rows)
            {
                Response.Write("<br>" + dr["title"]);
            }
        }
    }
}