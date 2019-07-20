using System;

namespace cdi.ad
{
    [Serializable]
    public abstract class InterstitialAdUnit
    {
        public string key = "";
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

        public InterstitialAdUnit(string key)
        {
            this.key = key;
        }
    }
}