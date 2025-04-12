using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameHandler_PauseMenu : MonoBehaviour {

        public static bool GameisPaused = false;
        public GameObject pauseMenuUI;
        public AudioMixer mixer;
        public static float volumeLevel = 1.0f;
        private Slider sliderVolumeCtrl;
        public AudioSource playSound;
        public AudioSource pauseSound; 

        void Awake(){
                pauseMenuUI.SetActive(true); // so slider can be set
                SetLevel (volumeLevel);
                GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
                if (sliderTemp != null){
                        sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
                        sliderVolumeCtrl.value = volumeLevel;
                }
        }

        void Start(){
                pauseMenuUI.SetActive(false);
                GameisPaused = false;
        }

        void Update(){
                if (Input.GetKeyDown(KeyCode.Escape)){
                        if (GameisPaused){
                                playSound.Play();
                                Resume();
                        }
                        else{
                                pauseSound.Play();
                                Pause();
                        }
                }
        }

        public void Pause(){
              if (!GameisPaused){
                pauseMenuUI.SetActive(true);
                Time.timeScale = 0f;
                GameisPaused = true;
                pauseSound.Play();}
             else{ Resume ();}
             //NOTE: This added conditional is for a pause button
        }

        public void Resume(){
                pauseMenuUI.SetActive(false);
                Time.timeScale = 1f;
                GameisPaused = false;
                playSound.Play();
        }

        public void SetLevel(float sliderValue){
                mixer.SetFloat("MasterVolume", Mathf.Log10 (sliderValue) * 20);
                volumeLevel = sliderValue;
        }
}