#if ADMOB
using GoogleMobileAds.Api;
using System;

namespace cdi.ad
{
    public class AdmobBannerLoader : BannerLoader<AdmobBannerUnit>
    {
        AdSize admobSize;
        AdPosition admobPosition;
        BannerView bannerView = null;

        public override void Init(AdmobBannerUnit adUnit)
        {
            this.admobSize = ConvertAdSize(adUnit.size);
            this.admobPosition = ConvertAdPosition(adUnit.position);
            base.Init(adUnit);
        }

        protected override void OnSendRequest()
        {
            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(AdUnit.AdId, admobSize, admobPosition);
            // Register for ad events.
            this.bannerView.OnAdLoaded += this.HandleBannerLoaded;
            this.bannerView.OnAdFailedToLoad += this.HandleBannerFailedToLoad;
            this.bannerView.OnAdLoaded += this.HandleBannerOpened;
            this.bannerView.OnAdClosed += this.HandleBannerClosed;
            this.bannerView.OnAdLeavingApplication += this.HandleBannerLeftApplication;
            // Load a banner ad.
            this.bannerView.LoadAd(AdmobProvider.CreateAdRequest());
            this.bannerView.Hide();
            //CLog.Log(this, "Send ad request :" + AdUnit.key + "\t" + AdUnit.AdId + "\t" + admobSize.ToString() + "\t" + admobPosition.ToString());
        }

        protected override void OnDisposeAd()
        {
            if (this.bannerView != null)
            {
                this.bannerView.OnAdLoaded -= this.HandleBannerLoaded;
                this.bannerView.OnAdFailedToLoad -= this.HandleBannerFailedToLoad;
                this.bannerView.OnAdLoaded -= this.HandleBannerOpened;
                this.bannerView.OnAdClosed -= this.HandleBannerClosed;
                this.bannerView.OnAdLeavingApplication -= this.HandleBannerLeftApplication;
                this.bannerView.Destroy();
                this.bannerView = null;
            }
        }

        protected override void OnShow()
        {
            base.OnShow();
            bannerView.Show();
        }

        protected override bool OnHide()
        {
            //if (bannerView != null)
            //{
            //    bannerView.Hide();
            //}
            //return false;
            // Note: bắt buộc destroy banner vì gặp lỗi không click được vào vùng hiển thị banner sau khi hide banner
            Destroy(this.gameObject);
            return true;
        }

        void HandleBannerLoaded(object sender, EventArgs args)
        {
            //CLog.Log(this, AdUnit.key + ":" + "HandleBannerLoaded event received");
            DidLoadedAdView();
        }

        void HandleBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            //CLog.Log(this, AdUnit.key + ":" + "HandleBannerFailedToLoad event received with message: " + args.Message);
            DidFailedAdview();
        }

        void HandleBannerOpened(object sender, EventArgs args)
        {
            //CLog.Log(this, AdUnit.key + ":" + "HandleBannerOpened event received");
        }

        void HandleBannerClosed(object sender, EventArgs args)
        {
            //CLog.Log(this, AdUnit.key + ":" + "HandleBannerClosed event received");
        }

        void HandleBannerLeftApplication(object sender, EventArgs args)
        {
            //CLog.Log(this, AdUnit.key + ":" + "HandleBannerLeftApplication event received");
        }

        AdPosition ConvertAdPosition(BannerPosition position)
        {
            if (position == BannerPosition.Top) return AdPosition.Top;
            else if (position == BannerPosition.Bottom) return AdPosition.Bottom;
            else if (position == BannerPosition.BottomLeft) return AdPosition.BottomLeft;
            else if (position == BannerPosition.BottomRight) return AdPosition.BottomRight;
            else if (position == BannerPosition.TopLeft) return AdPosition.TopLeft;
            else if (position == BannerPosition.TopRight) return AdPosition.TopRight;
            else return AdPosition.Center;
        }

        AdSize ConvertAdSize(BannerSize size)
        {
            if (size == BannerSize.Smart) return AdSize.SmartBanner;
            else if (size == BannerSize.IABBanner) return AdSize.IABBanner;
            else if (size == BannerSize.Leaderboard) return AdSize.Leaderboard;
            else if (size == BannerSize.MediumRectangle) return AdSize.MediumRectangle;
            else return AdSize.Banner;
        }
    }
}
#endif