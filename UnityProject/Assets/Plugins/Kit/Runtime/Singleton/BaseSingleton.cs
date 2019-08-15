// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

namespace Kit.Runtime
{
    /// <summary>
    ///  单例父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseSingleton<T> where T : class, new()
    {
        private static T instance;
        private static readonly object syslock = new object();
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syslock)
                    {
                        instance = new T();
                    }
                }
                return instance;
            }
            set { instance = value; }
        }
    } 
} 