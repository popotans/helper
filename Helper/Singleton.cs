using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Helper
{
    /// <summary>
    /// Base class used for singletons
    /// 为单例模式使用的基类
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors")]
    public class Singleton<T> where T : class
    {
        #region [构造函数]
        /// <summary>
        /// 构造函数
        /// </summary>
        protected Singleton() { }
        #endregion

        #region [属性]
        /// <summary>
        /// 获取单例模式的实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (Temp)
                    {
                        if (_Instance == null)
                        {
                            ConstructorInfo Constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                null, new Type[0], null);
                            if (Constructor == null || Constructor.IsAssembly)
                                throw new InvalidOperationException("Constructor is not private or protected for type " + typeof(T).Name);
                            _Instance = (T)Constructor.Invoke(null);
                        }
                    }
                }
                return _Instance;
            }
        }
        #endregion

        #region [私有变量]
        private static T _Instance = null;
        private static object Temp = 1;
        #endregion
    }
}
