using UnityEngine;

namespace cdi
{

    /// <summary>
    /// 
    /// </summary>
    public class NetworkUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool HasNetworkConnection()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}