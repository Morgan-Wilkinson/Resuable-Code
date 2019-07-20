using System.Collections;
using UnityEngine;

namespace cdi.ad
{
    public abstract class BaseRewardedManager : MonoBehaviour
    {
        protected abstract ProviderID providerId { get; }

        WaitForSeconds resendTime;
        bool isLoaded;

        void Awake()
        {
            Init();
        }

        void Init()
        {
            if (!AdsMaster.SupportedAdPlatform) return;
            if (!AdsMaster.IsAllowedReward(providerId) || !IsValidConfiguration()) return;
            resendTime = new WaitForSeconds(AdsMaster.RESEND_FAILED_REQUEST_DELAY);
            OnInitialize();
            SendRequest();
        }

        protected virtual bool IsValidConfiguration()
        {
            return true;
        }

        void SendRequest()
        {
            OnSentRequest();
        }

        IEnumerator SendRequestDelay()
        {
            yield return resendTime;
            SendRequest();
        }

        public void Show()
        {
            if (isLoaded)
            {
                isLoaded = false;
                OnShow();
            }
        }

        /// <summary>
        /// Thông báo kết quả của quá trình loading => success
        /// </summary>
        protected void DidLoadedVideo()
        {
            isLoaded = true;
        }

        /// <summary>
        /// THông báo kết quả của quá trình loading => đã failed
        /// </summary>
        protected void DidFailedToLoadVideo()
        {
            StartCoroutine(SendRequestDelay());
        }

        /// <summary>
        /// Thông báo video đã được đóng
        /// </summary>
        protected void DidClosedVideo()
        {
            AdsMaster.OnDimissAd(AdType.Rewarded);
            SendRequest();
        }

        /// <summary>
        /// Thông báo kết của của hành động xem video
        /// </summary>
        /// <param name="success"></param>
        protected void DidCompletedVideo(bool success)
        {
            AdsMaster.OnCompletedReward(providerId, success);
        }

        public virtual bool IsLoaded() { return isLoaded; }

        protected abstract void OnInitialize();

        protected abstract void OnSentRequest();

        protected abstract void OnShow();
    }
}