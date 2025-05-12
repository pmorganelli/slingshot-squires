using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using FMODUnity;

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
    public static float SLING_reload_time = 0.5f;
    public static float SLING_force_multiplier = 1.4f;

    public static Dictionary<string, ballStat> ballStats = new Dictionary<string, ballStat> {
        {"default", new ballStat(50f, 1.75f)},
        {"fireball", new ballStat(75f, 2f)},
        {"goldball", new ballStat(75f, 2f)},
        {"diamondball", new ballStat(100f, 2.25f)},
        {"bombball", new ballStat(60f, 1.25f)},
    };

    public static List<Crop> cropInventory = new List<Crop> { Crop.Tomato() };

    public static int coinCount = 50;
    public static int totalMade = 0;
    public static int enemiesKilled = 0;
    public static int seedsPlanted = 0;

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
    }

    // === GAME FLOW ===

    public void Intro1To2()
    {
        RuntimeManager.PlayOneShot("event:/SFX/UI Interact");
        SceneManager.LoadScene("Intro2");
    }
    public void Intro2To3()
    {
        RuntimeManager.PlayOneShot("event:/SFX/UI Interact");
        SceneManager.LoadScene("Intro3");
    }
    public void Intro3To4()
    {
        RuntimeManager.PlayOneShot("event:/SFX/UI Interact");
        SceneManager.LoadScene("Intro4");
    }


    public void PlayGame()
    {
        RuntimeManager.PlayOneShot("event:/Music/Play Music");
        SceneManager.LoadScene("Intro1");
    }

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
        Time.timeScale = 1f;
        coinCount = 50;
        totalMade = 0;
        seedsPlanted = 0;
        enemiesKilled = 0;
        waveStarted = false;
        lost = false;
        waveComplete = false;
        tomatoes = 0;
        pumpkins = 0;
        carrots = 0;
        watermelon = 0;
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
        seedsPlanted++;
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
