using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cdi
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    public delegate void OnDataFetched<T>(T data);

    /// <summary>
    /// Cho phép download 1 file và đổ dữ liệu dạng keyvalue vào gameobject
    /// </summary>
    public class RemoteFileSync : MonoBehaviour
    {
        #region Singleton
        static RemoteFileSync _Instance = null;

        /// <summary>
        /// Đảm bảo không bao giờ trả về null
        /// </summary>
        public static RemoteFileSync Instance
        {
            get
            {
                if (_Instance == null)
                {
                    //thử tìm trong scene
                    _Instance = GameObject.FindObjectOfType<RemoteFileSync>();
                    if (_Instance == null)  //nếu vẫn không có thì tự tạo
                    {
                        var gameobject = new GameObject();
                        gameobject.name = "RemoteFileSync";
                        _Instance = gameobject.AddComponent<RemoteFileSync>();
                    }
                }
                return _Instance;
            }
        }

        void Start()
        {
            if (_Instance == null)
            {
                _Instance = this;
            }
            else if (_Instance != this) //không destroy chính nó
            {
                DestroyImmediate(gameObject);
                return;
            }
            DontDestroyOnLoad(_Instance);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T>()
        {
            //lấy dữ liệu thuộc kiểu T
            var text = PlayerPrefs.GetString(GetKey<T>(), null);
            if (text != null)
            {
                var resObj = (T)Activator.CreateInstance(typeof(T), new object[] { });
                //lấy dữ liệu cho vào dict
                Dictionary<string, string> dict = new Dictionary<string, string>();
                string[] lines = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (line.Length > 0 && line.Contains("="))
                    {
                        var temps = line.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        if (temps.Length == 2)
                        {
                            dict.Add(temps[0], temps[1]);
                        }
                    }
                }
                //đổ dữ liệu từ dict vào obj
                var fields = typeof(T).GetFields();
                foreach (var field in fields)
                {
                    if (dict.ContainsKey(field.Name))
                    {
                        try
                        {
                            if (field.FieldType == typeof(bool) || field.FieldType == typeof(bool?))
                                field.SetValue(resObj, bool.Parse(dict[field.Name]));
                            else if (field.FieldType == typeof(string))
                                field.SetValue(resObj, dict[field.Name]);
                            else if (field.FieldType == typeof(float) || field.FieldType == typeof(float?))
                                field.SetValue(resObj, float.Parse(dict[field.Name]));
                            else if (field.FieldType == typeof(int) || field.FieldType == typeof(int?))
                                field.SetValue(resObj, int.Parse(dict[field.Name]));
                            else CLog.Log(this, "Has not data for " + field.Name);
                        }
                        catch (Exception ex) { CLog.Log(this, ex.Message); }
                    }
                }
                return resObj;
            }
            else
            {
                return default(T);
            }
        }

        private string GetKey<T>()
        {
            return "RemoteFileSync_" + typeof(T).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="callback"></param>
        /// <param name="tryToResend"></param>
        public void FetchData<T>(string url, OnDataFetched<T> callback = null, float tryToResend = 10f)
        {
            StartCoroutine(FetchDataSync<T>(url, callback, 0, tryToResend));
        }

        private IEnumerator FetchDataSync<T>(string url, OnDataFetched<T> callback, float delay, float tryToResend)
        {
            if (delay != 0)
                yield return new WaitForSeconds(delay);
            CLog.Log(this, "Loading " + url);
            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)  //load successfully
            {
                //save data to use next times
                PlayerPrefs.SetString(GetKey<T>(), www.text);
                T data = GetData<T>();
                if (callback != null)
                {
                    callback(data);
                }
            }
            else
            {
                if (tryToResend > 0)
                    StartCoroutine(FetchDataSync<T>(url, callback, tryToResend, tryToResend));
            }
        }
    }
}