using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.testDatabaseMysql
{
    public partial class mysqlDb : System.Web.UI.Page
    {
        Helper.Database.NjhData db = new Helper.Database.NjhData(Helper.Database.DataType.Mysql, "Server=localhost;Uid=root;Pwd=niejunhua;DATABASE=nq");
        protected void Page_Load(object sender, EventArgs e)
        {
            btnGet.Click += new EventHandler(btnGet_Click);
            btnInsert.Click += new EventHandler(btnInsert_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btndel.Click += new EventHandler(btndel_Click);
        }

        void btndel_Click(object sender, EventArgs e)
        {
            db.Delete<NewsInfo>(" idx=6 ");
            Response.Write("del suc");
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            NewsInfo ni = db.GetEntityV2<NewsInfo>("select * from NewsInfo where idx=7");
            ni.Title = "this is the new title";
            ni.Indate = DateTime.Now;
            ni.Content = "asdasdasdsadasasd";

            db.UpdateV2<NewsInfo>(ni);
            Response.Write("update suc");
        }

        void btnInsert_Click(object sender, EventArgs e)
        {
            NewsInfo ni = new NewsInfo()
            {
                Title = "title" + DateTime.Now
                 ,
                Content = "mysql is a greate db",
                img1 = "/uploads/aaa.jpg",
                Indate = DateTime.Now,
                IsTop = 1,
                Price = 44M
            };

            db.InsertV2(ni);
            Response.Write("insert suc");
        }

        void btnGet_Click(object sender, EventArgs e)
        {

        }
    }
}