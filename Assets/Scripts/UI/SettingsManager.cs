using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Rendering.FilterWindow;

public class SettingsManager : MonoBehaviour
{
    //public Slider volumeSlider;
    //public Toggle fullscreenToggle;
    //public TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private List<Setting> _settings = new List<Setting>();
    private Dictionary<SettingType, Setting> _settingsByType;
    private event Action SelectablesSetValues;

    private void Start()
    {
        _settings.ForEach(s => s.Load());
        _settingsByType = _settings.ToDictionary(s => s.SettingType, s => s);
        SettingInput[] settingInputs = FindObjectsByType<SettingInput>(FindObjectsSortMode.None);
        foreach (SettingInput settingInput in settingInputs)
        {
            Setting setting = _settingsByType[settingInput.settingType];
            Selectable selectable = settingInput.gameObject.GetComponent<Selectable>();
            switch (selectable)
            {
                case Button button:
                    //button.onClick.AddListener(PlayClickSound);
                    break;
                case Slider slider:
                    SelectablesSetValues += () => slider.value = (float)setting.Value;
                    slider.value = (float)setting.Value;
                    slider.onValueChanged.AddListener(_ => setting.Value = slider.value);
                    break;
                case Toggle toggle:
                    //StartCoroutine(DelayedListener(() => toggle.onValueChanged.AddListener(_ => PlayClickSound())));
                    break;
                case TMP_Dropdown dropdown:
                    //StartCoroutine(DelayedListener(() => dropdown.onValueChanged.AddListener(_ => PlayClickSound())));
                    //AddClickSoundToDropdown(dropdown);
                    break;
            }
        }
    }


    //private void Start()
    //{
    //    foreach (Selectable element in GetComponentsInChildren<Selectable>())
    //    {
    //        Setting setting = element.GetComponent<Setting>();
    //        if (setting is not null)
    //        {
    //            _settings.Add(setting);
    //            setting.Load();
    //            switch (element)
    //            {
    //                case Button button:
    //                    //button.onClick.AddListener(PlayClickSound);
    //                    break;
    //                case Slider slider:
    //                    slider.value = (float)setting.Value;
    //                    slider.onValueChanged.AddListener(_ => setting.Value = slider.value);
    //                    //StartCoroutine(DelayedListener(() => slider.onValueChanged.AddListener(_ => PlayClickSound())));
    //                    break;
    //                case Toggle toggle:
    //                    //StartCoroutine(DelayedListener(() => toggle.onValueChanged.AddListener(_ => PlayClickSound())));
    //                    break;
    //                case TMP_Dropdown dropdown:
    //                    //StartCoroutine(DelayedListener(() => dropdown.onValueChanged.AddListener(_ => PlayClickSound())));
    //                    //AddClickSoundToDropdown(dropdown);
    //                    break;
    //            }
    //        }
    //    }
    //}

    private void ApplySettings()
    {
        //AudioListener.volume = volumeSlider.value;
        //Screen.fullScreen = fullscreenToggle.isOn;

        //QualitySettings.SetQualityLevel(resolutionDropdown.value);

        //SaveSettings();
    }

    public void SaveSettings()
    {
        _settings.ForEach(s => s.Save());
        //PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);
        //PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        //PlayerPrefs.SetInt("QualityLevel", resolutionDropdown.value);
    }

    public void CancelSettings()
    {
        _settings.ForEach(s => s.Cancel());
        SelectablesSetValues?.Invoke();
    }

    private void LoadSettings()
    {
        //volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.7f);
        //AudioListener.volume = volumeSlider.value;

        //fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        //Screen.fullScreen = fullscreenToggle.isOn;


        //resolutionDropdown.value = PlayerPrefs.GetInt("QualityLevel", 2);
        //QualitySettings.SetQualityLevel(resolutionDropdown.value);
    }

}