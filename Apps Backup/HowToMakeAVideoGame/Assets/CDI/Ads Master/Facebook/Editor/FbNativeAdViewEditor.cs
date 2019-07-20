using UnityEditor;

namespace cdi.ad
{

    [CustomEditor(typeof(FbNativeAdView))]
    public class FbNativeAdViewEditor : Editor
    {
        FbNativeAdView view
        {
            get { return (FbNativeAdView)target; }
        }
    }
}
