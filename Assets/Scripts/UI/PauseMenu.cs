using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    private UIDocument document;
    private VisualElement pauseMenu;

    private Button buttonContinue;
    private Button buttonMainMenu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        document = GetComponent<UIDocument>();
        //DontDestroyOnLoad(document);
        pauseMenu = document.rootVisualElement.Q<VisualElement>("pause_menu");

        buttonContinue = pauseMenu.Q<Button>("button_continue");
        buttonMainMenu = pauseMenu.Q<Button>("button_main_menu");

        buttonContinue.clicked += () => { Debug.Log("buttonContinue"); GameManager.TogglePause(); };
        buttonMainMenu.clicked += () => { Debug.Log("buttonMainMenu"); GameManager.QuitToMainMenu(); };
    }

    public void SetVisible(bool visible)
    {
        pauseMenu.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
