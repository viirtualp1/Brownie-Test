using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("HallwayLargeRoom");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
