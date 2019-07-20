using UnityEngine;
using UnityEngine.Serialization;

namespace cdi.ad
{
    [CreateAssetMenu(fileName = "AdsMaster Settings", menuName = "CDI/AdsMaster/Settings", order = 1)]
    public class AdsMasterSetting : ScriptableObject
    {
        public AdConfig localConfig = new AdConfig();

        // ADMOB
        [FormerlySerializedAs("IsAdmobActived")]
        [SerializeField]
        bool _isAdmobActived;

        public bool IsAdmobActived
        {
            get
            {
                return _isAdmobActived;
            }
            set
            {
                if (value == _isAdmobActived) return;
                _isAdmobActived = value;
            }
        }

        // Chartboost
        [FormerlySerializedAs("IsChartBoostActived")]
        [SerializeField]
        bool _isChartBoostActived;
        public bool IsChartBoostActived
        {
            get
            {
                return _isChartBoostActived;
            }
            set
            {
                if (value == _isChartBoostActived) return;
                _isChartBoostActived = value;
            }
        }

        // UNITY ADS
        [FormerlySerializedAs("IsUnityAdActived")]
        [SerializeField]
        bool _isUnityAdActived;
        public bool IsUnityAdActived
        {
            get
            {
                return _isUnityAdActived;
            }
            set
            {
                if (value == _isUnityAdActived) return;
                _isUnityAdActived = value;
            }
        }

        // Vungle
        [FormerlySerializedAs("IsVungleActived")]
        [SerializeField]
        bool _isVungleActived;
        public bool IsVungleActived
        {
            get
            {
                return _isVungleActived;
            }
            set
            {
                if (value == _isVungleActived) return;
                _isVungleActived = value;
            }
        }

        // Facebook Data
        [FormerlySerializedAs("IsFbAdActived")]
        [SerializeField]
        bool _isFbAdActived;
        public bool IsFbAdActived
        {
            get
            {
                return _isFbAdActived;
            }
            set
            {
                if (value == _isFbAdActived) return;
                _isFbAdActived = value;
            }
        }

        // Debug flags
        public bool AdMobDebug = false;
        public bool UnityAdDebug = false;

        public bool TestMode { get { return AdMobDebug || UnityAdDebug; } }

        // Remote
        public bool ActiveRemoteConfig = false;
        public string RemoteConfigUrl = "";

        // Advanced Settings
        public bool ActiveZeroTimeScale;
    }
}