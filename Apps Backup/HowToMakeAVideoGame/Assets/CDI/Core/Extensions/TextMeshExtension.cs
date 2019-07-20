using UnityEngine;
using UnityEngine.UI;
namespace cdi
{
    public static class TextMeshExtension
    {
        public static void SetAlpha(this TextMesh textMesh, float alpha)
        {
            var color = textMesh.color;
            color.a = alpha;
            textMesh.color = color;
        }
    }
}