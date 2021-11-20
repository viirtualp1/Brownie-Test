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
        if (rgb.transform.position.x < point_end.position.x)
        {
            State = States.run;
            rgb.velocity = new Vector2(speed_Booba, rgb.velocity.y);   
        }
        else
        {
            Destroy(GameObject.Find("Booba"));

            await Task.Delay(1000);

            SceneManager.LoadScene("TheEnd");
        }
    }
}
