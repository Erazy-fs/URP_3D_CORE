using UnityEngine;

public class PenetratorInteraction : MonoBehaviour, IInteractable
{
    public string GetDescription()
    {
        return "Взять";
    }

    public void Interact()
    {
        gameObject.SetActive(false);
    }
}
