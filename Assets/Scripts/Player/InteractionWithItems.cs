using UnityEngine;
using TMPro;

public class InteractionWithItems : MonoBehaviour
{
    public Transform playerTransform;
    public float interactionDistance = 5f;
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;
    void Update()
    {
        if (isActivated is false)
            InteractionRay();
    }

    bool isActivated = false;
    private void InteractionRay()
    {
        Vector3 rayOrigin = playerTransform.position;
        Vector3 rayDirection = playerTransform.forward;
        RaycastHit hit;
        bool hitSomething = false;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable is not null)
            {
                hitSomething = true;
                interactionText.text = interactable.GetDescription();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                    hitSomething = false;
                    isActivated = true;
                }
            }
        }
        interactionUI.SetActive(hitSomething);
    }
}
