using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Net;
using System.IO;
namespace Helper.Web
{
    public class WebHelper
    {
        #region rss
        public static void RSSOutput(HttpContext context, RssInfo rss)
        {
            StringBuilder sb = new StringBuilder();
            context.Response.ClearHeaders();
            context.Response.ContentType = "text/xml";
            context.Response.Charset = "UTF-8";
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><rss version=\"2.0\"><channel>");
            sb.Append(string.Format("<title><![CDATA[{0}]]></title>", rss.Title));
            sb.Append(string.Format("<link><![CDATA[{0}]]></link>", rss.Link));
            sb.Append(string.Format("<description><![CDATA[{0}]]></description>", rss.Description));
            sb.Append(string.Format("<language>{0}</language>", rss.Laguage));
            sb.Append(string.Format("<pubDate>{0}</pubDate>", rss.PubDate));
            sb.Append(string.Format("<generator><![CDATA[{0}]]></generator>", rss.Generator));
            sb.Append(string.Format("", ""));
            foreach (RssItem item in rss.Items)
            {
                sb.Append("<item>");
                sb.Append(string.Format("<title><![CDATA[{0}]]></title>", item.Title));
                sb.Append(string.Format("<link><![CDATA[{0}]]></link>", item.Link));
                sb.Append(string.Format("<description><![CDATA[{0}]]></description>", item.Description));
                sb.Append(string.Format("<author><![CDATA[{0}]]></author>", item.Author));
                sb.Append(string.Format("<guid><![CDATA[{0}]]></guid>", item.Guid));
                sb.Append(string.Format("<pubDate>{0}</pubDate>", item.PubDate));
                if (!string.IsNullOrEmpty(item.Category))
                    sb.Append(string.Format("<category>{0}</category>", item.Category));
                sb.Append("</item>");
            }
            sb.Append("</channel></rss>");
            context.Response.Write(sb.ToString());
            context.Response.Flush();
        }
        #endregion

        #region cookie
        public static string GetCookie(string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[key] != null)
            {
                return HttpContext.Current.Request.Cookies[key].Value.ToString();
            }
            return string.Empty;
        }
        public static void SetCookie(string key, string value, int seconds, string domain)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
            {
                cookie = new HttpCookie(key);
            }
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddSeconds(seconds);
            if (!string.IsNullOrEmpty(domain)) cookie.Domain = domain;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region message
        public static void Alert(HttpContext context, string msg)
        {
            context.Response.Write("<script>alert('" + msg + "');</script>");
        }
        public static void AlertBack(System.Web.UI.Page pe, string msg)
        {
            pe.ClientScript.RegisterStartupScript(pe.GetType(), "aasdasd", "<script>alert('" + msg + "');history.back();</script>", false);

        }
        public static void AlertTo(System.Web.UI.Page pe, string msg, string to)
        {
            pe.ClientScript.RegisterStartupScript(pe.GetType(), "aasdasd", "<script>alert('" + msg + "');window.location='" + to + "';;</script>", false);

        }


        #endregion

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
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
                return "";

            return HttpContext.Current.Request.ServerVariables[strName].ToString();
        }

        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < BrowserName.Length; i++)
            {
                if (curBrowser.IndexOf(BrowserName[i]) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
                return false;

            string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < SearchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
                    return true;
            }
            return false;
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
        /// 获取某地址的状态码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HttpStatusCode GetHttpStatusCode(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode;
                }
            }
            catch
            {
                return HttpStatusCode.ServiceUnavailable;
            }
        }
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns></returns>
        public static bool IsPost
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod.Equals("POST");
            }
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns></returns>
        public static bool IsGet
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod.Equals("GET");
            }
        }

        /// <summary>
        /// 返回当前页面是否是跨站提交
        /// </summary>
        /// <returns></returns>
        public static bool IsCrossSitePost
        {
            get
            {
                if (IsPost)
                {
                    if (HttpContext.Current.Request.UrlReferrer == null || HttpContext.Current.Request.UrlReferrer.ToString().Length < 7)
                    {
                        return true;
                    }
                    Uri u = HttpContext.Current.Request.UrlReferrer;
                    return u.Host != HttpContext.Current.Request.Url.Host;
                }
                return false;
            }
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int QueryInt(string strName, int defaultval)
        {
            int val = -1;
            if (int.TryParse(HttpContext.Current.Request.QueryString[strName], out val))
            {
                return val;
            }
            else
                return defaultval;
        }
        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static string QueryString(string strName)
        {
            string sv = HttpContext.Current.Request.QueryString[strName];
            if (sv == null) sv = string.Empty;
            return sv;
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string FormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return string.Empty;
            }
            return HttpContext.Current.Request.Form[strName];
        }
        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int FormInt(string strName, int defaultval)
        {
            int val = -1;
            if (int.TryParse(HttpContext.Current.Request.Form[strName], out val))
            {
                return val;
            }
            else
                return defaultval;
        }


        /// <summary>
        /// Gets the client's IP address.
        /// This method takes into account the X-Forwarded-For header,
        /// in case the blog is hosted behind a load balancer or proxy.
        /// </summary>
        /// <returns>The client's IP address.</returns>
        public static string GetClientIP()
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                var request = context.Request;
                if (request != null)
                {
                    string xff = request.Headers["X-Forwarded-For"];
                    string clientIP = string.Empty;
                    if (!string.IsNullOrEmpty(xff))
                    {
                        int idx = xff.IndexOf(',');
                        if (idx > 0)
                        {
                            // multiple IP addresses, pick the first one
                            clientIP = xff.Substring(0, idx);
                        }
                        else
                        {
                            clientIP = xff;
                        }
                    }

                    return string.IsNullOrEmpty(clientIP) ? request.UserHostAddress : clientIP;
                }
            }

            return string.Empty;
        }

        //以GET方式抓取远程页面内容
        public static string GetHttp(string tUrl)
        {
            string strResult;
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(tUrl);
                hwr.Timeout = 19600;
                HttpWebResponse hwrs = (HttpWebResponse)hwr.GetResponse();
                Stream myStream = hwrs.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.GetEncoding("utf-8"));
                strResult = sr.ReadToEnd();
                sr.Close();
                hwrs.Close();
            }
            catch (Exception ee)
            {
                strResult = ee.Message;
            }
            return strResult;
        }

        public static string GetWebHtmlStr(string httppath, string encodingname)
        {
            if (string.IsNullOrEmpty(encodingname)) encodingname = "gb2312";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(httppath);
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 60 * 1000;
            request.Headers.Set("pragma", "no-cache");
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.0.04506)";
            request.Referer = httppath;
            string s = string.Empty;
            try
            {
                using (Stream stream = request.GetResponse().GetResponseStream())
                {
                    using (StreamReader r = new StreamReader(stream, System.Text.Encoding.GetEncoding(encodingname)))
                    {
                        s = r.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return s;
        }

        //以POST方式抓取远程页面内容
        //postData为参数列表
        public static string PostHttp(string url, string postData, string encodeType)
        {
            string strResult = null;
            try
            {
                Encoding encoding = Encoding.GetEncoding(encodeType);
                byte[] POST = encoding.GetBytes(postData);
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = POST.Length;
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(POST, 0, POST.Length); //设置POST
                newStream.Close();
                // 获取结果数据
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
                strResult = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            return strResult;
        }

        /// <summary>
        /// 将IP地址转为整数形式
        /// </summary>
        /// <returns>整数</returns>
        public static long IP2Long(IPAddress ip)
        {
            int x = 3;
            long o = 0;
            foreach (byte f in ip.GetAddressBytes())
            {
                o += (long)f << 8 * x--;
            }
            return o;
        }

        /// <summary>
        /// 将整数转为IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static IPAddress Long2IP(long l)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                b[3 - i] = (byte)(l >> 8 * i & 255);
            }
            return new IPAddress(b);
        }

        /// <summary>
        /// 自定义对url编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Obsolete("请使用UrlHelper类")]
        public static string EncodeUrl(string url)
        {
            return UrlHelper.UrlEncode2(url);
        }

        /// <summary>
        /// 自定义对url解码
        /// </summary>
        /// <param name="encodedUrl"></param>
        /// <returns></returns>
        [Obsolete("请使用UrlHelper类")]
        public static string DecodeUrl(string encodedUrl)
        {
            return UrlHelper.UrlDecode2(encodedUrl);
        }
    }

    public static class AscxHelper
    {
        public static Control LoadAscx(string path)
        {
            Page page = HttpContext.Current.CurrentHandler as Page;
            Control c = page.LoadControl(path);
            return c;
        }

        public static Control LoadAscx(string path, System.Web.UI.Page pge)
        {
            Page page = pge;
            Control c = page.LoadControl(path);
            pge.Controls.Add(c);
            return c;
        }
    }
}
