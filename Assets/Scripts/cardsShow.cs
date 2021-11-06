using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardsShow : MonoBehaviour
{
    public Rigidbody2D hero;
    private GameObject card;
    private List<string> cards;

    private void Awake()
    {
        cards = Hero.collectsCards;
    }

    private void Start()
    {
        Debug.Log(cards.Count);

        for (int i = 0; i < cards.Count; i++)
        {
            card = GameObject.Find(cards[i]);
            Debug.Log(card);

            card.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
