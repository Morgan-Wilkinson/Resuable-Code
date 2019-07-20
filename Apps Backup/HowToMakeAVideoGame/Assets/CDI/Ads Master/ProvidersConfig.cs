namespace cdi.ad
{
    public class ProvidersConfig
    {
        /// <summary>
        /// Đánh dấu khả năng support của các provider
        /// </summary>
        static bool[,] supported = new bool[,]
        {
            //Interstitial, Rewarded, Banner, Video ,Native
            // Admob
            { true, true, true},
            // Facebook Ad
            { true, false, true},
            // UnityAds
            { true, true, false},
            // Vungle
            { false, true, false},
            // Chartboost
            { true, true, false}
        };

        public static bool IsSupport(ProviderID id, AdType type)
        {
            return supported[(int)id, (int)type];
        }
    }
}