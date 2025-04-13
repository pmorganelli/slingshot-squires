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

    public class Crop
    {
        public string cropName;
        public float salePrice;

        public int totalGrowthStates;
        public int growthState;
        // Growth interval-- how many days between incrementing growthState
        public int growthInterval;
        // How many days has the plant been planted?
        public int age;
        public int totalHealth;
        public int currHealth;
        public Crop(string name, float price, int totalGrowthStates, int state, int interval, int age, int totalHealth, int currHealth)
        {
            this.totalGrowthStates = totalGrowthStates;
            this.cropName = name;
            this.salePrice = price;
            this.growthState = state;
            this.growthInterval = interval;
            this.age = age;
            this.totalHealth = totalHealth;
            this.currHealth = currHealth;
        }
    };

    // Define a list to reference various ball stats based off the above struct.
    public static Dictionary<string, ballStat> ballStats = new Dictionary<string, ballStat> {
        {"default", new ballStat(50f, 1.75f)}, {"fireball", new ballStat(75f, 2f)}
    };
    // Array containing all crops planted
    public static List<Crop> cropInventory = new List<Crop>();

    public static float SLING_reload_time = 1f;
    public static float SLING_force_multiplier = 1.25f;
    // Start is called before the first frame update
    public static bool waveComplete = false;
    public static int coinCount = 0;

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

        if (cropInventory.Count == 0)
        {
            Debug.Log("LOSE GAME HERE");
        }
    }

    public void PlayGame()
    {
        StartCoroutine(QuitAfter10());
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

    public void ProgressCrops()
    {
        for (int i = 0; i < cropInventory.Count; i++)
        {
            Crop crop = cropInventory[i];
            crop.age += 1;
            if (crop.age >= crop.growthInterval)
            {
                crop.growthState += 1;
                crop.age = 0;
            }
            if (crop.growthState >= crop.totalGrowthStates)
            {
                coinCount += (int)crop.salePrice;
                Debug.Log("Crop sold for: " + crop.salePrice);
                cropInventory.RemoveAt(i);
                i--;
            }
        }
    }

}
