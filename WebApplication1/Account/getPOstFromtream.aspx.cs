using Helper.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Account
{
    public partial class getPOstFromtream : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s = WebHelper.GetPostFromStream();
            s = HttpUtility.UrlDecode(s);
            Response.Write(s);
        }
    }
}