using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartGame");
        PlayerPrefs.SetInt("CompletedLevels", 0);
        GameManager.LoadScene();
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("OpenSettingsMenu");
        Debug.Log(Screen.resolutions.Length);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
    }

    public void SaveSettings()
    {
        SettingsManager.SaveSettings();
    }

    public void CancelSettings()
    {
        SettingsManager.CancelSettings();
    }
}
