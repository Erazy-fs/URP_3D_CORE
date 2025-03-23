using UnityEngine;

public interface IInteractable
{
    void Interact(KeyCode keyCode);
    string GetDescription();
}
