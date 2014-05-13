using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.testDatabase
{
    public partial class _default : System.Web.UI.Page
    {
        Helper.Database.NjhData db = new Helper.Database.NjhData(Helper.Database.DataType.Oledb, AppDomain.CurrentDomain.BaseDirectory + "app_data\\nq.mdb");

        protected void Page_Load(object sender, EventArgs e)
        {
            btnGet.Click += new EventHandler(btnGet_Click);
            btnInsert.Click += new EventHandler(btnInsert_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btndel.Click += new EventHandler(btndel_Click);
        }

        void btndel_Click(object sender, EventArgs e)
        {
            db.Delete<NewsInfo>(" idx=36 ");
            Response.Write("del suc");
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            NewsInfo ni = db.GetEntityV2<NewsInfo>("select * from NewsInfo where idx=35");

            ni.Content = "这是修改过后的content";
            ni.Img1 = "img.jpg";
            ni.InDate = DateTime.Now;

            db.UpdateV2<NewsInfo>(ni);
            Response.Write("update suc");
        }

        void btnInsert_Click(object sender, EventArgs e)
        {
            NewsInfo ni = new NewsInfo()
            {
                Title = "title" + DateTime.Now
            };

            db.InsertV2(ni);
            Response.Write("insert suc");
        }

        void btnGet_Click(object sender, EventArgs e)
        {

        }
    }
}