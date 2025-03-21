using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartGame");
        SceneManager.LoadScene("EnemyAiTesting");
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
}
