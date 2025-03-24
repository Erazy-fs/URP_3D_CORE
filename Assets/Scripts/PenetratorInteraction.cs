using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PenetratorInteraction : MonoBehaviour, IInteractable
{
    public bool isReadyForLamding = true;
    public bool isArrived = false;
    public bool isActivated = false;
    public bool isDestroyed = false;
    public bool isFinished = false;
    public PenetratorIcon _penetratorIcon;
    public event Action<bool> OnPenetratorArrived;
    public event Action<bool> OnPenetratorActivated;

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
            CallPenetrator();
        else if (keyCode is KeyCode.E)
            ActivatePenetrator();
    }

    public void Disable()
    {
        this.enabled = false;
    }

    private void ActivatePenetrator()
    {
        if (isArrived && !isActivated && !isDestroyed && !isFinished)
        {
            GameObject ground = GameObject.Find("ground_1");
            if (ground != null)
            {
                ground.GetComponent<GroundControl>().Activate();
                isActivated = true;
                OnPenetratorActivated?.Invoke(isActivated);
            }
            // gameObject.SetActive(false);
        }
    }

    private void CallPenetrator()
    {
        if (isReadyForLamding && !isArrived && !isActivated && !isDestroyed && !isFinished)
        {
            if (_penetratorIcon is null)
                SetInteractionToIcon();

            isArrived = true;
            isReadyForLamding = false;
            OnPenetratorArrived?.Invoke(isArrived);
        }
    }

    private void SetInteractionToIcon()
    {
        _penetratorIcon = GameObject.Find("PenetratorIconPanel").GetComponent<PenetratorIcon>();
        _penetratorIcon.PenetratorInteraction = this;
    }
}
