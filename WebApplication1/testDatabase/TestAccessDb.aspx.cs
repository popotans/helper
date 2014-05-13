using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper;
namespace WebApplication1.testDatabase
{
    public partial class TestAccessDb : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //DbMapAccess adb = new DbMapAccess("c:\\_DtCms.mdb");

            //Article art = adb.Get<Article>("select top 1 * from article ");


            //art = new Article()
            //{
            //    AddTime = DateTime.Now,
            //    Author = "niejunhuahdasd",
            //    ClassId = 1526695269,
            //    Content = "asdasd"
            //};
            //adb.Insert<Article>(art);


            // Response.Write(art.Title);

            //AccessCore ac = new AccessCore("c:\\_DtCms.mdb");
            //ac.CreateAll("aaaa","");

            DbMap map = new DbMapAccess(@"E:\快盘\project\CaiDown\CaiDown\down.mdb");
            Console.WriteLine(map.db.ConnStr + "#" + map.db.ToString());

            njh.list list = new njh.list()
            {
                classname = "classname2",
                href = Guid.NewGuid().ToString(),
                idx = 1,
                title = "Title" + DateTime.Now.Ticks

            };

            int i = map.Insert<njh.list>(list);

        }

    }

    public partial class Article
    {
        public string Title { get; set; }
        public int ID { get; set; }
        public string Author { get; set; }
        public string Form { get; set; }
        public string Keyword { get; set; }
        public string Zhaiyao { get; set; }
        public int ClassId { get; set; }
        public string ImgUrl { get; set; }

        public string Daodu { get; set; }
        public string Content { get; set; }
        public int Click { get; set; }
        public int IsMsg { get; set; }
        public int IsTop { get; set; }
        public int IsRed { get; set; }
        public int IsHot { get; set; }
        public int IsSlide { get; set; }
        public int IsLock { get; set; }
        public DateTime AddTime { get; set; }
    }

    public partial class Article : BaseMap
    {
        public Article()
        {

        }
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            dic["Title"] = Title;
            dic["ID"] = ID;
            dic["Author"] = Author;
            dic["Form"] = Form;
            dic["Keyword"] = Keyword;
            dic["Zhaiyao"] = Zhaiyao;
            dic["ClassId"] = ClassId;
            dic["ImgUrl"] = ImgUrl;
            dic["Daodu"] = Daodu;
            dic["Content"] = Content;
            dic["Click"] = Click;
            dic["IsMsg"] = IsMsg;
            dic["IsTop"] = IsTop;
            dic["IsRed"] = IsRed;
            dic["IsHot"] = IsHot;
            dic["IsSlide"] = IsSlide;
            dic["IsLock"] = IsLock;
            dic["AddTime"] = AddTime;
            return dic;
        }

        public override void Deserialise(IDictionary<string, object> dic)
        {
            Title = dic["Title"].ToString();
            ID = int.Parse(dic["id"].ToString());
            Author = dic["Author"].ToString();
            Form = dic["Form"].ToString();
            Keyword = dic["Keyword"].ToString();
            Zhaiyao = dic["Zhaiyao"].ToString();
            ClassId = int.Parse(dic["ClassId"].ToString());
            ImgUrl = dic["ImgUrl"].ToString();
            AddTime = DateTime.Parse(dic["AddTime"].ToString());

            Daodu = (dic["Daodu"].ToString());
            Content = (dic["Content"].ToString());
            Click = int.Parse(dic["Click"].ToString());
            IsMsg = int.Parse(dic["IsMsg"].ToString());
            IsTop = int.Parse(dic["IsTop"].ToString());
            IsRed = int.Parse(dic["IsRed"].ToString());
            IsHot = int.Parse(dic["IsHot"].ToString());
            IsSlide = int.Parse(dic["IsSlide"].ToString());
            IsLock = int.Parse(dic["IsLock"].ToString());
        }
    }
}