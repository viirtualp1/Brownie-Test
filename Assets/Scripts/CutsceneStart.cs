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

    private StatesLeather State
    {
        get { return (StatesLeather)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int)value); }
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
            State = StatesLeather.run;
            rgb.velocity = new Vector2(speed_leather, rgb.velocity.y);   
        }
        else
        {
            vcam.Follow = booba;
            Destroy(GameObject.Find("Leather"));
            GameObject.Find("booba-sprite").GetComponent<SpriteRenderer>().enabled = true;

            await Task.Delay(1000);

            SceneManager.LoadScene("BedRoomScene");
        }
    }
}

public enum StatesLeather
{
    idle,
    run
}