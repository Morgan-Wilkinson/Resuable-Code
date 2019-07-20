using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace cdi
{
    public static class EditorGUILayoutUtil
    {
        public static LayerMask LayerMaskField(string label, LayerMask layerMask, bool readOnly = false)
        {
            List<string> layers = new List<string>();
            List<int> layerNumbers = new List<int>();

            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName != "")
                {
                    layers.Add(layerName);
                    layerNumbers.Add(i);
                }
            }
            int maskWithoutEmpty = 0;
            for (int i = 0; i < layerNumbers.Count; i++)
            {
                if (((1 << layerNumbers[i]) & layerMask.value) > 0)
                    maskWithoutEmpty |= (1 << i);
            }
            maskWithoutEmpty = EditorGUILayout.MaskField(label, maskWithoutEmpty, layers.ToArray());
            if (readOnly)
            {
                int mask = 0;
                for (int i = 0; i < layerNumbers.Count; i++)
                {
                    if ((maskWithoutEmpty & (1 << i)) > 0)
                        mask |= (1 << layerNumbers[i]);
                }
                layerMask.value = mask;
            }
            return layerMask;
        }
    }
}