using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameHandler : MonoBehaviour
{
    public struct ballStat
    {
        public float damage;
        public float speed;

        public ballStat(float damage, float speed)
        {
            this.damage = damage;
            this.speed = speed;
        }
    };


    // Define a list to reference various ball stats based off the above struct.
    public static Dictionary<string, ballStat> ballStats = new Dictionary<string, ballStat> {
        {"default", new ballStat(50f, 1.75f)}, {"fireball", new ballStat(75f, 2f)}
    };
    // Array containing all crops planted
    public static List<Crop> cropInventory = new List<Crop> { Crop.Tomato() };

    public static float SLING_reload_time = 1f;
    public static float SLING_force_multiplier = 1.25f;
    // Start is called before the first frame update
    public static bool waveComplete = false;
    public static int coinCount = 50;
    public static bool lost = false;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject waveClear;
    public int textCount = 0;
    public static int tomatoes = 0;
    public static int carrots = 0;
    public static int pumpkins = 0;
    public static int watermelon = 0;
    public static int levelCount = 0;
    public static bool waveStarted = false;


    public void RestartGame()
    {
        coinCount = 50;
        lost = false;
        waveComplete = false;
        levelCount = 0;
        cropInventory.Clear();
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene("TitleScreen");
    }

    public void ReplayLastLevel()
    {
        GameHandler_PauseMenu.GameisPaused = false;
        waveComplete = false;
        SceneManager.LoadScene("Level1");
    }

    // Update is called once per frame
    void Update()
    {
        if (waveComplete)
        {
            waveComplete = false;
            Debug.Log("COMPLETE");
            SceneManager.LoadScene("waveWin");
        }

        if (lost)
        {
            lost = false;
            SceneManager.LoadScene("waveLose");
        }
    }
    IEnumerator WaitFiveSeconds()
    {
        yield return new WaitForSeconds(5f);
    }


    public void PlayGame()
    {
        SceneManager.LoadScene("Intro1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator QuitAfter10()
    {
        waveComplete = false;
        SceneManager.LoadScene("Level1");
        Debug.Log("Starting countdown");
        yield return new WaitForSeconds(120f);
        Debug.Log("Ending countdown");
        SceneManager.LoadScene("TitleScreen");
    }

    public void loadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void returnMain()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void enterIntro()
    {
        SceneManager.LoadScene("Intro1");
    }

    public void enterTutorial()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void nextText()
    {
        if (textCount == 0)
        {
            text1.gameObject.SetActive(false);
            text2.gameObject.SetActive(true);
            text3.gameObject.SetActive(false);
            textCount = textCount + 1;
        }
        else if (textCount == 1)
        {
            text1.gameObject.SetActive(false);
            text2.gameObject.SetActive(false);
            text3.gameObject.SetActive(true);
            textCount = textCount + 1;
        }
        else
        {
            SceneManager.LoadScene("ShopScene");
        }
    }

    public static void AddItem(int itemID)
    {
        if (itemID == 1)
        {
            cropInventory.Add(Crop.Tomato());
            int tomatoCount = cropInventory.Count(crop => crop.cropName == "Tomato");
            // Debug.Log("Tomato Count: " + tomatoCount);
        }
        else if (itemID == 2)
        {
            cropInventory.Add(Crop.Carrot());
        }
        else if (itemID == 3)
        {
            cropInventory.Add(Crop.Pumpkin());
        }
        else
        {
            cropInventory.Add(Crop.Watermelon());
        }
    }

    public static void subtractCoins(int amount)
    {
        coinCount = coinCount - amount;
        // Debug.Log("Coin Count: " + coinCount);
    }

    public int getCointAmount()
    {
        return coinCount;
    }

    public void backToShop()
    {
        SceneManager.LoadScene("andriaShopScene");
    }

    public void startLevel()
    {
        if (cropInventory.Count == 0)
        {
            // TODO: popup that you must have more than 0 seeds bought to start the round
            Debug.Log("0 CROPS CANNOT PROCEED");
            return;
        }
        else
        {
            nextLevel();
        }
    }

    public void nextLevel()
    {
        Debug.Log("Entering");
        if (cropInventory.Count == 0)
        {
            // TODO: Popup about 0 seeds-- they need to buy more.
            Debug.Log("Going To Shop");
            SceneManager.LoadScene("ShopScene");
        }
        else
        {
            SceneManager.LoadScene("Level1");
            levelCount = levelCount + 1;
            Debug.Log("NEXT LEVEL: " + levelCount);
        }
    }
}
