using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    private Bus masterBus;

    void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/"); // Reference to your FMOD bus
        volumeSlider.onValueChanged.AddListener(SetVolume);
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        SetVolume(volumeSlider.value);
    }

    public void SetVolume(float volume)
    {
        masterBus.setVolume(volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
