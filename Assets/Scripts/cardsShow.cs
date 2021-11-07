using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardsShow : MonoBehaviour
{
    public Rigidbody2D hero;
    private GameObject card;
    private List<string> cards;
    private GameObject cardObject;

    private void Start()
    {
        // cards = Hero.collectsCards;
        
        // for (int i = 0; i < cards.Count; i++)
        // {
        //     Debug.Log(cards.Count);

        //     for (int j = 0; j < 10; j++)
        //     {
        //         Debug.Log("card"+j);
        //         cardObject = GameObject.Find("card" + j);
        //         cardObject.GetComponent<SpriteRenderer>().color = Color.white;
        //     }
        // }
    }
}
