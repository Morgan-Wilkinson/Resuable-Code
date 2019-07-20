using System;

namespace cdi.ad
{
    [Serializable]
    public class FBRewardedUnit
    {
        public string androidAdId = "";
        public string iosAdId = "";

        public string AdId
        {
            get
            {
#if UNITY_IOS
                return iosAdId;
#elif UNITY_ANDROID
                return androidAdId;
#else
                return "";
#endif
            }
        }
    }
}