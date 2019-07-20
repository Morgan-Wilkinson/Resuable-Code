using UnityEngine;

namespace cdi.ad
{
    public abstract class AdProvider : MonoBehaviour
    {
        protected AdsMaster manager;

        public abstract ProviderID ID { get; }

        public void Initialize(AdsMaster manager)
        {
            this.manager = manager;
            // Only init if provider is assigned to load any ad.
            if ((IsSupport(AdType.Interstitial) && AdsMaster.IsAllowedInterstitial(ID)) ||
                (IsSupport(AdType.Rewarded) && AdsMaster.IsAllowedReward(ID)) ||
                (IsSupport(AdType.Banner) && AdsMaster.IsAllowedBanner(ID)))
            {
                OnIntialize();
            }
        }

        protected abstract void OnIntialize();

        public bool IsSupport(AdType adType) { return ProvidersConfig.IsSupport(ID, adType); }

        public virtual bool IsLoadedInterstitial(string key = null) { return false; }
        public virtual void ShowInterstitial(string key = null) { }

        /// <summary>
        /// Show the banner that has matched with key. Return ad unit if there is one banner matched.
        /// </summary>
        /// <returns></returns>
        public virtual BannerAdUnit ShowBanner(string key = null) { return null; }
        /// <summary>
        /// Return true if found the active banner and hide it
        /// </summary>
        /// <returns></returns>
        public virtual bool HideBanner(BannerAdUnit activedBanner) { return false; }

        public virtual bool IsRewardReady { get { return false; } }

        public virtual void ShowReward() { }
    }
}