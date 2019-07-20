using UnityEngine;
using UnityEngine.UI;

namespace cdi.ad
{
    enum Action
    {
        ShowInterstitial,
        ShowBanner,
        HideBanner
    }

    [AddComponentMenu("CDI/Ads Master/Action When Click (UGUI)")]
    [RequireComponent(typeof(Button))]
    public class ActionWhenClick : MonoBehaviour
    {
        [SerializeField]
        Action action;

        [SerializeField]
        string key = null;

        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(HandleClicked);
        }

        void HandleClicked()
        {
            if (action == Action.ShowInterstitial)
            {
                AdsMaster.ShowInterstitial(null, key);
            }
            else if (action == Action.ShowBanner)
            {
                AdsMaster.ShowBannerIfLoaded(key);
            }
            else if (action == Action.HideBanner)
            {
                AdsMaster.HideBanner();
            }
        }
    }
}