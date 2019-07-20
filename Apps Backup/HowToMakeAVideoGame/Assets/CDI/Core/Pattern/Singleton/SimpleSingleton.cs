using System;

namespace cdi
{
    /// <summary>
    /// Simple Singleton pattern.
    /// </summary>
    public abstract class SimpleSingleton<T>
    {
        protected static T _instance;

        /// <summary>
        /// Singleton design pattern
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)Activator.CreateInstance(typeof(T));
                }
                return _instance;
            }
        }
    }
}
