using UnityEditor;
using UnityEngine;

namespace cdi.ad
{
    [CustomEditor(typeof(AdsMasterSetting))]
    public class AdsMasterSettingEditor : Editor
    {
        AdsMasterSetting setting
        {
            get { return (AdsMasterSetting)target; }
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Edit Settings"))
            {
                AdsMasterEditor.OpenSettings();
            }
        }
    }
}