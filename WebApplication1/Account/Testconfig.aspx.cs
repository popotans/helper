using Helper.Config;
using Helper.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Account
{
    public partial class Testconfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ButtonRead.Click += ButtonRead_Click;
            ButtonCreate.Click += ButtonCreate_Click;
        }

        void ButtonCreate_Click(object sender, EventArgs e)
        {
            Person pes = new Person()
            {
                Age = 11,
                Birthday = DateTime.Now,
                Name = "niejunhua",
                Salary = 5263
            };
            KVConfig.Instance.Save<Person>("c:\\person.config", pes);

        }

        void ButtonRead_Click(object sender, EventArgs e)
        {
            Person person=KVConfig.Instance.Read<Person>("c:\\person.config");
            string s = XmlSerialize.Serialize<Person>(person);
            rs.Text = s;
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public decimal Salary { get; set; }
    }
}