using System;
using UnityEngine;

namespace cdi
{
    public class CLog
    {
        const string TIME_FORMAT = "hh:mm:ss.FFF";

        static string NowText
        {
            get
            {
                return DateTime.Now.ToString(TIME_FORMAT);
            }
        }

        public static bool IncludeTime = false;

        static bool EnabledLog = true;
        static bool EnabledDeviceLog = false;
        static bool IsEnabled = true;

        static CLog()
        {
            UpdateEnable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void SetEnableLog(bool value)
        {
            EnabledLog = value;
            UpdateEnable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void SetEnabledDeviceLog(bool value)
        {
            EnabledDeviceLog = value;
            SetEnableLog(true);
        }

        static void UpdateEnable()
        {
            IsEnabled = EnabledLog
                         && (Application.platform == RuntimePlatform.OSXEditor ||
                             Application.platform == RuntimePlatform.WindowsEditor ||
                             EnabledDeviceLog);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            if (!IsEnabled) return;
            Debug.Log(FullMessage(null, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Log(object obj, string message)
        {
            if (!IsEnabled) return;
            Debug.Log(FullMessage(obj.GetType().Name, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Err(string message)
        {
            if (!IsEnabled) return;
            Debug.LogError(FullMessage(null, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Err(object obj, string message)
        {
            if (!IsEnabled) return;
            Debug.LogError(FullMessage(obj.GetType().Name, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string message)
        {
            if (!IsEnabled) return;
            Debug.LogWarning(FullMessage(null, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Warn(object obj, string message)
        {
            if (!IsEnabled) return;
            Debug.LogWarning(FullMessage(obj.GetType().Name, message));
        }

        static string FullMessage(string tag, string message)
        {
            string msg = "CDI>";
            if (tag != null) msg += tag + ">";
            if (IncludeTime) msg += NowText + ">";
            msg += message;
            return msg;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ExtensionCLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Log(this MonoBehaviour obj, string message)
        {
            CLog.Log(obj, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Err(this MonoBehaviour obj, string message)
        {
            CLog.Err(obj, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Warn(this MonoBehaviour obj, string message)
        {
            CLog.Err(obj, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Log(this Transform obj, string message)
        {
            CLog.Log(obj, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Err(this Transform obj, string message)
        {
            CLog.Warn(obj, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        public static void Warn(this Transform obj, string message)
        {
            CLog.Err(obj, message);
        }
    }
}