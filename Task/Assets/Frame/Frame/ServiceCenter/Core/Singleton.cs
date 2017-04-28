// -----------------------------------------------------------------
// File:    Singleton.cs
// Author:  mouguangyi
// Date:    2017.03.13
// Description:
//      
// -----------------------------------------------------------------
namespace GameBox.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class Singleton
    {
        /// <summary>
        /// »ñÈ¡µ¥Àý¡£
        /// </summary>
        /// <returns></returns>
        public static T GetInstance<T>() where T : class, new()
        {
            if (null == InstanceStorage<T>.instance) {
                lock (locker) {
                    if (null == InstanceStorage<T>.instance) {
                        InstanceStorage<T>.instance = new T();
                    }
                }
            }

            return InstanceStorage<T>.instance;
        }

        private static class InstanceStorage<T> where T : class, new()
        {
            internal static volatile T instance = default(T);
        }

        private static object locker = new object();
    }
}