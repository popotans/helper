using System;
namespace Helper.Cache
{
    public interface ICache
    {
        object Get(string key, Type type);
        T Get<T>(string key);
        T GetCachedObject<T>(string key, int timeOutMinutes, Func<T> onCreateInstance);
        T GetCachedObject<T>(string key, System.Web.Caching.CacheDependency dependency, int timeOutMinutes, Func<T> onCreateInstance);
        string GetCachedStr(string key);
        void Insert(string key, object obj);
        void Insert(string key, object obj, int Minutes);
        void RemoveCache(string key);
    }
}
