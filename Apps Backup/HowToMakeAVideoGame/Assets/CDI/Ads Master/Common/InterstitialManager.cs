using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace cdi.ad
{
    public abstract class InterstitialManager<AdUnit, Loader> : MonoBehaviour where AdUnit : InterstitialAdUnit where Loader : InterstitialAdLoader<AdUnit>
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
            //CLog.Log(this, "Initializing...");
            if (!AdsMaster.IsAllowedInterstitial(this.providerId)) return;
            adUnits = GetAdUnitsFromConfigs();
            loaderDict = new Dictionary<string, Loader>();
            for (int i = 0; i < adUnits.Count; i++)
            {
                var adUnit = adUnits[i];
                InstantiateLoaders(adUnit);
            }
        }

        void InstantiateLoaders(AdUnit adUnit)
        {
            var go = new GameObject(adUnit.key);
            go.transform.parent = this.transform;
            Loader loader = go.AddComponent<Loader>();
            loader.Init(adUnit);
            // add to dict
            loaderDict.Add(adUnit.key, loader);
        }

        public bool IsLoaded(string key = null)
        {
            Loader loader = FindLoaderByKey(key);
            return loader != null && loader.IsLoaded;
        }

        public void Show(string key = null)
        {
            Loader loader = FindLoaderByKey(key);

            if (loader != null && loader.IsLoaded)
            {
                loader.Show();
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
    }
}