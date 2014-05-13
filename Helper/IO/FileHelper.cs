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

        public static string ReadFile(string path, Encoding encodingName)
        {
            return ReadFile(path, encodingName, false);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path">写入路径</param>
        /// <param name="contents">内容</param>
        /// <param name="encode">编码</param>
        /// <param name="isGzip">是够gzip压缩</param>
        public static void WriteFile(string path, string contents, System.Text.Encoding encode, bool isGzip)
        {
            if (File.Exists(path)) File.Delete(path);
            string folder = path.Substring(0, path.LastIndexOf("\\"));
            if (!Directory.Exists(path)) Directory.CreateDirectory(folder);

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
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(stream, encode))
                        {
                            sw.Write(contents);
                            sw.Flush();
                            sw.Close();
                        }
                    }
                }
        }

        public static void WriteFile(string path, string contents, string encodeName)
        {
            WriteFile(path, contents, Encoding.GetEncoding(encodeName), false);
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

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }
        /// <summary>
        /// 根据过期时间 判断文件是否有效
        /// </summary>
        /// <param name="path"></param>
        /// <param name="durationMin"></param>
        /// <returns></returns>
        public static bool Exists(string path, long durationMin)
        {
            if (Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                if (fi.LastWriteTime.AddMinutes(durationMin) > DateTime.Now)
                {
                    return true;
                }
                else
                    return false;
            }
            return false;
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
        public static string CreateFileDir(string file)
        {
            file = file.Replace("/", "\\");
            file = file.Substring(0, file.LastIndexOf("\\"));
            CreateDir(file);
            return file;
        }

        /// <summary>
        /// 复制文件
        /// 这个方法在6.0版本后改写，虽看似比前面的版本冗长，但避免了file2文件一直被占用的问题
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <param name="overwrite">如果已经存在是否覆盖？</param>
        public static void CopyFile(string file1, string file2, bool overwrite)
        {
            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(file1)))
            {
                if (overwrite)
                    System.IO.File.Copy(System.Web.HttpContext.Current.Server.MapPath(file1), System.Web.HttpContext.Current.Server.MapPath(file2), true);
                else
                {
                    if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(file2)))
                        System.IO.File.Copy(System.Web.HttpContext.Current.Server.MapPath(file1), System.Web.HttpContext.Current.Server.MapPath(file2));
                }
            }
        }
        private static void CopyDir(DirectoryInfo OldDirectory, DirectoryInfo NewDirectory)
        {
            string NewDirectoryFullName = NewDirectory.FullName + "\\" + OldDirectory.Name;

            if (!Directory.Exists(NewDirectoryFullName))
                Directory.CreateDirectory(NewDirectoryFullName);

            FileInfo[] OldFileAry = OldDirectory.GetFiles();
            foreach (FileInfo aFile in OldFileAry)
                File.Copy(aFile.FullName, NewDirectoryFullName + "\\" + aFile.Name, true);

            DirectoryInfo[] OldDirectoryAry = OldDirectory.GetDirectories();
            foreach (DirectoryInfo aOldDirectory in OldDirectoryAry)
            {
                DirectoryInfo aNewDirectory = new DirectoryInfo(NewDirectoryFullName);
                CopyDir(aOldDirectory, aNewDirectory);
            }
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="_Request"></param>
        /// <param name="_Response"></param>
        /// <param name="_fullPath">源文件路径</param>
        /// <param name="_speed"></param>
        /// <returns></returns>
        public static bool DownloadFile(System.Web.HttpRequest _Request, System.Web.HttpResponse _Response, string _fullPath, long _speed)
        {
            string _fileName = Helper.Str.StringHelper.GetFileName(false, _fullPath);
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    double pack = 10240; //10K bytes
                    //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                    int sleep = (int)Math.Floor(1000 * pack / _speed) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                            System.Threading.Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }


    }
}
