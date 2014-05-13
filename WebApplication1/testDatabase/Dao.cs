using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helper.DbDataType;
using Helper;

namespace WebApplication1.testDatabase
{



    public partial class NewsInfo : Helper.IDicSerialize
    {
        public System.String ClassId { get; set; }
        public System.Int32 Click { get; set; }
        public System.String Content { get; set; }
        public System.String Description { get; set; }

        public System.Int32 Idx { get; set; }
        public System.String Img1 { get; set; }
        public System.String Img2 { get; set; }
        public System.String Img3 { get; set; }
        public System.String Img4 { get; set; }
        public System.String Img5 { get; set; }
        public System.String Img6 { get; set; }
        public System.String Img7 { get; set; }
        public System.String Img8 { get; set; }
        public System.DateTime InDate { get; set; }
        public System.Int32 Istop { get; set; }
        public System.String Kwd { get; set; }
        public System.String Title { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public string TableName { get { return "NewsInfo"; } }

        public System.Collections.Generic.IDictionary<string, MyField> ToDic()
        {
            Dictionary<string, MyField> dic = new Dictionary<string, MyField>();
            dic["ClassId"] = new MyField(this.ClassId, typeof(System.String));
            dic["Click"] = new MyField(this.Click, typeof(System.Int32));
            dic["Content"] = new MyField(this.Content, typeof(System.String));
            dic["Description"] = new MyField(this.Description, typeof(System.String));
            dic["Idx"] = new MyField(this.Idx, typeof(System.Int32), true);
            dic["Img1"] = new MyField(this.Img1, typeof(System.String));
            dic["Img2"] = new MyField(this.Img2, typeof(System.String));
            dic["Img3"] = new MyField(this.Img3, typeof(System.String));
            dic["Img4"] = new MyField(this.Img4, typeof(System.String));
            dic["Img5"] = new MyField(this.Img5, typeof(System.String));
            dic["Img6"] = new MyField(this.Img6, typeof(System.String));
            dic["Img7"] = new MyField(this.Img7, typeof(System.String));
            dic["Img8"] = new MyField(this.Img8, typeof(System.String));
            dic["InDate"] = new MyField(this.InDate, typeof(System.DateTime));
            dic["Istop"] = new MyField(this.Istop, typeof(System.Int32));
            dic["Kwd"] = new MyField(this.Kwd, typeof(System.String));
            dic["Title"] = new MyField(this.Title, typeof(System.String));
            dic["UpdateTime"] = new MyField(this.UpdateTime, typeof(System.DateTime));
            return dic;
        }

        public object ToObj(System.Collections.Generic.IDictionary<string, MyField> dic)
        {
            this.ClassId = (System.String)(((MyField)dic["ClassId"]).Data);
            this.Click = (System.Int32)(((MyField)dic["Click"]).Data);
            this.Content = (System.String)(((MyField)dic["Content"]).Data);
            this.Description = (System.String)(((MyField)dic["Description"]).Data);
            this.Idx = (System.Int32)(((MyField)dic["Idx"]).Data);
            this.Img1 = (System.String)(((MyField)dic["Img1"]).Data);
            this.Img2 = (System.String)(((MyField)dic["Img2"]).Data);
            this.Img3 = (System.String)(((MyField)dic["Img3"]).Data);
            this.Img4 = (System.String)(((MyField)dic["Img4"]).Data);
            this.Img5 = (System.String)(((MyField)dic["Img5"]).Data);
            this.Img6 = (System.String)(((MyField)dic["Img6"]).Data);
            this.Img7 = (System.String)(((MyField)dic["Img7"]).Data);
            this.Img8 = (System.String)(((MyField)dic["Img8"]).Data);
            this.InDate = (System.DateTime)(((MyField)dic["InDate"]).Data);
            this.Istop = (System.Int32)(((MyField)dic["Istop"]).Data);
            this.Kwd = (System.String)(((MyField)dic["Kwd"]).Data);
            this.Title = (System.String)(((MyField)dic["Title"]).Data);
            this.UpdateTime = (System.DateTime)(((MyField)dic["UpdateTime"]).Data);
            return this;
        }
    }

  

  




}