using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    private UIDocument document;

    private VisualElement mainMenu;

    private VisualElement generalMenu;
    private VisualElement startMenu;
    private VisualElement settingsMenu;

    private Button buttonStart;
    private Button buttonSettings;
    private Button buttonExit;

    private Button buttonNewGame;
    private Button buttonContinue;

    private Slider volumeSlider;

    private void Start()
    {
        document = GetComponent<UIDocument>();
        mainMenu = document.rootVisualElement.Q<VisualElement>("main_menu");
        if (!GameManager.gameIsStarted)
        {
            mainMenu.style.display = DisplayStyle.Flex;

            generalMenu = mainMenu.Q<VisualElement>("general_menu");
            startMenu = mainMenu.Q<VisualElement>("start_game");
            settingsMenu = mainMenu.Q<VisualElement>("settings");

            buttonStart = mainMenu.Q<Button>("button_start");
            buttonSettings = mainMenu.Q<Button>("button_settings");
            buttonExit = mainMenu.Q<Button>("button_exit");

            buttonNewGame = mainMenu.Q<Button>("button_new_game");
            buttonContinue = mainMenu.Q<Button>("button_continue");

            volumeSlider = mainMenu.Q<Slider>("volume");


            buttonStart.clicked += OpenStartMenu;
            buttonSettings.clicked += OpenSettingsMenu;
            buttonExit.clicked += QuitGame;

            buttonNewGame.clicked += StartNewGame;
            buttonContinue.clicked += ContinueGame;

            volumeSlider.value = (float)SettingsManager.GetSettingValue(SettingType.Volume);
            volumeSlider.RegisterValueChangedCallback(e => { SettingsManager.SetSettingValue(SettingType.Volume, e.newValue); SettingsManager.SaveSettings(); });
        }
        else
        {
            StartNewGame();
        }
    }

    public void OpenStartMenu()
    {
        Debug.Log("OpenStartMenu");
        int completedLevels = PlayerPrefs.GetInt("CompletedLevels", 0);
        buttonContinue.style.display = completedLevels == 0 ? DisplayStyle.None : DisplayStyle.Flex;
        settingsMenu.style.display = DisplayStyle.None;
        startMenu.style.display = startMenu.style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
    }

    public void StartNewGame()
    {
        Debug.Log("StartNewGame");
        mainMenu.style.display = DisplayStyle.None;
        PlayerPrefs.SetInt("CompletedLevels", 0);
        GameManager.StartGame();
        //GameManager.LoadScene();
    }

    public void ContinueGame()
    {
        int currentLevelIndex = PlayerPrefs.GetInt("CompletedLevels", 0);
        GameManager.LoadScene(currentLevelIndex);
    }

    public void OpenSettingsMenu()
    {
        Debug.Log("OpenSettingsMenu");
        startMenu.style.display = DisplayStyle.None;
        settingsMenu.style.display = settingsMenu.style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }

    public void SaveSettings()
    {
        SettingsManager.SaveSettings();
        //settingsMenu.SetActive(false);
    }

    public void CancelSettings()
    {
        //SettingsManager.CancelSettings();
        //settingsMenu.SetActive(false);
    }
}
