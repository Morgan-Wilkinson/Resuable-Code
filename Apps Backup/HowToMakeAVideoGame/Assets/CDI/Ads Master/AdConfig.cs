using System;
using System.Collections.Generic;

namespace cdi.ad
{
    [Serializable]
    public class AdConfig
    {
        /// <summary>
        /// Tăng giá trị này khi thay đối tên bất kỳ trường nào trong class này.
        /// </summary>
        public const int DATA_VERSION = 7;

        // Just use to export to json
        public int Version;

        // Chỉ sử dụng dữ liệu khi dữ liệu tải về có version trùng với DATA_VERSION
        public bool IsSupported { get { return DATA_VERSION == this.Version; } }

        // ADMOB
        public string AdmobIOSRewarded = "";
        public string AdmobAndroidRewarded = "";
        public string AdmobRewarded
        {
            get
            {
#if UNITY_ANDROID

                return AdmobAndroidRewarded;
#elif UNITY_IOS
                return AdmobIOSRewarded;
#else
                return "unexpected_platform";
#endif
            }
        }

        public List<AdmobInterstitialAdUnit> admobInterstitials = new List<AdmobInterstitialAdUnit>();
        public List<AdmobBannerUnit> AdmobBannerUnits = new List<AdmobBannerUnit>();

        // UNITY ADS
        public string UnityAdIdIOS = "";
        public string UnityAdIOSVideoPlacementId = "video";
        public string UnityAdIOSRewardedPlacementId = "rewardedVideo";
        public string UnityAdIdAndroid = "";
        public string UnityAdAndroidVideoPlacementId = "video";
        public string UnityAdAndroidRewardedPlacementId = "rewardedVideo";

        // Vungle
        public string VungleAndroidAppId = "";
        public string VungleAndroidDefaultPlacementId = "";
        public string VungleIOSAppId = "";
        public string VungleIOSDefaultPlacementId = "";

        // Facebook Data
        public FBRewardedUnit fbRewarded = new FBRewardedUnit();
        public List<FbInterstitialUnit> fbInterstitials = new List<FbInterstitialUnit>();
        public List<FBBannerAdUnit> fbBanners = new List<FBBannerAdUnit>();
        public List<FBNativeAdUnit> fbNativeAds = new List<FBNativeAdUnit>();

        // Profiles
        public List<ProviderID> InterstitialIOS = new List<ProviderID>();
        public List<ProviderID> RewardIOS = new List<ProviderID>();
        public List<ProviderID> BannerIOS = new List<ProviderID>();

        public List<ProviderID> InterstitialAndroid = new List<ProviderID>();
        public List<ProviderID> RewardAndroid = new List<ProviderID>();
        public List<ProviderID> BannerAndroid = new List<ProviderID>();

        // Connection
        public float RequestTimeOut = 180f;
        public float AdExpiration = 3000f;

        // Time limit for interstitial
        public bool IsTimeLimited = false;
        public float DelayInterstitialFromStartApp = 100f;
        public float DelayInterstitialFromFristOpen = 100f;
        public float BetweenInterstitialLimited = 0f;
        public bool RequiredInternetConnection = false;

        // Profile Mode
        public ProfileMode interstitialProfileMode = ProfileMode.Priority;
        public ProfileMode bannerProfileMode = ProfileMode.Priority;
        public ProfileMode rewardedProfileMode = ProfileMode.Priority;

        // Limit for interstitial
        public bool IsSkipInterstitial = false;
        public int SkipInterstitialStep = 0;
    }
}