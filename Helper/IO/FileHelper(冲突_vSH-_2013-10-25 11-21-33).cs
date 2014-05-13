using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.IO;
using System.Web;
using System.Net;
using System.Threading;

namespace Helper.IO
{
    public partial class FileHelper
    {
        /// <summary>
        /// 使用execute生成静态页面
        /// </summary>
        /// <param name="aspxPath"></param>
        /// <param name="htmlPath"></param>
        /// <param name="encode"></param>
        /// <param name="isGzip"></param>
        public static void AspxToHtml(string aspxPath, string htmlPath, System.Text.Encoding encode, bool isGzip)
        {
            using (Stream stream = new FileStream(htmlPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (isGzip)
                {
                    using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                    {
                        using (StreamWriter sw = new StreamWriter(gzip, encode))
                        {
                            HttpContext.Current.Server.Execute(aspxPath, sw, true);
                            sw.Flush();
                            sw.Close();
                        }
                        gzip.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(stream, encode))
                    {
                        HttpContext.Current.Server.Execute(aspxPath, sw, true);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 读取静态文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encode"></param>
        /// <param name="isGzip"></param>
        /// <returns></returns>
        public static string ReadFile(string path, System.Text.Encoding encode, bool isGzip)
        {
            string rs = string.Empty;
            using (FileStream fs = File.OpenRead(path))
            {
                if (isGzip)
                {
                    using (GZipStream gz = new GZipStream(fs, CompressionMode.Decompress))
                    {
                        using (StreamReader sr = new StreamReader(gz, encode))
                        {
                            rs = sr.ReadToEnd();
                        }
                    }
                }
                else
                {
                    using (StreamReader sr = new StreamReader(fs, encode))
                    {
                        rs = sr.ReadToEnd();
                    }
                }
            }
            return rs;
        }

        public static string ReadFile(string path, string encodingName)
        {
            return ReadFile(path, Encoding.GetEncoding(encodingName), false);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path">写入路径</param>
        /// <param name="contents">内容</param>
        /// <param name="encode">编码</param>
        /// <param name="isGzip">是否gzip压缩</param>
        public static void WriteFile(string path, string contents, System.Text.Encoding encode, bool isGzip, bool isAppend)
        {
            if (isGzip)
            {
                using (Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    if (isGzip)
                    {
                        using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                        {
                            using (StreamWriter sw = new StreamWriter(gzip, encode))
                            {
                                sw.Write(contents);
                                sw.Flush();
                                sw.Close();
                            }
                            gzip.Close();
                        }
                    }
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(path, isAppend, encode))
                {
                    sw.Write(contents);
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        public static void WriteFile(string path, string contents, string encodeName)
        {
            //WriteFile(path, contents, Encoding.GetEncoding(encodeName), false, false);
            WriteFile(path, contents, encodeName, false);
        }

        public static void WriteFile(string path, string contents, string encodeName, bool isAppend)
        {
            WriteFile(path, contents, Encoding.GetEncoding(encodeName), false, isAppend);
        }


        private int _GetWebContentsRepeatTimes = 0;
        private bool _GetWebContentsIsException = false;

        /// <summary>
        /// 获取远程网页内容，指定编码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string GetWebContents(string url, Encoding encoding)
        {
            _GetWebContentsIsException = false;
            if (_GetWebContentsRepeatTimes > 10)
            {
                _GetWebContentsRepeatTimes = 0;
                throw new Exception("发生错误，重复10次仍链接超时。");
            }
            //  if (string.IsNullOrEmpty(encodingname)) encodingname = "gb2312";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = url;
            request.Timeout = 60 * 1000;
            request.Headers.Set("pragma", "no-cache");
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.0.04506)";
            string s = string.Empty;
            try
            {
                using (Stream stream = request.GetResponse().GetResponseStream())
                {
                    using (StreamReader r = new StreamReader(stream, encoding))
                    {
                        s = r.ReadToEnd();
                        r.Close();
                    }
                    stream.Close();
                }
                _GetWebContentsRepeatTimes = 0;
            }
            catch
            {
                _GetWebContentsIsException = true;
            }
            finally
            {
                if (_GetWebContentsIsException)
                {
                    _GetWebContentsRepeatTimes++;
                    //  Thread.Sleep(10 * 1000);
                    s = GetWebContents(url, encoding);
                }
            }
            return s;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dir"></param>
        public static void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

    }
}
