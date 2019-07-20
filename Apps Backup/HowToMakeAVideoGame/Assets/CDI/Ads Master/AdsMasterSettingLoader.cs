using UnityEngine;

namespace cdi.ad
{
    public class AdsMasterSettingLoader : MonoBehaviour
    {
        public AdsMasterSetting settings;

        void Awake()
        {
            AdsMaster.Initialize(settings);
        }
    }
}