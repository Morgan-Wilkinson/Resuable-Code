#if ADMOB
using GoogleMobileAds.Api;
using System;

namespace cdi.ad
{
    public class AdmobInterstitialLoader : InterstitialAdLoader<AdmobInterstitialAdUnit>
    {
        InterstitialAd interstitial;

        protected override void OnDisposeAd()
        {
            if (this.interstitial != null)
            {
                // Register for ad events.
                this.interstitial.OnAdLoaded -= this.HandleInterstitialLoaded;
                this.interstitial.OnAdFailedToLoad -= this.HandleInterstitialFailedToLoad;
                this.interstitial.OnAdOpening -= this.HandleInterstitialOpened;
                this.interstitial.OnAdClosed -= this.HandleInterstitialClosed;
                this.interstitial.OnAdLeavingApplication -= this.HandleInterstitialLeftApplication;
                this.interstitial.Destroy();
                this.interstitial = null;
            }
        }

        protected override void OnSendRequest()
        {
            // Create an interstitial.
            this.interstitial = new InterstitialAd(AdUnit.AdId);
            // Register for ad events.
            this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
            this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
            this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
            this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
            this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;
            // Load an interstitial ad.
            this.interstitial.LoadAd(AdmobProvider.CreateAdRequest());
        }

        protected override void OnShow()
        {
            if (this.interstitial != null) this.interstitial.Show();
        }

#region listener
        private void HandleInterstitialLoaded(object sender, EventArgs e)
        {
            DidLoadedAd();
        }

        private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            DidFailedAd();
        }

        private void HandleInterstitialOpened(object sender, EventArgs e)
        {
        }

        private void HandleInterstitialClosed(object sender, EventArgs e)
        {
            DidCloseAd();
        }

        private void HandleInterstitialLeftApplication(object sender, EventArgs e)
        {
        }
#endregion
    }
}
#endif