using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.IO;
using System.Threading;
namespace Helper.Cache
{
    public class TxtCache : Helper.Cache.ICache
    {
        private static object CacheLocker = new object();
        private TxtCache() { }
        static private ICache _cache = null;
        public static ICache Instance
        {
            get
            {
                if (_cache == null)
                {
                    lock (CacheLocker)
                    {
                        if (_cache == null)
                            _cache = new TxtCache();
                    }
                }
                return _cache;
            }
        }

        string _CacheConst = string.Format("{0}\\app_data\\.cache", AppDomain.CurrentDomain.BaseDirectory);
        static TxtCache()
        {
            if (!Directory.Exists(string.Format("{0}\\app_data\\.cache", AppDomain.CurrentDomain.BaseDirectory)))
                Directory.CreateDirectory(string.Format("{0}\\app_data\\.cache", AppDomain.CurrentDomain.BaseDirectory));
        }


        public void RemoveCache(string key)
        {
            string file = string.Format("{0}\\{1}.cache", _CacheConst, key);
            if (File.Exists(file))
                File.Delete(file);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>object对象</returns>
        public object Get(string key, Type type)
        {
            string file = string.Format("{0}\\{1}.cache", _CacheConst, key);
            FileInfo fi = new FileInfo(file);
            if (!fi.Exists)
            {
                return null;
            }
            if (fi.LastWriteTime < DateTime.Now) { fi.Delete(); return null; }

            string rs = Helper.IO.FileHelper.AsyncRead(file);

            if (!string.IsNullOrEmpty(rs))
                return Helper.Serialize.JsonHelper.ToObject(rs, type);
            else
                return null;
        }

        public string GetCachedStr(string key)
        {
            string file = string.Format("{0}\\{1}.cache", _CacheConst, key);

            FileInfo fi = new FileInfo(file);
            if (!fi.Exists)
            {
                return null;
            }
            else
            {

            }
            //  if ((DateTime.Now - fi.LastWriteTime).Minutes > expireMinuts)
            if ((DateTime.Now - fi.LastWriteTime).Seconds > 0)
            {
                //过期
                fi.Delete();
                return null;
            }
            else
            {
                string rs = Helper.IO.FileHelper.AsyncRead(file);
                return rs;
            }
        }


        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">T对象</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            object obj = Get(key, typeof(T));
            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="fileName">文件绝对路径</param>
        public void Insert(string key, object obj)
        {
            Insert(key, obj, 5);
        }

        /// <summary>
        /// 创建缓存项过期
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="Minutes">过期时间(分钟)</param>
        public void Insert(string key, object obj, int Minutes)
        {
            //创建缓存依赖项
            string file = string.Format("{0}\\{1}.cache", _CacheConst, key);
            string jsonStr = Helper.Serialize.JsonHelper.ToString(obj);
            Helper.IO.FileHelper.AsyncWrite(jsonStr, file);
            new FileInfo(file).LastWriteTime = DateTime.Now.AddMinutes(Minutes);
        }

        public T GetCachedObject<T>(string key, int timeOutMinutes, Func<T> onCreateInstance)
        {
            return GetCachedObject<T>(key, null, timeOutMinutes, onCreateInstance);
        }

        public T GetCachedObject<T>(string key, System.Web.Caching.CacheDependency dependency, int timeOutMinutes, Func<T> onCreateInstance)
        {
            if (timeOutMinutes > 0)
            {
                string file = string.Format("{0}\\{1}.cache", _CacheConst, key);
                FileInfo fi = new FileInfo(file);
                if (!fi.Exists) return onCreateInstance();

                //获取缓存的对象
                T cachedObject = Get<T>(key);

                if (cachedObject == null)
                {
                    //加锁确保多线程安全
                    lock (CacheLocker)
                    {
                        cachedObject = Get<T>(key);
                        if (cachedObject == null)
                        {
                            //创建新的对象
                            cachedObject = onCreateInstance();
                            //将创建的对象进行缓存
                            if (cachedObject != null)
                                Insert(key, cachedObject, timeOutMinutes);
                        }
                    }
                }
                return cachedObject;
            }
            else
            {
                //不设置缓存，则创建新的对象
                return onCreateInstance();
            }
        }
    }
}
