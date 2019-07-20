using System.Collections;
using UnityEngine;

namespace cdi.ad
{
    public abstract class BannerLoader<T> : MonoBehaviour where T : BannerAdUnit
    {
        public bool IsLoaded { get; private set; }
        public bool IsVisible { get; private set; }

        public T AdUnit { get; private set; }

        WaitForSeconds waitForResend;

        void Awake()
        {
            waitForResend = new WaitForSeconds(AdsMaster.RESEND_FAILED_REQUEST_DELAY);
        }

        public virtual void Init(T adUnit)
        {
            this.AdUnit = adUnit;
            // send first request
            StartCoroutine(SendFirstRequest());
        }

        /// <summary>
        /// Đối với FB Ads phải send request sau khi Update frame đầu. Chưa rõ nguyên nhân
        /// </summary>
        IEnumerator SendFirstRequest()
        {
            yield return null;
            SendRequest();
        }

        public void Show()
        {
            IsVisible = true;
            if (IsLoaded) OnShow();
        }

        /// <summary>
        /// Return true if destroy gameobject to hide
        /// </summary>
        public bool Hide()
        {
            return OnHide();
        }

        void SendRequest()
        {
            //CLog.Log(this, "Sending request for key " + AdUnit.key);
            // Bỏ qua request nếu id trống
            if (string.IsNullOrEmpty(AdUnit.AdId.Trim())) return;

            IsLoaded = false;
            DisposeAd();
            if (AdsMaster.SupportedAdPlatform) OnSendRequest();
        }

        protected void DidLoadedAdView()
        {
            //CLog.Log(this, "Loaded key " + AdUnit.key);
            IsLoaded = true;
        }

        protected void DidFailedAdview()
        {
            //CLog.Log(this, "Failed to load key " + AdUnit.key);
            // Try to reload
            StartCoroutine(TryToResendRequest());
        }

        IEnumerator TryToResendRequest()
        {
            DisposeAd();
            yield return waitForResend;
            SendRequest();
        }

        protected virtual void OnShow()
        {
            IsVisible = true;
        }

        protected abstract bool OnHide();

        protected abstract void OnSendRequest();

        void DisposeAd()
        {
            OnDisposeAd();
        }

        protected abstract void OnDisposeAd();

        void OnDestroy()
        {
            DisposeAd();
        }
    }
}