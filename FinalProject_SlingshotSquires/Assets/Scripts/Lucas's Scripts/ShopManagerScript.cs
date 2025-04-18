using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[5,5];
    public float coins;
    public Text CoinsTxt;
    GameHandler myGameHandler;
    public GameObject coinWarning; 
    public GameObject coinWarningBG;

    // Start is called before the first frame update
    void Start()
    {
        myGameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        coins = myGameHandler.getCointAmount();
        CoinsTxt.text = "Coins:" + coins.ToString();

        coinWarning.gameObject.SetActive(false);
        coinWarningBG.gameObject.SetActive(false);

        // IDs
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        // Price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;

        // Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
    }

    // Update is called once per frame
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {
            coinWarning.gameObject.SetActive(false);
            coinWarningBG.gameObject.SetActive(false);
            coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
            myGameHandler.AddItem(ButtonRef.GetComponent<ButtonInfo>().ItemID);
            myGameHandler.subtractCoins(shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID]);
            CoinsTxt.text = "Coins:" + coins.ToString();
            ButtonRef.GetComponent<ButtonInfo>().QuantityTxt.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
        } else {
            coinWarning.gameObject.SetActive(true);
            coinWarningBG.gameObject.SetActive(true);
        }
    }
}
