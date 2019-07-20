using UnityEditor;
using UnityEngine;
namespace cdi
{
    [CustomPropertyDrawer(typeof(SingleLayer))]
    public class SingleLayerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            SerializedProperty layerIndex = property.FindPropertyRelative("layerIndex");
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            layerIndex.intValue = EditorGUI.LayerField(position, layerIndex.intValue);
            EditorGUI.EndProperty();
        }
    }
}