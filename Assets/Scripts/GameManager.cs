using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool gameIsStarted = false;
    public SettingsManager settingsManager;
    public PauseMenu pauseMenu;
    public string[] levels;
    public bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(pauseMenu.gameObject);
            Debug.Log($"pauseMenu == null: {pauseMenu == null}");
            SceneManager.sceneLoaded += PrepareScene;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PrepareScene(Scene scene, LoadSceneMode loadSceneMode)
    {
        EventSystem[] eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (eventSystems.Length == 0)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
        settingsManager.Load();
        //settingsManager.SetUIEvents();
    }

    public static void StartGame()
    {
        gameIsStarted = true;
        try
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            InteractionWithItems interactionWithItems = player.GetComponent<InteractionWithItems>();
            TopDownControl topDownControl = player.GetComponent<TopDownControl>();
            interactionWithItems.enabled = true;
            topDownControl.enabled = true;
        }
        catch (Exception)
        {
        }
    }

    public static void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void LoadNextLevel()
    {
        int completedLevels = PlayerPrefs.GetInt("CompletedLevels", 0);
        completedLevels++;
        PlayerPrefs.SetInt("CompletedLevels", completedLevels);
        LoadScene(completedLevels);
    }

    public static void LoadScene(int levelNum = 0)
    {
        if (Instance.levels.Length >= levelNum + 1)
        {
            SceneManager.LoadScene(Instance.levels[levelNum]);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (gameIsStarted || SceneManager.GetActiveScene().buildIndex != 0))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.N)) // ��������� ������� ��� �������� �� ��������� �������
        {
            LoadNextLevel();
        }
    }

    public static void TogglePause()
    {
        Instance.isPaused = !Instance.isPaused;
        Instance.pauseMenu?.SetVisible(Instance.isPaused);
        Time.timeScale = Instance.isPaused ? 0f : 1f;
    }

    public static void QuitToMainMenu()
    {
        TogglePause();
        gameIsStarted = false;
        SceneManager.LoadScene(0);
    }

    public static bool IsPaused()
    {
        return Instance?.isPaused ?? false;
    }
}
