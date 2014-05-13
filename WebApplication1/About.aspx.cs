using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
namespace WebApplication1
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Person pp = Activator.CreateInstance<Person>();
            //Response.Write("Name:" + pp.Name + "<br/>aaa=" + Person.IdentityKeys);
            //get(pp);
            Helper.Web.ResponseCatcherHandle.Instance.Init(Request.RawUrl, 1);
        }


        string get<T>(T d)
        {
            FieldInfo[] fis = d.GetType().GetFields();
            foreach (FieldInfo fi in fis)
            {
                Response.Write("<br/>" + fi.GetValue(null) + "..." + fi.Name);
            }

            // d.GetType().
            return null;
        }

        object GetTypeField<T>(T t, string name)
        {
            return t.GetType().GetField(name).GetValue(null);
        }


    }



    class Person
    {
        public const string IdentityKeys = "a,b,c,d";
        public const string ssss = "sssssdd";
        public static string nnn = "mmmnnn";

        public string Name { get; set; }
    }
}
