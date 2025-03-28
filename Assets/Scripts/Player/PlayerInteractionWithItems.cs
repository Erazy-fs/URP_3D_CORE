using UnityEngine;
using TMPro;

public class PlayerInteractionWithItems : MonoBehaviour
{
    public Transform playerTransform;
    public float interactionDistance = 5f;
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;
    public GroundControl groundControl;

    void Update()
    {
        // if (isActivated is false)
            InteractionRay();
    }

    bool isActivated = false;
    private void InteractionRay()
    {
        // if (Input.GetKeyDown(KeyCode.F)){

        //     if (Physics.Raycast(transform.position, Vector3.down, out var hit, 2)) {
        //         var obj = hit.collider.gameObject;
        //         Debug.Log("object: " + obj.name);
        //         var plot = obj.GetComponent<PlotControl>();
        //         if (plot is not null)
        //             groundControl.CallInZEUS(hit.point, plot.colorIndex);
        //     } 
        // }
        // else
        // {
            Vector3 rayOrigin = playerTransform.position;
            Vector3 rayDirection = playerTransform.forward;
            RaycastHit hit;
            bool hitSomething = false;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactionDistance))
            {
                Transform rootTransform = hit.collider.transform.root;
                IInteractable interactable = rootTransform.GetComponent<IInteractable>();

                if (interactable is not null && interactable.CanInteract)
                {
                    hitSomething = true;
                    interactionText.text = interactable.GetDescription();

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.Interact(KeyCode.E);
                        hitSomething = false;
                        isActivated = true;
                    }
                }
            }
            interactionUI.SetActive(hitSomething);
        // }

    }
}
