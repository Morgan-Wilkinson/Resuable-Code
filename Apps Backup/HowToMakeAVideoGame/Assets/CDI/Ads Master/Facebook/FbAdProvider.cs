#if FBAD

namespace cdi.ad
{
    public class FbAdProvider : AdProvider
    {
        #region General
        public override ProviderID ID
        {
            get
            {
                return ProviderID.FbAd;
            }
        }

        protected override void OnIntialize()
        {
            interstitialManager = this.gameObject.AddComponent<FbInterstitialManager>();
            bannerManager = this.gameObject.AddComponent<FBBannerManager>();
        }

        void OnDestroy()
        {
        }
        #endregion

        #region Interstitial
        FbInterstitialManager interstitialManager;

        public override void ShowInterstitial(string key = null)
        {
            base.ShowInterstitial();
            interstitialManager.Show(key);
        }

        public override bool IsLoadedInterstitial(string key = null)
        {
            return interstitialManager.IsLoaded(key);
        }
        #endregion

        #region Banner
        FBBannerManager bannerManager;

        public override BannerAdUnit ShowBanner(string key = null)
        {
            return bannerManager.ShowBanner(key);
        }

        public override bool HideBanner(BannerAdUnit activedBanner)
        {
            return bannerManager.HideBanner(activedBanner);
        }
        #endregion
    }
}
#endif