using System;

namespace cdi.ad
{
    [Serializable]
    public abstract class BannerAdUnit
    {
        public string key = "";
        public string androidAdId = "";
        public string iosAdId = "";

        public BannerPosition position = BannerPosition.Bottom;

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

        public BannerAdUnit(string key)
        {
            this.key = key;
        }
    }
}