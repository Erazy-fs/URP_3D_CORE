using UnityEngine;

[CreateAssetMenu(fileName = "FullscreenSetting", menuName = "Settings/Fullscreen")]
public class FullscreenSetting : Setting
{
    public override SettingType SettingType { get; set; } = SettingType.Fullscreen;

    public override object Value
    {
        get => Screen.fullScreen;
        set => Screen.fullScreen = (bool)value;
    }

    public override void Load()
    {
        Value = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    public override void Save()
    {
        PlayerPrefs.SetInt("Fullscreen", (bool)Value ? 1 : 0);
    }

    public override void Cancel()
    {
        Load();
    }
}
