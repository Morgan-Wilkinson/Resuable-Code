using System;

namespace cdi.ad
{
    [Serializable]
    public class FBNativeAdUnit
    {
        public string key = "";
        public string androidPlacementId = "";
        public string iosPlacementId = "";

        public bool preload = true;
        public float minSecondsToReload = 30f;

        public string PlacementId
        {
            get
            {
#if UNITY_IOS
                return iosPlacementId;
#else
                return androidPlacementId;
#endif
            }
        }

        public FBNativeAdUnit(string key)
        {
            this.key = key;
        }
    }
}