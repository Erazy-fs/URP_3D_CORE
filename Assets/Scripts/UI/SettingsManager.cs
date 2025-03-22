using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    [SerializeField]
    private List<Setting> _settings = new List<Setting>();
    private Dictionary<SettingType, Setting> _settingsByType;
    private event Action SelectablesSetValues;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Load()
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
                    SelectablesSetValues += () => toggle.isOn = (bool)setting.Value;
                    toggle.isOn = (bool)setting.Value;
                    toggle.onValueChanged.AddListener(_ => setting.Value = toggle.isOn);
                    break;
                case TMP_Dropdown dropdown:
                    dropdown.ClearOptions();
                    dropdown.AddOptions(setting.Options.Cast<string>().ToList());
                    SelectablesSetValues += () => dropdown.value = (int)setting.Value;
                    dropdown.value = (int)setting.Value;
                    dropdown.onValueChanged.AddListener(_ => setting.Value = dropdown.value);
                    break;
            }
        }
    }

    public static void SaveSettings()
    {
        Instance._settings.ForEach(s => s.Save());
    }

    public static void CancelSettings()
    {
        Instance._settings.ForEach(s => s.Cancel());
        Instance.SelectablesSetValues?.Invoke();
    }
}