using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public static List<Crop> cropInventory = new List<Crop> { Crop.Carrot(), Crop.Lettuce(), Crop.Tomato(), Crop.Pumpkin(), Crop.Watermelon() };
    // 

    public static float SLING_reload_time = 1f;
    public static float SLING_force_multiplier = 1.25f;
    // Start is called before the first frame update
    public static bool waveComplete = false;
    public static int coinCount = 50;
    public static int waveCount = 0;
    public static bool lost = false;
    public GameObject text1; 
    public GameObject text2;
    public GameObject text3; 
    public int textCount = 0;
    public static int tomatoes = 0;
    public static int carrots = 0;
    public static int pumpkins = 0;
    public static int watermelon = 0;
    public static int levelCount = 0;

    public void RestartGame()
    {
        GameHandler_PauseMenu.GameisPaused = false;
        SceneManager.LoadScene("charlieScene");
    }

    public void ReplayLastLevel()
    {
        GameHandler_PauseMenu.GameisPaused = false;
        waveComplete = false;
        SceneManager.LoadScene("peterSlingScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (waveComplete)
        {
            waveComplete = false;
            SceneManager.LoadScene("charlieScene");
        }

        if (lost)
        {
            Debug.Log("LOSE GAME HERE");
        }
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
        SceneManager.LoadScene("peterSlingScene");
        Debug.Log("Starting countdown");
        yield return new WaitForSeconds(120f);
        Debug.Log("Ending countdown");
        SceneManager.LoadScene("charlieScene");
    }

    public void loadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void returnMain()
    {
        SceneManager.LoadScene("charlieScene");
    }

    public static void ProgressCrops()
    {
        for (int i = 0; i < cropInventory.Count; i++)
        {
            Crop crop = cropInventory[i];
            crop.growthState += 1;

            if (crop.growthState >= crop.totalGrowthStates)
            {
                coinCount += (int)crop.salePrice;
                Debug.Log("Crop sold for: " + crop.salePrice);
                cropInventory.RemoveAt(i);
                i--;
            }
        }
    }

    public void enterIntro()
    {
        SceneManager.LoadScene("Intro1");
    }

    public void enterTutorial()
    {
        SceneManager.LoadScene("lucasScene1");
    }

    public void nextText() 
    {
        if(textCount == 0) {
            text1.gameObject.SetActive(false);
            text2.gameObject.SetActive(true);
            text3.gameObject.SetActive(false);
            textCount = textCount + 1;
        } else if (textCount == 1) {
            text1.gameObject.SetActive(false);
            text2.gameObject.SetActive(false);
            text3.gameObject.SetActive(true);
            textCount = textCount + 1;
        } else {
            SceneManager.LoadScene("lucasScene1");
        }
    }

    public void AddItem(int itemID) 
    {   
        if (itemID == 1) {
            tomatoes = tomatoes + 1;
        } else if (itemID == 2) {
            carrots = carrots + 1;
        } else if (itemID == 3) {
            pumpkins = pumpkins + 1;
        } else {
            watermelon = watermelon + 1;
        }
    }

    public void subtractCoins(int amount) 
    {
        coinCount = coinCount - amount;
        Debug.Log("Coint Count: " + coinCount);
    }

    public int getCointAmount() 
    {
        return coinCount;
    }

    public void nextLevel() 
    {
        if (levelCount == 0) 
        {
            SceneManager.LoadScene("Tutorial");
            levelCount = levelCount + 1;
        } else {
           SceneManager.LoadScene("peterSlingScene");
        }
    }
}
