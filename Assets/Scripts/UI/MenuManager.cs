using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("StartGame");
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
