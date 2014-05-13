using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Collections;
using System.Web;

namespace Helper
{
    public static class CacheHelper
    {     /// <summary>
        /// Cache插入锁
        /// </summary>
        private static object CacheLocker = new object();


        public static void RemoveCache(string key)
        {
            System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;
            if (webCache[key] != null) webCache.Remove(key);

        }

        /// <summary>
        /// 获取缓存的对象。当没有缓存的时候，自动创建对象并进行缓存。只支持引用类型的缓存。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="timeOutSeconds">单位：秒</param>
        /// <param name="onCreateInstance">The on create instance.</param>
        /// <returns></returns>
        public static T GetCachedObject<T>(string key, int timeOutSeconds, Func<T> onCreateInstance)
        {
            return GetCachedObject<T>(key, null, timeOutSeconds, onCreateInstance);
        }

        /// <summary>
        /// 获取缓存的对象。当没有缓存的时候，自动创建对象并进行缓存。只支持引用类型的缓存。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dependency"></param>
        /// <param name="timeOutSeconds">单位：秒</param>
        /// <param name="onCreateInstance"></param>
        /// <returns></returns>
        public static T GetCachedObject<T>(string key, System.Web.Caching.CacheDependency dependency, int timeOutSeconds, Func<T> onCreateInstance)
        {
            if (timeOutSeconds > 0 || dependency != null)
            {
                //当前Cache对象
                System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;
                if (webCache == null)
                    return onCreateInstance();

                //获取缓存的对象
                T cachedObject = (T)webCache.Get(key);

                if (cachedObject == null)
                {
                    //加锁确保多线程安全
                    lock (CacheLocker)
                    {
                        cachedObject = (T)webCache.Get(key);
                        if (cachedObject == null)
                        {
                            //创建新的对象
                            cachedObject = onCreateInstance();
                            //将创建的对象进行缓存
                            if (cachedObject != null)
                                webCache.Insert(key, cachedObject, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, timeOutSeconds));
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


        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>object对象</returns>
        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return System.Web.HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">T对象</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object obj = Get(key);
            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// 创建缓存项的文件依赖
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="fileName">文件绝对路径</param>
        public static void Insert(string key, object obj, string fileName)
        {
            //创建缓存依赖项
            CacheDependency dep = new CacheDependency(fileName);
            //创建缓存
            System.Web.HttpRuntime.Cache.Insert(key, obj, dep);
        }

        /// <summary>
        /// 创建缓存项过期
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void Insert(string key, object obj, int expires)
        {
            System.Web.HttpRuntime.Cache.Insert(key, obj, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, expires));
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }

            foreach (string key in al)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        public static void Clear(string startWithKey)
        {
            IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                if (CacheEnum.Key != null && CacheEnum.Key.ToString().StartsWith(startWithKey))
                    al.Add(CacheEnum.Key);
            }

            foreach (string key in al)
            {
                HttpRuntime.Cache.Remove(key);
            }

        }

    }
}
