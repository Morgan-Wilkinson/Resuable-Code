using System;

namespace cdi.ad
{
    [Serializable]
    public class AdmobInterstitialAdUnit : InterstitialAdUnit
    {
        public AdmobInterstitialAdUnit(string key) : base(key)
        {
        }
    }
}