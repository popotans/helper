using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Helper.Web
{
    public class UrlHelper
    {

        /// <summary>
        /// 获取网址的路径信息 如：
        /// http://www.a.com/1.aspx/pid-10-site-kanhouba 
        /// 这个网址，则返回结果为：pid-10-site-kanhouba
        /// </summary>
        /// <returns></returns>
        public static string GetPathInfo()
        {
            string s = string.Empty;
            s = HttpContext.Current.Request.Url.AbsolutePath;
            s = s.Substring(s.LastIndexOf("/") + 1);
            return s;
        }

        /// <summary>
        /// 获取当前的域名 如：http://www.a.com 返回:www.a.com
        /// </summary>
        /// <returns></returns>
        public static string GetDomain()
        {
            if (HttpContext.Current.Request.Url.Port == 80)
                return HttpContext.Current.Request.Url.Host;
            else
                return string.Format("{0}:{1}", HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
        }

        /// <summary>
        /// 301服务器转向
        /// </summary>
        /// <param name="url"></param>
        public static void Redirect301(string url)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.StatusCode = 301;
            HttpContext.Current.Response.Status = "301 Moved Permanently";
            HttpContext.Current.Response.AddHeader("Location", url);

        }

        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }



        /// <summary>
        /// 自定义对url编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode2(string url)
        {
            url = url.ToLower();
            //http://dudu8.net/novel/10/10571/index.html
            //www.a.dcom?sour=all
            // www@a@dcom:sour;all
            url = url.Replace("http://", "");
            url = url.Replace(".", "@");
            url = url.Replace("/", "!");
            url = url.Replace("?", "|");
            url = url.Replace("&", ";");
            url = url.Replace("=", ":");
            url = url.Replace("+", "$");
            //,
            return url;
        }
        /// <summary>
        /// 自定义对url解码
        /// </summary>
        /// <param name="encodedUrl"></param>
        /// <returns></returns>
        public static string UrlDecode2(string encodedUrl)
        {
            encodedUrl = encodedUrl.ToLower();
            encodedUrl = encodedUrl.Replace("@", ".");
            encodedUrl = encodedUrl.Replace("!", "/");
            encodedUrl = encodedUrl.Replace("|", "?");
            encodedUrl = encodedUrl.Replace(";", "&");
            encodedUrl = encodedUrl.Replace(":", "=");
            encodedUrl = encodedUrl.Replace("$", "+");

            return "http://" + encodedUrl;
        }

        /// <summary>
        /// 为url地址添加新参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AddParam(string url, string paramName, string value)
        {
            if (url.IndexOf("?") == -1)
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "?" + paramName + "=" + eval);
            }
            else
            {
                string eval = HttpContext.Current.Server.UrlEncode(value);
                return String.Concat(url, "&" + paramName + "=" + eval);
            }
        }
    }
}
