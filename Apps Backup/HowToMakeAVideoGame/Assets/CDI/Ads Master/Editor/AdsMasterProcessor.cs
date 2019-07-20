using UnityEditor;

namespace cdi.ad
{
    public class AdsMasterProcessor : AssetPostprocessor
    {
        public static AssetDetector admobDetector = new AssetDetector(
            "Assets/GoogleMobileAds");

        public static AssetDetector vungleDetector = new AssetDetector(
            "Assets/Plugins/Vungle");

        public static AssetDetector fbadsDetector = new AssetDetector(
            "Assets/AudienceNetwork");

        public static AssetDetector chartboostDetector = new AssetDetector(
            "Assets/Chartboost");

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool isChanged = false;
            if (!admobDetector.IsValid && AdsMasterEditor.Settings.IsAdmobActived)
            {
                AdsMasterEditor.Settings.IsAdmobActived = false;
                isChanged = true;
            }
            if (!vungleDetector.IsValid && AdsMasterEditor.Settings.IsVungleActived)
            {
                AdsMasterEditor.Settings.IsVungleActived = false;
                isChanged = true;
            }
            if (!fbadsDetector.IsValid && AdsMasterEditor.Settings.IsFbAdActived)
            {
                AdsMasterEditor.Settings.IsFbAdActived = false;
                isChanged = true;
            }
            if (!chartboostDetector.IsValid && AdsMasterEditor.Settings.IsChartBoostActived)
            {
                AdsMasterEditor.Settings.IsChartBoostActived = false;
                isChanged = true;
            }
            if (isChanged)
            {
                AdsMasterEditor.UpdateDefineSympols();
            }
        }
    }

    /// <summary>
    /// Check asset is ready imported
    /// </summary>
    public class AssetDetector
    {
        string[] requiredPaths;

        /// <summary>
        /// All required paths are valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (var path in requiredPaths)
                {
                    if (!AssetDatabase.IsValidFolder(path)) return false;
                }
                return true;
            }
        }

        public AssetDetector(params string[] requiredPaths)
        {
            this.requiredPaths = requiredPaths;
        }
    }
}