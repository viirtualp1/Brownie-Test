using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titles : MonoBehaviour
{
    public void ExitTitles()
    {
        SceneManager.LoadScene("Menu");
    }
}
