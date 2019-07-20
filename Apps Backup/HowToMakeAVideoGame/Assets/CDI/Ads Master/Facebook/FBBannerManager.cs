#if FBAD

using System.Collections.Generic;

namespace cdi.ad
{
    public class FBBannerManager : BannerManager<FBBannerAdUnit, FbBannerLoader>
    {
        protected override ProviderID providerId
        {
            get
            {
                return ProviderID.FbAd;
            }
        }

        protected override List<FBBannerAdUnit> GetAdUnitsFromConfigs()
        {
            return AdsMaster.config.fbBanners;
        }
    }
}
#endif