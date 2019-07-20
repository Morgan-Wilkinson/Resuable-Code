#if FBAD
using AudienceNetwork;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace cdi.ad
{
    public class FbNativeAdLoader : MonoBehaviour
    {
        public bool IsLoaded { get; private set; }

        public FBNativeAdUnit AdUnit { get; private set; }
        public NativeAd NativeAd { get; private set; }

        bool didLoadedData;

        DateTime lastTimeSentRequest = DateTime.MinValue;

        public void Init(FBNativeAdUnit adUnit)
        {
            if (this.AdUnit != null) return;
            this.AdUnit = adUnit;
            if (adUnit.preload) LoadAd();
        }

        public void LoadAd()
        {
            // Giới hạn thời gian giữa 2 lần gửi request
            var now = DateTime.Now;
            if (now.Subtract(lastTimeSentRequest).TotalSeconds < AdUnit.minSecondsToReload) return;
            lastTimeSentRequest = now;
            //CLog.Log("[FBAD]Loading ads " + AdUnit.name);
            IsLoaded = false;
            didLoadedData = false;

            if (!AdsMaster.SupportedAdPlatform) return;
            // Create a native ad request with a unique placement ID (generate your own on the Facebook app settings).
            // Use different ID for each ad placement in your app.
            NativeAd = new AudienceNetwork.NativeAd(AdUnit.PlacementId);

            // Wire up GameObject with the native ad; the specified buttons will be clickable.
            NativeAd.RegisterGameObjectForImpression(gameObject, new Button[] { });

            // Set delegates to get notified on changes or when the user interacts with the ad.
            NativeAd.NativeAdDidLoad = HandleAdDidLoad;
            NativeAd.NativeAdDidFailWithError = HandleAdDidFailWithError;
            NativeAd.NativeAdWillLogImpression = HandleAdWillLogImpression;
            NativeAd.NativeAdDidClick = HandleAdDidClick;

            // Initiate a request to load an ad.
            NativeAd.LoadAd();
        }

        void HandleAdDidClick()
        {
            //CLog.Log("[FBAD]Clicked ads " + AdUnit.name);
        }

        void HandleAdWillLogImpression()
        {
            //CLog.Log("[FBAD]Log impression " + AdUnit.name);
        }

        void HandleAdDidFailWithError(string error)
        {
            //CLog.Log("[FBAD]Load Failed " + AdUnit.name);
        }

        void HandleAdDidLoad()
        {
            if (didLoadedData) return;

            didLoadedData = true;
            //CLog.Log("[FBAD]Ad data loaded " + AdUnit.name);
            // Use helper methods to load images from native ad URLs
            StartCoroutine(LoadIconImageTask());
            StartCoroutine(LoadCoverImageTask());
        }

        IEnumerator LoadIconImageTask()
        {
            yield return NativeAd.LoadIconImage(NativeAd.IconImageURL);
            //CLog.Log("[FBAD]Loaded icon image for " + AdUnit.name);
            if (this.NativeAd.CoverImage && this.NativeAd.IconImage)
            {
                OnLoadedImages();
            }
        }

        IEnumerator LoadCoverImageTask()
        {
            yield return NativeAd.LoadCoverImage(NativeAd.CoverImageURL);
            //CLog.Log("[FBAD]Loaded cover image for " + AdUnit.name);
            if (this.NativeAd.CoverImage && this.NativeAd.IconImage)
            {
                OnLoadedImages();
            }
        }

        void OnLoadedImages()
        {
            //CLog.Log("[FBAD]Loaded ad for name " + AdUnit.name);
            IsLoaded = true;
            FbNativeAdManager.OnLoadedAd(AdUnit.key, NativeAd);
        }
    }
}
#endif