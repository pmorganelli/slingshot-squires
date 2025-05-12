using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;
public class ShopManagerScript : MonoBehaviour
{

    public int[,] shopItems = new int[5, 7];
    public int itemNum;
    public Text CoinsTxt;

    public GameObject[] windows;
    public GameObject buy_back_buttons;
    public GameObject insuff_fund_warnings;
    public GameObject panel;

    public GameObject tButton;
    public GameObject wButton;

    // public CollectableType[] itemTypes;
    public Sprite[] itemIcons;
    public Player player;
    public Inventory_UI inventoryUI;

    public GameObject goldBallPrefab;
    public GameObject diamondBallPrefab;
    public GameObject bombBallPrefab;


    // Start is called before the first frame update
    void Start()
    {
        RuntimeManager.PlayOneShot("event:/Parameter Controllers/Buy Phase");
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
        shopItems[1, 4] = 4;   // Gold Ball
        shopItems[1, 5] = 5;   // Diamond Ball
        shopItems[1, 6] = 6;   // Bomb Ball

        // Price
        shopItems[2, 0] = 10;
        shopItems[2, 1] = 20;
        shopItems[2, 2] = 30;
        shopItems[2, 3] = 40;
        shopItems[2, 4] = 40;  // Gold Ball
        shopItems[2, 5] = 90;  // Diamond Ball
        shopItems[2, 6] = 100;  // bomb Ball
    }

    //the code below will gray out the windows but the balls aren't included, still an issue
    // private void Update()
    // {
    //     bool anyWindowOpen = panel.activeSelf;
    //     tButton.GetComponent<Button>().interactable = !anyWindowOpen;
    //     wButton.GetComponent<Button>().interactable = !anyWindowOpen;
    // }
    public void Buy()
    {
        RuntimeManager.PlayOneShot("event:/SFX/UI Interact");

        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        // Debug.Log("here");
        insuff_fund_warnings.SetActive(false);
        itemNum = ButtonRef.GetComponent<ButtonInfo>().ItemID;


        panel.SetActive(true);
        Button btnComponentT = tButton.GetComponent<Button>();
        Button btnComponentW = wButton.GetComponent<Button>();

        btnComponentT.enabled = false;
        btnComponentW.enabled = false;


        // confirm first 
        windows[itemNum].SetActive(true);
        buy_back_buttons.SetActive(true);

    }

    public void ConfirmPurchase()
    {
        int price = shopItems[2, itemNum];

        if (GameHandler.coinCount >= price)
        {
            RuntimeManager.PlayOneShot("event:/SFX/Purchase Crop");
            // Take money
            GameHandler.subtractCoins(price);

            // Add to inventory
            // Debug.Log("ADDING: " + itemNum);
            // player.inventory.Add(itemTypes[itemNum], itemIcons[itemNum]);
            if (itemNum <= 3)
            {
                // Add crop if not buying ball
                GameHandler.AddItem(itemNum);
            }
            else
            {
                switch (itemNum)
                {
                    case 4:  // gold ball
                        Sling.ChangeBall(goldBallPrefab);
                        break;
                    case 5:  // diamond ball
                        Sling.ChangeBall(diamondBallPrefab);
                        break;
                    case 6:  // bomb ball
                        Sling.ChangeBall(bombBallPrefab);
                        break;
                }
            }

            // Refresh inventory popup if it is open
            if (inventoryUI.inventoryPanel.activeSelf)
                inventoryUI.Refresh();

            // UI bookkeeping
            CoinsTxt.text = GameHandler.coinCount.ToString();
            shopItems[3, itemNum]++;
        }
        else
        {
            RuntimeManager.PlayOneShot("event:/SFX/UI Fail");
            insuff_fund_warnings.SetActive(true);
        }


    }

    public void back()
    {
        RuntimeManager.PlayOneShot("event:/SFX/UI Interact");
        insuff_fund_warnings.SetActive(false);
        if (itemNum >= 0 && itemNum < windows.Length)
            windows[itemNum].SetActive(false);
        buy_back_buttons.SetActive(false);
        panel.SetActive(false);
        Button btnComponentT = tButton.GetComponent<Button>();
        Button btnComponentW = wButton.GetComponent<Button>();

        btnComponentT.enabled = true;
        btnComponentW.enabled = true;
    }
}
