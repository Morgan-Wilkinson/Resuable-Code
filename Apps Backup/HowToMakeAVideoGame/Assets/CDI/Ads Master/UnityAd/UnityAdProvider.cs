#if UNITYAD
using UnityEngine.Advertisements;

namespace cdi.ad
{
    public class UnityAdProvider : AdProvider
    {
        string VideoPlacementId
        {
            get
            {
#if UNITY_ANDROID

                return AdsMaster.config.UnityAdAndroidVideoPlacementId;
#elif UNITY_IOS
                return AdsMaster.config.UnityAdIOSVideoPlacementId;
#else
                return "";
#endif
            }
        }

        string RewardedPlacementId
        {
            get
            {
#if UNITY_ANDROID

                return AdsMaster.config.UnityAdAndroidRewardedPlacementId;
#elif UNITY_IOS
                return AdsMaster.config.UnityAdIOSRewardedPlacementId;
#else
                return "";
#endif
            }
        }
        public override ProviderID ID
        {
            get
            {
                return ProviderID.UnityAd;
            }
        }

        protected override void OnIntialize()
        {
            string gameId = null;
#if UNITY_ANDROID
            gameId = AdsMaster.config.UnityAdIdAndroid;
#elif UNITY_IOS
            gameId = AdsMaster.config.UnityAdIdIOS;
#endif
            if (Advertisement.isSupported && !Advertisement.isInitialized)
            {
                Advertisement.Initialize(gameId, AdsMaster.settings.UnityAdDebug);
            }
        }

        public override bool IsLoadedInterstitial(string key = null)
        {
            return Advertisement.IsReady(VideoPlacementId);
        }

        public override void ShowInterstitial(string key = null)
        {
            if (IsLoadedInterstitial())
            {
                AdsMaster.OnDisplayAd(this, AdType.Interstitial);
                var options = new ShowOptions { resultCallback = HandleShowResultInterstitial };
                Advertisement.Show(VideoPlacementId, options);
            }
        }

        void HandleShowResultInterstitial(ShowResult result)
        {
            AdsMaster.OnDimissAd(AdType.Interstitial);
        }

        public override bool IsRewardReady
        {
            get
            {
                return Advertisement.IsReady(RewardedPlacementId);
            }
        }

        public override void ShowReward()
        {
            if (IsRewardReady)
            {
                var options = new ShowOptions { resultCallback = HandleShowResultReward };
                AdsMaster.OnDisplayAd(this, AdType.Rewarded);
                Advertisement.Show(RewardedPlacementId, options);
            }
        }

        private void HandleShowResultReward(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    CLog.Log(this, "The ad was successfully shown.");
                    AdsMaster.OnCompletedReward(ProviderID.UnityAd, true);
                    break;
                case ShowResult.Skipped:
                    CLog.Log(this, "The ad was skipped before reaching the end.");
                    AdsMaster.OnCompletedReward(ProviderID.UnityAd, false);
                    break;
                case ShowResult.Failed:
                    CLog.Err(this, "The ad failed to be shown.");
                    AdsMaster.OnCompletedReward(ProviderID.UnityAd, false);
                    break;
            }
            AdsMaster.OnDimissAd(AdType.Rewarded);
        }
    }
}
#endif