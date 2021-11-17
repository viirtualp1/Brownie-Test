using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInGame : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject menuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        menuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        menuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Exit()
    {
        Application.Quit();
    } 

    public void Settings() {}
}
