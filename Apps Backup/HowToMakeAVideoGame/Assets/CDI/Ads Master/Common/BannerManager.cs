using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace cdi.ad
{
    public abstract class BannerManager<AdUnit, Loader> : MonoBehaviour where AdUnit : BannerAdUnit where Loader : BannerLoader<AdUnit>
    {
        protected abstract ProviderID providerId { get; }

        /// <summary>
        /// Chỉ gọi một lần duy nhất khi khởi tạo Manager
        /// </summary>
        /// <returns></returns>
        protected abstract List<AdUnit> GetAdUnitsFromConfigs();

        List<AdUnit> adUnits;

        string DefaultKey
        {
            get
            {
                if (adUnits.Count > 0) return adUnits[0].key;
                else return null;
            }
        }

        /// <summary>
        /// Chứa các Banner View đã được tạo
        /// </summary>
        Dictionary<string, Loader> loaderDict;

        void Awake()
        {
            if (!AdsMaster.IsAllowedBanner(this.providerId)) return;
            //CLog.Log(this, "Initializing banners...");
            adUnits = GetAdUnitsFromConfigs();
            loaderDict = new Dictionary<string, Loader>();
            for (int i = 0; i < adUnits.Count; i++)
            {
                var adUnit = adUnits[i];
                InstantiateLoader(adUnit);
            }
        }

        void InstantiateLoader(AdUnit adUnit)
        {
            var go = new GameObject(adUnit.key);
            go.transform.parent = this.transform;
            Loader loader = go.AddComponent<Loader>();
            loader.Init(adUnit);
            // add to dict
            loaderDict.Add(adUnit.key, loader);
        }

        public AdUnit ShowBanner(string key = null)
        {
            if (!AdsMaster.IsAllowedBanner(this.providerId)) return null;
            Loader loader = FindLoaderByKey(key);

            if (loader != null && loader.IsLoaded)
            {
                // Hiển thị banner mới
                loader.Show();
                return loader.AdUnit;
            }
            else
            {
                return null;
            }
        }

        Loader FindLoaderByKey(string key = null)
        {
            if (key == null)
            {
                key = DefaultKey;
            }
            Loader loader = null;
            if (key != null)
            {
                loaderDict.TryGetValue(key, out loader);
            }
            else
            {
                loader = loaderDict.First().Value;
            }
            return loader;
        }

        public bool HideBanner(BannerAdUnit adUnit)
        {
            //CLog.Log(this, "try to hide banner " + adUnit.key);
            Loader loader = FindLoaderByKey(adUnit.key);
            if (loader != null && loader.IsVisible)
            {
                //CLog.Log(this, "Will hide banner " + adUnit.key);
                if (loader.Hide())
                {
                    // banner has been destroyed
                    loaderDict.Remove(loader.AdUnit.key);
                    // Create new banner for ad Unit
                    InstantiateLoader(loader.AdUnit);
                    //CLog.Log(this, "Hidden " + adUnit.key);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}