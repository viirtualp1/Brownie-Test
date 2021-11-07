using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("BedRoomScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
