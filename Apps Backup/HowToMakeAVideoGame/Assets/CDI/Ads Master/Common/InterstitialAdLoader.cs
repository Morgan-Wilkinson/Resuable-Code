using System.Collections;
using UnityEngine;

namespace cdi.ad
{
    public abstract class InterstitialAdLoader<T> : MonoBehaviour where T : InterstitialAdUnit
    {
        public T AdUnit { get; private set; }
        public bool IsLoaded { get; private set; }

        WaitForSeconds waitForResend;
        WaitForSeconds waitForTimeout;
        WaitForSeconds waitForExpiredAdTime;

        Coroutine waitForExpiredAdTimeTask;
        Coroutine waitForTimeoutTask;

        void Awake()
        {
            waitForResend = new WaitForSeconds(AdsMaster.RESEND_FAILED_REQUEST_DELAY);
            waitForTimeout = new WaitForSeconds(AdsMaster.config.RequestTimeOut);
            waitForExpiredAdTime = new WaitForSeconds(AdsMaster.config.AdExpiration);
        }

        public virtual void Init(T adUnit)
        {
            this.AdUnit = adUnit;
            //CLog.Log(this, "Init id " + AdUnit.key);
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
            if (IsLoaded)
            {
                IsLoaded = false;
                StopWaitForExpiredAdTask();
                OnShow();
            }
        }

        void SendRequest()
        {
            //CLog.Log(this, "Send request id " + AdUnit.key);
            if (!AdsMaster.SupportedAdPlatform) return;
            DisposeAd();
            OnSendRequest();

            // start timeout task
            StartWaitTimeoutTask();
        }

        void DisposeAd()
        {
            IsLoaded = false;
            OnDisposeAd();
        }

        protected void DidLoadedAd()
        {
            //CLog.Log(this, "Loaded id " + AdUnit.key);
            IsLoaded = true;
            StopWaitTimeoutTask();
            StartWaitForExpiredAdTask();
        }

        protected void DidFailedAd()
        {
            //CLog.Log(this, "Failed to load id " + AdUnit.key);
            StopWaitTimeoutTask();
            // Try to reload
            StartCoroutine(TryToResendRequest());
        }

        protected void DidCloseAd()
        {
            AdsMaster.OnDimissAd(AdType.Interstitial);
            SendRequest();
        }

        IEnumerator TryToResendRequest()
        {
            DisposeAd();
            yield return waitForResend;
            SendRequest();
        }

        protected abstract void OnDisposeAd();

        protected abstract void OnSendRequest();

        protected abstract void OnShow();

        void StartWaitTimeoutTask()
        {
            //CLog.Log(this, "Start waiting for timeout");
            StopWaitTimeoutTask();
            waitForTimeoutTask = StartCoroutine(DoTimeoutTask());
        }

        void StopWaitTimeoutTask()
        {
            if (waitForTimeoutTask != null)
            {
                //CLog.Log(this, "Stop waiting for timeout");
                StopCoroutine(waitForTimeoutTask);
                waitForTimeoutTask = null;
            }
        }

        IEnumerator DoTimeoutTask()
        {
            yield return waitForTimeout;
            // Nếu load không thành công. Sẽ đến block code này
            // Resend later
            SendRequest();
        }

        void StartWaitForExpiredAdTask()
        {
            //CLog.Log(this, "Start waiting for Expired");
            StopWaitForExpiredAdTask();
            waitForExpiredAdTimeTask = StartCoroutine(DoExpiredAdTask());
        }

        void StopWaitForExpiredAdTask()
        {
            if (waitForExpiredAdTimeTask != null)
            {
                //CLog.Log(this, "Stop waiting for Expired");
                StopCoroutine(waitForExpiredAdTimeTask);
                waitForExpiredAdTimeTask = null;
            }
        }

        IEnumerator DoExpiredAdTask()
        {
            yield return waitForExpiredAdTime;
            // Nếu load không thành công. Sẽ đến block code này
            // Resend later
            SendRequest();
        }
    }
}