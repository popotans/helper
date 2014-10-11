using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper;
namespace WebApplication1.testAttributes
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TestAttribute rs = ClassHelper.ContainsAttributebyMethod<TestAttribute>(typeof(Person), "GetName");
            w(rs);
        }

        void w(TestAttribute s)
        {
            if (s == null) Response.Write("null");
            Response.Write(s + " " + "<br/>");
        }

    }

    [TestAttribute]
    public class Person
    {
        [TestAttribute]
        public int Id { get; set; }
        public string Name { get; set; }

        [MethodArrtibute("niejunhua")]
        public string GetName()
        {
            return "";
        }
    }

    public class TestAttribute : Attribute
    {

    }
    public class TypeAttribute : Attribute
    {

    }

    public class MethodArrtibute : Attribute
    {
        public string s { get; set; }
        public MethodArrtibute() { }
        public MethodArrtibute(string s)
        {
            this.s = s;
        }
    }
}