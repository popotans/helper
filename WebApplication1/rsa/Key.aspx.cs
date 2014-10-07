using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper.Security;
namespace WebApplication1.rsa
{
    public partial class Key : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RsaEncrypt re = new RsaEncrypt();
            string privatek = "", publick = "";
            string[] arr = RsaEncrypt.GenerateKeys(ref privatek, ref publick);
            Response.Write(arr[0]);

            Response.Write("<br/><br/><br/><hr/><br/><br/><br/>");

            Response.Write(arr[0]);
        }
    }
}