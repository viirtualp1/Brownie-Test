using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class Hero : MonoBehaviour
{
    private Animator anim;

    // Лист собранных предметов (!добавлять id предметов, чтобы потом было их легко удалить!)
    public static List<string> items;
    public static List<string> collectsCards;
    public static List<string> itemsSprites;

    // Приватные поля
    public static float speed = 3f; // Скорость движения
    public float jumpForce = 15f; // Сила прыжка

    // Приватные переменные
    private bool isGrounded = false;

    // Ссылки
    private Rigidbody2D rb;
    public SpriteRenderer sprite;
    
    public static Hero Instance { get; set; }

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
    // private bool isHelper;

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

    private bool isStickyNote;

    // Открыт ли стикер
    private bool isStickyNoteYes = false;

    private int collectsCardsCount;
    private int itemsCount;

    private int itemsSpritesCount;

    private string currentTaskString = "Убрать спальню и большую прихожую";

    // Взял ли игрок отвертку
    private bool isTakeInstuments = false;

    private Collider2D light;
    private bool isLightOff = false;
    private bool isLightTrigger = false;

    private int c_voicetrns2 = 0;

    private int c_voiceclock1 = 0;
    private int c_voiceclock2 = 0;

    private int c_voiceclock3_sp = 0;
    
    private int c_voiceStickyNote1 = 0;
    private int c_voiceStickyNote2 = 0;

    private int c_voicebaika = 0;
    private int c_voicetrcan = 0;
    private int c_voicebed = 0;
    private int c_voicebed2 = 0;

    private int c_voicetel1 = 0;
    private int c_voicetel2 = 0;
    private int c_voicepopcorn = 0;
    //private int c_voicecream = 0;
    private int c_voicesock1 = 0;
    //private int c_voicesocks2 = 0;
    private int c_voiceumbrella = 0;

    private int c_voicewheel = 0;
    private int c_voiceimages = 0;
    private int c_voicepain = 0;

    private int c_voicefermas1 = 0;
    private int c_voicefermas2 = 0;
    private int c_voiceprovod = 0;
    private int c_chitok = 0;
    private int c_voicecactus = 0;

    private int c_voicerazor = 0;
    private int c_voicesleeve = 0;
    private int c_voicefishRod = 0;

    private int c_voiceegg = 0;
    private int c_voicesink = 0;
    private int c_voicechol = 0;

    private int c_voiceflowers = 0;
    private int c_voiceplush = 0;

    private int c_voicematches = 0;

    // public Sprite bedNew;
    private bool isBed;
    // private bool isBad;

    private bool isStend = false;
    private bool isImages = false;

    private bool isTakeFertilizer;
    private bool isTakeDichlorvos;
    private bool isTakeScissors;
    private bool isTakeWateringCan;

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

    private bool isLoot;

    bool isCanBoobaSleep;

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

        // Var for voices scenes
        //PlayerPrefs.SetInt("c_voiceclock1", 0);
        //PlayerPrefs.Save();

        getSaves();

        // CHIT FOR DEVELOPER DAY
        //day = 1;

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
            if (GameObject.Find(collectsCards[i]))
                Destroy(GameObject.Find(collectsCards[i]));

        try {
            if (day >= 3)
                GameObject.Find("helper").GetComponent<SpriteRenderer>().sprite = GameObject.Find("TriggerItems").GetComponent<BedRoom>().cactus_new;
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

        try {
            stickyNoteSR = GameObject.Find("stickyNote").GetComponent<SpriteRenderer>();
        } catch {}

        TriggerItems = GameObject.Find("TriggerItems");

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
 
        if (SceneManager.GetActiveScene().name != "BedRoomScene" && SceneManager.GetActiveScene().name != "HomeBooba" && day < 7)
        {
            Debug.Log(day);
            GameObject.Find("Day " + day + " BG").GetComponent<AudioSource>().Play();
        }

        for (int i = 0; i < itemsSprites.Count; i++)
        {
            try
            {
                GameObject.Find(itemsSprites[i]).SetActive(false);
                GameObject.Find(itemsSprites[i] + " new").GetComponent<SpriteRenderer>().enabled = true;
            }
            catch
            {
                Debug.Log(itemsSprites[i]); 
            }
        }
    }

    // private void currentTask()
    // {
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
    // }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private async void animationLoot()
    {
        if (!isBadBooba)
        {
            isGrounded = false;
            isBoobaCanJump = false;
            isBoobaCanMove = false;
            isLoot = true;
            State = States.loot;

            await Task.Delay(900);

            isGrounded = true;
            isBoobaCanJump = true;
            isBoobaCanMove = true;
            isLoot = false;   
        }
    }

    private void Update()
    {   

        // Если Буба стоит на земле => анимация idle
        if (isGrounded) State = States.idle;
        if (!isLoot && Input.GetKeyDown(KeyCode.X)) animationLoot();

        // Ходьба и прыжки Бубы
        if (isBoobaCanMove && Input.GetButton("Horizontal"))
            Run();
        if (isBoobaCanJump && isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();

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

        // Voice for StickyNote
        if (isStickyNote && c_voiceStickyNote1 == 0)
        {
            GameObject.Find("B-1-5").GetComponent<AudioSource>().Play();
            c_voiceStickyNote1 = 1;
        }

        // Открываем стикер с обучением
        if (!isStickyNoteYes && isStickyNote)
        {   

            // Voice for StickyNote
            if (isStickyNote && c_voiceStickyNote2 == 0)
            {
                GameObject.Find("B-1-4").GetComponent<AudioSource>().PlayDelayed(4f);
                c_voiceStickyNote2 = 1;
            }

            isStickyNoteYes = true;

            stickyNoteSR.enabled = true;
            isBoobaCanMove = false;
            isBoobaCanJump = false;
        }

        // Закрываем стикер с обучением
        if (isStickyNote && Input.GetKeyDown(KeyCode.C))
        {
            stickyNoteSR.enabled = false;

            Destroy(TriggerItems.GetComponent<BedRoom>().stickyNote);

            isBoobaCanMove = true;
            isBoobaCanJump = true;
            isStickyNote = false;
            isStickyNoteYes = false;

            items.Add("training-item");
            saveItems();
        }

        // попросить помощи у Кактуса (нажав Z)
        // if (isHelper && Input.GetKeyDown(KeyCode.Z))
        // {
            // Проверка задания (currentTaskString)
            // TODO Озвучка кактуса
        // }

        // Вход в N комнату
        if (isDoor && Input.GetKeyDown(KeyCode.X))
        {
            string nextRoom = room.gameObject.GetComponent<GoToDoor>().nextRoom;

            // Voices for translate
            // && c_voicetrns1 == 0
            if (nextRoom == "HallwayLargeRoom")
            {
                //try
                //{
                    //GameObject.Find("K-1-8").GetComponent<AudioSource>().Play();
                //}
                //catch
                //{
                    //Debug.Log("huy");
                //}
                //yield return new WaitForSeconds(0.5f);
                SceneManager.LoadScene(nextRoom);
            } else 
            {
                SceneManager.LoadScene(nextRoom);
            }
        }

        // Выход в меню
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {

            // SceneManager.LoadScene("Menu");
        // }

        // Voices for translate and headband scenes
        if(SceneManager.GetActiveScene().name == "HallwayLargeRoom" && day == 1 && !PlayerPrefs.HasKey("c_voicetrns1"))
        {   
            PlayerPrefs.SetInt("c_voicetrns1", 1);

            GameObject.Find("K-1-8").GetComponent<AudioSource>().Play();
            //c_voicetrns1 = 1;
        }

        if(SceneManager.GetActiveScene().name == "HomeBooba" && c_voicetrns2 == 0 && day < 3)
        {
            GameObject.Find("B-2-11").GetComponent<AudioSource>().Play();
            c_voicetrns2 = 1;
        }

        if(SceneManager.GetActiveScene().name == "HomeBooba" && c_voicetrns2 == 0 && day == 3)
        {
            GameObject.Find("B-3-12").GetComponent<AudioSource>().Play();
            c_voicetrns2 = 1;
        }


        if(SceneManager.GetActiveScene().name == "BedRoomScene" && day == 2 && !PlayerPrefs.HasKey("c_voicetrns3"))
        {   
            PlayerPrefs.SetInt("c_voicetrns3", 1);

            GameObject.Find("B-2-1").GetComponent<AudioSource>().Play();
            GameObject.Find("K-2-1").GetComponent<AudioSource>().PlayDelayed(3.5f);
            //c_voicetrns3 = 1;
        }

        if(SceneManager.GetActiveScene().name == "LivingRoom" && day == 2 && !PlayerPrefs.HasKey("c_voicetrns4"))
        {
            PlayerPrefs.SetInt("c_voicetrns4", 1);
            //PlayerPrefs.Save();

            GameObject.Find("K-2-2").GetComponent<AudioSource>().Play();
            //c_voicetrns4 = 1;
        }


        if(SceneManager.GetActiveScene().name == "Garage" && day == 3 && !PlayerPrefs.HasKey("c_voicetrns5"))
        {
            PlayerPrefs.SetInt("c_voicetrns5", 1);

            GameObject.Find("K-3-4").GetComponent<AudioSource>().Play();
        }


        if(SceneManager.GetActiveScene().name == "BathRoom" && day == 4 && !PlayerPrefs.HasKey("c_voicetrns6"))
        {   
            PlayerPrefs.SetInt("c_voicetrns6", 1);

            GameObject.Find("B-4-3").GetComponent<AudioSource>().Play();
            //c_voicetrns3 = 1;
        }

        if(SceneManager.GetActiveScene().name == "BedRoomScene" && day == 4 && !PlayerPrefs.HasKey("c_voicetrns7"))
        {   
            PlayerPrefs.SetInt("c_voicetrns7", 1);

            GameObject.Find("K-4-1").GetComponent<AudioSource>().Play();
            GameObject.Find("B-4-1").GetComponent<AudioSource>().PlayDelayed(3.5f);
            GameObject.Find("K-4-2").GetComponent<AudioSource>().PlayDelayed(6.5f);
            //c_voicetrns3 = 1;
        }


        if(SceneManager.GetActiveScene().name == "Kitchen" && day == 5 && !PlayerPrefs.HasKey("c_voicetrns8"))
        {   
            PlayerPrefs.SetInt("c_voicetrns8", 1);

            GameObject.Find("K-5-1").GetComponent<AudioSource>().Play();
            //c_voicetrns3 = 1;
        }


        if(SceneManager.GetActiveScene().name == "BedRoomScene" && day == 6 && !PlayerPrefs.HasKey("c_voicetrns9"))
        {   
            PlayerPrefs.SetInt("c_voicetrns9", 1);

            GameObject.Find("B-6-1").GetComponent<AudioSource>().Play();
            GameObject.Find("K-6-1").GetComponent<AudioSource>().PlayDelayed(3f);
            //c_voicetrns3 = 1;
        }

        // Special voice finale day 6
        if(SceneManager.GetActiveScene().name == "roofScene" && day == 6){

            currentTaskString = PlayerPrefs.GetString("currentTaskString");
            //Debug.Log(PlayerPrefs.GetInt("c_voiceclock1"));

            if (currentTaskString == "Идти спать" && c_voiceclock3_sp == 0)
            {
                GameObject.Find("B-6-6").GetComponent<AudioSource>().PlayDelayed(2.5f);
                GameObject.Find("K-6-3").GetComponent<AudioSource>().PlayDelayed(5f);
                GameObject.Find("K-6-4").GetComponent<AudioSource>().PlayDelayed(8f);
                c_voiceclock3_sp = 1;
            }
        }


        // Вход в домик Бубы
        if (isDoorBooba && Input.GetKeyDown(KeyCode.X))
            SceneManager.LoadScene("homeBooba");

        // Включение/Выключенеи телевизора
        if (isTVOff && isRCB && Input.GetKeyDown(KeyCode.X))
        {
            GameObject TV = GameObject.Find("TV");
            GameObject RC = GameObject.Find("RC");
            AudioClip TVAudioSound = RC.gameObject.GetComponent<anySound>().sound;
            
            // Voice for tv1
            //if (c_voicetel1 == 0)
            //{
                //GameObject.Find("B-1-11").GetComponent<AudioSource>().Play();
                //GameObject.Find("K-1-9").GetComponent<AudioSource>().PlayDelayed(3f);

                //c_voicetel1 = 1;
            //}


            TV.GetComponent<SpriteRenderer>().sprite = RC.gameObject.GetComponent<LargeRoom>().TVOn;
            RC.GetComponent<AudioSource>().clip = TVAudioSound;
            RC.GetComponent<AudioSource>().Play();

            isTVOff = false;
        } else if (!isTVOff && isRCB && Input.GetKeyDown(KeyCode.X)) 
        {
            GameObject TV = GameObject.Find("TV");
            GameObject RC = GameObject.Find("RC");
            AudioClip TVAudioSound = RC.gameObject.GetComponent<anySound>().sound;

            // Voice for tv2
            //if (c_voicetel2 == 0)
            //{
                //GameObject.Find("B-1-12").GetComponent<AudioSource>().Play();

                //c_voicetel2 = 1;
            //}

            TV.GetComponent<SpriteRenderer>().sprite = RC.gameObject.GetComponent<LargeRoom>().TVOff;
            RC.GetComponent<AudioSource>().clip = TVAudioSound;
            RC.GetComponent<AudioSource>().Stop();

            isTVOff = true;
        }

        isCanBoobaSleep = checkCompleteTasks();
        Debug.Log(isCanBoobaSleep);

        // Буба ложится спать на свою кровать
        if (isCanBoobaSleep && isBadBooba && Input.GetKeyDown(KeyCode.X))
        {   
            // Voices for Буба ложится спать
            GameObject.Find("K-" + day + "-S").GetComponent<AudioSource>().Play();

            newDay();

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

        // Voice1 for bed coll
        if (isBed && c_voicebed == 0 && day == 1)
        {
            GameObject.Find("B-1-6").GetComponent<AudioSource>().Play();
            c_voicebed = 1;
        }

        if (isBed && c_voicebed == 0 && day == 3)
        {
            GameObject.Find("B-3-4").GetComponent<AudioSource>().Play();
            c_voicebed = 1;
        }

        // Voice for images coll
        if (isImages && c_voiceimages == 0)
        {
            GameObject.Find("B-2-6").GetComponent<AudioSource>().Play();
            c_voiceimages = 1;
        }


        if (isBed && Input.GetKeyDown(KeyCode.X))
        {   
            //GameObject.Find("B-1-6").GetComponent<AudioSource>().Play();

            GameObject bedNow = GameObject.Find("Bed");
            try
            {
                GameObject.Find("Bed").SetActive(false);
            } catch {}
            GameObject.Find("Bed new").GetComponent<SpriteRenderer>().enabled = true;
            itemsSprites.Add("Bed");

            if (c_voicebed2 == 0) 
            {
                // Voice2 for bed add
                GameObject.Find("B-1-7").GetComponent<AudioSource>().PlayDelayed(1f);

                c_voicebed2 = 1;
            }

            if (c_voicebed2 == 0 && day == 3)
            {
                // Voice2 for bed add
                GameObject.Find("B-1-7").GetComponent<AudioSource>().PlayDelayed(1f);

                c_voicebed2 = 1;
            }

            // CHIT FOR DEVELOPER "INVENTAR"
            //items.Clear();

            if (day == 1) day1Tasks++;

            saveCountersTasks();
            saveItems();
        }

        if (isImages && Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("img1").SetActive(false);
            GameObject.Find("img1 new").GetComponent<SpriteRenderer>().enabled = true;
            itemsSprites.Add("img1");
            day2Tasks++;

            saveCountersTasks();
            saveItems();
        }
        
        if (isStend && Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("stend1").SetActive(false);
            GameObject.Find("stend1 new").GetComponent<SpriteRenderer>().enabled = true;
            itemsSprites.Add("stend1");
            day2Tasks++;

            saveCountersTasks();
            saveItems();
        }

        if (isLightTrigger && Input.GetKeyDown(KeyCode.X))
        {
            isLightOff = true;

            // Voice for chitok
            if (c_chitok == 0){
                GameObject.Find("B-3-11").GetComponent<AudioSource>().Play();
                c_chitok = 1;
            }

            try
            {
                GameObject.Find("Shitok1").SetActive(false);
                GameObject.Find("Shitok1 new").GetComponent<SpriteRenderer>().enabled = true;   
            } catch {}
            itemsSprites.Add("Shitok1");
            day3Tasks++;

            saveItems();
            saveCountersTasks();
        }
        
        // Voices and BG_Music for items coll
        if(SceneManager.GetActiveScene().name != "HomeBooba"){

            currentTaskString = PlayerPrefs.GetString("currentTaskString");
            //Debug.Log(PlayerPrefs.GetInt("c_voiceclock1"));

            // && !PlayerPrefs.HasKey("c_voiceclock1")
            if (currentTaskString == "Идти спать")
            {

                if (c_voiceclock1 == 0 && day != 6)
                {
                    GameObject.Find("K-1-11").GetComponent<AudioSource>().PlayDelayed(2.5f);
                    c_voiceclock1 = 1;
                }

            // Variable!
                //PlayerPrefs.SetInt("c_voiceclock1", 1);
                //PlayerPrefs.Save();
            }

            if (currentTaskString == "Идти спать" && c_voiceclock2 == 0)
            {
                GameObject.Find("Day " + day + " BG").GetComponent<AudioSource>().Stop();
                GameObject.Find("Day-" + day + "-clock-loop").GetComponent<AudioSource>().Play();

                 c_voiceclock2 = 1;
            }
        }

        // Voice for day1
        if (day1Tasks < 9 && isItemDay1 && Input.GetKeyDown(KeyCode.Z))
        {
            string itemHW = itemDay1.GetComponent<CollectableScript>().itemType;

            Debug.Log(isItemDay1);
            Debug.Log(itemHW);

            // Voice for scene1 bedroom
            if (itemHW == "baika" && c_voicebaika == 0)
            {   
                c_voicebaika = 1;
                GameObject.Find("B-1-8").GetComponent<AudioSource>().Play();
                GameObject.Find("K-1-6").GetComponent<AudioSource>().PlayDelayed(3f);
                GameObject.Find("K-1-7").GetComponent<AudioSource>().PlayDelayed(12f);
            }

            if (itemHW == "trcan" && c_voicetrcan == 0)
            {   
                c_voicetrcan = 1;
                GameObject.Find("B-1-10").GetComponent<AudioSource>().Play();
            }

            // Voice for scene2 hallwaylarge
            if (itemHW == "popcorn" && c_voicepopcorn== 0)
            {   
                c_voicepopcorn = 1;
                GameObject.Find("B-1-18").GetComponent<AudioSource>().Play();
            }

            if (itemHW == "Umbrella" && c_voiceumbrella == 0)
            {   
                c_voiceumbrella = 1;
                GameObject.Find("B-1-16").GetComponent<AudioSource>().Play();
            }

            if (itemHW == "sock1" && c_voicesock1 == 0)
            {   
                c_voicesock1 = 1;
                GameObject.Find("B-1-14").GetComponent<AudioSource>().Play();
            }

        }

        // Voices for day2
        if (day2Tasks < 3 && isItemDay2 && Input.GetKeyDown(KeyCode.Z))
        {
            string itemHW = itemDay2.GetComponent<CollectableScript>().itemType;

            if (itemHW == "wheel" && c_voicewheel == 0 && Input.GetKeyDown(KeyCode.Z))
            {   
                c_voicewheel = 1;
                GameObject.Find("B-2-5").GetComponent<AudioSource>().Play();
            }

            if (itemHW == "краски" && c_voicepain == 0)
            {   
                c_voicepain = 1;
                GameObject.Find("B-2-9").GetComponent<AudioSource>().Play();
                GameObject.Find("K-2-4").GetComponent<AudioSource>().PlayDelayed(3f);
            }
        }

        // Voices for day3
        if (day3Tasks < 6 && isItemDay3 && Input.GetKeyDown(KeyCode.Z))
        {
            string itemHW = itemDay3.GetComponent<CollectableScript>().itemType;

            if (itemHW == "Fermas" && c_voicefermas1 == 0)
            {
                c_voicefermas1 = 1;

                GameObject.Find("B-3-7").GetComponent<AudioSource>().Play();
                GameObject.Find("B-3-8").GetComponent<AudioSource>().PlayDelayed(3f);
                GameObject.Find("K-3-5").GetComponent<AudioSource>().PlayDelayed(6f);
            }

            if (itemHW == "Fermas" && isTakeInstuments && c_voicefermas2 == 0)
            {
                c_voicefermas2 = 1;

                GameObject.Find("B-3-10").GetComponent<AudioSource>().Play();
            }

            if (itemHW == "provod5" && c_voiceprovod == 0)
            {
                c_voiceprovod = 1;

                GameObject.Find("B-3-9").GetComponent<AudioSource>().Play();
            }

            if (itemHW == "cactus" && c_voicecactus == 0)
            {
                c_voicecactus = 1;

                GameObject.Find("B-3-1").GetComponent<AudioSource>().Play();
                GameObject.Find("K-3-1").GetComponent<AudioSource>().PlayDelayed(3f);
                GameObject.Find("B-3-2").GetComponent<AudioSource>().PlayDelayed(6f);
                GameObject.Find("K-3-2").GetComponent<AudioSource>().PlayDelayed(9f);
                GameObject.Find("B-3-3").GetComponent<AudioSource>().PlayDelayed(14f);
            }

        }

        // Voices for day4
        if (day4Tasks < 5 && isItemDay4 && Input.GetKeyDown(KeyCode.Z))
        {
            string itemHW = itemDay4.GetComponent<CollectableScript>().itemType;

            if (itemHW == "razor" && c_voicerazor == 0)
            {
                c_voicerazor = 1;

                GameObject.Find("B-4-4").GetComponent<AudioSource>().Play();
                GameObject.Find("K-4-3").GetComponent<AudioSource>().PlayDelayed(3f);
            }

            if (itemHW == "sleeve" && c_voicesleeve == 0)
            {
                c_voicesleeve = 1;

                GameObject.Find("B-4-5").GetComponent<AudioSource>().Play();
            }

            if (itemHW == "fishRod" && c_voicefishRod == 0)
            {
                c_voicefishRod = 1;

                GameObject.Find("B-4-9").GetComponent<AudioSource>().Play();
            }

        }

        // Voices for day5
        if (day5Tasks < 3 && isItemDay5 && Input.GetKeyDown(KeyCode.Z))
        {
            string itemHW = itemDay5.GetComponent<CollectableScript>().itemType;

            if (itemHW == "egg" && c_voiceegg == 0 && Input.GetKeyDown(KeyCode.X))
            {
                c_voiceegg = 1;

                GameObject.Find("B-5-1").GetComponent<AudioSource>().Play();
            }

            if (itemHW == "sink" && c_voicesink == 0)
            {
                c_voicesink = 1;

                GameObject.Find("B-5-4").GetComponent<AudioSource>().Play();
            }

            if (itemHW == "chol" && c_voicechol == 0)
            {
                c_voicechol = 1;

                GameObject.Find("B-5-6").GetComponent<AudioSource>().Play();
            }

        }

        if (isItemDay5 && Input.GetKeyDown(KeyCode.Z))
        {
            string itemHW = itemDay5.GetComponent<CollectableScript>().itemType;
            
            if (itemHW == "matches" && c_voicematches == 0)
                {
                    c_voicematches = 1;

                    GameObject.Find("K-6-6").GetComponent<AudioSource>().Play();
                }
        }

        // Voices for day6
        if (day6Tasks < 7 && isItemDay6 && Input.GetKeyDown(KeyCode.Z))
        {
            string itemHW = itemDay6.GetComponent<CollectableScript>().itemType;

            if (itemHW == "flowers" && c_voiceflowers == 0)
            {
                c_voiceflowers = 1;

                GameObject.Find("B-6-2").GetComponent<AudioSource>().Play();
            }

            if (itemHW == "plush" && c_voiceplush == 0)
            {
                c_voiceplush = 1;

                GameObject.Find("B-6-4").GetComponent<AudioSource>().Play();
                GameObject.Find("K-6-2").GetComponent<AudioSource>().PlayDelayed(3f);
            }
        }


        if (day1Tasks < 9 && isItemDay1 && Input.GetKeyDown(KeyCode.X))
        {
            string itemHW = itemDay1.GetComponent<CollectableScript>().itemType;

            // Voice baika add
            if (itemHW == "baika")
                GameObject.Find("B-1-9").GetComponent<AudioSource>().Play();

            // Voice cream add
            if (itemHW == "cream")
                GameObject.Find("B-1-13").GetComponent<AudioSource>().Play();

            // Voice Umbrella add
            if (itemHW == "Umbrella")
                GameObject.Find("B-1-17").GetComponent<AudioSource>().Play();

            items.Add(itemHW);
            saveItems();

            Destroy(GameObject.Find(itemHW));
            day1Tasks++;
            saveCountersTasks();
            Debug.Log(day1Tasks);
        } else if (day2Tasks < 3 && isItemDay2 && Input.GetKeyDown(KeyCode.X))
        {
            string itemHW = itemDay2.GetComponent<CollectableScript>().itemType;

            // Voice pain add
            if (itemHW == "краски")
                GameObject.Find("B-2-10").GetComponent<AudioSource>().Play();

            items.Add(itemHW);
            saveItems();

            Destroy(GameObject.Find(itemHW));
            day2Tasks++;
            saveCountersTasks();
        } else if (day3Tasks < 6 && isItemDay3 && Input.GetKeyDown(KeyCode.X))
        {
            string itemHW = itemDay3.GetComponent<CollectableScript>().itemType;

            if (itemHW == "Instruments")
                isTakeInstuments = true;
            if (itemHW == "Fermas") {
                if (isTakeInstuments && isLightOff) {
                    items.Add(itemHW);
                    saveItems();

                    Destroy(GameObject.Find(itemHW));
                    day3Tasks++;
                    saveCountersTasks();
                }
            } else if (itemHW != "Instruments") {
                items.Add(itemHW);
                saveItems();

                Destroy(GameObject.Find(itemHW));
                day3Tasks++;
                saveCountersTasks();
            }
            else
            {
                items.Add(itemHW);
                saveItems();

                Destroy(GameObject.Find(itemHW));
                day3Tasks++;
                saveCountersTasks();
            }
        } else if (day4Tasks < 5 && isItemDay4 && Input.GetKeyDown(KeyCode.X))
        {
            string itemHW = itemDay4.GetComponent<CollectableScript>().itemType;

            // Voice sleeve  add
            if (itemHW == "sleeve")
                GameObject.Find("B-4-6").GetComponent<AudioSource>().Play();

            if (itemHW == "Mirror")
            {
                GameObject.Find("Mirror").GetComponent<SpriteRenderer>().sprite = GameObject.Find("Script").GetComponent<BathRoom>().new_Mirror;
                itemsSprites.Add("new_Mirror");
                day4Tasks++;

                saveCountersTasks();
                saveItems();
            }
            
            if (itemHW == "fishRod")
            {
                GameObject.Find("B-4-10").GetComponent<AudioSource>().Play();
                GameObject.Find("angler").GetComponent<SpriteRenderer>().enabled = true;
                day4Tasks++;

                saveCountersTasks();
            } else 
            {
                items.Add(itemHW);
                saveItems();

                Destroy(GameObject.Find(itemHW));
                day4Tasks++;
                saveCountersTasks();
            }
        } else if (day5Tasks < 3 && isItemDay5 && Input.GetKeyDown(KeyCode.X))
        {
            string itemHW = itemDay5.GetComponent<CollectableScript>().itemType;

            if (itemHW == "sink")
            {
                GameObject.Find("sink").SetActive(false);
                GameObject.Find("sink new").GetComponent<SpriteRenderer>().enabled = true;
                itemsSprites.Add("sink");
                day5Tasks++;

                saveItems();
                saveCountersTasks();
            } else
            {
                items.Add(itemHW);
                saveItems();

                Destroy(GameObject.Find(itemHW));
                day5Tasks++;
                saveCountersTasks();
            }
        } else if (day6Tasks < 7 && isItemDay6 && Input.GetKeyDown(KeyCode.X))
        {
            string itemHW = itemDay6.GetComponent<CollectableScript>().itemType;


            if (itemHW == "plush" && isTakeScissors && isTakeDichlorvos)
            {
                GameObject.Find("plush").SetActive(false);
                GameObject.Find("plush new").GetComponent<SpriteRenderer>().enabled = true;
                itemsSprites.Add("plush");

                // Voice plush  add
                GameObject.Find("B-6-5").GetComponent<AudioSource>().Play();

                day6Tasks++;

                saveCountersTasks();
                saveItems();
            } else if (itemHW == "flowers" && isTakeWateringCan && isTakeFertilizer)
            {
                GameObject.Find("flowers").SetActive(false);
                GameObject.Find("flowers new").GetComponent<SpriteRenderer>().enabled = true;
                itemsSprites.Add("flowers");

                // Voice flowers  add
                GameObject.Find("B-6-3").GetComponent<AudioSource>().Play();

                day6Tasks++;

                saveCountersTasks();
                saveItems();
            } else if (itemHW == "fertilizer")
            {
                isTakeFertilizer = true;
                items.Add(itemHW);
                saveItems();

                Destroy(GameObject.Find(itemHW));
                day6Tasks++;
                saveCountersTasks();
            } else if (itemHW == "dichlorvos")
            {
                isTakeDichlorvos = true;
                items.Add(itemHW);
                saveItems();

                Destroy(GameObject.Find(itemHW));
                day6Tasks++;
                saveCountersTasks();
            } else if (itemHW == "scissors")
            {
                isTakeScissors = true;
                items.Add(itemHW);
                saveItems();

                Destroy(GameObject.Find(itemHW));
                day6Tasks++;
                saveCountersTasks();
            } else if (itemHW == "watering_can")
            {
                isTakeWateringCan = true;
                items.Add(itemHW);
                saveItems();

                Destroy(GameObject.Find(itemHW));
                day6Tasks++;
                saveCountersTasks();
            }
        }

        if (day != 6 && SceneManager.GetActiveScene().name == "Kitchen")
            GameObject.Find("matches").SetActive(false);
        
        if (day1Tasks >= 9 && day == 1) saveAndUpdateCurrentTask("Идти спать");
        if (day2Tasks >= 3 && day == 2) saveAndUpdateCurrentTask("Идти спать");
        if (day3Tasks >= 7 && day == 3) saveAndUpdateCurrentTask("Идти спать");
        if (day4Tasks >= 5 && day == 4) saveAndUpdateCurrentTask("Идти спать");
        if (day5Tasks >= 3 && day == 5) saveAndUpdateCurrentTask("Идти спать");
        if (day6Tasks >= 6 && day == 6) saveAndUpdateCurrentTask("Идти спать");
        if (day == 7) SceneManager.LoadScene("CutsceneEnd");
    }

    private bool checkCompleteTasks()
    {
        switch (day)
        {
            case 1: if (day1Tasks >= 9) isCanBoobaSleep = true; break;
            case 2: if (day2Tasks >= 3) isCanBoobaSleep = true; break;
            case 3: if (day3Tasks >= 7) isCanBoobaSleep = true; break;
            case 4: if (day4Tasks >= 5) isCanBoobaSleep = true; break;
            case 5: if (day5Tasks >= 3) isCanBoobaSleep = true; break;
            case 6: if (day6Tasks >= 6) isCanBoobaSleep = true; break;

            default: return false;
        }

        return isCanBoobaSleep;
    }


    private void saveAndUpdateCurrentTask(string task)
    {
        Debug.Log(task);

        currentTaskString = task;
        GameObject.Find("cts").GetComponent<Text>().text = "Текущее задание: " + currentTaskString;
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
        GameObject.Find("cd").GetComponent<Text>().text = "День: " + day.ToString();

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

        PlayerPrefs.SetInt("items_sprites_counter", itemsSprites.Count);
        for (int i = 0; i < itemsSprites.Count; i++)
            PlayerPrefs.SetString("items_sprites_list" + i, itemsSprites[i]);

        Debug.Log("items saved: " + items.Count);
        Debug.Log("items sprites saved: " + itemsSprites.Count);
    }

    public void getSaves()
    {
        // Сохраняем день
        day = PlayerPrefs.GetInt("Day");
        if (day == 0) day = 1;

        GameObject.Find("cd").GetComponent<Text>().text = "День: " + day.ToString();

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

        // Получаем спрайти предметов
        itemsSpritesCount = PlayerPrefs.GetInt("items_sprites_counter");
        itemsSprites = new List<string>();

        for (int i = 0; i < itemsSpritesCount; i++)
            itemsSprites.Add(PlayerPrefs.GetString("items_sprites_list" + i));

        Debug.Log("Items sprites: " + itemsSprites.Count);

        // Получаем текущее заданий
        currentTaskString = PlayerPrefs.GetString("currentTaskString");
        saveAndUpdateCurrentTask(currentTaskString);
        Debug.Log("Текущее задание: " + currentTaskString);

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
        if (collision.CompareTag("door"))
        {
            room = collision;
            isDoor = true;
        }

        if (collision.CompareTag("card")) { isCardTake = true; cardTake = collision; }

        if (collision.CompareTag("badBooba")) { isBadBooba = true; }

        if (collision.CompareTag("RCB")) { TV = collision; isRCB = true; }

        if (collision.CompareTag("training")) { isStickyNote = true; stickyNote = collision; }

        if (collision.CompareTag("light")) { isLightTrigger = true; light = collision; }

        if (collision.CompareTag("bed")) { isBed = true; }

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
        item = null;
        isDoor = false;
        isBadBooba = false;
        isCardTake = false;
        cardTake = null;
        isRCB = false;
        TV = null;
        isBoobaCanMove = true;
        isBoobaCanJump = true;

        isImages = false;
        isStend = false;

        isItemDay1 = false;
        itemDay1 = null;

        isItemDay2 = false;
        itemDay2 = null;

        isItemDay3 = false;
        itemDay2 = null;

        isItemDay4 = false;
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