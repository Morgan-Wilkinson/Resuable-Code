using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cdi.ad
{
    public class AdsMaster : MonoBehaviour
    {
        #region Constants
        const string KeyRemoteConfigAd = "cdi.ad.remote_config";

        /// <summary>
        /// Sử dụng để xác định platform hiện tại có hỗ trợ Ads hay không. Tránh việc gọi native code trên editor.
        /// </summary>
        public static bool SupportedAdPlatform
        {
            get
            {
#if UNITY_EDITOR
                return false;
#elif UNITY_IOS
                return true;
#elif UNITY_ANDROID
                return true;
#else
                return false;
#endif
            }
        }

        public const float RESEND_FAILED_REQUEST_DELAY = 10f;
        #endregion

        #region GUI
        void OnGUI()
        {
            if (settings != null && settings.TestMode)
            {
                GUIStyle myStyle = new GUIStyle();
                myStyle.fontSize = 15;
                GUI.Label(new Rect(10, 10, 100, 20), "Debug", myStyle);
            }
        }
        #endregion

        #region Listeners
        public static event InterstitialCallBack allInterstitialsEvent;

        /// <summary>
        /// Không hỗ trợ bắt sự kiện load rewarded vì UnityAds không hỗ trợ bắt loaded ads
        /// </summary>
        //public event Action<bool> ReadyRewardedListener;
        #endregion

        #region Profiles
        public static bool IsAllowedInterstitial(ProviderID network)
        {
            List<ProviderID> networks = null;
#if UNITY_ANDROID
            networks = config.InterstitialAndroid;
#elif UNITY_IOS
            networks = config.InterstitialIOS;
#endif
            return networks != null && networks.Contains(network);
        }

        public static bool IsAllowedReward(ProviderID network)
        {
            List<ProviderID> networks = null;
#if UNITY_ANDROID
            networks = config.RewardAndroid;
#elif UNITY_IOS
            networks = config.RewardIOS;
#endif
            return networks != null && networks.Contains(network);
        }

        public static bool IsAllowedBanner(ProviderID network)
        {
            List<ProviderID> networks = null;
#if UNITY_ANDROID
            networks = config.BannerAndroid;
#elif UNITY_IOS
            networks = config.BannerIOS;
#endif
            return networks != null && networks.Contains(network);
        }
        #endregion

        #region General
        static AdsMaster instance;
        static List<AdProvider> Providers = new List<AdProvider>();
        static DateTime AppLaunchTime;
        static float? savedTimeScale = null;

        static int _firstOpen = -1;
        static bool FirstOpen
        {
            get
            {
                // Data is not ready
                if (_firstOpen < 0)
                {
                    // load from cache
                    _firstOpen = PlayerPrefs.GetInt(KeyRemoteConfigAd + "FirstOpen", 1);
                    if (_firstOpen == 1) // the first time open app
                    {
                        // Save for next time open app
                        PlayerPrefs.SetInt(KeyRemoteConfigAd + "FirstOpen", 0);
                        PlayerPrefs.Save();
                    }
                }
                return _firstOpen == 1;
            }
        }
#if ADMOB
        static AdmobProvider admobProvider;
#endif

        public static AdsMasterSetting settings;
        public static AdConfig config { get { return remoteConfig != null ? remoteConfig : settings.localConfig; } }

        public static void Initialize(AdsMasterSetting settings)
        {
            if (instance != null || settings == null) return;
            // Tạo game object
            var go = new GameObject("AdsMaster");
            instance = go.AddComponent<AdsMaster>();
            DontDestroyOnLoad(go);
            // Assign settings
            AdsMaster.settings = settings;
            // Load remote config if need
            instance.InitRemoteConfig();
            InitAdNetworks();
#if FBAD
            FbNativeAdManager.Initialize();
#endif
        }

        static void InitAdNetworks()
        {
            AppLaunchTime = DateTime.Now;
#if ADMOB
            // Admob plugin không hỗ trợ Editor. Không sử dụng #if lồng nhau được
            admobProvider = instance.gameObject.AddComponent<AdmobProvider>();
            Providers.Add(admobProvider);
#endif
#if CHARTBOOST
            var Chartboost = instance.gameObject.AddComponent<ChartBoostProvider>();
            Providers.Add(Chartboost);
#endif
#if UNITYAD
            var UnityAd = instance.gameObject.AddComponent<UnityAdProvider>();
            Providers.Add(UnityAd);
#endif
#if VUNGLE
            var vungle = instance.gameObject.AddComponent<VungleProvider>();
            Providers.Add(vungle);
#endif
#if FBAD
            var fbad = instance.gameObject.AddComponent<FbAdProvider>();
            Providers.Add(fbad);
#endif
            foreach (AdProvider partner in Providers)
            {
                if (partner != null)
                {
                    partner.Initialize(instance);
                }
            }
        }

        static List<ProviderID> GetProfileByType(AdType type)
        {
            List<ProviderID> selectedProviderIds = null;
            if (type == AdType.Interstitial)
            {
#if UNITY_ANDROID
                selectedProviderIds = config.InterstitialAndroid;
#elif UNITY_IOS
                selectedProviderIds = config.InterstitialIOS;
#endif
            }
            else if (type == AdType.Rewarded)
            {
#if UNITY_ANDROID
                selectedProviderIds = config.RewardAndroid;
#elif UNITY_IOS
                selectedProviderIds = config.RewardIOS;
#endif
            }
            else if (type == AdType.Banner)
            {
#if UNITY_ANDROID
                selectedProviderIds = config.BannerAndroid;
#elif UNITY_IOS
                selectedProviderIds = config.BannerIOS;
#endif
            }
            return selectedProviderIds;
        }

        public static AdProvider FindActiveAdProvider(ProviderID idProvider)
        {
            if (instance == null) return null;
            AdProvider res = null;
            foreach (var provider in Providers)
            {
                if (provider.ID == idProvider)
                {
                    res = provider;
                    break;
                }
            }
            return res;
        }
        #endregion

        #region Remote
        static AdConfig remoteConfig;

        void InitRemoteConfig()
        {
            if (!settings.ActiveRemoteConfig)
            {
                PlayerPrefs.DeleteKey(KeyRemoteConfigAd);
                return;
            }
            //CLog.Log(this, "Init remote config");
            // Kiểm tra config đã lưu
            string json = PlayerPrefs.GetString(KeyRemoteConfigAd, null);
            if (!string.IsNullOrEmpty(json))
            {
                // try to convert
                try
                {
                    var cachedRemoteConfig = JsonUtility.FromJson<AdConfig>(json);
                    // Không sử dụng cached của phien bản khác
                    if (cachedRemoteConfig.IsSupported)
                    {
                        PlayerPrefs.DeleteKey(KeyRemoteConfigAd);
                    }
                    else
                    {
                        remoteConfig = cachedRemoteConfig;
                    }
                }
                catch (Exception)
                {
                    //CLog.Warn(this, "Existing config is corrupted");
                }
            }
            //CLog.Log(this, "Inited remote config");
            StartCoroutine(LoadRemoteConfig(remoteConfig == null));
        }

        IEnumerator LoadRemoteConfig(bool applyImmediately)
        {
            WWW www = new WWW(settings.RemoteConfigUrl);
            yield return www;
            if (www.error == null)
            {
                try
                {
                    var config = JsonUtility.FromJson<AdConfig>(www.text);
                    if (config == null || !config.IsSupported) yield break;
                    if (applyImmediately) remoteConfig = config;
                    // Chỉ lưu json khi đảm bảo nó convert được sang object
                    PlayerPrefs.SetString(KeyRemoteConfigAd, www.text);
                    PlayerPrefs.Save();
                }
                catch (Exception)
                {
                    //CLog.Warn(this, "Existing config is corrupted");
                }
            }
            // Không gửi request tiếp nếu bị lỗi => giảm traffic đến server
        }
        #endregion

        #region Interstitial
        /// <summary>
        /// Cached call back của interstitial gần nhất. Được giải phóng ngay khi thực hiện xong interstitial
        /// </summary>
        static InterstitialCallBack cachedInterstitialCallback;
        //////////Interstitial limitted
        static DateTime LastInterstitialTime;
        static bool _EnableInterstitial = false;
        static bool EnableInterstitial
        {
            get
            {
                if (!_EnableInterstitial)
                {
                    double timeFromAppLauch = TimeUtil.SecondsFrom(AppLaunchTime);

                    bool notLimitByFirstOpen = !FirstOpen || config.DelayInterstitialFromFristOpen == 0 ||
                         timeFromAppLauch >= config.DelayInterstitialFromFristOpen;

                    bool notLimitByStartApp = config.DelayInterstitialFromStartApp == 0 ||
                        timeFromAppLauch >= config.DelayInterstitialFromStartApp;

                    if (notLimitByFirstOpen && notLimitByStartApp)
                    {
                        _EnableInterstitial = true;
                    }
                }
                return _EnableInterstitial;
            }
        }

        // Cho phép hiển thị lần đầu
        static int skipInterstitialCount = 0;

        public static bool ShowInterstitial(InterstitialCallBack callback = null, string key = null)
        {
            if (instance == null) return false;
            // Kiểm tra kết nối internet.
            if (config.RequiredInternetConnection && !NetworkUtil.HasNetworkConnection())
            {
                NotifyInterstitialEvent(callback, ShowAdResult.NoInternet);
                return false;
            }
            // Kiểm tra giới hạn thời gian
            if (config.IsTimeLimited &&
                (!EnableInterstitial || TimeUtil.SecondsFrom(LastInterstitialTime) < config.BetweenInterstitialLimited))
            {
                //CLog.Log(this, "Showing interstitial has been skipped for startup");
                NotifyInterstitialEvent(callback, ShowAdResult.SkipByOther);
                return false;
            }
            // Check skip by step
            if (config.IsSkipInterstitial)
            {
                if (skipInterstitialCount == -1)
                    skipInterstitialCount = config.SkipInterstitialStep;
                if (skipInterstitialCount == 0)
                {
                    //bắt đầu đếm lại
                    skipInterstitialCount = config.SkipInterstitialStep;
                    //CLog.Log(this, "Showing interstitial has been skipped for startup");
                    NotifyInterstitialEvent(callback, ShowAdResult.SkipByOther);
                    return false;
                }
            }

            var selectedProviderIds = GetProfileByType(AdType.Interstitial);
            for (int i = 0; i < selectedProviderIds.Count; i++)
            {
                ProviderID id = selectedProviderIds[i];
                var provider = FindActiveAdProvider(id);
                if (provider == null || !provider.IsSupport(AdType.Interstitial))
                {
                    //CLog.Warn(this, id.ToString() + " has not supported Interstitial!");
                    continue;
                }
                else if (provider.IsLoadedInterstitial(key))
                {
                    LastInterstitialTime = DateTime.Now;
                    skipInterstitialCount--;
                    // Cache callback to involke later
                    cachedInterstitialCallback = callback;
                    // Only sequence mode
                    if (config.interstitialProfileMode == ProfileMode.Sequence)
                    {
                        MoveItemToEndOfList(i, selectedProviderIds);
                    }
                    else if (config.interstitialProfileMode == ProfileMode.Random)
                    {
                        ShuffleProviders(selectedProviderIds);
                    }
                    if (AdsMaster.settings.ActiveZeroTimeScale) DisableTimeScale();
                    provider.ShowInterstitial();
                    return true;
                }
            }
            //has no ad
            NotifyInterstitialEvent(callback, ShowAdResult.NoAds);
            return false;
        }

        static void DisableTimeScale()
        {
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        static void RevertTimeScale()
        {
            if (savedTimeScale != null)
            {
                Time.timeScale = (float)savedTimeScale;
                savedTimeScale = null;
            }
        }

        /// <summary>
        /// Move provider id to end of list. Use for requence mode.
        /// </summary>
        static void MoveItemToEndOfList(int index, List<ProviderID> list)
        {
            var item = list[index];
            list.RemoveAt(index);
            list.Add(item);
        }

        /// <summary>
        /// Shuffle profiles. Use for random mode.
        /// </summary>
        static void ShuffleProviders(List<ProviderID> list)
        {
            var count = list.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = list[i];
                list[i] = list[r];
                list[r] = tmp;
            }
        }

        static void NotifyInterstitialEvent(InterstitialCallBack callback, ShowAdResult result)
        {
            if (callback != null) callback(result);
            if (allInterstitialsEvent != null) allInterstitialsEvent(result);
        }
        #endregion

        #region Banner
        // The banner unit is visible
        static BannerAdUnit activedBannerUnit = null;
        static Coroutine showBannerSoonTask = null;
        static WaitForSecondsRealtime waitOneSecondRealtime = null;

        /// <summary>
        /// Looking for banner by key and show it up. If key has been not set, the first banner will show up.
        /// Other banners will be hidden if they are visible. It will do nothing if the matched banner is shown ready.
        /// Note: Recommend only use key NULL if you have single banner unit
        /// </summary>
        /// <param name="key">key of banner unit</param>
        /// <returns>Return true if any banner has shown up.</returns>
        public static bool ShowBannerIfLoaded(string key = null)
        {
            // Not support platform
            if (instance == null) return false;
            // Already to show up
            if (IsActivedBannerUnit(key))
            {
                //CLog.Log("ShowBanner", "Banner is ready for key " + key);
                return false;
            }
            // Hide the actived banner
            if (activedBannerUnit != null) HideBanner();
            // Looking for matched banner and show it up
            var providerIds = GetProfileByType(AdType.Banner);
            for (int i = 0; i < providerIds.Count; i++)
            {
                ProviderID id = providerIds[i];
                var provider = FindActiveAdProvider(id);
                if (provider == null || !provider.IsSupport(AdType.Banner)) continue;
                activedBannerUnit = provider.ShowBanner(key);
                // Did showing banner up
                if (activedBannerUnit != null)
                {
                    if (config.bannerProfileMode == ProfileMode.Sequence)
                    {
                        MoveItemToEndOfList(i, providerIds);
                    }
                    else if (config.bannerProfileMode == ProfileMode.Random)
                    {
                        ShuffleProviders(providerIds);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Banner will be show up as soon as posible.
        /// </summary>
        /// <param name="key">key of banner unit</param>
        public static void ShowBanner(string key = null)
        {
            StopShowBannerTask();
            showBannerSoonTask = instance.StartCoroutine(ShowBannerSoonTask(key));
        }

        private static void StopShowBannerTask()
        {
            if (showBannerSoonTask != null)
            {
                instance.StopCoroutine(showBannerSoonTask);
                showBannerSoonTask = null;
            }
        }

        public static bool IsActivedBannerUnit(string key = null)
        {
            // đang hiển thị banner
            return
                // kiểm tra unit đầu tiên trong setting đã được hiển thị
                (key == null && activedBannerUnit != null) ||
                // kiểm tra chính xác banner có key dược hiển thị
                (key != null && activedBannerUnit != null && key.Equals(activedBannerUnit.key));
        }

        /// <summary>
        /// Hide the banner is visible. Do nothing if there are no actived banner
        /// </summary>
        public static void HideBanner()
        {
            if (instance == null) return;
            StopShowBannerTask();
            if (activedBannerUnit == null) return;
            // Looking for banner to hide
            var providerIds = GetProfileByType(AdType.Banner);
            foreach (ProviderID id in providerIds)
            {
                //CLog.Log("Checking provider " + id);
                var provider = FindActiveAdProvider(id);
                if (provider == null) continue;
                if (provider.HideBanner(activedBannerUnit))
                {
                    activedBannerUnit = null;
                    //CLog.Log("Hided banner " + id.ToString());
                    break;
                }
            }
        }

        private static IEnumerator ShowBannerSoonTask(string key = null)
        {
            bool shownAdUp = false;
            do
            {
                shownAdUp = ShowBannerIfLoaded(key);
                if (!shownAdUp)
                {
                    if (waitOneSecondRealtime == null) waitOneSecondRealtime = new WaitForSecondsRealtime(1f);
                    yield return waitOneSecondRealtime;
                }
            } while (!shownAdUp);
            showBannerSoonTask = null;
        }
        #endregion

        #region Rewarded
        static Action<bool> cachedRewardedCallback;

        public static void ShowReward(Action<bool> callback)
        {
            if (instance == null) return;

            var selectedProviderIds = GetProfileByType(AdType.Rewarded);
            for (int i = 0; i < selectedProviderIds.Count; i++)
            {
                ProviderID id = selectedProviderIds[i];
                var provider = FindActiveAdProvider(id);
                if (provider == null || !provider.IsSupport(AdType.Rewarded)) continue;
                if (provider.IsRewardReady) // Match once provider
                {
                    if (AdsMaster.settings.ActiveZeroTimeScale) DisableTimeScale();
                    provider.ShowReward();
                    cachedRewardedCallback = callback;
                    if (config.rewardedProfileMode == ProfileMode.Sequence)
                    {
                        MoveItemToEndOfList(i, selectedProviderIds);
                    }
                    else if (config.rewardedProfileMode == ProfileMode.Random)
                    {
                        ShuffleProviders(selectedProviderIds);
                    }
                    return;
                }
            }
            // Could not found any provider to show rewarded
            callback(false);
        }

        public static bool HasReward()
        {
            if (instance == null) return false;
            var provider = FindReadyProviderForReward();
            return provider != null;
        }

        static AdProvider FindReadyProviderForReward()
        {
            var selectedProviderIds = GetProfileByType(AdType.Rewarded);
            foreach (ProviderID id in selectedProviderIds)
            {
                var provider = FindActiveAdProvider(id);
                if (provider == null || !provider.IsSupport(AdType.Rewarded))
                {
                    continue;
                }
                if (provider.IsRewardReady) //show success
                {
                    return provider;
                }
                else //check other providers
                {
                    continue;
                }
            }
            return null;
        }
        #endregion

        #region Handle events from providers
        //call whenever provider display ad
        public static void OnDisplayAd(AdProvider provider, AdType type)
        {
        }

        //call whenever provider dissmiss ad
        public static void OnDimissAd(AdType type)
        {
            if (type == AdType.Interstitial && cachedInterstitialCallback != null)
            {
                cachedInterstitialCallback(ShowAdResult.Shown);
                cachedInterstitialCallback = null;
            }
            RevertTimeScale();
        }

        public static void OnCompletedReward(ProviderID providerId, bool success)
        {
            if (cachedRewardedCallback != null)
            {
                cachedRewardedCallback(success);
                cachedRewardedCallback = null;
            }
        }
        #endregion
    }
}