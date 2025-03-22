using UnityEngine;
using TMPro;
using System;

public class InteractionWithItems : MonoBehaviour
{
    public Camera mainCamera;
    public float interactionDistance = 5f;
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    void Update()
    {
        InteractionRay();
    }

    private void InteractionRay()
    {
        Ray ray = mainCamera.ViewportPointToRay(Vector3.one / 2f);
        RaycastHit hit;
        bool hitSomething = false;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable is not null)
            {
                hitSomething = true;
                interactionText.text = interactable.GetDescription();

                if (Input.GetKeyDown(KeyCode.E))
                    interactable.Interact();
            }
        }

        if(interactionUI.activeSelf != hitSomething)
            interactionUI.SetActive(hitSomething);
    }
}
