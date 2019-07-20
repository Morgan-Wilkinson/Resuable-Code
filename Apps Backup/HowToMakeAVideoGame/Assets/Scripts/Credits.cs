using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void BacktoMenu ()
    {
        Debug.Log("Back");
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

}
