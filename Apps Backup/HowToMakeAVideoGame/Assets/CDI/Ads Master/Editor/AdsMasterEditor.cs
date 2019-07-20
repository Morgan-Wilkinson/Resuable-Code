using Rotorz.ReorderableList;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace cdi.ad
{
    class AdIssue
    {
        public string name;
        public string answer;
        public string url;

        public AdIssue(string name, string answer, string url)
        {
            this.name = name;
            this.answer = answer;
            this.url = url;
        }
    }

    public class AdsMasterEditor : EditorWindow
    {
        #region constants
        //flag for configure setting
        const string FLAG_ADMOB = "ADMOB";
        const string FLAG_UNITYAD = "UNITYAD";
        const string FLAG_CHARTBOOST = "CHARTBOOST";
        const string FLAG_VUNGLE = "VUNGLE";
        const string FLAG_FBAD = "FBAD";

        GUILayoutOption[] rootMenuToolbarOptions = new GUILayoutOption[] { GUILayout.MinWidth(0), GUILayout.Height(35) };
        GUILayoutOption[] networkToolBarOptions = new GUILayoutOption[] { GUILayout.MinWidth(0), GUILayout.Height(40) };
        GUILayoutOption[] longButtonOptions = new GUILayoutOption[] { GUILayout.Width(120f) };
        #endregion

        #region GUI
        static AdsMasterSetting _settings;

        public static AdsMasterSetting Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = LibResourceUtil.LoadAndCreateSetting<AdsMasterSetting>();
                }
                return _settings;
            }
        }

        [MenuItem("CDI/Ads Master/Settings")]
        public static void OpenSettings()
        {
            var window = EditorWindow.GetWindow(typeof(AdsMasterEditor)) as AdsMasterEditor;
            window.titleContent = new GUIContent("Ads Master");
        }

        int ToolbarSelectedIndex;
        Vector2 contentScrollPos;

        void OnGUI()
        {
            if (Settings == null)
            {
                return;
            }
            GUI.changed = false;
            OnGUIRootMenu();
            if (GUI.changed)
            {
                UpdateDefineSympols();
            }
        }

        public static void UpdateDefineSympols()
        {
            EditorUtility.SetDirty(Settings);
            PlayerSettingsUtil.UpdateDefineSymbol(FLAG_ADMOB, Settings.IsAdmobActived);
            PlayerSettingsUtil.UpdateDefineSymbol(FLAG_CHARTBOOST, Settings.IsChartBoostActived);
            PlayerSettingsUtil.UpdateDefineSymbol(FLAG_UNITYAD, Settings.IsUnityAdActived);
            PlayerSettingsUtil.UpdateDefineSymbol(FLAG_VUNGLE, Settings.IsVungleActived);
            PlayerSettingsUtil.UpdateDefineSymbol(FLAG_FBAD, Settings.IsFbAdActived);
        }

        void OnGUIRootMenu()
        {
            ToolbarSelectedIndex = GUILayout.Toolbar(ToolbarSelectedIndex,
                new GUIContent[] { new GUIContent("Profile", ProfileIcon, ""), new GUIContent("Providers", NetworkIcon, ""),
                     new GUIContent("Setting", SettingIcon, ""), new GUIContent("About", InforIcon, "")}, rootMenuToolbarOptions);

            contentScrollPos = EditorGUILayout.BeginScrollView(contentScrollPos);

            EditorGUI.BeginChangeCheck();
            if (ToolbarSelectedIndex == 0)
            {
                ProfileSetting();
            }
            else if (ToolbarSelectedIndex == 1)
            {
                AdNetworkSetting();
            }
            else if (ToolbarSelectedIndex == 2)
            {
                OnGUISettings();
            }
            else if (ToolbarSelectedIndex == 3)
            {
                OnGUIAbout();
            }
            GUILayout.Space(20f);
            EditorGUI.EndChangeCheck();
            EditorGUILayout.EndScrollView();
        }

        #endregion

        #region Textures
        private Texture2D _addIcon;
        private Texture2D _deleteIcon;
        private Texture2D _downloadIcon;
        private Texture2D _dashboardIcon;
        private Texture2D _admobIcon;
        private Texture2D _fbadIcon;
        private Texture2D _unityadIcon;
        private Texture2D _vungleIcon;
        private Texture2D _chartboostIcon;
        private Texture2D _profileIcon;
        private Texture2D _settingIcon;
        private Texture2D _networkIcon;
        private Texture2D _inforIcon;
        private Texture2D _toggleOnIcon;
        private Texture2D _toggleOffIcon;

        private Texture2D _availableIcon;
        private Texture2D _notAvailableIcon;

        private string iconsPath;
        private string IconsPath
        {
            get
            {
                if (string.IsNullOrEmpty(iconsPath))
                {
                    //Assets/UltimatePlayerPrefsEditor/Editor/PlayerPrefsEditor.cs
                    string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));

                    //Strip PlayerPrefsEditor.cs
                    path = path.Substring(0, path.LastIndexOf('/'));

                    //Strip Editor/
                    path = path.Substring(0, path.LastIndexOf('/') + 1);

                    iconsPath = path + "Icons/";
                }

                return iconsPath;
            }
        }

        public Texture2D AddIcon
        {
            get
            {
                if ((UnityEngine.Object)_addIcon == (UnityEngine.Object)null)
                    _addIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "add.png", typeof(Texture2D)) as Texture2D;
                return _addIcon;
            }
        }

        public Texture2D DeleteIcon
        {
            get
            {
                if ((UnityEngine.Object)_deleteIcon == (UnityEngine.Object)null)
                    _deleteIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "delete.png", typeof(Texture2D)) as Texture2D;
                return _deleteIcon;
            }
        }

        public Texture2D DownloadIcon
        {
            get
            {
                if ((UnityEngine.Object)_downloadIcon == (UnityEngine.Object)null)
                    _downloadIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "download.png", typeof(Texture2D)) as Texture2D;
                return _downloadIcon;
            }
        }

        public Texture2D DashboardIcon
        {
            get
            {
                if ((UnityEngine.Object)_dashboardIcon == (UnityEngine.Object)null)
                    _dashboardIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "dashboard.png", typeof(Texture2D)) as Texture2D;
                return _dashboardIcon;
            }
        }

        public Texture2D AdmobIcon
        {
            get
            {
                if ((UnityEngine.Object)_admobIcon == (UnityEngine.Object)null)
                    _admobIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "admob.png", typeof(Texture2D)) as Texture2D;
                return _admobIcon;
            }
        }

        public Texture2D FbadIcon
        {
            get
            {
                if ((UnityEngine.Object)_fbadIcon == (UnityEngine.Object)null)
                    _fbadIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "facebookads.png", typeof(Texture2D)) as Texture2D;
                return _fbadIcon;
            }
        }

        public Texture2D ChartboostIcon
        {
            get
            {
                if ((UnityEngine.Object)_chartboostIcon == (UnityEngine.Object)null)
                    _chartboostIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "chartboost.png", typeof(Texture2D)) as Texture2D;
                return _chartboostIcon;
            }
        }

        public Texture2D UnityAdIcon
        {
            get
            {
                if ((UnityEngine.Object)_unityadIcon == (UnityEngine.Object)null)
                    _unityadIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "unityads.png", typeof(Texture2D)) as Texture2D;
                return _unityadIcon;
            }
        }

        public Texture2D VungleIcon
        {
            get
            {
                if ((UnityEngine.Object)_vungleIcon == (UnityEngine.Object)null)
                    _vungleIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "vungle.png", typeof(Texture2D)) as Texture2D;
                return _vungleIcon;
            }
        }

        public Texture2D ProfileIcon
        {
            get
            {
                if ((UnityEngine.Object)_profileIcon == (UnityEngine.Object)null)
                    _profileIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "profile.png", typeof(Texture2D)) as Texture2D;
                return _profileIcon;
            }
        }

        public Texture2D NetworkIcon
        {
            get
            {
                if ((UnityEngine.Object)_networkIcon == (UnityEngine.Object)null)
                    _networkIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "network.png", typeof(Texture2D)) as Texture2D;
                return _networkIcon;
            }
        }

        public Texture2D SettingIcon
        {
            get
            {
                if ((UnityEngine.Object)_settingIcon == (UnityEngine.Object)null)
                    _settingIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "settings.png", typeof(Texture2D)) as Texture2D;
                return _settingIcon;
            }
        }

        public Texture2D InforIcon
        {
            get
            {
                if ((UnityEngine.Object)_inforIcon == (UnityEngine.Object)null)
                    _inforIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "infor.png", typeof(Texture2D)) as Texture2D;
                return _inforIcon;
            }
        }

        public Texture2D ToggleOnIcon
        {
            get
            {
                if ((UnityEngine.Object)_toggleOnIcon == (UnityEngine.Object)null)
                    _toggleOnIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "toggle_on.png", typeof(Texture2D)) as Texture2D;
                return _toggleOnIcon;
            }
        }

        public Texture2D ToggleOffIcon
        {
            get
            {
                if ((UnityEngine.Object)_toggleOffIcon == (UnityEngine.Object)null)
                    _toggleOffIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "toggle_off.png", typeof(Texture2D)) as Texture2D;
                return _toggleOffIcon;
            }
        }

        public Texture2D AvailableIcon
        {
            get
            {
                if ((UnityEngine.Object)_availableIcon == (UnityEngine.Object)null)
                    _availableIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "available_icon.png", typeof(Texture2D)) as Texture2D;
                return _availableIcon;
            }
        }

        public Texture2D NotAvailableIcon
        {
            get
            {
                if ((UnityEngine.Object)_notAvailableIcon == (UnityEngine.Object)null)
                    _notAvailableIcon = AssetDatabase.LoadAssetAtPath(IconsPath + "not_available_icon.png", typeof(Texture2D)) as Texture2D;
                return _notAvailableIcon;
            }
        }
        #endregion

        #region Profiles
        void ProfileSetting()
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.Space();

            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Android");
            if (AdGUIHelper.HeaderButton(InforIcon)) AdGUIHelper.ShowMessage(AdString.profile_infor);
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            EditorGUI.indentLevel++;
            ProviderListDrawer(AdType.Interstitial, Settings.localConfig.InterstitialAndroid);
            ProviderListDrawer(AdType.Rewarded, Settings.localConfig.RewardAndroid);
            ProviderListDrawer(AdType.Banner, Settings.localConfig.BannerAndroid);
            EditorGUI.indentLevel--;
            AdGUIHelper.EndGroupedControls();

            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("iOS");
            if (AdGUIHelper.HeaderButton(InforIcon)) AdGUIHelper.ShowMessage(AdString.profile_infor);
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            EditorGUI.indentLevel++;
            ProviderListDrawer(AdType.Interstitial, Settings.localConfig.InterstitialIOS);
            ProviderListDrawer(AdType.Rewarded, Settings.localConfig.RewardIOS);
            ProviderListDrawer(AdType.Banner, Settings.localConfig.BannerIOS);
            EditorGUI.indentLevel--;
            AdGUIHelper.EndGroupedControls();
        }

        void ProviderListDrawer(AdType adType, List<ProviderID> networkIds)
        {
            EditorGUILayout.BeginHorizontal();
            ReorderableListGUI.Title(adType.ToString());

            var allIds = Enum.GetValues(typeof(ProviderID));
            if (GUILayout.Button("Add", GUILayout.Width(70)))
            {
                foreach (ProviderID networkId in allIds)
                {
                    if (IsSuportAd(networkId, adType) && !networkIds.Contains(networkId))
                    {
                        networkIds.Add(networkId);
                        break;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            ReorderableListGUI.ListField(networkIds, ProviderItemDrawer, DrawEmptyProvider, ReorderableListFlags.HideAddButton);
        }

        private ProviderID ProviderItemDrawer(Rect position, ProviderID itemValue)
        {
            position.width -= 50;
            EditorGUI.LabelField(position, itemValue.ToString());

            position.x = position.xMax + 5;
            position.width = 45;

            return itemValue;
        }

        private void DrawEmptyProvider()
        {
            GUILayout.Label("No provider in list.", EditorStyles.miniLabel);
        }

        /// <summary>
        /// Kiểm tra sự provider id có hỗ trợ ad hay không. Từ provider Id không lấy ra được instance 
        /// nên phải dùng cách này trên editor
        /// </summary>
        /// <returns></returns>
        bool IsSuportAd(ProviderID id, AdType type)
        {
            return ProvidersConfig.IsSupport(id, type);
        }
        #endregion

        #region Networks settings
        int networkSelectedIndex;

        void AdNetworkSetting()
        {
            EditorGUI.indentLevel = 0;

            EditorGUILayout.Space();
            networkSelectedIndex = GUILayout.Toolbar(networkSelectedIndex,
                 new GUIContent[] { new GUIContent("Admob", AdmobIcon), new GUIContent("Facebook", FbadIcon)
                 , new GUIContent("UnityAd", UnityAdIcon), new GUIContent("Vungle", VungleIcon)
                 , new GUIContent("Chartboost", ChartboostIcon)}, networkToolBarOptions);
            if (networkSelectedIndex == 0)
            {
                OnGUIAdmobSetting();
            }
            else if (networkSelectedIndex == 1)
            {
                OnGuiFacebookAdSetting();
            }
            else if (networkSelectedIndex == 2)
            {
                OnGUIUnityAdSetting();
            }
            else if (networkSelectedIndex == 3)
            {
                OnGUIVungleSetting();
            }
            else if (networkSelectedIndex == 4)
            {
                OnGUIChartBoostSetting();
            }
        }

        void OnGUIAdColonySetting()
        {
            //EditorGUILayout.BeginVertical(GUI.skin.box);
            //Settings.IsAdColonyActived = EditorGUILayout.ToggleLeft("AdColony", Settings.IsAdColonyActived);

            //if (Settings.IsAdColonyActived)
            //{
            //    Settings.AdColonyIOSAppId = EditorGUILayout.TextField("IOS App Id", Settings.AdColonyIOSAppId);
            //    Settings.AdColonyIOSZoneId = EditorGUILayout.TextField("IOS Zone Id", Settings.AdColonyIOSZoneId);
            //    Settings.AdColonyAndroidAppId = EditorGUILayout.TextField("Android App Id", Settings.AdColonyAndroidAppId);
            //    Settings.AdColonyAndroidZoneId = EditorGUILayout.TextField("Android Zone Id", Settings.AdColonyAndroidZoneId);
            //}
            //EditorGUILayout.EndVertical();
        }

        void OnGUIVungleSetting()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            AdGUIHelper.AvailableIcon(AdsMasterProcessor.vungleDetector.IsValid, AvailableIcon, NotAvailableIcon);
            Settings.IsVungleActived = AdGUIHelper.ActiveToggle(Settings.IsVungleActived, ToggleOnIcon, ToggleOffIcon);
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DashboardIcon, AdString.open_dashboard_btn_hint))
            {
                Application.OpenURL("https://dashboard.vungle.com/dashboard/accounts/pub");
            }
            if (AdGUIHelper.Button(DownloadIcon, AdString.download_sdk_btn_hint))
            {
                Application.OpenURL("https://v.vungle.com/sdk");
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(4f);
            if (Settings.IsVungleActived)
            {
                EditorGUILayout.BeginVertical();
                AdGUIHelper.Session("Android");
                AdGUIHelper.BeginGroupedControls();
                EditorGUI.indentLevel++;
                Settings.localConfig.VungleAndroidAppId = EditorGUILayout.TextField("App Id", Settings.localConfig.VungleAndroidAppId).Trim();
                Settings.localConfig.VungleAndroidDefaultPlacementId = EditorGUILayout.TextField("Default Placement Id", Settings.localConfig.VungleAndroidDefaultPlacementId).Trim();
                EditorGUI.indentLevel--;
                AdGUIHelper.EndGroupedControls();

                AdGUIHelper.Session("iOS");
                AdGUIHelper.BeginGroupedControls();
                EditorGUI.indentLevel++;
                Settings.localConfig.VungleIOSAppId = EditorGUILayout.TextField("App Id", Settings.localConfig.VungleIOSAppId).Trim();
                Settings.localConfig.VungleIOSDefaultPlacementId = EditorGUILayout.TextField("Default Placement Id", Settings.localConfig.VungleIOSDefaultPlacementId).Trim();
                EditorGUI.indentLevel--;
                AdGUIHelper.EndGroupedControls();

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }

        void OnGUIUnityAdSetting()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            Settings.IsUnityAdActived = AdGUIHelper.ActiveToggle(Settings.IsUnityAdActived, ToggleOnIcon, ToggleOffIcon);
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DashboardIcon, AdString.open_dashboard_btn_hint))
            {
                Application.OpenURL("https://operate.dashboard.unity3d.com/");
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(4f);
            if (Settings.IsUnityAdActived)
            {
                AdGUIHelper.Session("Android");
                AdGUIHelper.BeginGroupedControls();
                EditorGUI.indentLevel++;
                Settings.localConfig.UnityAdIdAndroid = EditorGUILayout.TextField("Android Id", Settings.localConfig.UnityAdIdAndroid).Trim();
                Settings.localConfig.UnityAdAndroidVideoPlacementId = EditorGUILayout.TextField("Video Placement Id", Settings.localConfig.UnityAdAndroidVideoPlacementId).Trim();
                Settings.localConfig.UnityAdAndroidRewardedPlacementId = EditorGUILayout.TextField("Rewarded Placement Id", Settings.localConfig.UnityAdAndroidRewardedPlacementId).Trim();
                EditorGUI.indentLevel--;
                AdGUIHelper.EndGroupedControls();

                AdGUIHelper.Session("iOS");
                AdGUIHelper.BeginGroupedControls();
                EditorGUI.indentLevel++;
                Settings.localConfig.UnityAdIdIOS = EditorGUILayout.TextField("Game Id", Settings.localConfig.UnityAdIdIOS).Trim();
                Settings.localConfig.UnityAdIOSVideoPlacementId = EditorGUILayout.TextField("Video Placement Id", Settings.localConfig.UnityAdIOSVideoPlacementId).Trim();
                Settings.localConfig.UnityAdIOSRewardedPlacementId = EditorGUILayout.TextField("Rewarded Placement Id", Settings.localConfig.UnityAdIOSRewardedPlacementId).Trim();
                EditorGUI.indentLevel--;
                AdGUIHelper.EndGroupedControls();
            }
            EditorGUILayout.EndVertical();
        }

        void OnGUIChartBoostSetting()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            AdGUIHelper.AvailableIcon(AdsMasterProcessor.chartboostDetector.IsValid, AvailableIcon, NotAvailableIcon);
            Settings.IsChartBoostActived = AdGUIHelper.ActiveToggle(Settings.IsChartBoostActived, ToggleOnIcon, ToggleOffIcon);
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DashboardIcon, AdString.open_dashboard_btn_hint))
            {
                Application.OpenURL("https://dashboard.chartboost.com/");
            }
            if (AdGUIHelper.Button(DownloadIcon, AdString.download_sdk_btn_hint))
            {
                Application.OpenURL("https://answers.chartboost.com/en-us/articles/download");
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(4f);
            if (Settings.IsChartBoostActived)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Open ChartBoost Settings"))
                {
                    Selection.activeObject = Resources.Load("ChartboostSettings");
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Admob
        void OnGUIAdmobSetting()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            AdGUIHelper.AvailableIcon(AdsMasterProcessor.admobDetector.IsValid, AvailableIcon, NotAvailableIcon);

            Settings.IsAdmobActived = AdGUIHelper.ActiveToggle(Settings.IsAdmobActived, ToggleOnIcon, ToggleOffIcon);
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DashboardIcon, AdString.open_dashboard_btn_hint))
            {
                Application.OpenURL("http://apps.admob.com/");
            }
            if (AdGUIHelper.Button(DownloadIcon, AdString.download_sdk_btn_hint))
            {
                Application.OpenURL("https://github.com/googleads/googleads-mobile-unity/releases");
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(4f);
            if (Settings.IsAdmobActived)
            {
                OnGUIAdmobInterstitialSettings();
                OnGUIAdmobBanner();
                OnGUIAdmobRewarded();
            }
            EditorGUILayout.EndVertical();
        }

        private void OnGUIAdmobRewarded()
        {
            AdGUIHelper.Session("Rewarded");
            AdGUIHelper.BeginGroupedControls();
            Settings.localConfig.AdmobAndroidRewarded = EditorGUILayout.TextField("Android Ad Unit", Settings.localConfig.AdmobAndroidRewarded).Trim();
            Settings.localConfig.AdmobIOSRewarded = EditorGUILayout.TextField("iOS Ad Unit", Settings.localConfig.AdmobIOSRewarded).Trim();
            AdGUIHelper.EndGroupedControls();
        }

        void OnGUIAdmobBanner()
        {

            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Banners");
            if (AdGUIHelper.HeaderButton(AddIcon))
            {
                Settings.localConfig.AdmobBannerUnits.Add(new AdmobBannerUnit(""));
            }
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            if (Settings.localConfig.AdmobBannerUnits.Count > 0)
            {
                EditorGUILayout.BeginVertical();
                for (int i = 0; i < Settings.localConfig.AdmobBannerUnits.Count; i++)
                {
                    var banner = Settings.localConfig.AdmobBannerUnits[i];
                    OnGUIAdmobBannerAdUnit(banner, Settings.localConfig.AdmobBannerUnits);
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                AdGUIHelper.Help(AdString.empty_list_hint);
            }
            AdGUIHelper.EndGroupedControls();
        }

        void OnGUIAdmobInterstitialSettings()
        {
            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Interstitials");
            if (AdGUIHelper.HeaderButton(AddIcon))
            {
                Settings.localConfig.admobInterstitials.Add(new AdmobInterstitialAdUnit(""));
            }
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            if (Settings.localConfig.admobInterstitials.Count > 0)
            {
                for (int i = 0; i < Settings.localConfig.admobInterstitials.Count; i++)
                {
                    var ad = Settings.localConfig.admobInterstitials[i];
                    OnAdmobInterstitialAdUnit(ad, Settings.localConfig.admobInterstitials);
                }
            }
            else
            {
                AdGUIHelper.Help(AdString.empty_list_hint);
            }
            AdGUIHelper.EndGroupedControls();
        }

        void OnAdmobInterstitialAdUnit(AdmobInterstitialAdUnit ad, List<AdmobInterstitialAdUnit> list)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            var newKey = AdGUIHelper.TextField(ad.key, AdString.key_label, AdString.key_infor);

            // just change if there are no same key
            int countKey = 0;
            if (newKey != ad.key)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    if (newKey.Trim().Equals(list[k].key.Trim())) countKey++;
                }
            }
            if (countKey == 0)
            {
                ad.key = newKey.Trim();
            }
            ad.androidAdId = EditorGUILayout.TextField("Android Unit Id", ad.androidAdId).Trim();
            ad.iosAdId = EditorGUILayout.TextField("IOS Unit Id", ad.iosAdId).Trim();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DeleteIcon) && ConfirmRemoveItem())
            {
                list.Remove(ad);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            GUILayout.Space(5f);
        }

        void OnGUIAdmobBannerAdUnit(AdmobBannerUnit ad, List<AdmobBannerUnit> list)
        {
            if (ad == null)
            {
                list.Remove(ad);
                return;
            }

            EditorGUILayout.BeginVertical(GUI.skin.box);

            var newKey = AdGUIHelper.TextField(ad.key, AdString.key_label, AdString.key_infor);

            // just change if there are no same key
            int countKey = 0;
            if (newKey != ad.key)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    if (newKey.Trim().Equals(list[k].key.Trim())) countKey++;
                }
            }
            if (countKey == 0)
            {
                ad.key = newKey.Trim();
            }

            ad.androidAdId = EditorGUILayout.TextField("Android Ad Unit", ad.androidAdId).Trim();
            ad.iosAdId = EditorGUILayout.TextField("IOS Ad Unit", ad.iosAdId).Trim();
            ad.position = (BannerPosition)EditorGUILayout.EnumPopup("Position", ad.position);
            ad.size = (BannerSize)EditorGUILayout.EnumPopup("Size", ad.size);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DeleteIcon) && ConfirmRemoveItem())
            {
                list.Remove(ad);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            GUILayout.Space(5f);
        }
        #endregion

        #region Facebook Ad
        void OnGuiFacebookAdSetting()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            AdGUIHelper.AvailableIcon(AdsMasterProcessor.fbadsDetector.IsValid, AvailableIcon, NotAvailableIcon);
            Settings.IsFbAdActived = AdGUIHelper.ActiveToggle(Settings.IsFbAdActived, ToggleOnIcon, ToggleOffIcon);
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DashboardIcon, AdString.open_dashboard_btn_hint))
            {
                Application.OpenURL("https://developers.facebook.com/apps/");
            }
            if (AdGUIHelper.Button(DownloadIcon, AdString.download_sdk_btn_hint))
            {
                Application.OpenURL("https://developers.facebook.com/docs/unity");
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(4f);
            if (Settings.IsFbAdActived)
            {
                //
                OnFBInterstitialSettings();
                //
                OnFBBannerSettings();
                //
                //OnFBRewardedSettings();
                // 
                OnFbNativeSettings();
                GUILayout.Space(10f);
            }
            EditorGUILayout.EndVertical();
        }

        void OnFBBannerSettings()
        {
            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Banners");
            if (AdGUIHelper.HeaderButton(AddIcon))
            {
                Settings.localConfig.fbBanners.Add(new FBBannerAdUnit(""));
            }
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            if (Settings.localConfig.fbBanners.Count > 0)
            {
                for (int i = Settings.localConfig.fbBanners.Count - 1; i >= 0; i--)
                {
                    var ad = Settings.localConfig.fbBanners[i];
                    OnFbBannerAdUnit(ad, Settings.localConfig.fbBanners);
                }
            }
            else
            {
                AdGUIHelper.Help(AdString.empty_list_hint);
            }
            AdGUIHelper.EndGroupedControls();
        }

        private void OnFBRewardedSettings()
        {
            EditorGUILayout.HelpBox("Rewarded Video", MessageType.None);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            Settings.localConfig.fbRewarded.androidAdId = EditorGUILayout.TextField("Android Placement Id", Settings.localConfig.fbRewarded.androidAdId).Trim();
            Settings.localConfig.fbRewarded.iosAdId = EditorGUILayout.TextField("IOS Placement Id", Settings.localConfig.fbRewarded.iosAdId).Trim();
            EditorGUILayout.EndVertical();
            GUILayout.Space(10f);
        }

        void OnFBInterstitialSettings()
        {
            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Interstitials");
            if (AdGUIHelper.HeaderButton(AddIcon))
            {
                Settings.localConfig.fbInterstitials.Add(new FbInterstitialUnit(""));
            }
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            if (Settings.localConfig.fbInterstitials.Count > 0)
            {
                for (int i = Settings.localConfig.fbInterstitials.Count - 1; i >= 0; i--)
                {
                    var ad = Settings.localConfig.fbInterstitials[i];
                    OnFbInterstitialAdUnit(ad, Settings.localConfig.fbInterstitials);
                }
            }
            else
            {
                AdGUIHelper.Help(AdString.empty_list_hint);
            }
            AdGUIHelper.EndGroupedControls();
        }

        void OnFbNativeSettings()
        {
            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Native Ads");
            if (AdGUIHelper.HeaderButton(AddIcon))
            {
                Settings.localConfig.fbNativeAds.Add(new FBNativeAdUnit(""));
            }
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            if (Settings.localConfig.fbNativeAds.Count > 0)
            {
                for (int i = Settings.localConfig.fbNativeAds.Count - 1; i >= 0; i--)
                {
                    var ad = Settings.localConfig.fbNativeAds[i];
                    OnFbNativeAdUnit(ad, Settings.localConfig.fbNativeAds);
                }
            }
            else
            {
                AdGUIHelper.Help(AdString.empty_list_hint);
            }
            AdGUIHelper.EndGroupedControls();
        }

        void OnFbInterstitialAdUnit(FbInterstitialUnit ad, List<FbInterstitialUnit> list)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            var newKey = AdGUIHelper.TextField(ad.key, AdString.key_label, AdString.key_infor);

            int countKey = 0;
            if (newKey != ad.key)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    if (newKey.Trim().Equals(list[k].key.Trim())) countKey++;
                }
            }
            if (countKey == 0)
            {
                ad.key = newKey.Trim();
            }
            ad.androidAdId = EditorGUILayout.TextField("Android Placement Id", ad.androidAdId).Trim();
            ad.iosAdId = EditorGUILayout.TextField("IOS Placement Id", ad.iosAdId).Trim();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DeleteIcon) && ConfirmRemoveItem())
            {
                list.Remove(ad);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            GUILayout.Space(5f);
        }

        void OnFbBannerAdUnit(FBBannerAdUnit ad, List<FBBannerAdUnit> list)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            var newKey = AdGUIHelper.TextField(ad.key, AdString.key_label, AdString.key_infor);

            int countKey = 0;
            if (newKey != ad.key)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    if (newKey.Trim().Equals(list[k].key.Trim())) countKey++;
                }
            }
            if (countKey == 0)
            {
                ad.key = newKey.Trim();
            }
            ad.androidAdId = EditorGUILayout.TextField("Android Placement Id", ad.androidAdId).Trim();
            ad.iosAdId = EditorGUILayout.TextField("IOS Placement Id", ad.iosAdId).Trim();
            ad.position = (BannerPosition)EditorGUILayout.EnumPopup("Position", ad.position);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DeleteIcon) && ConfirmRemoveItem())
            {
                list.Remove(ad);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            GUILayout.Space(5f);
        }

        void OnFbNativeAdUnit(FBNativeAdUnit ad, List<FBNativeAdUnit> list)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            var newKey = AdGUIHelper.TextField(ad.key, AdString.key_label, AdString.key_infor);

            int countKey = 0;
            if (newKey != ad.key)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    if (newKey.Trim().Equals(list[k].key.Trim())) countKey++;
                }
            }
            if (countKey == 0)
            {
                ad.key = newKey.Trim();
            }
            ad.androidPlacementId = EditorGUILayout.TextField("Android Placement Id", ad.androidPlacementId).Trim();
            ad.iosPlacementId = EditorGUILayout.TextField("IOS Placement Id", ad.iosPlacementId).Trim();

            ad.minSecondsToReload = EditorGUILayout.Slider("Min Seconds To Reload", ad.minSecondsToReload, 30f, 120f);
            ad.preload = EditorGUILayout.ToggleLeft("Preload", ad.preload);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (AdGUIHelper.Button(DeleteIcon) && ConfirmRemoveItem())
            {
                list.Remove(ad);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            GUILayout.Space(5f);
        }
        #endregion

        #region Settings
        void OnGUISettings()
        {
            EditorGUI.indentLevel = 0;

            OnGUIInterstitialRules();

            OnGUIConnections();

            OnGUIProfileMode();

            OnGUITestModes();

            OnGUIRemoteConfig();

            OnGUIAdvancedOptions();

            OnGUIEditorSetting();
        }

        void OnGUIEditorSetting()
        {
            EditorGUILayout.Space();
            AdGUIHelper.Session("Editor");

            AdGUIHelper.BeginGroupedControls();
            AdString.language = (Language)EditorGUILayout.EnumPopup(new GUIContent(AdString.language_label.Text, AdString.language_tooltip.Text), AdString.language);
            AdGUIHelper.EndGroupedControls();
        }

        void OnGUIRemoteConfig()
        {
            EditorGUILayout.Space();

            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Remote Config");
            if (AdGUIHelper.HeaderButton(InforIcon)) AdGUIHelper.ShowMessage(AdString.remote_config_infor);
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox(AdString.data_config_version_warn.Text, MessageType.None);
            Settings.ActiveRemoteConfig = EditorGUILayout.Toggle("Active", Settings.ActiveRemoteConfig);
            if (Settings.ActiveRemoteConfig)
            {
                Settings.RemoteConfigUrl = EditorGUILayout.TextField("Json Config Url", Settings.RemoteConfigUrl).Trim();
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Data Version", AdConfig.DATA_VERSION.ToString());
            if (GUILayout.Button("Export to Json", longButtonOptions))
            {
                ExportConfigToFile();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            AdGUIHelper.EndGroupedControls();
        }

        void OnGUITestModes()
        {
            EditorGUILayout.Space();

            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Test Mode");
            if (AdGUIHelper.HeaderButton(InforIcon)) AdGUIHelper.ShowMessage(AdString.test_mode_infor);
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox(AdString.debug_warning.Text, MessageType.None);
            Settings.AdMobDebug = EditorGUILayout.Toggle("Admob Debug", Settings.AdMobDebug);
            Settings.UnityAdDebug = EditorGUILayout.Toggle("Unity Ad Debug", Settings.UnityAdDebug);
            EditorGUI.indentLevel--;
            AdGUIHelper.EndGroupedControls();
        }

        void OnGUIConnections()
        {
            EditorGUILayout.Space();
            AdGUIHelper.Session("Connection");
            AdGUIHelper.BeginGroupedControls();
            EditorGUI.indentLevel++;

            Settings.localConfig.RequestTimeOut = AdGUIHelper.FloatField(Settings.localConfig.RequestTimeOut, AdString.request_timeout_label, AdString.request_timeout_tooltip);

            Settings.localConfig.AdExpiration = AdGUIHelper.FloatField(Settings.localConfig.AdExpiration, AdString.ad_expiration_label, AdString.ad_expiration_tooltip);

            EditorGUI.indentLevel--;
            AdGUIHelper.EndGroupedControls();
        }

        void OnGUIProfileMode()
        {
            EditorGUILayout.Space();

            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Profile Mode");
            if (AdGUIHelper.HeaderButton(InforIcon)) AdGUIHelper.ShowMessage(AdString.profile_mode_infor);
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            EditorGUI.indentLevel++;

            Settings.localConfig.interstitialProfileMode = (ProfileMode)EditorGUILayout.EnumPopup("Interstitial", Settings.localConfig.interstitialProfileMode);

            Settings.localConfig.rewardedProfileMode = (ProfileMode)EditorGUILayout.EnumPopup("Rewarded Video", Settings.localConfig.rewardedProfileMode);

            Settings.localConfig.bannerProfileMode = (ProfileMode)EditorGUILayout.EnumPopup("Banner", Settings.localConfig.bannerProfileMode);


            EditorGUI.indentLevel--;
            AdGUIHelper.EndGroupedControls();
        }

        void OnGUIAdvancedOptions()
        {
            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Advanced");
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            EditorGUI.indentLevel++;
            Settings.ActiveZeroTimeScale = EditorGUILayout.Toggle(new GUIContent("Zero TimeScale Ads", AdString.zero_timescale_tooltip.Text),
                Settings.ActiveZeroTimeScale);
            EditorGUI.indentLevel--;
            AdGUIHelper.EndGroupedControls();
        }

        void OnGUIInterstitialRules()
        {
            EditorGUILayout.Space();

            AdGUIHelper.BeginSessionGroup();
            AdGUIHelper.Session("Interstitial");
            if (AdGUIHelper.HeaderButton(InforIcon)) AdGUIHelper.ShowMessage(AdString.interstitial_limit_infor);
            AdGUIHelper.EndSessionGroup();

            AdGUIHelper.BeginGroupedControls();
            EditorGUI.indentLevel++;

            Settings.localConfig.IsTimeLimited = EditorGUILayout.Toggle(new GUIContent("Time Limit", AdString.interstitial_time_limit_tooltip.Text),
                Settings.localConfig.IsTimeLimited);

            if (Settings.localConfig.IsTimeLimited)
            {
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;

                Settings.localConfig.DelayInterstitialFromFristOpen = EditorGUILayout.FloatField(new GUIContent("First Open Delay", AdString.first_open_delay_tooltip.Text),
                    Settings.localConfig.DelayInterstitialFromFristOpen);

                Settings.localConfig.DelayInterstitialFromStartApp = EditorGUILayout.FloatField(new GUIContent("Start Delay", AdString.start_delay_tooltip.Text),
                    Settings.localConfig.DelayInterstitialFromStartApp);

                Settings.localConfig.BetweenInterstitialLimited = EditorGUILayout.FloatField(new GUIContent("Between 2 Ads", AdString.between_2ads_tooltip.Text),
                    Settings.localConfig.BetweenInterstitialLimited);
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }

            Settings.localConfig.IsSkipInterstitial = EditorGUILayout.Toggle(new GUIContent("Skip Interstitial", AdString.skip_inerstitial_tooltip.Text),
                Settings.localConfig.IsSkipInterstitial);
            if (Settings.localConfig.IsSkipInterstitial)
            {
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;
                Settings.localConfig.SkipInterstitialStep = EditorGUILayout.IntField(new GUIContent("Step", AdString.skip_inerstitial_tooltip.Text),
                    Settings.localConfig.SkipInterstitialStep);
                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
            }

            Settings.localConfig.RequiredInternetConnection = AdGUIHelper.Toggle(Settings.localConfig.RequiredInternetConnection, AdString.required_connection_label, AdString.required_connection_tooltip);

            EditorGUI.indentLevel--;
            AdGUIHelper.EndGroupedControls();
        }

        void ExportConfigToFile()
        {
            var path = EditorUtility.SaveFilePanel(
                "Export config",
                "",
                "ads_config.json",
                "json");
            if (!string.IsNullOrEmpty(path))
            {
                Settings.localConfig.Version = AdConfig.DATA_VERSION;
                string json = JsonUtility.ToJson(Settings.localConfig, true);
                File.WriteAllText(path, json);
            }
        }

        #endregion

        #region Help
        static readonly AdIssue[] Issues = new AdIssue[] {
            new AdIssue("Interstitial does not show up",
                "- Check the Key of interstitial at AdsMaster.ShowInterstitial() and settings. Both should be same.",null)
        };
        int pickedIssueIndex;

        void OnGUIHelp()
        {
            EditorGUILayout.HelpBox("Sequence issues", MessageType.None);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            for (int i = 0; i < Issues.Length; i++)
            {
                var issue = Issues[i];
                EditorGUILayout.BeginVertical(GUI.skin.box);
                if (EditorGUILayout.Foldout(pickedIssueIndex == i, i.ToString() + "." + issue.name))
                {
                    pickedIssueIndex = i;
                    EditorGUILayout.TextArea(issue.answer);
                    if (!string.IsNullOrEmpty(issue.url) && GUILayout.Button("Open", new GUILayoutOption[] { GUILayout.Width(40f) }))
                        Application.OpenURL(issue.url);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region About
        void OnGUIAbout()
        {
            AdGUIHelper.BeginGroupedControls();
            GUILayout.Space(10F);
            EditorGUILayout.LabelField("WELCOME");
            EditorGUILayout.TextField("Version " + AdString.VersionName + "\nThank you for purchasing Ads Master!");

            GUILayout.Space(10f);
            EditorGUILayout.LabelField("QUICK START");

            GUILayout.Space(5f);
            string step1 = "Step 1 - Import all of network SDK (such as Google Ads, Audience Network)";
            EditorGUILayout.TextArea(step1);

            GUILayout.Space(5f);
            string step2 = "Step 2 - Configure unit ids at Providers Tab";
            EditorGUILayout.TextArea(step2);

            GUILayout.Space(5f);
            string step3 = "Step 3 - Add networks you want to show up by priority at Profile Tab";
            EditorGUILayout.TextArea(step3);

            GUILayout.Space(5f);
            string step4 = "Step 4 - Control ads by simple code";
            step4 += "\nInterstitial\n>To show interstitial, invoke simple method \"AdsMaster.ShowInterstitial()\"." +
                " Ads Master take care all loading interstitial progresses.";

            step4 += "\n\nBanner\nAds Master allow you show once banner per screen. It mean only one banner is visible in the same time." +
                "\n>To show banner as soon as posible, invoke method \"AdsMaster.ShowBanner()\". Ads Master show it up when compatible banner is loaded." +
                "\n>To hide banner, invoke method \"AdsMaster.HideBanner()\".";

            step4 += "\n\nRewarded Video" +
                "\n>To check rewarded video if it loaded, use result of method \"AdsMaster.HasReward()\"." +
                "\n>To show rewarded video and handle callback, invoke method \"AdsMaster.ShowReward(success => { if (success) coin++; });\".";
            EditorGUILayout.TextArea(step4);

            GUILayout.Space(10f);
            EditorGUILayout.LabelField("SUPPORT");
            EditorGUILayout.TextField("Email cs@vmodev.com");
            EditorGUILayout.TextField("Group Skype https://join.skype.com/Q5GQFaTzipna");
            GUILayout.Space(20f);
            AdGUIHelper.EndGroupedControls();
        }
        #endregion

        #region Common
        bool ConfirmRemoveItem()
        {
            return EditorUtility.DisplayDialog("REMOVE",
                AdString.confirm_remove.Text, "Remove", "Nope");
        }
        #endregion
    }
}