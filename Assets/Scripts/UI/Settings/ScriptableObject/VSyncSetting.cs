using UnityEngine;

[CreateAssetMenu(fileName = "VSyncSetting", menuName = "Settings/VSync")]
public class VSyncSetting : Setting
{
    public override SettingType SettingType { get; set; } = SettingType.VSync;

    public override object Value
    {
        get => QualitySettings.vSyncCount == 1;
        set => QualitySettings.vSyncCount = (bool)value ? 1 : 0;
    }

    public override void Load()
    {
        Value = PlayerPrefs.GetInt("VSync", 1) == 1;
    }

    public override void Save()
    {
        PlayerPrefs.SetInt("VSync", (bool)Value ? 1 : 0);
    }

    public override void Cancel()
    {
        Load();
    }
}
