using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InputActionReference interact;

    private PlayerInteractZone interactZone;

    private void Awake() =>
        interactZone = GetComponentInChildren<PlayerInteractZone>();

    private void OnEnable()
    {
        interact.action.Enable();
        interact.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        interact.action.performed -= OnInteract;
        interact.action.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (interactZone == null)
        {
            return;
        }

        Interactable interactable = interactZone.GetLastInteractable();

        if (interactable == null)
        {
            return;
        }

        SO_InteractableData result = interactable.Interact();

        if (result == null || Inventory.Instance == null)
        {
            return;
        }

        switch (interactable)
        {
            case InteractableCollectable collectable:
            {
                if (result is not SO_InteractableCollectableData collectableData)
                {
                    return;
                }

                if (Inventory.Instance.AddCollectable(collectableData))
                {
                    Collider collectableCollider = collectable.GetComponent<Collider>();

                    if (collectableCollider != null)
                    {
                        collectableCollider.enabled = false;
                    }

                    interactZone.PickedUpInteractable();
                }

                break;
            }

            // DropOffChest handles removing the item from Inventory in Interact().
            case DropOffChest:
                break;
        }
    }
}
