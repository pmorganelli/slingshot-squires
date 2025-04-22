using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[5,5];
    public float coins;
    public int itemNum;
    public Text CoinsTxt;
    GameHandler myGameHandler;
    public GameObject coinWarning; 
    public GameObject coinWarningBG;

    public GameObject TomatoImage;
    public GameObject CarrotImage;
    public GameObject PumpkinImage;
    public GameObject WatermelonImage;
    public GameObject TomatoTitle;
    public GameObject CarrotTitle;
    public GameObject PumpkinTitle;
    public GameObject WatermelonTitle;
    public GameObject TomatoDescription;
    public GameObject CarrotDescription;
    public GameObject PumpkinDescription;
    public GameObject WatermelonDescription;
    public GameObject InfoPanel;
    public GameObject ConfirmButton;
    public GameObject backButton;




    // Start is called before the first frame update
    void Start()
    {
        myGameHandler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
        coins = myGameHandler.getCointAmount();
        CoinsTxt.text = "Coins: " + coins.ToString();

        coinWarning.gameObject.SetActive(false);
        coinWarningBG.gameObject.SetActive(false);
        TomatoImage.gameObject.SetActive(false);
        TomatoDescription.gameObject.SetActive(false);
        TomatoTitle.gameObject.SetActive(false);
        CarrotImage.gameObject.SetActive(false);
        CarrotDescription.gameObject.SetActive(false);
        CarrotTitle.gameObject.SetActive(false);
        PumpkinImage.gameObject.SetActive(false);
        PumpkinDescription.gameObject.SetActive(false);
        PumpkinTitle.gameObject.SetActive(false);
        WatermelonImage.gameObject.SetActive(false);
        WatermelonDescription.gameObject.SetActive(false);
        WatermelonTitle.gameObject.SetActive(false);
        InfoPanel.gameObject.SetActive(false);
        ConfirmButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);


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
    }

    // Update is called once per frame
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {
            coinWarning.gameObject.SetActive(false);
            coinWarningBG.gameObject.SetActive(false);
            itemNum = ButtonRef.GetComponent<ButtonInfo>().ItemID;
            // confirm first 
            if(ButtonRef.GetComponent<ButtonInfo>().ItemID == 1) {
               //tomato
                TomatoImage.gameObject.SetActive(true);
                TomatoDescription.gameObject.SetActive(true);
                TomatoTitle.gameObject.SetActive(true);
                InfoPanel.gameObject.SetActive(true);
                ConfirmButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(true);
            } else if (ButtonRef.GetComponent<ButtonInfo>().ItemID == 2) {
                //carrot
                CarrotImage.gameObject.SetActive(true);
                CarrotDescription.gameObject.SetActive(true);
                CarrotTitle.gameObject.SetActive(true);
                InfoPanel.gameObject.SetActive(true);
                ConfirmButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(true);
            } else if (ButtonRef.GetComponent<ButtonInfo>().ItemID == 3) {
                //pumpkin
                PumpkinImage.gameObject.SetActive(true);
                PumpkinDescription.gameObject.SetActive(true);
                PumpkinTitle.gameObject.SetActive(true);
                InfoPanel.gameObject.SetActive(true);
                ConfirmButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(true);
            } else {
                //watermelon
                WatermelonImage.gameObject.SetActive(true);
                WatermelonDescription.gameObject.SetActive(true);
                WatermelonTitle.gameObject.SetActive(true);
                InfoPanel.gameObject.SetActive(true);
                ConfirmButton.gameObject.SetActive(true);
                backButton.gameObject.SetActive(true);
            }       
        } else {
            coinWarning.gameObject.SetActive(true);
            coinWarningBG.gameObject.SetActive(true);
        }
    }

    public void ConfirmPurchase() 
    {
        if (coins >= shopItems[2, itemNum]) {
            coins -= shopItems[2, itemNum];
            shopItems[3, itemNum]++;
            myGameHandler.AddItem(itemNum);
            myGameHandler.subtractCoins(shopItems[2, itemNum]);
            CoinsTxt.text = "Coins: " + coins.ToString();
        } else {
            coinWarning.gameObject.SetActive(true);
            coinWarningBG.gameObject.SetActive(true);
        }      
    }

    public void back()
    {
        coinWarning.gameObject.SetActive(false);
        coinWarningBG.gameObject.SetActive(false);
        TomatoImage.gameObject.SetActive(false);
        TomatoDescription.gameObject.SetActive(false);
        TomatoTitle.gameObject.SetActive(false);
        CarrotImage.gameObject.SetActive(false);
        CarrotDescription.gameObject.SetActive(false);
        CarrotTitle.gameObject.SetActive(false);
        PumpkinImage.gameObject.SetActive(false);
        PumpkinDescription.gameObject.SetActive(false);
        PumpkinTitle.gameObject.SetActive(false);
        WatermelonImage.gameObject.SetActive(false);
        WatermelonDescription.gameObject.SetActive(false);
        WatermelonTitle.gameObject.SetActive(false);
        InfoPanel.gameObject.SetActive(false);
        ConfirmButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

    }
}
