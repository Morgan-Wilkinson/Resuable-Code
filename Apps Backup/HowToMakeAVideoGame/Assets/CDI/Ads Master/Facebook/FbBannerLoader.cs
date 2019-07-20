#if FBAD
using AudienceNetwork;
using AudienceNetwork.Utility;

namespace cdi.ad
{
    public class FbBannerLoader : BannerLoader<FBBannerAdUnit>
    {
        AdView adView;
        AdSize adSize;
        double DeltaY
        {
            get
            {
                if (DeviceUtil.IsTablet) return AdUtility.height() - 90.0;
                else return AdUtility.height() - 50f;
            }
        }

        public override void Init(FBBannerAdUnit adUnit)
        {
            if (DeviceUtil.IsTablet) adSize = AdSize.BANNER_HEIGHT_90;
            else adSize = AdSize.BANNER_HEIGHT_50;
            base.Init(adUnit);
        }

        protected override void OnSendRequest()
        {
            this.adView = new AdView(AdUnit.AdId, adSize);
            this.adView.Register(this.gameObject);

            // Set delegates to get notified on changes or when the user interacts with the ad.
            this.adView.AdViewDidLoad = HandleAdViewDidLoad;
            adView.AdViewDidFailWithError = HandleAdViewDidFailWithError;
            adView.AdViewWillLogImpression = HandleAdViewWillLogImpression;
            adView.AdViewDidClick = HandleAdViewDidClick;
            adView.LoadAd();
        }

        void HandleAdViewDidClick()
        {
        }

        void HandleAdViewWillLogImpression()
        {
        }

        void HandleAdViewDidFailWithError(string error)
        {
            DidFailedAdview();
        }

        void HandleAdViewDidLoad()
        {
            DidLoadedAdView();
        }

        protected override void OnShow()
        {
            base.OnShow();
            if (IsLoaded)
            {
                if (AdUnit.position == BannerPosition.Top)
                {
                    adView.Show(0d);
                }
                else
                {
                    adView.Show(DeltaY);
                }
            }
        }

        protected override void OnDisposeAd()
        {
            // Dispose of banner ad when the scene is destroyed
            if (this.adView)
            {
                this.adView.Dispose();
                this.adView = null;
            }
        }

        protected override bool OnHide()
        {
            Destroy(this.gameObject);
            return true;
        }
    }
}
#endif