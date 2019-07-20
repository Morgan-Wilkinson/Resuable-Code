using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cdi;

namespace cdi.ad
{
    public class DeviceUtil
    {
        static float DeviceDiagonalSizeInInches()
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
            return diagonalInches;
        }

        static bool? _IsTablet = null;

        public static bool IsTablet
        {
            get
            {
                if (_IsTablet == null)
                {
                    if (Application.platform == RuntimePlatform.Android && DeviceDiagonalSizeInInches() > 6.5f)
                    {
                        _IsTablet = true;
                    }
                    else
                    {
                        _IsTablet = false;
                    }
                }
                return (bool)_IsTablet;
            }

        }
    }
}