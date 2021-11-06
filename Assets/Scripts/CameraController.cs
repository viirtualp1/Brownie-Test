using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;
    private Camera cam;
    private int camSpeed = 7;

    private void Awake()
    {
        if (!player)
            player = FindObjectOfType<Hero>().transform;
    }

    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        pos = player.position;
        pos.z = -10f;
        
        cam.transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * camSpeed);
    }
}
