using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper.Database;
namespace WebApplication1
{
    public partial class AccesstestOperation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NjhData db = new NjhData(DataType.Oledb, @"E:\快盘\project\XidongCaiService\XidongCaiService\XidongNet.mdb");
            //object obj = db.Insert<Category>(new Category
            //     {
            //         Title = DateTime.Now.ToString()
            //     });
            //       Response.Write(obj);

            //Category cat = db.GetEntity<Category>("select * from category where idx=1");
            //Response.Write(string.Format("idx:{0},title:{1}", cat.Idx, cat.Title));
            //cat.Title = DateTime.Now.ToString();
            //db.Update<Category>(cat);
            //cat = db.GetEntity<Category>("select * from category where idx=1");
            //Response.Write(string.Format("<br/>idx:{0},title:<b>{1}</b>", cat.Idx, cat.Title));
        }
    }


    public partial class Category
    {
        [DbColumn(true, true)]
        public System.Int32 Idx { get; set; }
        public System.String Title { get; set; }
        public const string IdentitKeys = "Idx";
    }

}