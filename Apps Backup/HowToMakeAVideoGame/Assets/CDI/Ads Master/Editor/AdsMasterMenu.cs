using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace cdi.ad
{
    public class AdsMasterMenu : EditorWindow
    {
        [MenuItem("CDI/Ads Master/Add Loader")]
        public static void AddObject()
        {
            if (GameObject.FindObjectOfType<AdsMasterSettingLoader>() == null)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = "AdsMaster Loader";
                var loader = gameObj.AddComponent<AdsMasterSettingLoader>();
                loader.settings = LibResourceUtil.LoadAndCreateSetting<AdsMasterSetting>();
                EditorSceneManager.MarkAllScenesDirty();
            }
        }
    }
}