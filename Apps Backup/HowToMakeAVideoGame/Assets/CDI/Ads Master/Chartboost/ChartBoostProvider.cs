#if CHARTBOOST
using ChartboostSDK;
using System.Collections;
using UnityEngine;

namespace cdi.ad
{
    public class ChartBoostProvider : AdProvider
    {
        public override ProviderID ID
        {
            get
            {
                return ProviderID.ChartBoost;
            }
        }

        protected override void OnIntialize()
        {
            Chartboost.didCacheInterstitial += didCacheInterstitial;
            Chartboost.didFailToLoadInterstitial += didFailToLoadInterstitial;
            Chartboost.didDismissInterstitial += didDismissInterstitial;
            Chartboost.didCloseInterstitial += didCloseInterstitial;

            Chartboost.didCacheRewardedVideo += didCacheRewardedVideo;
            Chartboost.didFailToLoadRewardedVideo += didFailToLoadRewardedVideo;
            Chartboost.didCloseRewardedVideo += didCloseRewardedVideo;
            Chartboost.didDismissRewardedVideo += didDismissRewardedVideo;
            Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;

            if (AdsMaster.IsAllowedReward(ProviderID.ChartBoost))
            {
                StartCoroutine(CacheRewardTask(2f));
            }
            if (AdsMaster.IsAllowedInterstitial(ProviderID.ChartBoost))
            {
                StartCoroutine(CacheInterstitialTask(0f));
            }
        }

        void Awake()
        {
            if (GameObject.FindObjectOfType<Chartboost>() == null)
            {
                var gameobject = new GameObject();
                gameobject.name = "Chartboost";
                gameobject.AddComponent<Chartboost>();
            }
        }

        #region Interstitial
        public override bool IsLoadedInterstitial(string key = null)
        {
            return Chartboost.hasInterstitial(CBLocation.Default);
        }

        IEnumerator CacheInterstitialTask(float delay)
        {
            yield return new WaitForSeconds(delay);
            Chartboost.cacheInterstitial(CBLocation.Default);
        }

        public override void ShowInterstitial(string key = null)
        {
            if (IsLoadedInterstitial(key))
            {
                Chartboost.showInterstitial(CBLocation.Default);
                AdsMaster.OnDisplayAd(this, AdType.Interstitial);
            }
        }

        private void didFailToLoadInterstitial(CBLocation location, CBImpressionError error)
        {
            //CLog.Log(this, "didFailToLoadInterstitial");
            StartCoroutine(CacheInterstitialTask(AdsMaster.RESEND_FAILED_REQUEST_DELAY));
        }

        private void didCacheInterstitial(CBLocation location)
        {
            //CLog.Log(this, "didFailToLoadInterstitial");
        }

        private void didCloseInterstitial(CBLocation location)
        {
            //CLog.Log(this, "didCloseInterstitial");
        }

        private void didDismissInterstitial(CBLocation location)
        {
            //CLog.Log(this, "didDismissInterstitial");
            AdsMaster.OnDimissAd(AdType.Interstitial);
        }

        #endregion

        #region Rewarded
        public override bool IsRewardReady
        {
            get
            {
                return Chartboost.hasRewardedVideo(CBLocation.Default);
            }
        }

        IEnumerator CacheRewardTask(float delay)
        {
            yield return new WaitForSeconds(delay);
            Chartboost.cacheRewardedVideo(CBLocation.Default);
        }

        public override void ShowReward()
        {
            if (IsRewardReady)
            {
                Chartboost.showRewardedVideo(CBLocation.Default);
                AdsMaster.OnDisplayAd(this, AdType.Rewarded);
            }
        }

        private void didCompleteRewardedVideo(CBLocation location, int reward)
        {
            //CLog.Log(this, "didCompleteRewardedVideo");
            AdsMaster.OnCompletedReward(ID, true);
        }

        private void didFailToLoadRewardedVideo(CBLocation location, CBImpressionError error)
        {
            //CLog.Log(this, "didFailToLoadRewardedVideo");
            StartCoroutine(CacheRewardTask(AdsMaster.RESEND_FAILED_REQUEST_DELAY));
        }

        private void didCacheRewardedVideo(CBLocation location)
        {
            //CLog.Log(this, "didCacheRewardedVideo");
        }

        private void didCloseRewardedVideo(CBLocation location)
        {
            //CLog.Log(this, "didCloseRewardedVideo");
            AdsMaster.OnDimissAd(AdType.Rewarded);
        }

        private void didDismissRewardedVideo(CBLocation location)
        {
            //CLog.Log(this, "didDismissRewardedVideo");
            AdsMaster.OnDimissAd(AdType.Rewarded);
            StartCoroutine(CacheRewardTask(2f));
        }
        #endregion
    }
}
#endif