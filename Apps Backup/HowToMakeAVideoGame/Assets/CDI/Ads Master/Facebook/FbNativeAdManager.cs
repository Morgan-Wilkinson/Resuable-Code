#if FBAD
using AudienceNetwork;
using System.Collections.Generic;
using UnityEngine;

namespace cdi.ad
{
    public class FbNativeAdManager : MonoBehaviour
    {
        static FbNativeAdManager instance;

        /// <summary>
        /// Quản lý việc request ad của các ad unit theo thứ tự
        /// </summary>
        static SortedDictionary<string, FbNativeAdLoader> adLoaders;

        /// <summary>
        /// Báo cho các views khi NativeAd được update
        /// </summary>
        static SortedDictionary<string, FbNativeAdView> adViews;

        public static void Initialize()
        {
            if (instance)
            {
                CLog.Warn("[FBAD]There are multiple instance in run time");
                return;
            }
            // Create GameObject
            var go = new GameObject("FacebookNativeAdManager");
            instance = go.AddComponent<FbNativeAdManager>();
            DontDestroyOnLoad(go);
            // Init data
            InitAds();
        }

        /// <summary>
        /// Khởi tạo dữ liệu cho các unit. Chỉ gọi 1 lần.
        /// </summary>
        static void InitAds()
        {
            adViews = new SortedDictionary<string, FbNativeAdView>();

            adLoaders = new SortedDictionary<string, FbNativeAdLoader>();
            var adUnits = AdsMaster.config.fbNativeAds;
            for (int i = 0; i < adUnits.Count; i++)
            {
                var adUnit = adUnits[i];
                var go = new GameObject(adUnit.key);
                go.transform.SetParent(instance.transform);

                var loader = go.AddComponent<FbNativeAdLoader>();
                loader.Init(adUnit);
                adLoaders.Add(adUnit.key, loader);
            }
            //CLog.Log("[FBAD]Init all fb native ads " + adUnits.Count);
        }

        public static void Register(FbNativeAdView adView)
        {
            //CLog.Log("[FBAD]Native ad " + adView.adName + " has been registered");
            if (!instance) return;
            var loader = FindLoaderByAdName(adView.adName);
            if (loader != null && loader.IsLoaded) adView.OnUpdateAd(loader.NativeAd);
            else adView.OnUpdateAd(null);
            adViews.Add(adView.adName, adView);
        }

        public static void Unregister(FbNativeAdView adView)
        {
            //CLog.Log("[FBAD]Native ad " + adView.adName + " has been unregistered");
            if (!instance) return;
            adViews.Remove(adView.adName);
        }

        public static void LoadAd(string adName)
        {
            if (!instance) return;
            FbNativeAdLoader loader = FindLoaderByAdName(adName);
            if (loader)
            {
                loader.LoadAd();
            }
        }

        static FbNativeAdLoader FindLoaderByAdName(string adName)
        {
            FbNativeAdLoader loader = null;
            if (!adLoaders.TryGetValue(adName, out loader))
            {
                // CLog.Warn("[FBAD]There are no fb native ad name as " + adName);
                return null;
            }
            return loader;
        }

        /// <summary>
        /// Báo cho view là ad đã được cập nhật
        /// </summary>
        public static void OnLoadedAd(string adName, NativeAd ad)
        {
            FbNativeAdView view = null;
            if (!adViews.TryGetValue(adName, out view))
            {
                return;
            }
            view.OnUpdateAd(ad);
        }
    }
}
#endif