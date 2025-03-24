using UnityEngine;

public class PenetratorInteraction : MonoBehaviour, IInteractable
{
    bool isActivated = false;
    public string GetDescription()
    {
        return "Активировать";
    }

    public void Interact()
    {
        if (isActivated is false)
        {
            GameObject ground = GameObject.Find("ground_1");
            if (ground != null)
            {
                ground.GetComponent<GroundControl>().Activate(); // Вызов метода
            }
            // gameObject.SetActive(false);
        }
    }
}
