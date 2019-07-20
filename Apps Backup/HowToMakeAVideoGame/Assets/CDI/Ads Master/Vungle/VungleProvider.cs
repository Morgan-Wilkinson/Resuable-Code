#if VUNGLE
namespace cdi.ad
{
    public class VungleProvider : AdProvider
    {
        public override ProviderID ID
        {
            get
            {
                return ProviderID.Vungle;
            }
        }

        protected override void OnIntialize()
        {
            Vungle.setLogEnable(AdsMaster.settings.TestMode);
#if UNITY_IPHONE
            Vungle.init(AdsMaster.config.VungleIOSAppId, new string[] { AdsMaster.config.VungleIOSDefaultPlacementId });
#else
            Vungle.init(AdsMaster.config.VungleAndroidAppId, new string[] { AdsMaster.config.VungleAndroidDefaultPlacementId });
#endif
            Vungle.adPlayableEvent += HandleLoadedAds;
            Vungle.onAdFinishedEvent += HandleAdFinished;
            Vungle.onAdStartedEvent += HandleAdStarted;
        }

        void HandleAdStarted(string placementID)
        {
            AdsMaster.OnDisplayAd(this, AdType.Rewarded);
        }

        void HandleAdFinished(string placementID, AdFinishedEventArgs arg)
        {
            AdsMaster.OnCompletedReward(ID, arg.IsCompletedView);
            AdsMaster.OnDimissAd(AdType.Rewarded);
        }

        void HandleLoadedAds(string placementID, bool adPlayable)
        {

        }

        public override bool IsRewardReady
        {
            get
            {
#if UNITY_IPHONE
                return Vungle.isAdvertAvailable(AdsMaster.config.VungleIOSDefaultPlacementId);
#else
                return Vungle.isAdvertAvailable(AdsMaster.config.VungleAndroidDefaultPlacementId);
#endif
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Vungle.onPause();
            }
            else
            {
                Vungle.onResume();
            }
        }

        public override void ShowReward()
        {
            base.ShowReward();
#if UNITY_IPHONE
            Vungle.playAd(AdsMaster.config.VungleIOSDefaultPlacementId);
#else
            Vungle.playAd(AdsMaster.config.VungleAndroidDefaultPlacementId);
#endif
        }
    }
}
#endif