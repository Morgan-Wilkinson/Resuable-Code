using UnityEngine.UI;
namespace cdi
{
    public static class ImageExtension
    {
        public static void SetAlpha(this Image image, float value)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }

        public static void SetFade(this Image image, float value)
        {
            var color = image.color;
            color.a = value;
            image.color = color;
        }
    }
}