using System;

namespace cdi.ad
{
    [Serializable]
    public class AdmobBannerUnit : BannerAdUnit
    {
        public BannerSize size = BannerSize.Smart;

        public AdmobBannerUnit(string key) : base(key)
        {
        }
    }
}