using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Common.Tools
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static object SynRoot = new object();
        private static T _instance;

        public static T Instance
        {
            get
            {
                if ((object)Singleton<T>._instance != null)
                    return Singleton<T>._instance;
                lock (Singleton<T>.SynRoot)
                {
                    if ((object)Singleton<T>._instance != null)
                        return Singleton<T>._instance;
                    return Singleton<T>._instance = Activator.CreateInstance<T>();
                }
            }
        }
    }
}
