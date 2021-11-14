using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class homeBooba : MonoBehaviour
{
    private GameObject cardObject;
    public Sprite badWithBooba;
    public Sprite badWithoutBooba;

    public void showCards()
    {
        Hero.speed = 6f;

        for (int i = 0; i < Hero.collectsCards.Count; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                cardObject = GameObject.Find("card1 (" + j + ")");
                cardObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    public async void BoobaSleep()
    {
        GameObject.Find("bedBooba").GetComponent<SpriteRenderer>().sprite = badWithBooba;
        GameObject.Find("Circle").GetComponent<SpriteRenderer>().enabled = false;
        Hero.isBoobaCanMove = false;
        Hero.isBoobaCanJump = false;

        await Task.Delay(2000);

        GameObject.Find("bedBooba").GetComponent<SpriteRenderer>().sprite = badWithoutBooba;
        GameObject.Find("Circle").GetComponent<SpriteRenderer>().enabled = true;
        Hero.isBoobaCanMove = true;
        Hero.isBoobaCanJump = true;
    }
}
