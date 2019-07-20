using UnityEngine;

namespace cdi
{
    public class LayerMaskUtil
    {
        public static bool ContainLayer(LayerMask layerMask, int layer)
        {
            if (((1 << layer) & layerMask) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static LayerMask AddLayer(LayerMask layerMask, int layer)
        {
            return layerMask | (1 << layer);
        }
    }
}