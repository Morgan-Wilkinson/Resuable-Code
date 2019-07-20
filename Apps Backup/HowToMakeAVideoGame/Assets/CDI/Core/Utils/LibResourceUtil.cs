#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace cdi
{
    public class LibResourceUtil
    {
        const string AssetExtension = ".asset";
        const string AssetPath = "Assets";
        const string DataPath = "Assets\\CDI Data";
        const string DataFolder = "CDI Data";

        public static T LoadAndCreateSetting<T>(params string[] subFolders) where T : ScriptableObject
        {
            // determind filePath
            string filePath = DataPath;
            for (int i = 0; i < subFolders.Length; i++)
            {
                filePath = Path.Combine(filePath, subFolders[i]);
            }
            filePath = filePath + "\\" + typeof(T).Name + AssetExtension;
            T instance = AssetDatabase.LoadAssetAtPath<T>(filePath);
            if (instance == null)
            {
                if (!Directory.Exists(DataPath))
                {
                    AssetDatabase.CreateFolder(AssetPath, DataFolder);
                }
                // Create subfolder if need
                if (subFolders != null && subFolders.Length > 0)
                {
                    string path = DataPath;
                    for (int i = 0; i < subFolders.Length; i++)
                    {
                        var newPath = Path.Combine(path, subFolders[i]);
                        if (!Directory.Exists(newPath))
                        {
                            AssetDatabase.CreateFolder(path, subFolders[i]);
                        }
                        path = newPath;
                    }
                }
                // Create Instance
                instance = ScriptableObject.CreateInstance<T>();
                // Save instance to file
                AssetDatabase.CreateAsset(instance, filePath);
            }
            return instance;
        }
    }
}
#endif