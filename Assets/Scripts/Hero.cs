using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Hero : MonoBehaviour
{
    private Animator anim;

    // Лист собранных предметов (!добавлять id предметов, чтобы потом было их легко удалить!)
    public static List<string> items;
    public static List<string> collectsCards;

    // Приватные поля
    [SerializeField] private float speed = 3f; // Скорость движения
    [SerializeField] private float jumpForce = 15f; // Сила прыжка

    // Приватные переменные
    private bool isGrounded = false;

    public GameObject cactus;
    public Sprite cactus_new;

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

    private Collider2D stickyNote;

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

	// Переменные для музыки
	// public AudioSource dayloop1;
	// public AudioSource dayloop2;
	// public AudioSource dayloop3;
	// public AudioSource dayloop4;
	// public AudioSource dayloop5;
	// public AudioSource dayloop6;
	// public AudioSource dayclockloop1;
	// public AudioSource dayclockloop2;
	// public AudioSource dayclockloop3;
	// public AudioSource dayclockloop4;
	// public AudioSource dayclockloop5;
	// public AudioSource dayclockloop6;
	// public AudioSource chill;
	// public AudioSource intromenu;
	// public AudioSource menu;
	// public AudioSource titles;
	
    public static int day = 1;
    
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

    private bool isStickyNote;
    private GameObject stickyNote_original;
    private GameObject stickyNote_duplicate;

    // Текст обучения
    public GameObject training;

    // Открыт ли стикер
    private bool isStickyNoteYes = false;

    // Sprite bad with sleep Booba
    public Sprite badWithBooba;

    private int collectsCardsCount;
    private int itemsCount;

    public GameObject stickyNote_o;

    private GameObject cardObject;

    private Collider2D wire;
    private bool isWire = false;

    private int wireCounter = 0;
    
    private string currentTaskString = "Убрать спальню";

    // Касается ли игрок инструментов
    private bool isInstruments = false;

    // Взял ли игрок отвертку
    private bool isTakeInstuments = false;

    // Касается ли игрок майнинг фермы
    private bool isMining = false;

    private Collider2D mining;

    private Collider2D light;
    private bool isLightOff = false;
    private bool isLightTrigger = false;
    public Sprite newLight;

    public Sprite bedNew;
    private bool isBed;

    private bool isBaika = false;

    public Sprite newStend;
    public Sprite newImages;
    public Sprite newWheel;
    private bool isStend = false;
    private bool isImages = false;
    private bool isWheel = false;

    private bool isSleeve;
    private bool isPuddle;

    private bool isFishing;

    private bool isEgg;
    private bool isMatches;
    private bool isSink;
    private bool isMatchesComplete = false;
    private bool isEggComplete = false;

    private bool isFertilizer;
    private bool isDichlorvos;
    private bool isScissors;
    private bool isWateringCan;
    private bool isFlowers;

    public Sprite newSink;
    public Sprite newPlush;
    public Sprite newFlowers;

    private bool isTakeFertilizer;
    private bool isTakeDichlorvos;
    private bool isTakeScissors;
    private bool isTakeWateringCan;

    private bool isPlush;

    private bool isBoobaSleep;

    public Sprite badWithNoBooba;

    private int day1Tasks = 0;

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private static bool isNullOrEmpty(string s)
    {
        return (s == null || s == String.Empty) ? true : false;
    }

    // Методы
    void Start()
    {
        getSaves();

        if (isNullOrEmpty(currentTaskString))
        {
            currentTaskString = "Убрать спальню";
            saveAndUpdateCurrentTask(currentTaskString);
        }

        for (int i = 0; i < items.Count; i++)
            Destroy(GameObject.Find(items[i]));

        if (SceneManager.GetActiveScene().name == "HomeBooba")
        {
            speed = 6f;

            for (int i = 0; i < collectsCards.Count; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    cardObject = GameObject.Find("card1 (" + j + ")");
                    cardObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        for (int i = 0; i < collectsCards.Count; i++)
        {
            if (GameObject.Find(collectsCards[i]))
                Destroy(GameObject.Find(collectsCards[i]));
        }

        try {
            if (day >= 3)
                cactus.GetComponent<SpriteRenderer>().sprite = cactus_new;
        } catch { }

        try {
            if (day < 2)
                GameObject.Find("stairsToLivingRoom").GetComponent<BoxCollider2D>().enabled = false;
        } catch { }

        try {
            if (day < 3)
                GameObject.Find("DoorToGarage").GetComponent<BoxCollider2D>().enabled = false;
        } catch { }

        try {
            if (day < 4)
                GameObject.Find("DoorToBathRoom").GetComponent<BoxCollider2D>().enabled = false;
        } catch { }

        try {
            if (day < 5)
                GameObject.Find("DoorToKitchen").GetComponent<BoxCollider2D>().enabled = false;
        } catch { }

        try {
            if (day < 6)
                GameObject.Find("DoorToRoof").GetComponent<BoxCollider2D>().enabled = false;
        } catch { }
		
		
		// if (SceneManager.GetActiveScene().name == "BedRoomScene" && day == 1)
        //     GameObject.Find("Day 1 Start").GetComponent<AudioSource>().Play();

        // GameObject.Find("Day " + day + " BG").GetComponent<AudioSource>().Play();

        GetOrDupe.GetComponent<Canvas>().enabled = false;
        training.GetComponent<Canvas>().enabled = false;
    }

    private void currentTask()
    {
        // if (day == 1)
        //     currentTaskString = "Убрать спальню";
        // else if (day == 2)
        //     currentTaskString = "Убрать гостиную";
        // else if (day == 3)
        //     currentTaskString = "Убрать гараж";
        // else if (day == 4)
        //     currentTaskString = "Убрать ванную";
        // else if (day == 5)
        //     currentTaskString = "Убрать кухню";
        // else if (day == 6)
        //     currentTaskString = "Убрать крышу";
        // else if (day == 7)
        //     currentTaskString = "Конец";

        // saveAndUpdateCurrentTask(currentTaskString);
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Если Буба стоит на земле => анимация idle
        if (isGrounded) State = States.idle;

        // Ходьба и прыжки Бубы
        if (isBoobaCanMove && Input.GetButton("Horizontal"))
            Run();
        if (isBoobaCanJump && isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();

        // Подбираем N предмет
        if (isTriggeredCollectable && isCollectable && Input.GetKeyDown(KeyCode.X))
        {
            string itemType = item.gameObject.GetComponent<CollectableScript>().itemType;
            items.Add(itemType);

            Destroy(item.gameObject);
        }

        // Подбираем карточку
        if (isCardTake && Input.GetKeyDown(KeyCode.X))
        {   
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

        // Открываем стикер с обучением
        if (!isStickyNoteYes && isStickyNote)
        {
            isStickyNoteYes = true;

            stickyNote_original = stickyNote.gameObject;
            stickyNote_duplicate = GameObject.Instantiate(stickyNote.gameObject);
            stickyNote_duplicate.transform.localScale = new Vector3(1.387061f, 1.301505f, 1);
            stickyNote_duplicate.transform.position = new Vector3(-11.49f, -4.54f, 112.7f);
            stickyNote_duplicate.GetComponent<SpriteRenderer>().color = Color.white;

            isBoobaCanMove = false;
            isBoobaCanJump = false;

            training.GetComponent<Canvas>().enabled = true;
        }

        // Закрываем стикер с обучением
        if (isStickyNote && Input.GetKeyDown(KeyCode.C))
        {
            training.GetComponent<Canvas>().enabled = false;
            Destroy(stickyNote_duplicate);
            Destroy(stickyNote_original);

            isBoobaCanMove = true;
            isBoobaCanJump = true;
            isStickyNote = false;
            isStickyNoteYes = false;

            items.Add("training-item");
            saveItems();
        }

        // Выбор: Что делать с карточкой? Выбросить, либо подобрать
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

                saveCards();
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

        // попросить помощи у Кактуса (нажав Z)
        if (isHelper && Input.GetKeyDown(KeyCode.Z))
        {
            // Проверка задания (currentTaskString)
            // TODO Озвучка кактуса
        }

        // Вход в N комнату
        if (isDoor && Input.GetKeyDown(KeyCode.X))
        {
            string nextRoom = room.gameObject.GetComponent<GoToDoor>().nextRoom;
            SceneManager.LoadScene(nextRoom);
        }

        // Выход в меню
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Menu");

        // Вход в домик Бубы
        if (isDoorBooba && Input.GetKeyDown(KeyCode.X))
            SceneManager.LoadScene("homeBooba");

        // Включение/Выключенеи телевизора
        if (isTVOff && isRCB && Input.GetKeyDown(KeyCode.X))
        {
            GameObject TVOff = GameObject.FindGameObjectWithTag("TVOff");
            AudioClip TVAudioSound = TV.gameObject.GetComponent<anySound>().sound;

            TVOff.GetComponent<SpriteRenderer>().sprite = TVOnSprite;
            TV.GetComponent<AudioSource>().clip = TVAudioSound;
            TV.GetComponent<AudioSource>().Play();

            isTVOff = false;
        } else if (!isTVOff && isRCB && Input.GetKeyDown(KeyCode.X)) 
        {
            GameObject TVOff = GameObject.FindGameObjectWithTag("TVOff");

            TVOff.GetComponent<SpriteRenderer>().sprite = TVOffSprite;
            TV.GetComponent<AudioSource>().Pause();

            isTVOff = true;
        }

        // Буба ложится спать на свою кровать
        if (isBadBooba && Input.GetKeyDown(KeyCode.X))
        {
            newDay();
            // GameObject.Find("Day " + day + " Start").GetComponent<AudioSource>().Play();

            switch (day)
            {
                case 2:
                    currentTaskString = "Убрать гостиную"; break;
                case 3:
                    currentTaskString = "Убрать гараж"; break;
                case 4:
                    currentTaskString = "Убрать ванную"; break;
                case 5:
                    currentTaskString = "Убрать кухню"; break;
                case 6:
                    currentTaskString = "Убрать крышу"; break;
                case 7:
                    currentTaskString = "Конец"; break;
            }

            saveAndUpdateCurrentTask(currentTaskString);
        }


        // Триггеры в гараже
        if (wireCounter < 4 && isWire && Input.GetKeyDown(KeyCode.X))
        {
            string provodType = wire.gameObject.GetComponent<CollectableScript>().itemType;
            GameObject wireObj = GameObject.Find(provodType);
            items.Add(provodType);
            saveItems();

            wireCounter++;
            PlayerPrefs.SetInt("wire_counter", wireCounter);

            Destroy(wireObj);
        }

        if (wireCounter == 4 && isInstruments && Input.GetKeyDown(KeyCode.X))
            isTakeInstuments = true;

        if (isLightTrigger && Input.GetKeyDown(KeyCode.X))
        {
            string lightSprite = light.gameObject.GetComponent<CollectableScript>().itemType;
            GameObject lightObj = GameObject.Find(lightSprite);
            lightObj.GetComponent<SpriteRenderer>().sprite = newLight;
            isLightOff = true;
        }

        if (isTakeInstuments && isLightOff && isMining && Input.GetKeyDown(KeyCode.X))
        {
            string miningType = mining.gameObject.GetComponent<CollectableScript>().itemType;
            GameObject miningObj = GameObject.Find(miningType);
            items.Add(miningType);
            saveItems();

            Destroy(miningObj);

            saveAndUpdateCurrentTask("Идти спать");
        }

        if (isBed && Input.GetKeyDown(KeyCode.X))
        {
            GameObject bedNow = GameObject.Find("Bed");
            bedNow.GetComponent<SpriteRenderer>().sprite = bedNew;

            if (day == 1) day1Tasks++;
        }

        if (isBaika && Input.GetKeyDown(KeyCode.X))
        {
            GameObject baika = GameObject.Find("baika");
            Destroy(baika);

            items.Add("baika");
            saveItems();

            day1Tasks++;
        }

        if (isStend && Input.GetKeyDown(KeyCode.X))
        {
            GameObject stend = GameObject.Find("stend1");
            stend.GetComponent<SpriteRenderer>().sprite = newStend;
            
            items.Add("stend1");
            saveItems();
        }

        if (isImages && Input.GetKeyDown(KeyCode.X))
        {
            GameObject images = GameObject.Find("img1");
            images.GetComponent<SpriteRenderer>().sprite = newImages;
        
            items.Add("img1");
            saveItems();
        }

        if (isWheel && Input.GetKeyDown(KeyCode.X))
        {
            GameObject wheel = GameObject.Find("wheel");
            wheel.GetComponent<SpriteRenderer>().sprite = newWheel;
        
            items.Add("wheel");
            saveItems();

            saveAndUpdateCurrentTask("Идти спать");
        }
    
        if (isPuddle && Input.GetKeyDown(KeyCode.X))
        {
            GameObject puddle = GameObject.Find("puddle");
            Destroy(puddle);
            
            items.Add("puddle");
            saveItems();
        }

        if (isSleeve && Input.GetKeyDown(KeyCode.X))
        {
            GameObject sleeve = GameObject.Find("sleeve");
            Destroy(sleeve);

            items.Add("sleeve");
            saveItems();
        }

        if (isFishing && Input.GetKeyDown(KeyCode.X))
        {
            GameObject ud = GameObject.Find("ud");
            ud.GetComponent<SpriteRenderer>().enabled = true;
            
            saveAndUpdateCurrentTask("Идти спать");
        }

        if (isEgg && Input.GetKeyDown(KeyCode.X))
        {
            GameObject egg = GameObject.Find("egg");
            Destroy(egg);

            isEggComplete = true;
        }

        if (isMatches && Input.GetKeyDown(KeyCode.X))
        {
            GameObject mat = GameObject.Find("matches");
            Destroy(mat);

            isMatchesComplete = true;
        }

        if (isMatchesComplete && isEggComplete && isSink && Input.GetKeyDown(KeyCode.X))
        {
            GameObject sink = GameObject.Find("sink");
            sink.GetComponent<SpriteRenderer>().sprite = newSink;

            saveAndUpdateCurrentTask("Идти спать");
        }

        if (isFertilizer && Input.GetKeyDown(KeyCode.X))
        {
            GameObject fertilizer = GameObject.Find("fertilizer");
            Destroy(fertilizer);
            
            items.Add("fertilizer");
            saveItems();

            isTakeFertilizer = true;
        }

        if (isDichlorvos && Input.GetKeyDown(KeyCode.X))
        {
            GameObject dichlorvos = GameObject.Find("dichlorvos");
            Destroy(dichlorvos);

            items.Add("dichlorvos");
            saveItems();

            isTakeDichlorvos = true;
        }

        if (isScissors && Input.GetKeyDown(KeyCode.X))
        {
            GameObject scissors = GameObject.Find("scissors");
            Destroy(scissors);

            items.Add("scissors");
            saveItems();

            isTakeScissors = true;
        }

        if (isWateringCan && Input.GetKeyDown(KeyCode.X))
        {
            GameObject watering_can = GameObject.Find("watering_can");
            Destroy(watering_can);

            items.Add("watering_can");
            saveItems();
            
            isTakeWateringCan = true;
        }

        if (isTakeWateringCan && isTakeScissors && isTakeDichlorvos && isTakeFertilizer && isPlush && Input.GetKeyDown(KeyCode.X))
        {  
            GameObject plush = GameObject.Find("plush");
            plush.GetComponent<SpriteRenderer>().sprite = newPlush;

            saveAndUpdateCurrentTask("Идти спать");
        } else if (isTakeWateringCan && isTakeScissors && isTakeDichlorvos && isTakeFertilizer && isFlowers && Input.GetKeyDown(KeyCode.X))
        {
            GameObject flowers = GameObject.Find("flowers");
            flowers.GetComponent<SpriteRenderer>().sprite = newFlowers;

            saveAndUpdateCurrentTask("Идти спать");
        }

        if (day == 1 && day1Tasks >= 2)
            saveAndUpdateCurrentTask("Идти спать");
    }
        

    // hide text from helper
    void Hide() { helperText.text = ""; }

    private void saveAndUpdateCurrentTask(string task)
    {
        currentTaskString = task;
        GameObject.Find("cts").GetComponent<Text>().text = currentTaskString;
        PlayerPrefs.SetString("currentTaskString", currentTaskString);
    }

    private void newDay()
    {
        day++;

        // Сохраняем день
        PlayerPrefs.SetInt("Day", day);
    }

    private void saveCards()
    {
        PlayerPrefs.SetInt("cards_counter", collectsCards.Count);
        for (int i = 0; i < collectsCards.Count; i++)
            PlayerPrefs.SetString("collectsCards_list" + i, collectsCards[i]);
    }

    private void saveItems()
    {
        PlayerPrefs.SetInt("items_counter", items.Count);
        for (int i = 0; i < items.Count; i++)
            PlayerPrefs.SetString("items_list" + i, items[i]);
    }

    public void getSaves()
    {
        // Сохраняем день
        day = PlayerPrefs.GetInt("Day");
        if (day == 0) day = 1;

        Debug.Log("Day: " + day);
            
        // Получаем собранные карточки
        collectsCardsCount = PlayerPrefs.GetInt("cards_counter");
        collectsCards = new List<string>();

        for (int i = 0; i < collectsCardsCount; i++)
            collectsCards.Add(PlayerPrefs.GetString("collectsCards_list" + i));

        Debug.Log("Cards: " + collectsCards.Count);

        // Получаем собранные предметы
        itemsCount = PlayerPrefs.GetInt("items_counter");
        items = new List<string>();

        for (int i = 0; i < itemsCount; i++)
            items.Add(PlayerPrefs.GetString("items_list" + i));

        Debug.Log("Items: " + items.Count);

        // Получаем текущее заданий
        currentTaskString = PlayerPrefs.GetString("currentTaskString");
        saveAndUpdateCurrentTask(currentTaskString);
        Debug.Log("Текущее задание: " + currentTaskString);

        // Получаем собранное число проводов
        wireCounter = PlayerPrefs.GetInt("wire_counter");
    }

    private void Run()
    {
        if (isGrounded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        isGrounded = false;
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

        if (!isGrounded) State = States.jump;
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

        if (collision.CompareTag("card")) { State = States.loot; isCardTake = true; cardTake = collision; }

        if (collision.CompareTag("badBooba")) { isBadBooba = true; }

        if (collision.CompareTag("RCB")) { TV = collision; isRCB = true; }

        if (collision.CompareTag("training")) { isStickyNote = true; stickyNote = collision; }
        
        if (collision.CompareTag("wires")) { isWire = true; wire = collision; }

        if (collision.CompareTag("instruments")) { isInstruments = true; }

        if (collision.CompareTag("mining")) { isMining = true; mining = collision; }

        if (collision.CompareTag("light")) { isLightTrigger = true; light = collision; }

        if (collision.CompareTag("bed")) { isBed = true; }

        if (collision.CompareTag("baika")) { isBaika = true; }

        if (collision.CompareTag("images")) { isImages = true; }
        if (collision.CompareTag("stend")) { isStend = true; }

        if (collision.CompareTag("puddle")) { isPuddle = true; }
        if (collision.CompareTag("sleeve")) { isSleeve = true; }
        if (collision.CompareTag("fishing_rod")) { isFishing = true; }
        
        if (collision.CompareTag("matches")) { isMatches = true; }
        if (collision.CompareTag("sink")) { isSink = true; }
        if (collision.CompareTag("egg")) { isEgg = true; }

        if (collision.CompareTag("fertilizer")) { isFertilizer = true; }
        if (collision.CompareTag("dichlorvos")) { isDichlorvos = true; }
        if (collision.CompareTag("scissors")) { isScissors = true; }
        if (collision.CompareTag("watering_can")) { isWateringCan = true; }

        if (collision.CompareTag("flowers")) { isFlowers = true; }
        if (collision.CompareTag("plush")) { isPlush = true; }
    
        if (collision.CompareTag("ground")) { isGrounded = true; }
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
        isBoobaCanMove = true;
        isBoobaCanJump = true;
        isWire = false;
        wire = null;
        isInstruments = false;
        isMining = false;
        mining = null;
        isLightOff = false;
        isMatches = false;
        isSink = false;
        isEgg = false;
        isFlowers = false;
        isPlush = false;
        isDichlorvos = false;
        isScissors = false;
        isWateringCan = false;
    }
}

public enum States
{
    idle,
    run,
    jump,
    loot,
    craft
}