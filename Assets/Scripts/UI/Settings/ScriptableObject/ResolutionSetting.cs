using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ResolutionSetting", menuName = "Settings/Resolution")]
public class ResolutionSetting : Setting
{
    public override SettingType SettingType { get; set; } = SettingType.Resolution;
    public override object Value
    {
        get => resolutions.FindIndex(r => r.width == Screen.currentResolution.width && r.height == Screen.currentResolution.height);
        set => Screen.SetResolution(resolutions[(int)value].width, resolutions[(int)value].height, Screen.fullScreen);
    }
    public override List<object> Options { get => resolutions.Select(r => (object)$"{r.width} x {r.height}").ToList(); }
    private List<Resolution> resolutions;

    private void OnEnable()
    {
        resolutions = Screen.resolutions.ToList();
    }

    public override void Load()
    {
        Value = PlayerPrefs.GetInt("Resolution", resolutions.Count - 1);
    }

    public override void Save()
    {
        PlayerPrefs.SetInt("Resolution", (int)Value);
    }

    public override void Cancel()
    {
        Load();
    }
}
