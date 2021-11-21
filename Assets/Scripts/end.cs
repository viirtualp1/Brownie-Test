using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class end : MonoBehaviour
{
    // Start is called before the first frame update
    private async void Start()
    {
        GameObject.Find("K-7-1").GetComponent<AudioSource>().Play();
        GameObject.Find("B-7-5").GetComponent<AudioSource>().PlayDelayed(6f);
        GameObject.Find("B-7-6").GetComponent<AudioSource>().PlayDelayed(10f);

        await Task.Delay(13000);
        SceneManager.LoadScene("Titles");
    }
}
