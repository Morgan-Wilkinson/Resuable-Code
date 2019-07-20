using UnityEngine;

namespace cdi.ad
{
    [SerializeField]
    public enum ProviderID
    {
        // Note. Không đổi các giá trị được gán dưới đây. Có thể đổi thử tự
        Admob = 0,
        FbAd = 1,
        UnityAd = 2,
        Vungle = 3,
        ChartBoost = 4
    }
}