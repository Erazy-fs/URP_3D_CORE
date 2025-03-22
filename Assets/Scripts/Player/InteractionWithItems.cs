using UnityEngine;
using TMPro;

public class InteractionWithItems : MonoBehaviour
{
    public Transform playerTransform;
    public float interactionDistance = 5f;
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;

    private GameObject heldItem = null; // Текущий предмет в инвентаре

    void Update()
    {
        if (heldItem != null)
            PlaceItem(); // Если предмет уже в руках, ставим его перед персонажем
        else
            InteractionRay(); // Если предмета нет, пытаемся взять новый
    }

    private void InteractionRay()
    {
        if (heldItem != null) return; // Если уже что-то держим, пропускаем

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
                    heldItem = hit.collider.gameObject;
                    heldItem.SetActive(false); // Отключаем объект
                    hitSomething = false;
                }
            }
        }
        interactionUI.SetActive(hitSomething);
    }

    private void PlaceItem()
    {
        if (heldItem == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Размещаем предмет перед персонажем
            heldItem.transform.position = playerTransform.position + playerTransform.forward * 1.5f;
            heldItem.SetActive(true);
            heldItem = null; // Очищаем инвентарь
        }
    }
}
