using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[5, 5];
    public int itemNum;
    public Text CoinsTxt;

    public GameObject[] windows;
    public GameObject buy_back_buttons;
    public GameObject insuff_fund_warnings;
    public GameObject panel;




    // Start is called before the first frame update
    void Start()
    {
        CoinsTxt.text = GameHandler.coinCount.ToString();

        insuff_fund_warnings.SetActive(false);
        buy_back_buttons.SetActive(false);
        panel.SetActive(false);
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }


        // IDs
        shopItems[1, 0] = 0;
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;

        // Price
        shopItems[2, 0] = 10;
        shopItems[2, 1] = 20;
        shopItems[2, 2] = 30;
        shopItems[2, 3] = 40;
    }

    // Update is called once per frame
    // private void Update()
    // {
    //     // CoinsTxt.text = GameHandler.coinCount.ToString();
    // }
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (GameHandler.coinCount >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {
            Debug.Log("here");
            insuff_fund_warnings.SetActive(false);
            itemNum = ButtonRef.GetComponent<ButtonInfo>().ItemID;
            // confirm first 
            windows[itemNum].SetActive(true);
            panel.SetActive(true);
            buy_back_buttons.SetActive(true);
        }
        else
        {
            Debug.Log("insuif");
            insuff_fund_warnings.SetActive(true);
        }
    }

    public void ConfirmPurchase()
    {
        int price = shopItems[2, itemNum];
        Debug.Log("Called");
        if (GameHandler.coinCount >= price)
        {
            Debug.Log("In loop");
            shopItems[3, itemNum]++;

            Debug.Log("Price: " + price);

            Debug.Log("Ct before: " + GameHandler.coinCount);
            GameHandler.subtractCoins(price);
            GameHandler.AddItem(itemNum);
            Debug.Log("Ct after: " + GameHandler.coinCount);
            CoinsTxt.text = GameHandler.coinCount.ToString();
        }

        else
        {
            Debug.Log("Insuff funds");
            insuff_fund_warnings.SetActive(true);
        }
    }

    public void back()
    {
        insuff_fund_warnings.SetActive(false);
        windows[itemNum].SetActive(false);
        panel.SetActive(false);
        buy_back_buttons.SetActive(false);
    }
}
