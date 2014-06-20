using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;

namespace Helper.Web
{
    public class NPager : Literal, INamingContainer
    {
        public delegate void EventHandler(object sender, int currentPage);
        public event EventHandler OnPageIndexChanged;
        private HttpContext context = HttpContext.Current;
        private int currentPage = 1;
        #region public
        private string linkCss = "";
        /// <summary>
        /// 链接样式
        /// </summary>
        public string LinkCss
        {
            get { return linkCss; }
            set { linkCss = value; }
        }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            get
            {
                if (ViewState["PageSize"] == null) ViewState["PageSize"] = 10;
                int pageSize = Convert.ToInt32(ViewState["PageSize"]);
                if (pageSize < 1)
                {
                    pageSize = 10;
                }
                return pageSize;
            }
            set
            {
                ViewState["PageSize"] = value;
            }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalRecords
        {
            get
            {
                if (ViewState["TotalRecords"] == null) return 0;
                return Convert.ToInt32(ViewState["TotalRecords"]);
            }
            set
            {
                ViewState["TotalRecords"] = value;
            }
        }

        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling(TotalRecords * 1.0 / PageSize);
            }
        }

        public bool IsPostBack
        {
            get
            {
                if (ViewState["IsPostBack"] == null) return false;
                return bool.Parse(ViewState["IsPostBack"].ToString());
            }
            set
            {
                ViewState["IsPostBack"] = value;
            }
        }

        /// <summary>
        /// 当前页,从1 开始
        /// </summary>
        public int PageIndex
        {
            get
            {
                int _pageIndex = 0;
                if (ViewState["PageIndex"] == null)
                {
                    ViewState["PageIndex"] = 1;
                    _pageIndex = 1;
                }
                else
                {
                    if (!IsPostBack && (Convert.ToInt32(context.Request["page"]) > 0))
                    {
                        _pageIndex = Convert.ToInt32(context.Request["page"]);
                    }
                    else
                    {
                        if (int.TryParse(context.Request["__EVENTARGUMENT"], out _pageIndex))
                        {
                            if (OnPageIndexChanged != null) OnPageIndexChanged(null, _pageIndex);
                        }
                        else
                        {
                            _pageIndex = Convert.ToInt32(ViewState["PageIndex"]);
                        }
                    }

                    if (_pageIndex < 1)
                    {
                        _pageIndex = 1;
                    }

                    if (_pageIndex > TotalPage)
                        _pageIndex = TotalPage;
                }
                return _pageIndex;
            }
            set
            {
                ViewState["PageIndex"] = value;
                if (OnPageIndexChanged != null) OnPageIndexChanged(null, 1);
            }
        }

        public int Seed
        {
            get
            {
                if (ViewState["Seed"] == null) return 3;
                return int.Parse(ViewState["Seed"].ToString());
            }
            set
            {
                ViewState["Seed"] = value;
            }
        }

        public string HrefPath
        {
            get
            {
                if (ViewState["HrefPath"] == null) return string.Empty;
                return ViewState["HrefPath"].ToString();
            }
            set { ViewState["HrefPath"] = value; }
        }

        #endregion

        #region override

        public override void RenderControl(HtmlTextWriter writer)
        {
            if (HrefPath.Length == 0)
                RenderDefault(writer);
            else
                RenderQueryPageNum(writer);
            base.RenderControl(writer);
        }

        protected override void OnLoad(EventArgs e)
        {
            currentPage = PageIndex;
            base.OnLoad(e);
        }

        #endregion

        /// <summary>
        /// 默认
        /// </summary>
        /// <param name="writer"></param>
        private void RenderDefault(HtmlTextWriter writer)
        {
            string script = @"";
            int current = currentPage;
            int total = TotalPage;
            int seed = Seed;

            if (current > total) current = total;
            if (current < 1) current = 1;
            string rs = "";
            int begin = 1;
            if (current - seed > 0) begin = current - seed;
            for (int i = begin; i < current; i++)
            {
                rs += string.Format("<a id='ap_{0}' href=\"{1}\">{0}</a> ", i, "javascript:__doPostBack('Control4PagedData','" + i + "')");
            }

            int end = total;
            if (total - current > seed) end = current + seed;
            else { end = total; }
            string style = "";
            for (int i = current; i <= end; i++)
            {
                if (i == current)
                {
                    //  style = " style='color:green; font-weight:bold' ";
                }
                //rs += string.Format("<a id='ap_{0}' href=\"{1}\" {2}>{0}</a> ", i, "javascript:__doPostBack('Control4PagedData','" + i + "')", style);

                if (i == current)
                {
                    rs += string.Format(" {0} ", i);
                }
                else
                {
                    rs += string.Format(" <a id='ap_{0}' href=\"{1}\" {2}>{0}</a> ", i, "javascript:__doPostBack('Control4PagedData','" + i + "')", style);
                }
            }
            writer.Write(script);
            writer.Write(rs);
        }

        public void RenderQueryPageNum(HtmlTextWriter writer)
        {
            string pageplaceholder = this.HrefPath;
            int seed = this.Seed;
            int total = this.TotalPage;
            int current = this.currentPage;

            if (current > total) current = total;
            if (current < 1) current = 1;
            string rs = "";
            int begin = 1;
            if (current - seed > 0) begin = current - seed;
            string href = string.Empty;
            for (int i = begin; i < current; i++)
            {
                href = pageplaceholder.Replace("{page}", i.ToString());
                rs += string.Format("<a id='ap_{0}' href=\"{1}\">{0}</a> ", i, href);
            }

            int end = total;
            if (total - current > seed) end = current + seed - 1;
            else { end = total; }
            string style = "";
            for (int i = current; i <= end; i++)
            {
                //   if (i == current) style = " style='color:#000; font-weight:bold' "; else style = string.Empty;
                if (i == current)
                {
                    rs += string.Format(" {0} ", i);
                }
                else
                {
                    href = pageplaceholder.Replace("{page}", i.ToString());
                    rs += string.Format(" <a id='ap_{0}' href=\"{1}\" {2}>{0}</a> ", i, href, style);
                }

            }
            writer.Write(rs);
        }
    }


    public class PagedHelper
    {
        /// <summary>
        /// 显示分页页码
        /// </summary>
        /// <param name="current"></param>
        /// <param name="total"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static string RenderPostBackPageNum(int current, int total, int seed)
        {
            if (current > total) current = total;
            if (current < 1) current = 1;
            string rs = "";
            int begin = 1;
            if (current - seed > 0) begin = current - seed;
            for (int i = begin; i < current; i++)
            {
                rs += string.Format("<a id='ap_{0}' href=\"{1}\">{0}</a> ", i, "javascript:__doPostBack('Control4PagedData','" + i + "')");
            }

            int end = total;
            if (total - current > seed) end = current + seed - 1;
            else { end = total; }
            string style = "";
            for (int i = current; i <= end; i++)
            {
                //   if (i == current) style = " style='color:#000; font-weight:bold' "; else style = string.Empty;
                if (i == current)
                {
                    rs += string.Format(" {0} ", i);
                }
                else
                {
                    rs += string.Format(" <a id='ap_{0}' href=\"{1}\" {2}>{0}</a> ", i, "javascript:__doPostBack('Control4PagedData','" + i + "')", style);
                }

            }
            return rs;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="total"></param>
        /// <param name="seed"></param>
        /// <param name="placeholder">/news-3-{page}.aspx || /3/news.aspx?page={page}</param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string RenderQueryPageNum(int current, int total, int seed, string pageplaceholder)
        {
            if (current > total) current = total;
            if (current < 1) current = 1;
            string rs = "";
            int begin = 1;
            if (current - seed > 0) begin = current - seed;
            string href = string.Empty;
            for (int i = begin; i < current; i++)
            {
                href = pageplaceholder.Replace("{page}", i.ToString());
                rs += string.Format("<a id='ap_{0}' href=\"{1}\">{0}</a> ", i, href);
            }

            int end = total;
            if (total - current > seed) end = current + seed - 1;
            else { end = total; }
            string style = "";
            for (int i = current; i <= end; i++)
            {
                //   if (i == current) style = " style='color:#000; font-weight:bold' "; else style = string.Empty;
                if (i == current)
                {
                    rs += string.Format(" {0} ", i);
                }
                else
                {
                    href = pageplaceholder.Replace("{page}", i.ToString());
                    rs += string.Format(" <a id='ap_{0}' href=\"{1}\" {2}>{0}</a> ", i, href, style);
                }

            }
            return rs;
        }

        /// <summary>
        /// 页面回发事件
        /// </summary>
        public static void DoPostBack(Action fun)
        {
            if (HttpContext.Current.Request.Params.Get("__EVENTTARGET") == "Control4PagedData")
                if (fun != null) fun();
        }
    }

}
