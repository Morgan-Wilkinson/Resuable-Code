#if FBAD
using System.Collections.Generic;

namespace cdi.ad
{
    public class FbInterstitialManager : InterstitialManager<FbInterstitialUnit, FbInterstitialLoader>
    {
        protected override ProviderID providerId
        {
            get
            {
                return ProviderID.FbAd;
            }
        }

        protected override List<FbInterstitialUnit> GetAdUnitsFromConfigs()
        {
            return AdsMaster.config.fbInterstitials;
        }

    }
}
#endif