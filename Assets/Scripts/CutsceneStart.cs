using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class CutsceneStart : MonoBehaviour
{
    private int speed_leather = 3;
    public Transform point_end;
    private Rigidbody2D rgb;
    private Animator anim;
    public Transform booba;
    public CinemachineVirtualCamera vcam;

    private int SQ = 0;

    private StatesLeather State
    {
        get { return (StatesLeather)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
        
    }

    private void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        GameObject.Find("Day-Start").GetComponent<AudioSource>().Play();
    }
    
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene("BedRoomScene");
        
        if (rgb.transform.position.x < point_end.position.x)
        {
            State = StatesLeather.run;
            rgb.velocity = new Vector2(speed_leather, rgb.velocity.y);
        }
        else
        {
            vcam.Follow = booba;
            Destroy(GameObject.Find("Leather"));

            // Subsequence (voice) Start
            if (SQ == 0)
            {
                GameObject.Find("SubseQuence").GetComponent<AudioSource>().Play();
                SQ = 1;
            }

            GameObject.Find("booba-sprite").GetComponent<SpriteRenderer>().enabled = true;

            await Task.Delay(28000);

            SceneManager.LoadScene("BedRoomScene");
        }
    }
}

public enum StatesLeather
{
    idle,
    run
}