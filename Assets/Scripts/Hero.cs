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
    public static float speed = 3f; // Скорость движения
    public float jumpForce = 15f; // Сила прыжка

    // Приватные переменные
    private bool isGrounded = false;

    // Ссылки
    private Rigidbody2D rb;
    public SpriteRenderer sprite;

    public static Hero Instance { get; set; }

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
	
    public static int day = 1;
    
    // public Sprite TVOnSprite;
    // public Sprite TVOffSprite;
    private bool isRCB = false;
    private Collider2D TV;
    private bool isTVOff = true;

    public static bool isBoobaCanMove = true;
    public static bool isBoobaCanJump = true;

    // public GameObject GetOrDupe;
    // private GameObject card_duplicate;
    // private bool isChooseCardTakeOrDupe = false;
    // private GameObject card_original;

    private bool isStickyNote;
    // private GameObject stickyNote_original;
    // private GameObject stickyNote_duplicate;

    // Текст обучения
    // public GameObject training;

    // Открыт ли стикер
    private bool isStickyNoteYes = false;

    // Sprite bad with sleep Booba
    // public Sprite badWithBooba;

    private int collectsCardsCount;
    private int itemsCount;

    // public GameObject stickyNote_o;

    // private GameObject cardObject;

    private Collider2D wire;
    private bool isWire = false;

    private int wireCounter = 0;
    
    private string currentTaskString = "Убрать спальню и большую прихожую";

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

    // public Sprite bedNew;
    private bool isBed;
    private bool isBad;

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

    // private bool isBoobaSleep;

    // public Sprite badWithNoBooba;

    private int day1Tasks = 0;
    private int day2Tasks = 0;
    private int day3Tasks = 0;
    private int day4Tasks = 0;
    private int day5Tasks = 0;
    private int day6Tasks = 0;
    private int day7Tasks = 0;

    private SpriteRenderer stickyNoteSR;

    private GameObject TriggerItems;

    private homeBooba scriptHomeBooba;

    private bool isItemDay1;
    private Collider2D itemDay1;

    private bool isItemDay2;
    private Collider2D itemDay2;
    
    private bool isItemDay3;
    private Collider2D itemDay3;

    private bool isItemDay4;
    private Collider2D itemDay4;

    private bool isItemDay5;
    private Collider2D itemDay5;

    private bool isItemDay6;
    private Collider2D itemDay6;

    private bool isItemDay7;
    private Collider2D itemDay7;

    private bool isItemDay8;
    private Collider2D itemDay8;

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
            currentTaskString = "Убрать спальню и большую прихожую";
            saveAndUpdateCurrentTask(currentTaskString);
        }

        for (int i = 0; i < items.Count; i++)
            Destroy(GameObject.Find(items[i]));

        if (SceneManager.GetActiveScene().name == "HomeBooba")
        {
            scriptHomeBooba = GameObject.Find("Script").GetComponent<homeBooba>();

            speed = 6f;
            scriptHomeBooba.showCards();
        } else speed = 3f;

        for (int i = 0; i < collectsCards.Count; i++)
        {
            if (GameObject.Find(collectsCards[i]))
                Destroy(GameObject.Find(collectsCards[i]));
        }

        try {
            // if (day >= 3)
                // cactus.GetComponent<SpriteRenderer>().sprite = cactus_new;
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

        //day = 6;

        // BG Music for BedRoomScene and HomeBooba
		if (SceneManager.GetActiveScene().name == "BedRoomScene" && day < 7)
        {
            Debug.Log(day);
            GameObject.Find("Day-start").GetComponent<AudioSource>().Play();
        }

        if (SceneManager.GetActiveScene().name == "HomeBooba" && day < 7)
        {
            Debug.Log(day);
            GameObject.Find("Chill").GetComponent<AudioSource>().Play();
        }

        // BG Music 6 day for homes not BedRoomScene and HomeBooba

        if (SceneManager.GetActiveScene().name != "BedRoomScene" && SceneManager.GetActiveScene().name != "HomeBooba")
        {
            Debug.Log(day);
            GameObject.Find("Day " + day + " BG").GetComponent<AudioSource>().Play();
        }

        // if (SceneManager.GetActiveScene().name != "BedRoomScene" && SceneManager.GetActiveScene().name != "HomeBooba" && day == 1)
        // {
        //     Debug.Log(day);
        //     GameObject.Find("Day 1 BG").GetComponent<AudioSource>().Play();
        // }

        // if (SceneManager.GetActiveScene().name != "BedRoomScene" && SceneManager.GetActiveScene().name != "HomeBooba" && day == 2)
        // {
        //     Debug.Log(day);
        //     GameObject.Find("Day 2 BG").GetComponent<AudioSource>().Play();
        // }

        // if (SceneManager.GetActiveScene().name != "BedRoomScene" && SceneManager.GetActiveScene().name != "HomeBooba" && day == 3)
        // {
        //     Debug.Log(day);
        //     GameObject.Find("Day 3 BG").GetComponent<AudioSource>().Play();
        // }

        // if (SceneManager.GetActiveScene().name != "BedRoomScene" && SceneManager.GetActiveScene().name != "HomeBooba" && day == 4)
        // {
        //     Debug.Log(day);
        //     GameObject.Find("Day 4 BG").GetComponent<AudioSource>().Play();
        // }

        // if (SceneManager.GetActiveScene().name != "BedRoomScene" && SceneManager.GetActiveScene().name != "HomeBooba" && day == 5)
        // {
        //     Debug.Log(day);
        //     GameObject.Find("Day 5 BG").GetComponent<AudioSource>().Play();
        // }

        // if (SceneManager.GetActiveScene().name != "BedRoomScene" && SceneManager.GetActiveScene().name != "HomeBooba" && day == 6)
        // {
        //     Debug.Log(day);
        //     GameObject.Find("Day 6 BG").GetComponent<AudioSource>().Play();
        // }


        // GameObject.Find("Day " + day + " BG").GetComponent<AudioSource>().Play();

        // GetOrDupe.GetComponent<Canvas>().enabled = false;

        try {
            stickyNoteSR = GameObject.Find("stickyNote").GetComponent<SpriteRenderer>();
        } catch {}

        TriggerItems = GameObject.Find("TriggerItems");
    }

    private void currentTask()
    {
        // if (day == 1)
        //     currentTaskString = "Убрать спальню и большую прихожую";
        // else if (day == 3)
        //     currentTaskString = "Убрать гостиную";
        // else if (day == 4)
        //     currentTaskString = "Убрать гараж";
        // else if (day == 5)
        //     currentTaskString = "Убрать ванную";
        // else if (day == 6)
        //     currentTaskString = "Убрать кухню";
        // else if (day == 7)
        //     currentTaskString = "Убрать крышу";

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
            string cardType = cardTake.gameObject.GetComponent<CollectableScript>().itemType;
            collectsCards.Add(cardType);
            
            isBoobaCanMove = true;
            isBoobaCanJump = true;
            isCardTake = false;

            Destroy(GameObject.FindGameObjectWithTag("card"));
            saveCards();
        }

        // Открываем стикер с обучением
        if (!isStickyNoteYes && isStickyNote)
        {
            isStickyNoteYes = true;

            stickyNoteSR.enabled = true;
            isBoobaCanMove = false;
            isBoobaCanJump = false;

            TriggerItems.GetComponent<BedRoom>().Training.GetComponent<Canvas>().enabled = true;
        }

        // Закрываем стикер с обучением
        if (isStickyNote && Input.GetKeyDown(KeyCode.C))
        {
            TriggerItems.GetComponent<BedRoom>().Training.GetComponent<Canvas>().enabled = false;
            stickyNoteSR.enabled = false;

            Destroy(TriggerItems.GetComponent<BedRoom>().stickyNote);

            isBoobaCanMove = true;
            isBoobaCanJump = true;
            isStickyNote = false;
            isStickyNoteYes = false;

            items.Add("training-item");
            saveItems();
        }

        // Выбор: Что делать с карточкой? Выбросить, либо подобрать
        // if (isChooseCardTakeOrDupe)
        // {
        //     if (Input.GetKeyDown(KeyCode.F))
        //     {
        //         string cardType = cardTake.gameObject.GetComponent<CollectableScript>().itemType;
        //         collectsCards = new List<string>();
        //         collectsCards.Add(cardType);
                
        //         Destroy(card_original);
        //         Destroy(card_duplicate);
        //         isCardTake = false;

        //         GetOrDupe.GetComponent<Canvas>().enabled = false;
        //         isBoobaCanMove = true;
        //         isBoobaCanJump = true;
        //         isChooseCardTakeOrDupe = false;

        //         saveCards();
        //     } 
            
        //     if (Input.GetKeyDown(KeyCode.C)) 
        //     {
        //         GetOrDupe.GetComponent<Canvas>().enabled = false;
        //         Destroy(card_duplicate);

        //         isBoobaCanMove = true;
        //         isBoobaCanJump = true;
        //         isChooseCardTakeOrDupe = false;
        //     }
        // }

        // попросить помощи у Кактуса (нажав Z)
        if (isHelper && Input.GetKeyDown(KeyCode.Z))
        {
            // Проверка задания (currentTaskString)
            // TODO Озвучка кактуса

            // Озвучка Deg

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
            GameObject TV = GameObject.Find("TV");
            GameObject RC = GameObject.Find("RC");
            AudioClip TVAudioSound = RC.gameObject.GetComponent<anySound>().sound;

            TV.GetComponent<SpriteRenderer>().sprite = RC.gameObject.GetComponent<LargeRoom>().TVOn;
            RC.GetComponent<AudioSource>().clip = TVAudioSound;
            RC.GetComponent<AudioSource>().Play();

            isTVOff = false;
        } else if (!isTVOff && isRCB && Input.GetKeyDown(KeyCode.X)) 
        {
            GameObject TV = GameObject.Find("TV");
            GameObject RC = GameObject.Find("RC");
            AudioClip TVAudioSound = RC.gameObject.GetComponent<anySound>().sound;

            TV.GetComponent<SpriteRenderer>().sprite = RC.gameObject.GetComponent<LargeRoom>().TVOff;
            RC.GetComponent<AudioSource>().clip = TVAudioSound;
            RC.GetComponent<AudioSource>().Stop();

            isTVOff = true;
        }

        // Буба ложится спать на свою кровать
        if (isBadBooba && Input.GetKeyDown(KeyCode.X))
        {
            newDay();

            // GameObject.Find("Day " + day + " Start").GetComponent<AudioSource>().Play();

            scriptHomeBooba.BoobaSleep();

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
            }

            saveAndUpdateCurrentTask(currentTaskString);
        }


        // Триггеры в гараже
        // if (wireCounter < 4 && isWire && Input.GetKeyDown(KeyCode.X))
        // {
        //     string provodType = wire.gameObject.GetComponent<CollectableScript>().itemType;
        //     GameObject wireObj = GameObject.Find(provodType);
        //     items.Add(provodType);
        //     saveItems();

        //     wireCounter++;
        //     PlayerPrefs.SetInt("wire_counter", wireCounter);

        //     Destroy(wireObj);
        // }

        // if (wireCounter == 4 && isInstruments && Input.GetKeyDown(KeyCode.X))
        //     isTakeInstuments = true;

        // if (isLightTrigger && Input.GetKeyDown(KeyCode.X))
        // {
        //     string lightSprite = GetComponent<Light>().gameObject.GetComponent<CollectableScript>().itemType;
        //     GameObject lightObj = GameObject.Find(lightSprite);
        //     lightObj.GetComponent<SpriteRenderer>().sprite = newLight;
        //     isLightOff = true;
        // }

        // if (isTakeInstuments && isLightOff && isMining && Input.GetKeyDown(KeyCode.X))
        // {
        //     string miningType = mining.gameObject.GetComponent<CollectableScript>().itemType;
        //     GameObject miningObj = GameObject.Find(miningType);
        //     items.Add(miningType);
        //     saveItems();

        //     Destroy(miningObj);

        //     saveAndUpdateCurrentTask("Идти спать");
        // }

        // if (isBed)
        // {
        //     Debug.Log(isBed);
        //     GameObject.Find("B-1-6").GetComponent<AudioSource>().PlayDelayed();
        // }

        if (isBed && Input.GetKeyDown(KeyCode.X))
        {   
            GameObject.Find("B-1-6").GetComponent<AudioSource>().Play();
            GameObject bedNow = GameObject.Find("Bed");
            bedNow.GetComponent<SpriteRenderer>().sprite = TriggerItems.GetComponent<BedRoom>().BedNew;
             GameObject.Find("B-1-7").GetComponent<AudioSource>().PlayDelayed(2f);

            if (day == 1) day1Tasks++;

            saveCountersTasks();
        }

        // if (isBaika && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject baika = GameObject.Find("baika");
        //     Destroy(baika);

        //     items.Add("baika");
        //     saveItems();

        //     day1Tasks++;

        //     Debug.Log(day1Tasks);
        // }

        // if (isStend && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject stend = GameObject.Find("stend1");
        //     stend.GetComponent<SpriteRenderer>().sprite = newStend;
            
        //     items.Add("stend1");
        //     saveItems();
        // }

        // if (isImages && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject images = GameObject.Find("img1");
        //     images.GetComponent<SpriteRenderer>().sprite = newImages;
        
        //     items.Add("img1");
        //     saveItems();
        // }

        // if (isWheel && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject wheel = GameObject.Find("wheel");
        //     wheel.GetComponent<SpriteRenderer>().sprite = newWheel;
        
        //     items.Add("wheel");
        //     saveItems();

        //     saveAndUpdateCurrentTask("Идти спать");
        // }
    
        // if (isPuddle && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject puddle = GameObject.Find("puddle");
        //     Destroy(puddle);
            
        //     items.Add("puddle");
        //     saveItems();
        // }

        // if (isSleeve && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject sleeve = GameObject.Find("sleeve");
        //     Destroy(sleeve);

        //     items.Add("sleeve");
        //     saveItems();
        // }

        // if (isFishing && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject ud = GameObject.Find("ud");
        //     ud.GetComponent<SpriteRenderer>().enabled = true;
            
        //     saveAndUpdateCurrentTask("Идти спать");
        // }

        // if (isEgg && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject egg = GameObject.Find("egg");
        //     Destroy(egg);

        //     isEggComplete = true;
        // }

        // if (isMatches && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject mat = GameObject.Find("matches");
        //     Destroy(mat);

        //     isMatchesComplete = true;
        // }

        // if (isMatchesComplete && isEggComplete && isSink && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject sink = GameObject.Find("sink");
        //     sink.GetComponent<SpriteRenderer>().sprite = newSink;

        //     saveAndUpdateCurrentTask("Идти спать");
        // }

        // if (isFertilizer && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject fertilizer = GameObject.Find("fertilizer");
        //     Destroy(fertilizer);
            
        //     items.Add("fertilizer");
        //     saveItems();

        //     isTakeFertilizer = true;
        // }

        // if (isDichlorvos && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject dichlorvos = GameObject.Find("dichlorvos");
        //     Destroy(dichlorvos);

        //     items.Add("dichlorvos");
        //     saveItems();

        //     isTakeDichlorvos = true;
        // }

        // if (isScissors && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject scissors = GameObject.Find("scissors");
        //     Destroy(scissors);

        //     items.Add("scissors");
        //     saveItems();

        //     isTakeScissors = true;
        // }

        // if (isWateringCan && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject watering_can = GameObject.Find("watering_can");
        //     Destroy(watering_can);

        //     items.Add("watering_can");
        //     saveItems();
            
        //     isTakeWateringCan = true;
        // }

        // if (isTakeWateringCan && isTakeScissors && isTakeDichlorvos && isTakeFertilizer && isPlush && Input.GetKeyDown(KeyCode.X))
        // {  
        //     GameObject plush = GameObject.Find("plush");
        //     plush.GetComponent<SpriteRenderer>().sprite = newPlush;

        //     saveAndUpdateCurrentTask("Идти спать");
        // } else if (isTakeWateringCan && isTakeScissors && isTakeDichlorvos && isTakeFertilizer && isFlowers && Input.GetKeyDown(KeyCode.X))
        // {
        //     GameObject flowers = GameObject.Find("flowers");
        //     flowers.GetComponent<SpriteRenderer>().sprite = newFlowers;

        //     saveAndUpdateCurrentTask("Идти спать");
        // }

        if (isImages && Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("img1").GetComponent<SpriteRenderer>().sprite = GameObject.Find("Script").GetComponent<livingRoom>().new_images;
            day2Tasks++;

            saveCountersTasks();
        }

        if (isStend && Input.GetKeyDown(KeyCode.X))
        {   
            GameObject.Find("stend1").GetComponent<SpriteRenderer>().sprite = GameObject.Find("Script").GetComponent<livingRoom>().new_stend;
            day2Tasks++;

            saveCountersTasks();
        }

        if (isLightTrigger && Input.GetKeyDown(KeyCode.X))
        {
            isLightOff = true;
            GameObject.Find("Shitok1").GetComponent<SpriteRenderer>().sprite = GameObject.Find("Script").GetComponent<Garage>().new_light;
            saveCountersTasks();
        }

        if (day1Tasks < 9 && isItemDay1 && Input.GetKeyDown(KeyCode.X))
        {
            string itemHW = itemDay1.GetComponent<CollectableScript>().itemType;
            items.Add(itemHW);
            saveItems();

            Destroy(GameObject.Find(itemHW));
            day1Tasks++;
            saveCountersTasks();
        } else if (day2Tasks < 3 && isItemDay2 && Input.GetKeyDown(KeyCode.X))
        {
            string itemHW = itemDay2.GetComponent<CollectableScript>().itemType;
            items.Add(itemHW);
            saveItems();

            Destroy(GameObject.Find(itemHW));
            day2Tasks++;
            saveCountersTasks();
        } else if (day3Tasks < 7 && isItemDay3 && Input.GetKeyDown(KeyCode.X))
        {
            string itemHW = itemDay3.GetComponent<CollectableScript>().itemType;

            if (itemHW == "Instruments")
                isTakeInstuments = true;
            if (itemHW == "Fermas")
                if (isTakeInstuments && isLightOff) {
                    items.Add(itemHW);
                    saveItems();

                    Destroy(GameObject.Find(itemHW));
                    day3Tasks++;
                    saveCountersTasks();
                }
            else {
                items.Add(itemHW);
                saveItems();

                Destroy(GameObject.Find(itemHW));
                day3Tasks++;
                saveCountersTasks();
            }
        }
        
        if (day1Tasks >= 9 && day == 1) saveAndUpdateCurrentTask("Идти спать");
        if (day2Tasks >= 3 && day == 2) saveAndUpdateCurrentTask("Идти спать");
        if (day3Tasks >= 7 && day == 3) saveAndUpdateCurrentTask("Идти спать");
    }


    // hide text from helper
    // void Hide() { helperText.text = ""; }

    private void saveAndUpdateCurrentTask(string task)
    {
        Debug.Log(task);

        currentTaskString = task;
        GameObject.Find("cts").GetComponent<Text>().text = currentTaskString;
        PlayerPrefs.SetString("currentTaskString", currentTaskString);
    }

    private void saveCountersTasks()
    {
        PlayerPrefs.SetInt("day1Tasks", day1Tasks);
        PlayerPrefs.SetInt("day2Tasks", day2Tasks);
        PlayerPrefs.SetInt("day3Tasks", day3Tasks);
        PlayerPrefs.SetInt("day4Tasks", day4Tasks);
        PlayerPrefs.SetInt("day5Tasks", day5Tasks);
        PlayerPrefs.SetInt("day6Tasks", day6Tasks);
        PlayerPrefs.SetInt("day7Tasks", day7Tasks);
    }

    private void newDay()
    {
        day++;

        // Обновляем день в Text объекте (для UX)
        GameObject.Find("cd").GetComponent<Text>().text = day.ToString();

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

        GameObject.Find("cd").GetComponent<Text>().text = day.ToString();

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

        day1Tasks = PlayerPrefs.GetInt("day1Tasks");
        day2Tasks = PlayerPrefs.GetInt("day2Tasks");
        day3Tasks = PlayerPrefs.GetInt("day3Tasks");
        day4Tasks = PlayerPrefs.GetInt("day4Tasks");
        day5Tasks = PlayerPrefs.GetInt("day5Tasks");
        day6Tasks = PlayerPrefs.GetInt("day6Tasks");
        day7Tasks = PlayerPrefs.GetInt("day7Tasks");
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("door"))
        {
            room = collision;
            isDoor = true;
        }

        if (collision.CompareTag("images")) { isImages = true; }
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

        if (collision.CompareTag("training")) { isStickyNote = true; stickyNote = collision; }
        
        // if (collision.CompareTag("wires")) { isWire = true; wire = collision; }

        // if (collision.CompareTag("instruments")) { isInstruments = true; }

        // if (collision.CompareTag("mining")) { isMining = true; mining = collision; }

        if (collision.CompareTag("light")) { isLightTrigger = true; light = collision; }

        if (collision.CompareTag("bed")) { isBed = true; }

        // if (collision.CompareTag("baika")) { isBaika = true; }

        // if (collision.CompareTag("puddle")) { isPuddle = true; }
        // if (collision.CompareTag("sleeve")) { isSleeve = true; }
        // if (collision.CompareTag("fishing_rod")) { isFishing = true; }
        
        // if (collision.CompareTag("matches")) { isMatches = true; }
        // if (collision.CompareTag("sink")) { isSink = true; }
        // if (collision.CompareTag("egg")) { isEgg = true; }

        // if (collision.CompareTag("fertilizer")) { isFertilizer = true; }
        // if (collision.CompareTag("dichlorvos")) { isDichlorvos = true; }
        // if (collision.CompareTag("scissors")) { isScissors = true; }
        // if (collision.CompareTag("watering_can")) { isWateringCan = true; }

        // if (collision.CompareTag("flowers")) { isFlowers = true; }
        // if (collision.CompareTag("plush")) { isPlush = true; }

        if (collision.CompareTag("stend")) { isStend = true; }

        if (collision.CompareTag("ground")) { isGrounded = true; }
    
        if (collision.CompareTag("day1_items")) { itemDay1 = collision; isItemDay1 = true; }
        if (collision.CompareTag("day2_items")) { itemDay2 = collision; isItemDay2 = true; }
        if (collision.CompareTag("day3_items")) { itemDay3 = collision; isItemDay3 = true; }
        if (collision.CompareTag("day4_items")) { itemDay4 = collision; isItemDay4 = true; }
        if (collision.CompareTag("day5_items")) { itemDay5 = collision; isItemDay5 = true; }
        if (collision.CompareTag("day6_items")) { itemDay6 = collision; isItemDay6 = true; }
        if (collision.CompareTag("day7_items")) { itemDay7 = collision; isItemDay7 = true; }
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

        isImages = false;
        isStend = false;
        // isWire = false;
        // wire = null;
        // isInstruments = false;
        // isMining = false;
        // mining = null;
        // isLightOff = false;
        // isMatches = false;
        // isSink = false;
        // isEgg = false;
        // isFlowers = false;
        // isPlush = false;
        // isDichlorvos = false;
        // isScissors = false;
        // isWateringCan = false;
        isBad = false;
        isBaika = false;
        isItemDay1 = false;
        itemDay1 = null;
        isItemDay2 = false;
        itemDay2 = null;
        isItemDay3 = false;
        itemDay4 = null;
        isItemDay5 = false;
        itemDay5 = null;
        isItemDay6 = false;
        itemDay6 = null;
        isItemDay7 = false;
        itemDay7 = null;
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