using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }
    public void LevelMenu()
    {
        SceneManager.LoadScene("Level Menu", LoadSceneMode.Single);
    }
}
