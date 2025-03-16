using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutions.Select(r => $"{r.width} x {r.height}").ToList());

        resolutionDropdown.value = resolutions.ToList().FindIndex(r => r.width == Screen.currentResolution.width && r.height == Screen.currentResolution.height);

        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Debug.Log($"SetResolution: {resolutionIndex}");
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}