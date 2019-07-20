using UnityEngine;

namespace cdi
{
    public class RectTransformUtil
    {
        public static RectTransform CreateFullUIObject(string name, Transform parent)
        {
            return CreateRectObject(name, parent, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        public static RectTransform CreateRectObject(string name, Transform parent, Vector2 anchorMin,
            Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            RectTransform rectTransform;
            GameObject gameobj = new GameObject();
            gameobj.name = name;
            rectTransform = gameobj.AddComponent(typeof(RectTransform)) as RectTransform;
            rectTransform.SetParent(parent, false);
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = sizeDelta;
            return rectTransform;
        }
    }
}