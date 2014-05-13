using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Date111time : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dt = Helper.DateHelper.ConverttoCsharpTime(1332316850);
            Response.Write(dt.ToString());
            Response.Write("<br>");
            long ddd = Helper.DateHelper.ConverttoUnixTime(dt);
            Response.Write(ddd);


        }
    }
}