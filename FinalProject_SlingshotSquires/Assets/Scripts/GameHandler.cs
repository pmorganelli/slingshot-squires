using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    }

    // === GAME CONFIG ===
    public static float SLING_reload_time = 1f;
    public static float SLING_force_multiplier = 1.25f;

    public static Dictionary<string, ballStat> ballStats = new Dictionary<string, ballStat> {
        {"default", new ballStat(50f, 1.75f)},
        {"fireball", new ballStat(75f, 2f)},
        {"goldball", new ballStat(75f, 2f)},
        {"diamondball", new ballStat(100f, 2.25f)},
    };

    public static List<Crop> cropInventory = new List<Crop> { };

    public static int coinCount = 50;

    public static int goldAmmo = 0;
    public static int diamondAmmo = 0;

    public static bool waveComplete = false;
    public static bool waveStarted = false;
    public static bool lost = false;

    public static int levelCount = 0;

    // === Keyboard Mode Global Toggle ===
    public static bool keyboardModeEnabled = false;

    // === CROP TRACKING ===
    public static int tomatoes = 0;
    public static int carrots = 0;
    public static int pumpkins = 0;
    public static int watermelon = 0;

    // === TUTORIAL UI ELEMENTS ===
    public GameObject waveClear;
    public int textCount = 0;

    // === INIT LOGIC ===
    private void Awake()
    {
        // Restore keyboard mode from PlayerPrefs
        if (PlayerPrefs.HasKey("KeyboardMode"))
        {
            keyboardModeEnabled = PlayerPrefs.GetInt("KeyboardMode") == 1;
            Debug.Log("[GameHandler] Loaded keyboardModeEnabled = " + keyboardModeEnabled);
        }
    }

    private void Update()
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

    // === GAME FLOW ===

    public void Intro1To2()
    {
        SceneManager.LoadScene("Intro2");
    }
    public void Intro2To3()
    {
        SceneManager.LoadScene("Intro3");
    }
    public void Intro3To4()
    {
        SceneManager.LoadScene("Intro4");
    }


    public void PlayGame() => SceneManager.LoadScene("Intro1");
    public void QuitGame() => Application.Quit();
    public void loadCredits() => SceneManager.LoadScene("Credits");
    public void returnMain() => SceneManager.LoadScene("TitleScreen");
    public void enterIntro() => SceneManager.LoadScene("Intro1");
    public void enterTutorial() => SceneManager.LoadScene("ShopScene");

    public bool startLevel()
    {
        if (cropInventory.Count == 0)
        {
            Debug.Log("0 CROPS CANNOT PROCEED");
            return false;
        }
        nextLevel();
        return true;
    }

    public void nextLevel()
    {
        if (cropInventory.Count == 0)
        {
            Debug.Log("Going To Shop");
            SceneManager.LoadScene("ShopScene");
        }
        else
        {
            SceneManager.LoadScene("Level1");
            Debug.Log("NEXT LEVEL: " + levelCount);
        }
    }

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

    public void backToShop() => SceneManager.LoadScene("ShopScene");

    // === SHOP / INVENTORY ===

    public static void AddItem(int itemID)
    {
        switch (itemID)
        {
            case 0: cropInventory.Add(Crop.Tomato()); break;
            case 1: cropInventory.Add(Crop.Carrot()); break;
            case 2: cropInventory.Add(Crop.Pumpkin()); break;
            default: cropInventory.Add(Crop.Watermelon()); break;
        }
    }

    public static void subtractCoins(int amount) => coinCount -= amount;

    public int getCointAmount() => coinCount;
}
