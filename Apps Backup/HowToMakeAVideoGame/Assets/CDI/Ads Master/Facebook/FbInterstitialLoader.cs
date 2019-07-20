#if FBAD
using AudienceNetwork;

namespace cdi.ad
{
    public class FbInterstitialLoader : InterstitialAdLoader<FbInterstitialUnit>
    {
        InterstitialAd interstitialAd;

        protected override void OnDisposeAd()
        {
            if (interstitialAd != null)
            {
                interstitialAd.InterstitialAdDidLoad = null;
                interstitialAd.InterstitialAdDidFailWithError = null;
                interstitialAd.InterstitialAdWillLogImpression = null;
                interstitialAd.InterstitialAdDidClick = null;
                interstitialAd.InterstitialAdWillClose = null;
                interstitialAd.Dispose();
                interstitialAd = null;
            }
        }

        protected override void OnSendRequest()
        {
            //CLog.Log(this, "Interstitial is loading");
            // Create the interstitial unit with a placement ID (generate your own on the Facebook app settings).
            // Use different ID for each ad placement in your app.
            interstitialAd = new InterstitialAd(AdUnit.AdId);
            interstitialAd.Register(this.gameObject);

            // Set delegates to get notified on changes or when the user interacts with the ad.
            interstitialAd.InterstitialAdDidLoad = HandleInterstitialDidLoad;
            interstitialAd.InterstitialAdDidFailWithError = HandleInterstitialLoadFail;
            interstitialAd.InterstitialAdWillLogImpression = HandleInterstitialWillImpression;
            interstitialAd.InterstitialAdDidClick = HandleInterstitialDidClick;
            interstitialAd.interstitialAdDidClose = HandleInterstitialDidClose;
            // Initiate the request to load the ad.
            interstitialAd.LoadAd();
            //CLog.Log(this, "Sent request id " + AdUnit.AdId);
        }

        protected override void OnShow()
        {
            if (interstitialAd) interstitialAd.Show();
        }

        void HandleInterstitialDidLoad()
        {
            DidLoadedAd();
        }

        void HandleInterstitialLoadFail(string error)
        {
            DidFailedAd();
        }

        void HandleInterstitialWillImpression()
        {
        }

        void HandleInterstitialDidClick()
        {
        }

        void HandleInterstitialDidClose()
        {
            DidCloseAd();
        }
    }
}
#endif