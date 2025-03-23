using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject settingsMenu;

    public void OpenStartMenu()
    {
        if (!startMenu.activeSelf)
        {
            int completedLevels = PlayerPrefs.GetInt("CompletedLevels", 0);
            RectTransform rectTransform = startMenu.GetComponent<RectTransform>();
            int height = 200;
            if (completedLevels == 0)
            {
                height = 100;
                Button newGameButton = startMenu.GetComponentsInChildren<Button>().FirstOrDefault(b => b.onClick.GetPersistentMethodName(0) == "ContinueGame");
                if (newGameButton != null)
                {
                    newGameButton.gameObject.SetActive(false);
                }
            }
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        }
        settingsMenu.SetActive(false);
        startMenu.SetActive(!startMenu.activeSelf);
    }

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("CompletedLevels", 0);
        GameManager.LoadScene();
    }

    public void ContinueGame()
    {
        int currentLevelIndex = PlayerPrefs.GetInt("CompletedLevels", 0);
        GameManager.LoadScene(currentLevelIndex);
    }

    public void OpenSettingsMenu()
    {
        startMenu.SetActive(false);
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    public void QuitGame()
    {
        throw new NotImplementedException("QuitGame");
    }

    public void SaveSettings()
    {
        SettingsManager.SaveSettings();
        settingsMenu.SetActive(false);
    }

    public void CancelSettings()
    {
        SettingsManager.CancelSettings();
        settingsMenu.SetActive(false);
    }
}
