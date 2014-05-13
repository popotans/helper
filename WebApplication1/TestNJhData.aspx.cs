using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper.Database;
namespace WebApplication1
{
    public partial class TestNJhData : System.Web.UI.Page
    {
        NjhData db = new NjhData(DataType.Oledb, @"c:\data\nq.mdb");
        protected void Page_Load(object sender, EventArgs e)
        {
            BtnInsert.Click += new EventHandler(BtnInsert_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            Delete();
        }

        void btnUpdate_Click(object sender, EventArgs e)
        {
            Update();
            FindAll();
        }

        void BtnInsert_Click(object sender, EventArgs e)
        {
            Insert();
        }

        void Insert()
        {
            //AboutusInfo aui = new AboutusInfo()
            //{
            //    Classid = 111111,
            //    Content = "aboutus ingffo content",
            //    OrderNo = 999,
            //    Title = Guid.NewGuid().ToString(),
            //    Url = DateTime.Now.ToString()
            //};
            //object obj = db.Insert<AboutusInfo>(aui);

            //Helper.PageModel pm = db.GetPageModel<AboutusInfo>("*", "AboutusInfo", "", "idx", "asc", 3, 2);
            //IList<AboutusInfo> list = pm.List as List<AboutusInfo>;
            //Response.Write("count:" + list.Count);

            //foreach (AboutusInfo ai in list)
            //{
            //    Response.Write("<br/>" + ai.Title + ",,," + ai.Url);
            //}
            //Response.Write("total=" + pm.TotalRecord + ",pagecount=" + pm.TotalPage);
            // Response.Write("insert itno suc,the object num is " + obj);
        }

        void FindAll()
        {
            //var list = db.GetEntityList<AboutusInfo>("select * from AboutusInfo");
            //Response.Write("list count is:" + list.Count);
            //foreach (var l in list)
            //{
            //    Response.Write(l.Url + ",");
            //}
        }

        void Update()
        {
            //var abus = db.GetEntity<AboutusInfo>(" select top 1 * from AboutusInfo ");
            //Response.Write("oled Url=" + abus.Url);

            //abus.Url = DateTime.Now.ToString();
            //db.Update<AboutusInfo>(abus);
            //abus = db.GetEntity<AboutusInfo>(" select top 1 * from AboutusInfo ");
            //Response.Write("new Url=" + abus.Url);
        }

        void Delete()
        {
            db.Delete<AboutusInfo>(3);
        }
    }
}