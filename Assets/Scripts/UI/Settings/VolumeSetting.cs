using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "VolumeSetting", menuName = "Settings/Volume Setting")]
public class VolumeSetting : Setting
{
    public override SettingType SettingType { get; set; } = SettingType.Volume;

    public override object Value
    {
        get => AudioListener.volume;
        set => AudioListener.volume = (float)value;
    }

    public override void Load()
    {
        //volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.7f);
        //AudioListener.volume = volumeSlider.value;
        Value = PlayerPrefs.GetFloat("MasterVolume", 0.7f);
    }

    public override void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", (float)Value);
    }

    public override void Cancel()
    {
        Load();
    }

    //public void OnChange(object value)
    //{
    //    //AudioListener.volume = volumeSlider.value;
    //    AudioListener.volume = (float)value;
    //}
}
