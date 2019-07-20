using UnityEditor;
using UnityEngine;

namespace cdi.ad
{
    public class AdGUIHelper
    {
        public static readonly Color HeaderColor = new Color(.3f, .8f, 1f);

        public static void BeginGroupedControls()
        {
            GUI.backgroundColor = Color.white;
            GUILayout.BeginHorizontal();
            GUILayout.Space(2f);
#if UNITY_2017_1_OR_NEWER
            EditorGUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(10f));
#else
            EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
#endif

            GUILayout.BeginVertical();
            GUILayout.Space(3f);
        }

        public static void EndGroupedControls()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(0f);
            GUILayout.EndHorizontal();

            GUILayout.Space(3f);
        }

        public static void Session(string secsionName)
        {
            GUI.backgroundColor = AdGUIHelper.HeaderColor;
            string text = secsionName;
            text = "<b><size=11>" + text + "</size></b>";
            GUILayout.Toggle(true, text, "dragtab");
        }

        public static void BeginSessionGroup()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = AdGUIHelper.HeaderColor;
        }

        public static void EndSessionGroup()
        {
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
        }

        public static bool HeaderButton(Texture icon, Pharse pharse = null)
        {
            return GUILayout.Button(new GUIContent(icon, pharse != null ? pharse.Text : ""), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false), GUILayout.MaxHeight(17f));
        }

        public static bool Button(Texture icon, Pharse pharse = null)
        {
            return GUILayout.Button(new GUIContent(icon, pharse != null ? pharse.Text : ""), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
        }

        public static void Icon(Texture icon, Pharse pharse = null)
        {
            GUILayout.Label(new GUIContent(icon, pharse != null ? pharse.Text : ""), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
        }

        public static void AvailableIcon(bool available, Texture availableIcon, Texture notAvailableIcon)
        {
            Icon(available ? availableIcon : notAvailableIcon, available ? AdString.sdk_is_available : AdString.sdk_is_not_available);
        }

        public static void Help(Pharse pharse)
        {
            EditorGUILayout.LabelField(pharse.Text);
        }

        public static void ShowMessage(Pharse pharse)
        {
            EditorUtility.DisplayDialog(AdString.infor_popup_title.Text, pharse.Text, AdString.infor_popup_ok.Text);
        }

        public static string TextFieldInfor(string label, string value, Texture texture, Pharse pharse)
        {
            EditorGUILayout.BeginHorizontal();
            var newValue = EditorGUILayout.TextField(label, value).Trim();
            if (GUILayout.Button(new GUIContent(texture), EditorStyles.label, GUILayout.Width(20f))) AdGUIHelper.ShowMessage(pharse);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static string TextField(string value, Pharse label, Pharse tooltip)
        {
            return EditorGUILayout.TextField(new GUIContent(label.Text, tooltip.Text), value);
        }

        public static float FloatField(float value, Pharse label, Pharse tooltip)
        {
            return EditorGUILayout.FloatField(new GUIContent(label.Text, tooltip.Text), value);
        }

        public static bool Toggle(bool value, Pharse label, Pharse tooltip)
        {
            return EditorGUILayout.Toggle(new GUIContent(label.Text, tooltip.Text), value);
        }

        public static bool ActiveToggle(bool active, Texture2D onIcon, Texture2D offIcon)
        {
            if (GUILayout.Button(new GUIContent("", active ? onIcon : offIcon, AdString.active_provider_tip.Text), EditorStyles.label))
            {
                active = !active;
            }
            return active;
        }
    }
}