using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    void Start()
    {
        LoadSettings();
    }

    public void ApplySettings()
    {
        AudioListener.volume = volumeSlider.value;
        Screen.fullScreen = fullscreenToggle.isOn;

        //QualitySettings.SetQualityLevel(resolutionDropdown.value);

        SaveSettings();
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        //PlayerPrefs.SetInt("QualityLevel", resolutionDropdown.value);
    }

    void LoadSettings()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);

        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = fullscreenToggle.isOn;


        //resolutionDropdown.value = PlayerPrefs.GetInt("QualityLevel", 2);
        //QualitySettings.SetQualityLevel(resolutionDropdown.value);
    }
}