using UnityEngine;
namespace cdi
{
    public static class SpriteRendererExtension
    {
        public static void SetAlpha(this SpriteRenderer renderer, float alpha)
        {
            var color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }
}