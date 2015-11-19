using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class getImgFromHtml : System.Web.UI.Page
    {
        /// <summary>
        /// 匹配页面的图片地址
        /// </summary>
        /// <param name="HtmlCode"></param>
        /// <param name="imgHttp">要补充的http://路径信息</param>
        /// <returns></returns>
        public List<string> GetImgSrc(string HtmlCode)
        {
            List<string> list = new List<string>();
            string Reg = @"";
            Reg = "(?is)<img.*?src=(['\"]?)(?<url>[^'\" ]+)(?=\\1)[^>]*>";
            foreach (Match m in Regex.Matches(HtmlCode, Reg, RegexOptions.IgnoreCase))
            {
                list.Add(m.Groups["url"].Value);
            }
            return list;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string source = @"<div><img src='http://a.com/1/55.jpg'/><li><img alt='asd' ttile=125 src='http://a.com/122/563.jpg'/></li></div>";

            List<string> list = GetImgSrc(source);
            foreach (string item in list)
            {
                Response.Write(item + "<br/>");
            }
        }
    }
}