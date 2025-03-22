using UnityEngine;

[CreateAssetMenu(fileName = "VolumeSetting", menuName = "Settings/Volume")]
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
}
