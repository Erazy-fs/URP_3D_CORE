using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PenetratorInteraction : MonoBehaviour, IInteractable
{
    public bool isReadyForLanding = true;
    public bool isReadyForActivation = false;
    public bool isArrived = false;
    public bool isActivated = false;
    public bool isDestroyed = false;
    public bool isFinished = false;
    public PenetratorIcon _penetratorIcon;
    public LevelNarrator narrator;

    public bool CanInteract { get; set; }

    public event Action<bool> OnPenetratorArrived;
    public event Action<bool> OnPenetratorActivated;

    void Start()
    {
        // _penetratorIcon = GameObject.Find("PenetratorIconPanel").GetComponent<PenetratorIcon>();
        // _penetratorIcon.PenetratorInteraction = this;
        narrator.OnReadyForActivation += ReadyForActivation;
    }

    private void ReadyForActivation(bool isReady)
    {
        CanInteract = isReady;
        narrator.OnReadyForActivation -= ReadyForActivation;
    }

    public string GetDescription()
    {
        return "Активировать";
    }

    public void Interact(KeyCode keyCode)
    {
        // if (keyCode is KeyCode.F)
        //     CallPenetrator();
        // else if (keyCode is KeyCode.E)
        if (keyCode is KeyCode.E)
            ActivatePenetrator();
    }

    public void Disable()
    {
        this.enabled = false;
    }

    private void ActivatePenetrator()
    {
        if (CanInteract && !isActivated && !isDestroyed && !isFinished)
        {
            GameObject ground = GameObject.Find("ground_1");
            if (ground != null)
            {
                ground.GetComponent<GroundControl>().Activate();
                CanInteract = false;
                isActivated = true;
                OnPenetratorActivated?.Invoke(isActivated);
            }
            // gameObject.SetActive(false);
        }
    }

    private void CallPenetrator()
    {
        if (isReadyForLanding && !isArrived && !isActivated && !isDestroyed && !isFinished)
        {
            if (_penetratorIcon is null)
                SetInteractionToIcon();

            isArrived = true;
            isReadyForLanding = false;
            OnPenetratorArrived?.Invoke(isArrived);
        }
    }

    private void SetInteractionToIcon()
    {
        var penetratorIconPanel = GameObject.Find("PenetratorIconPanel");
        if (penetratorIconPanel is not null)
        {
            _penetratorIcon = penetratorIconPanel.GetComponent<PenetratorIcon>();
            _penetratorIcon.PenetratorInteraction = this;
        }
    }
}
