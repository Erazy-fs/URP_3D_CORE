using System;
using UnityEngine;
using UnityEngine.UI;

public class PenetratorInteraction : MonoBehaviour, IInteractable
{
    bool isActivated = false;
    bool isArrived = false;
    public event Action<bool> OnPenetratorArrived;
    public event Action<bool> OnPenetratorActivated;
    public PenetratorIcon _penetratorIcon;

    void Start()
    {
        // _penetratorIcon = GameObject.Find("PenetratorIconPanel").GetComponent<PenetratorIcon>();
        // _penetratorIcon.PenetratorInteraction = this;
    }
    public string GetDescription()
    {
        return "Активировать";
    }

    public void Interact(KeyCode keyCode)
    {
        if (keyCode is KeyCode.F)
        {
            if (_penetratorIcon is null)
            {
                _penetratorIcon = GameObject.Find("PenetratorIconPanel").GetComponent<PenetratorIcon>();
                _penetratorIcon.PenetratorInteraction = this;
            }
            
            isArrived = true;
            OnPenetratorArrived?.Invoke(isArrived);
        }
        else if(keyCode is KeyCode.E)
        {
            if (isActivated is false)
            {
                GameObject ground = GameObject.Find("ground_1");
                if (ground != null)
                {
                    ground.GetComponent<GroundControl>().Activate(); // Вызов метода
                    isActivated = true;
                    OnPenetratorActivated?.Invoke(isActivated);
                }
                // gameObject.SetActive(false);
            }
        }
    }
}
