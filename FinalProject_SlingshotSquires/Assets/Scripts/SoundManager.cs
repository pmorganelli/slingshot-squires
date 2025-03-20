using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

        public Slider volumeSlider;
        public AudioMixer masterMixer;
        private void start() 
        {
                SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
        }
        public void SetVolume(float value) 
        {
                if (value < 1) {
                        value = .001f;
                }
                RefreshSlider(value);
                PlayerPrefs.SetFloat("SavedMasterVolume", value);
                masterMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f);
        }
        public void SetVolumeFromSlider() 
        {
                SetVolume(volumeSlider.value);
        }
        public void RefreshSlider(float value) 
        {
                volumeSlider.value = value;
        }
}