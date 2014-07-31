using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WebApplication1
{
    public partial class TestdbUtility : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("idx", typeof(int));
            dt.Columns.Add("title", typeof(string));
            dt.Columns.Add("money", typeof(decimal));
            dt.Columns.Add("birthday", typeof(DateTime));
            DataRow dr = dt.NewRow();
            dr[0] = 1;
            dr[1] = "title11";
            dr[2] = 56.36D;
            dr[3] = DateTime.Now;
            dt.Rows.Add(dr);

            object money = Helper.Database.DbUtility.GetDbValue<DateTime>(dt.Rows[0], "birthday");
            Response.Write(money);

        }
    }
}