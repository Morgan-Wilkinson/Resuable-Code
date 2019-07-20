#if ADMOB
using GoogleMobileAds.Api;
using System;

namespace cdi.ad
{
    public class AdmobRewardedManager : BaseRewardedManager
    {
        protected override ProviderID providerId
        {
            get
            {
                return ProviderID.Admob;
            }
        }

        public override bool IsLoaded()
        {
            if (AdsMaster.SupportedAdPlatform) return RewardBasedVideoAd.Instance.IsLoaded();
            else return false;
        }

        protected override bool IsValidConfiguration()
        {
            return !string.IsNullOrEmpty(AdsMaster.config.AdmobRewarded);
        }

        protected override void OnInitialize()
        {
            var rewardBasedVideo = RewardBasedVideoAd.Instance;
            rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
            // has failed to load.
            rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            // is opened.
            //rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
            // has started playing.
            //rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
            // has rewarded the user.
            rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
            // is closed.
            rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
            // is leaving the application.
            //rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
        }

        protected override void OnSentRequest()
        {
            AdRequest request = AdmobProvider.CreateAdRequest();
            RewardBasedVideoAd.Instance.LoadAd(request, AdsMaster.config.AdmobRewarded);
        }

        protected override void OnShow()
        {
            RewardBasedVideoAd.Instance.Show();
        }

        #region listener
        void HandleRewardBasedVideoClosed(object sender, EventArgs e)
        {
            DidClosedVideo();
        }

        void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
        {
            DidLoadedVideo();
        }

        void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs e)
        {
            DidFailedToLoadVideo();
        }

        void HandleRewardBasedVideoRewarded(object sender, Reward e)
        {
            DidCompletedVideo(true);
        }
        #endregion
    }
}
#endif