namespace cdi.ad
{
    public class FbRewardedManager : BaseRewardedManager
    {
        protected override ProviderID providerId
        {
            get
            {
                return ProviderID.FbAd;
            }
        }

        protected override void OnInitialize()
        {
            
        }

        protected override void OnSentRequest()
        {
        }

        protected override void OnShow()
        {
        }
    }
}