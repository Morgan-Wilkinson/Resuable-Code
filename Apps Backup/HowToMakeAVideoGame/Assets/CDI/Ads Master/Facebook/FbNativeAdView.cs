#if FBAD
using AudienceNetwork;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace cdi.ad
{
    /// <summary>
    /// Có chức năng hiển thị NativeAd
    /// </summary>
    public class FbNativeAdView : MonoBehaviour
    {
        public string adName;
        public bool requestAfterDisable = true;

        // UI elements in scene
        [Header("Text:")]
        public Text title;
        public Text socialContext;
        [Header("Images:")]
        public Image coverImage;
        public Image iconImage;
        [Header("Buttons:")]
        public Text callToAction;
        public Button[] callToActionButtons;

#if FBAD
        private void Awake()
        {
            //CLog.Log("[FBAD]Awake view " + adName);
        }

        void OnEnable()
        {
            //CLog.Log("[FBAD]Enable view " + adName);
            FbNativeAdManager.Register(this);
        }

        void OnDisable()
        {
            //CLog.Log("[FBAD]Disable view " + adName);
            FbNativeAdManager.Unregister(this);
            if (requestAfterDisable) LoadAd();
        }

        public void LoadAd()
        {
            FbNativeAdManager.LoadAd(adName);
        }

        public void OnUpdateAd(NativeAd nativeAd)
        {
            //CLog.Log("[FBAD]Update view for name " + adName);
            bool activeAd = nativeAd != null;

            if (coverImage) coverImage.enabled = activeAd;
            if (iconImage) iconImage.enabled = activeAd;
            if (title) title.enabled = activeAd;
            if (socialContext) socialContext.enabled = activeAd;
            if (callToAction) callToAction.enabled = activeAd;
            if (callToActionButtons != null)
            {
                for (int i = 0; i < callToActionButtons.Length; i++)
                {
                    var button = callToActionButtons[i];
                    button.gameObject.SetActive(activeAd);
                }
            }

            if (nativeAd)
            {
                if (coverImage) coverImage.sprite = nativeAd.CoverImage;
                if (iconImage) iconImage.sprite = nativeAd.IconImage;
                if (title) title.text = nativeAd.Title;
                if (socialContext) socialContext.text = nativeAd.SocialContext;
                if (callToAction) callToAction.text = nativeAd.CallToAction;

                if (callToActionButtons != null)
                {
                    for (int i = 0; i < callToActionButtons.Length; i++)
                    {
                        var button = callToActionButtons[i];
                        button.onClick.RemoveAllListeners();
                        button.onClick.AddListener(() =>
                        {
                            nativeAd.ExternalClick();
                        });
                    }
                }
            }
        }
#endif
    }
}
