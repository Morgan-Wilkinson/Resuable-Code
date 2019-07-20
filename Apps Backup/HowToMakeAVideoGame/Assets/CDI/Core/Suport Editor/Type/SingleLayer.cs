using UnityEngine;

namespace cdi
{
    [System.Serializable]
    public class SingleLayer
    {
        [SerializeField]
        private int layerIndex = 0;

        public int LayerIndex
        {
            get { return layerIndex; }
            set
            {
                if (value > 0 && value < 32)
                {
                    layerIndex = value;
                }
            }
        }

        public static SingleLayer From(int layerIndex)
        {
            var res = new SingleLayer();
            res.LayerIndex = layerIndex;
            return res;
        }
    }
}