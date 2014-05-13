using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helper.DbDataType;

namespace WebApplication1.testDatabaseMysql
{






    public partial class NewsInfo : Helper.IDicSerialize
    {

        public System.Int32 IDx { get; set; }
        public System.String Title { get; set; }
        public System.DateTime Indate { get; set; }
        public System.String Content { get; set; }
        public System.String img1 { get; set; }
        public System.String img2 { get; set; }
        public System.Int32 IsTop { get; set; }
        public System.Decimal Price { get; set; }
        public string TableName { get { return "newsinfo"; } }

        public System.Collections.Generic.IDictionary<string, MyField> ToDic()
        {
            Dictionary<string, MyField> dic = new Dictionary<string, MyField>();
            dic["IDx"] = new MyField(this.IDx, typeof(System.Int32), true);
            dic["Title"] = new MyField(this.Title, typeof(System.String));
            dic["Indate"] = new MyField(this.Indate, typeof(System.DateTime));
            dic["Content"] = new MyField(this.Content, typeof(System.String));
            dic["img1"] = new MyField(this.img1, typeof(System.String));
            dic["img2"] = new MyField(this.img2, typeof(System.String));
            dic["IsTop"] = new MyField(this.IsTop, typeof(System.Int32));
            dic["Price"] = new MyField(this.Price, typeof(System.Decimal));
            return dic;
        }

        public object ToObj(System.Collections.Generic.IDictionary<string, MyField> dic)
        {
            this.IDx = (System.Int32)(((MyField)dic["IDx"]).Data);
            this.Title = (System.String)(((MyField)dic["Title"]).Data);
            this.Indate = (System.DateTime)(((MyField)dic["Indate"]).Data);
            this.Content = (System.String)(((MyField)dic["Content"]).Data);
            this.img1 = (System.String)(((MyField)dic["img1"]).Data);
            this.img2 = (System.String)(((MyField)dic["img2"]).Data);
            this.IsTop = (System.Int32)(((MyField)dic["IsTop"]).Data);
            this.Price = (System.Decimal)(((MyField)dic["Price"]).Data);
            return this;
        }
    }

  

  
}