using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace cdi.ad
{
    public class AdsMasterSample : MonoBehaviour
    {
        [SerializeField]
        Text LabelSelectedNetwork;

        [SerializeField]
        Text LabelCoins;

        [SerializeField]
        Text LabelRewardedStatus;

        int SelectedAdNetwork = 0;
        AdProvider adPartner;
        int coin = 0;
        bool hasRewarded;
        WaitForSecondsRealtime checkLoadedRewardedDelay;

        void Awake()
        {
            CLog.IncludeTime = true;
            CLog.SetEnableLog(true);
            CLog.SetEnabledDeviceLog(true);
            checkLoadedRewardedDelay = new WaitForSecondsRealtime(1);
            StartCoroutine(CheckRewardedTimer());
        }

        IEnumerator CheckRewardedTimer()
        {
            yield return checkLoadedRewardedDelay;
            do
            {
                hasRewarded = AdsMaster.HasReward();
                yield return checkLoadedRewardedDelay;
            } while (true);
        }

        string ToNameNetwork()
        {
            if (SelectedAdNetwork == 0)
                return "All";
            else if (SelectedAdNetwork == 1)
                return "Admob";
            else if (SelectedAdNetwork == 2)
                return "Chartboost";
            else if (SelectedAdNetwork == 3)
                return "UnityAd";
            else if (SelectedAdNetwork == 4)
                return "Vungle";
            else if (SelectedAdNetwork == 5)
                return "Facebook";
            else
                return "Unknown";
        }

        public void OnClickedChangeNetwork()
        {
            SelectedAdNetwork++;
            if (SelectedAdNetwork > 5)
                SelectedAdNetwork = 0;
            if (SelectedAdNetwork == 1)
            {
                adPartner = AdsMaster.FindActiveAdProvider(ProviderID.Admob);
            }
            else if (SelectedAdNetwork == 2)
            {
                adPartner = AdsMaster.FindActiveAdProvider(ProviderID.ChartBoost);
            }
            else if (SelectedAdNetwork == 3)
            {
                adPartner = AdsMaster.FindActiveAdProvider(ProviderID.UnityAd);
            }
            else if (SelectedAdNetwork == 4)
            {
                adPartner = AdsMaster.FindActiveAdProvider(ProviderID.Vungle);
            }
            else if (SelectedAdNetwork == 5)
            {
                adPartner = AdsMaster.FindActiveAdProvider(ProviderID.FbAd);
            }
            else adPartner = null;
        }

        public void OnClickedShowInterstitial()
        {
            if (SelectedAdNetwork == 0)
                AdsMaster.ShowInterstitial((result) =>
                {
                    CLog.Log("Interstitial result " + result);
                });
            else if (adPartner != null && adPartner.IsSupport(AdType.Interstitial))
                adPartner.ShowInterstitial();
        }

        public void OnClickedShowInterstitialAd1()
        {
            AdsMaster.ShowInterstitial(null, "ad1");
        }

        public void OnClickedShowInterstitialAd2()
        {
            AdsMaster.ShowInterstitial(null, "ad2");
        }

        public void OnClickedTopBanner()
        {
            AdsMaster.ShowBannerIfLoaded("top banner");
        }

        public void OnClickedBottomBanner()
        {
            AdsMaster.ShowBannerIfLoaded("bottom banner");
        }

        public void OnClickedDefaultBanner()
        {
            AdsMaster.ShowBannerIfLoaded();
        }

        public void OnClickedHideBanner()
        {
            AdsMaster.HideBanner();
        }

        public void OnClickedShowRewarded()
        {
            if (SelectedAdNetwork == 0)
            {
                AdsMaster.ShowReward(success => { if (success) coin++; });
            }
            else if (adPartner != null && adPartner.IsSupport(AdType.Rewarded))
                adPartner.ShowReward();
        }

        public void OnClickedShowBannerSoon()
        {
            AdsMaster.ShowBanner();
        }

        void Update()
        {
            if (LabelSelectedNetwork) LabelSelectedNetwork.text = ToNameNetwork();
            if (LabelCoins) LabelCoins.text = "Coin " + coin;
            if (LabelRewardedStatus) LabelRewardedStatus.text = hasRewarded ? "Loaded Rewarded Video" : "Loading Rewarded Video";
        }
    }
}