using System;
using System.Collections.Generic;
using System.Text;

namespace Coder
{
    public partial class Article
    {
        public System.Int32 IDX { get; set; }
        public System.String Title { get; set; }
        public System.String Url { get; set; }
        public System.Int32 IsEnable { get; set; }
        public System.Int32 OrderIndex { get; set; }
        public System.String EnTitle { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.String Author { get; set; }
        public System.Int32 Recommend { get; set; }
        public System.Int32 IsTop { get; set; }
        public System.Int32 Channel { get; set; }
        public System.Int32 Category1 { get; set; }
        public System.Int32 Category2 { get; set; }
        public System.Int32 Category3 { get; set; }

    }

}