using System;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public UnityEvent OnClick;
    private void OnMouseDown()
    {
        Debug.Log("ButtonClicked");
    }
}
