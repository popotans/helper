using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.Web
{
  

    public class RssInfo
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Laguage { get; set; }
        public DateTime PubDate { get; set; }
        public string Generator { get; set; }
        public IList<RssItem> Items { get; set; }
    }
    public class RssItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime PubDate { get; set; }
        public string Guid { get; set; }
        public string Category { get; set; }
    }
}
