using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Helper.Web
{
    public class ResponseCatcherHandle
    {
        private ResponseCatcherHandle()
        {

        }

        private static ResponseCatcherHandle _ResponseCatcherHandle = new ResponseCatcherHandle();
        public static ResponseCatcherHandle Instance
        {
            get
            {
                return _ResponseCatcherHandle;
            }
        }

        public void Init(string url, long durationMin)
        {
            //string pagepath = url.Substring(0, url.IndexOf(".aspx"));
            //  this.InitwithUrl(url, durationMin, pagepath);
            //  url = url.Substring(1).ToLower();
            url = GetUrl4fileCached(url);
            InitNoCat(url, durationMin);
        }
        string cacheBaseDir = "/_cache/";
        string aglothim = "";

        /// <summary>
        /// 初始化写入缓存
        /// </summary>
        /// <param name="durationMin">缓存时间，以分钟为单位</param>
        /// <param name="cat">缓存分类</param>
        public void Init(long durationMin, string cat)
        {
            this.InitwithUrl(HttpContext.Current.Request.Url.PathAndQuery.ToLower(), durationMin, cat);
        }

        private void InitwithUrl(string url, long durationMin, string cat)
        {
            string baseDir = cacheBaseDir;
            durationMin = durationMin <= 0 ? int.MaxValue - 555555 : durationMin;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(baseDir + cat)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(baseDir + cat));
            }

            //string url = HttpContext.Current.Request.Url.PathAndQuery.ToLower();
            string hash = Helper.Security.SecurityHelper.SHA1(url);
            string file = HttpContext.Current.Server.MapPath(baseDir + cat + "/" + hash + ".htm");
            if (!Helper.IO.FileHelper.Exists(file, durationMin))
            {
                HttpContext.Current.Response.Filter = new ResponseCatcher(HttpContext.Current.Response.Filter, file, "utf-8");
            }
            else
            {
                HttpContext.Current.Response.ClearContent();
                string readContent = Helper.IO.FileHelper.ReadFile(file, System.Text.Encoding.GetEncoding("utf-8"));
                StringBuilder sb = new StringBuilder(readContent);
                sb.Append("<!--cache-->");
                string rs = sb.ToString();
                Write(rs);
            }
        }

        private string GetUrl4fileCached(string source)
        {
            return source.Substring(1).ToLower().Replace(".aspx", ".pg");
        }
        private void InitNoCat(string url, long durationMin)
        {
            string baseDir = cacheBaseDir;
            durationMin = durationMin <= 0 ? int.MaxValue - 555555 : durationMin;
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(baseDir)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(baseDir));
            }

            //string url = HttpContext.Current.Request.Url.PathAndQuery.ToLower();
            string hash = null;
            if (aglothim == "sha1")
                Helper.Security.SecurityHelper.SHA1(url);
            else if (aglothim.Length == 0)
                hash = url;

            string file = HttpContext.Current.Server.MapPath(baseDir + hash + ".ascx");
            if (!Helper.IO.FileHelper.Exists(file, durationMin))
            {
                HttpContext.Current.Response.Filter = new ResponseCatcher(HttpContext.Current.Response.Filter, file, "utf-8");
            }
            else
            {
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.TransmitFile(file);
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 初始化 写入缓存,自动创建分类文件夹
        /// 
        /// </summary>
        /// <param name="durationMin">缓存时间，分钟为单位</param>
        public void Init(long durationMin)
        {
            string cat = HttpContext.Current.Request.RawUrl;
            cat = GetUrl4fileCached(cat);
            //cat = cat.Substring(0, HttpContext.Current.Request.Url.PathAndQuery.ToLower().IndexOf(".aspx"));
            //Init(durationMin, cat);
            InitNoCat(cat, durationMin);
        }

        private void Write(string rs)
        {
            if (rs.Length > 50)
            {
                HttpContext.Current.Response.Write(rs);
            }
            else
                HttpContext.Current.Response.Write("content length is not enough.");
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Flush();
        }

        public void ClearCache(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new Exception("url  is null or empty");
            url = GetUrl4fileCached(url);
            string hash = null;
            if (aglothim == "")
                hash = url;
            else
                hash = Helper.Security.SecurityHelper.SHA1(url);
            string file = HttpContext.Current.Server.MapPath(cacheBaseDir + hash + ".ascx");
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        public void ClearCache(string cat, string url)
        {
            if (string.IsNullOrEmpty(cat)) return;
            string hash = Helper.Security.SecurityHelper.SHA1(url.ToLower());
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(cacheBaseDir + cat)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(cacheBaseDir + cat));
            }
            string filePath = HttpContext.Current.Server.MapPath(cacheBaseDir + cat + "/" + hash + ".htm");
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ClearCatCache(string url)
        {
            string pagepath = url.Substring(0, url.IndexOf(".aspx"));
            ClearCacheFolderFiles(pagepath);
        }

        public void ClearAll()
        {
            ClearCacheFolderFiles("");
        }

        void ClearCacheFolderFiles(string cat)
        {
            if (string.IsNullOrEmpty(cat)) cat = string.Empty;
            string path = HttpContext.Current.Server.MapPath(cacheBaseDir + cat);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string[] _files = Directory.GetFiles(path);
            foreach (string f in _files)
            {
                // HttpContext.Current.Response.Write("<br/>" + f);
                if (File.Exists(f))
                    File.Delete(f);
            }
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                string[] files = Directory.GetFiles(dir);
                try
                {
                    foreach (string f in files)
                    {
                        // HttpContext.Current.Response.Write("<br/>" + f);
                        if (File.Exists(f))
                            File.Delete(f);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }



    public class ResponseCatcher : Stream
    {
        private Stream myStream;
        internal StreamWriter sw;
        private System.Text.Encoding _Encoding;

        public ResponseCatcher(Stream _stream, string outputFileName)
        {
            _Encoding = System.Text.Encoding.GetEncoding("utf-8");
            myStream = _stream;
            sw = new StreamWriter(outputFileName, false, _Encoding);
        }

        public ResponseCatcher(Stream _stream, string outputFileName, string encodeName)
        {
            this._Encoding = System.Text.Encoding.GetEncoding(encodeName);
            //  _Encoding = System.Text.Encoding.Default;
            myStream = _stream;
            sw = new StreamWriter(outputFileName, false, this._Encoding);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return myStream.Length; }
        }

        public override long Position
        {
            get
            {
                return myStream.Position;
            }
            set
            {
                myStream.Position = value;
            }
        }


        public override void Flush()
        {
            //  sw.Flush();
            myStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return myStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            myStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return myStream.Read(buffer, offset, count);
        }

        private void Log(string msg)
        {
            HttpContext.Current.Response.Write(msg + "<br/>");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            string htmlString = this._Encoding.GetString(buffer);
            sw.Write(htmlString);
            myStream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            sw.Write(DateTime.Now.ToString("<!--yyyyMMddHHmmss-->"));
            sw.Close();
            myStream.Close();
        }

        /// <summary>
        /// 清除指定分类下的缓存
        /// </summary>
        /// <param name="cat">缓存分类名称</param>
        /// <param name="url">产生缓存的url地址，如：/default.aspx,/news/read1000.aspx</param>
        public static void ClearCache(string cat, string url)
        {
            if (string.IsNullOrEmpty(cat)) return;
            string hash = Helper.Security.SecurityHelper.SHA1(url.ToLower());
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/_cache/" + cat)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/_cache/" + cat));
            }
            string filePath = HttpContext.Current.Server.MapPath("/_cache/" + cat + "/" + hash + ".htm");
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 清除指定分类下的缓存
        /// </summary>
        /// <param name="cat">缓存分类名称</param>
        /// <param name="url">产生缓存的url地址数组，如：/default.aspx,/news/read1000.aspx</param>
        public static void ClearCache(string cat, string[] url)
        {
            if (string.IsNullOrEmpty(cat)) return;
            foreach (string item in url)
            {
                ClearCache(cat, item);
            }
        }


        public static void ClearCacheFolderFiles(string cat)
        {
            if (string.IsNullOrEmpty(cat)) cat = string.Empty;
            string path = HttpContext.Current.Server.MapPath("/_cache/" + cat);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string[] _files = Directory.GetFiles(path);
            foreach (string f in _files)
            {
                // HttpContext.Current.Response.Write("<br/>" + f);
                if (File.Exists(f))
                    File.Delete(f);
            }

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                string[] files = Directory.GetFiles(dir);
                try
                {
                    foreach (string f in files)
                    {
                        // HttpContext.Current.Response.Write("<br/>" + f);
                        if (File.Exists(f))
                            File.Delete(f);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }


        public static void ClearCache(string url)
        {
            string pagepath = url.Substring(0, url.IndexOf(".aspx"));
            ClearCache(pagepath, url);

        }
    }
}
