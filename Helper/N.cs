using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Helper
{
    public class N
    {
        public static void Echo(string s)
        {
            if (HttpContext.Current == null) throw new ApplicationException("HttpContext.Current is NULL!");
            HttpContext.Current.Response.Write(s);
        }

        public static void Die(string s)
        {
            if (HttpContext.Current == null) throw new ApplicationException("HttpContext.Current is NULL!");
            HttpContext.Current.Response.Write(s);
            HttpContext.Current.Response.End();
        }

        public static void Alert(string msg)
        {
            HttpContext.Current.Response.Write("<script>alert('" + msg + "');</script>");
        }
        public static void AlertBack(System.Web.UI.Page pe, string msg)
        {
            HttpContext.Current.Response.Write("<script>alert('" + msg + "');history.back();</script>");

        }
        public static void AlertTo(System.Web.UI.Page pe, string msg, string to)
        {
            pe.ClientScript.RegisterStartupScript(pe.GetType(), "aasdasd", "<script>alert('" + msg + "');window.location='" + to + "';;</script>", false);
            HttpContext.Current.Response.Write("<script>alert('" + msg + "');window.location='" + to + "';;</script>");
        }
        public static void AlertClose(string msg)
        {
            HttpContext.Current.Response.Write("<script>alert('" + msg + "');window.close();</script>");
        }

        public static void Location(string url)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Location", url);
        }

        public static void LocationReferrer()
        {
            Uri url = HttpContext.Current.Request.UrlReferrer;
            if (url == null) throw new ApplicationException("UrlReferrer is NULL!");
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Location", url.ToString());
        }

    }
}
