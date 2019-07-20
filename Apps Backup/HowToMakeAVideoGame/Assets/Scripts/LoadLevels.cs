using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevels : MonoBehaviour
{

    public void Level01()
    {
         SceneManager.LoadScene("Level01", LoadSceneMode.Single);
    }

    public void Level02()
    {
        SceneManager.LoadScene("Level02", LoadSceneMode.Single);
    }

    public void Level03()
    {
        SceneManager.LoadScene("Level03", LoadSceneMode.Single);
    }

    public void Level04()
    {
        SceneManager.LoadScene("Level04", LoadSceneMode.Single);
    }

    public void Level05()
    {
        SceneManager.LoadScene("Level05", LoadSceneMode.Single);
    }

    public void Level06()
    {
        SceneManager.LoadScene("Level06", LoadSceneMode.Single);
    }
}
