using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1
{



    public partial class AboutusInfo
    {
        public System.Int32 Classid { get; set; }
        public System.String Content { get; set; }
        public System.Int32 Idx { get; set; }
        public System.Int32 OrderNo { get; set; }
        public System.String Title { get; set; }
        public System.String Url { get; set; }
        public const string IdentitKeys = "Idx";
    }






    public partial class ComClass
    {
        public System.String Childs { get; set; }
        public System.String Deep { get; set; }
        public System.Int32 IDX { get; set; }
        public System.String ModelTypeId { get; set; }
        public System.Int32 OrderNo { get; set; }
        public System.Int32 Parent { get; set; }
        public System.String Title { get; set; }
        public const string IdentitKeys = "Idx";
    }






    public partial class ModelEnums
    {
        public System.Int32 Idx { get; set; }
        public System.Int32 OrderNo { get; set; }
        public System.String Title { get; set; }
        public const string IdentitKeys = "Idx";
    }




    public partial class Single
    {
        public System.Int32 Classid { get; set; }
        public System.String Content { get; set; }
        public System.Int32 Idx { get; set; }
        public System.DateTime InDate { get; set; }
        public System.String Title { get; set; }
        public const string IdentitKeys = "Idx";
    }






    public partial class TopNav
    {
        public System.String Childs { get; set; }
        public System.Int32 Idx { get; set; }
        public System.Int32 OrderNo { get; set; }
        public System.String Title { get; set; }
        public System.String Url { get; set; }
        public const string IdentitKeys = "Idx";
    }




}