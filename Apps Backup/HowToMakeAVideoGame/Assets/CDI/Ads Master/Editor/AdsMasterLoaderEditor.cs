using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace cdi.ad
{
    [CustomEditor(typeof(AdsMasterSettingLoader))]
    public class AdsMasterLoaderEditor : Editor
    {
        AdsMasterSettingLoader Loader
        {
            get { return (AdsMasterSettingLoader)target; }
        }

        public override void OnInspectorGUI()
        {
            GUI.changed = false;

            EditorGUI.BeginChangeCheck();
            Loader.settings = EditorGUILayout.ObjectField("Settings", Loader.settings, typeof(AdsMasterSetting), false) as AdsMasterSetting;
            if (Loader.settings == null)
            {
                Loader.settings = LibResourceUtil.LoadAndCreateSetting<AdsMasterSetting>();
                GUI.changed = true;
            }

            if (GUILayout.Button("Edit Settings"))
            {
                AdsMasterEditor.OpenSettings();
            }
            EditorGUI.EndChangeCheck();

            if (GUI.changed)
            {
                EditorSceneManager.MarkAllScenesDirty();
            }
        }
    }
}