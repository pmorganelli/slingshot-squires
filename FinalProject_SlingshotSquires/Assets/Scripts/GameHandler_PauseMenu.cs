using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using FMODUnity;

public class GameHandler_PauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;
    public AudioMixer mixer;
    public static float volumeLevel = 1.0f;
    public static bool keyboardModeEnabled = false;
    private Slider sliderVolumeCtrl;
    private Toggle keyboardToggle;

    void Awake()
    {
        pauseMenuUI.SetActive(true);

        // Setup volume slider
        SetLevel(volumeLevel);
        GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
        if (sliderTemp != null)
        {
            sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
            sliderVolumeCtrl.value = volumeLevel;
        }

        //keyboard mode toggle
        GameObject toggleObj = GameObject.Find("KeyboardModeToggle");
        if (toggleObj != null)
        {
            keyboardToggle = toggleObj.GetComponent<Toggle>();
            keyboardToggle.onValueChanged.AddListener(SetKeyboardMode);
        }
    }

    void Start()
    {
        pauseMenuUI.SetActive(false);
        GameisPaused = false;

        if (keyboardToggle != null)
        {
            keyboardToggle.isOn = keyboardModeEnabled;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RuntimeManager.PlayOneShot("event:/SFX/UI Interact");
            if (GameisPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        if (!GameisPaused)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameisPaused = true;
        }
        else
        {
            Resume();
        }
    }


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
        RuntimeManager.PlayOneShot("event:/SFX/UI Interact");
    }

    // void OnEnable()
    // {
    //         if (keyboardToggle != null)
    //         {
    //                 keyboardToggle.isOn = keyboardModeEnabled;
    //         }
    // }


    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        volumeLevel = sliderValue;
    }


    public void SetKeyboardMode(bool isOn)
    {
        keyboardModeEnabled = isOn; // global state
    }


}
