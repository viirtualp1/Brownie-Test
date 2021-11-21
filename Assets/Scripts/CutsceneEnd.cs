using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class CutsceneEnd : MonoBehaviour
{
    private int speed_Booba = 3;
    public Transform point_end;
    private Rigidbody2D rgb;
    private Animator anim;
    public Transform booba;

    private int EQ = 0;

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        rgb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private async void Update()
    {

        // Subsequence (voice) Start
        if (EQ == 0)
        {
            GameObject.Find("B-7-1").GetComponent<AudioSource>().Play();
            GameObject.Find("B-7-2").GetComponent<AudioSource>().PlayDelayed(3f);
            GameObject.Find("B-7-3").GetComponent<AudioSource>().PlayDelayed(8f);
            GameObject.Find("B-7-4").GetComponent<AudioSource>().PlayDelayed(16f);

            //GameObject.Find("K-7-1").GetComponent<AudioSource>().PlayDelayed(21f);
            //GameObject.Find("B-7-5").GetComponent<AudioSource>().PlayDelayed(26f);
            //GameObject.Find("B-7-6").GetComponent<AudioSource>().PlayDelayed(35f);

            EQ = 1;
        }

        if (rgb.transform.position.x < point_end.position.x)
        {
            State = States.run;
            rgb.velocity = new Vector2(speed_Booba, rgb.velocity.y);
        }
        else
        {
            Destroy(GameObject.Find("Booba"));

            await Task.Delay(5000);

            SceneManager.LoadScene("TheEnd");
        }

        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("TheEnd");
    }
}
