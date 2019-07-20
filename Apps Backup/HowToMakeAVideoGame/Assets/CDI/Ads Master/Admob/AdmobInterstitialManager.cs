#if ADMOB
using System.Collections.Generic;

namespace cdi.ad
{
    public class AdmobInterstitialManager : InterstitialManager<AdmobInterstitialAdUnit, AdmobInterstitialLoader>
    {
        protected override ProviderID providerId
        {
            get
            {
                return ProviderID.Admob;
            }
        }

        protected override List<AdmobInterstitialAdUnit> GetAdUnitsFromConfigs()
        {
            return AdsMaster.config.admobInterstitials;
        }
    }
}
#endif