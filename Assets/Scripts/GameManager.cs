using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public SettingsManager settingsManager;
    public GameObject pauseMenu;
    private bool isPaused = false;
    public SceneAsset[] levels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(pauseMenu);
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
    }

    public static void LoadScene(int levelNum = 0)
    {
        if (Instance.levels.Length >= levelNum + 1)
        {
            SceneManager.LoadScene(Instance.levels[levelNum].name);
        }
    }

    public static void LoadNextLevel()
    {
        int completedLevels = PlayerPrefs.GetInt("CompletedLevels", 0);
        completedLevels++;
        PlayerPrefs.SetInt("CompletedLevels", completedLevels);
        LoadScene(completedLevels);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.N)) // Временное решение для перехода на следующий уровень
        {
            LoadNextLevel();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu?.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void QuitToMainMenu()
    {
        TogglePause();
        SceneManager.LoadScene(0);
    }
}
