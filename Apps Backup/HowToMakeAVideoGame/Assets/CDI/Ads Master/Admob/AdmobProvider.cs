#if ADMOB
using GoogleMobileAds.Api;

namespace cdi.ad
{
    public class AdmobProvider : AdProvider
    {
        #region General
        public override ProviderID ID
        {
            get
            {
                return ProviderID.Admob;
            }
        }

        protected override void OnIntialize()
        {
            //GoogleMobileAds.Api.MobileAds.Initialize("ca-app-pub-3940256099942544~3347511713");
            interstitialManager = this.gameObject.AddComponent<AdmobInterstitialManager>();
            bannerManager = this.gameObject.AddComponent<AdmobBannerManager>();
            rewardedManager = this.gameObject.AddComponent<AdmobRewardedManager>();
        }

        public static AdRequest CreateAdRequest()
        {
            var request = new AdRequest.Builder();
            if (AdsMaster.settings.AdMobDebug)
            {
                request.AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("0123456789ABCDEF0123456789ABCDEF");
            }
            return request.Build();
            /// IMPORTANCE Khi thêm 2 dòng sau vào sẽ có thể không hiển thị banner (Báo No Fill)
            ///   .TagForChildDirectedTreatment(false)
            //    .AddExtra("color_bg", "9B30FF")
            ///
        }
        #endregion

        #region ========================== Banner
        AdmobBannerManager bannerManager;

        public override BannerAdUnit ShowBanner(string key = null)
        {
            return bannerManager.ShowBanner(key);
        }

        public override bool HideBanner(BannerAdUnit activedBanner)
        {
            return bannerManager.HideBanner(activedBanner);
        }
        #endregion

        #region Interstitial
        AdmobInterstitialManager interstitialManager;

        public override void ShowInterstitial(string key = null)
        {
            base.ShowInterstitial(key);
            interstitialManager.Show(key);
        }

        public override bool IsLoadedInterstitial(string key = null)
        {
            return interstitialManager.IsLoaded(key);
        }
        #endregion

        #region ========================== Rewarded
        AdmobRewardedManager rewardedManager;

        public override bool IsRewardReady
        {
            get
            {
                return rewardedManager.IsLoaded();
            }
        }

        public override void ShowReward()
        {
            rewardedManager.Show();
        }
        #endregion
    }
}
#endif