using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour
{
    public static List<string> items;
    public static List<string> collectsCards;

    // Приватные поля
    [SerializeField] private float speed = 3f; // Скорость движения
    [SerializeField] private float jumpForce = 15f; // Сила прыжка

    // Приватные переменные
    private bool isGrounded = false;

    // Ссылки
    private Rigidbody2D rb;
    public SpriteRenderer sprite;

    public static Hero Instance { get; set; }

    // Получаем текст кактуса
    public TMP_Text helperText;

    // Триггер
    private bool isTriggeredCollectable = false;

    // Подбираемый ли предмет
    private bool isCollectable = false;

    // Получаем коллайдер предмета (который хочет подобрать игрок)
    private Collider2D item;

    // Получаем коллайдер карточки (которую игрок хочет подобрать)
    private Collider2D cardTake;

    // Получаем карточку, у которой нам надо сменить цвет
    private string card;

    // Получаем комнату (куда ведет дверь) 
    private Collider2D room;

    // Триггерится ли игрок с кактусом
    private bool isHelper;

    // Триггерится ли игрок с дверью
    private bool isDoor = false;

    // Триггерится ли игрок с дверю дома Бубы
    private bool isDoorBooba = false;

    // Триггерится ли игрок с кроватью Бубы
    private bool isBadBooba = false;
    
    // Триггерится ли игрок с карточкой
    private bool isCardTake = false;

    private float time = 2f;

    string[] tasks = { "Собрать носки", "Вынести мусор" };
    private int counterTask1 = 0;

    private int day = 1;
    public Sprite TVOnSprite;
    public Sprite TVOffSprite;
    private bool isRCB = false;
    private Collider2D TV;
    private bool isTVOff = true;

    private bool isBoobaCanMove = true;
    private bool isBoobaCanJump = true;

    public GameObject GetOrDupe;

    private GameObject card_duplicate;

    private bool isChooseCardTakeOrDupe = false;

    private GameObject card_original;


    // Методы
    void Start()
    {
        GetOrDupe.GetComponent<Canvas>().enabled = false;
        items = new List<string>();
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isBoobaCanMove && Input.GetButton("Horizontal"))
            Run();
        if (isBoobaCanJump && isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Triggered for sock
        if (isTriggeredCollectable && isCollectable && Input.GetKeyDown(KeyCode.X))
        {
            string itemType = item.gameObject.GetComponent<CollectableScript>().itemType;
            items.Add(itemType);

            Destroy(item.gameObject);
        }

        // Triggered for carts
        if (isCardTake && Input.GetKeyDown(KeyCode.X))
        {   
            Debug.Log("test?");
            isCardTake = false;
            isChooseCardTakeOrDupe = true;

            card_original = cardTake.gameObject;
            card_duplicate = GameObject.Instantiate(cardTake.gameObject);
            card_duplicate.transform.localScale = new Vector3(0.2527358f, 0.2373716f, 1);
            card_duplicate.transform.position = new Vector3(-3.45f, -3.37f, -2.29f);
            card_duplicate.GetComponent<SpriteRenderer>().color = Color.white;
            
            isBoobaCanMove = false;
            isBoobaCanJump = false;

            GetOrDupe.GetComponent<Canvas>().enabled = true;
        }

        if (isChooseCardTakeOrDupe)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                string cardType = cardTake.gameObject.GetComponent<CollectableScript>().itemType;
                collectsCards = new List<string>();
                collectsCards.Add(cardType);
                
                Destroy(card_original);
                Destroy(card_duplicate);
                isCardTake = false;

                GetOrDupe.GetComponent<Canvas>().enabled = false;
                isBoobaCanMove = true;
                isBoobaCanJump = true;
                isChooseCardTakeOrDupe = false;
            } 
            
            if (Input.GetKeyDown(KeyCode.C)) 
            {
                GetOrDupe.GetComponent<Canvas>().enabled = false;
                Destroy(card_duplicate);

                isBoobaCanMove = true;
                isBoobaCanJump = true;
                isChooseCardTakeOrDupe = false;
            }
        }

        // ask helper cactus (key press G) say about task1
        if (isHelper && Input.GetKeyDown(KeyCode.Z))
        {
            helperText.text = "Здарова, Леопольд";
            Invoke("Hide", time);
        }

        if (isDoor && Input.GetKeyDown(KeyCode.X))
        {
            string nextRoom = room.gameObject.GetComponent<GoToDoor>().nextRoom;
            SceneManager.LoadScene(nextRoom);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Menu");

        if (isDoorBooba && Input.GetKeyDown(KeyCode.X))
            SceneManager.LoadScene("homeBooba");

        if (isTVOff && isRCB && day == 1 && Input.GetKeyDown(KeyCode.X))
        {
            GameObject TVOff = GameObject.FindGameObjectWithTag("TVOff");
            AudioClip TVAudioSound = TV.gameObject.GetComponent<anySound>().sound;

            TVOff.GetComponent<SpriteRenderer>().sprite = TVOnSprite;
            TV.GetComponent<AudioSource>().clip = TVAudioSound;
            TV.GetComponent<AudioSource>().Play();

            isTVOff = false;
        } else if (!isTVOff && isRCB && day == 1 && Input.GetKeyDown(KeyCode.X)) 
        {
            GameObject TVOff = GameObject.FindGameObjectWithTag("TVOff");

            TVOff.GetComponent<SpriteRenderer>().sprite = TVOffSprite;
            TV.GetComponent<AudioSource>().Pause();

            isTVOff = true;
        }

        // TODO: camera fade out
        // if (isBadBooba && Input.GetKeyDown(KeyCode.X))
        // { 
            // cam.GetComponent<CameraFade>.Out();
        // }
    }

    // hide text from helper
    void Hide() { helperText.text = ""; }

    private void Run()
    {
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            item = collision;
            isCollectable = true;
            isTriggeredCollectable = true;
        }

        if (collision.CompareTag("door"))
        {
            room = collision;
            isDoor = true;
        }

        if (collision.CompareTag("helper")) { isHelper = true; }

        if (collision.CompareTag("homeBooba")) { isDoorBooba = true; }

        if (collision.CompareTag("card")) { isCardTake = true; cardTake = collision; }

        if (collision.CompareTag("badBooba")) { isBadBooba = true; }

        if (collision.CompareTag("RCB")) { TV = collision; isRCB = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isHelper = false;
        item = null;
        isCollectable = false;
        isTriggeredCollectable = false;
        isDoor = false;
        isDoorBooba = false;
        isBadBooba = false;
        isCardTake = false;
        cardTake = null;
        isRCB = false;
        TV = null;
    }
}
