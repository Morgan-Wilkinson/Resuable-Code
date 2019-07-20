using UnityEngine.UI;
namespace cdi
{
    public static class TextExtension
    {
        public static void SetAlpha(this Text text, float alpha)
        {
            var color = text.color;
            color.a = alpha;
            text.color = color;
        }
    }
}