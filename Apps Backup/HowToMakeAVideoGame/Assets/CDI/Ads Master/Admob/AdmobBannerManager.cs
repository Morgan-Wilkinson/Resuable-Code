#if ADMOB
using System.Collections.Generic;

namespace cdi.ad
{
    public class AdmobBannerManager : BannerManager<AdmobBannerUnit, AdmobBannerLoader>
    {
        protected override ProviderID providerId
        {
            get
            {
                return ProviderID.Admob;
            }
        }

        protected override List<AdmobBannerUnit> GetAdUnitsFromConfigs()
        {
            return AdsMaster.config.AdmobBannerUnits;
        }
    }
}
#endif