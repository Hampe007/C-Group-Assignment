using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerInteraction : MonoBehaviour
{
    private PlayerInteractZone interactZone;

    private void Awake() {
        interactZone = GetComponentInChildren<PlayerInteractZone>(true);

        if (interactZone == null) {
            Debug.LogError(
                $"{nameof(PlayerInteraction)} requires a {nameof(PlayerInteractZone)} " +
                "somewhere under the Player hierarchy.",
                this);
        }
    }

    /*private void OnEnable()
    {
        if (interact == null || interact.action == null)
        {
            Debug.LogError(
                $"{nameof(PlayerInteraction)} has no Interact Input Action assigned.",
                this);
            return;
        }

        interact.action.performed += OnInteract;
        interact.action.Enable();
    }

    private void OnDisable()
    {
        if (interact == null || interact.action == null)
        {
            return;
        }

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

        if (interactable is InteractableCollectable collectable)
        {
            Collider collectableCollider = collectable.GetComponent<Collider>();

            // Prevent the same collectable being added twice before it is destroyed.
            if (collectableCollider == null || !collectableCollider.enabled)
            {
                return;
            }

            if (Inventory.Instance == null)
            {
                Debug.LogWarning("No Inventory instance exists in the scene.", this);
                return;
            }

            if (collectable.Interact() is not SO_InteractableCollectableData collectableData)
            {
                return;
            }

            if (!Inventory.Instance.AddCollectable(collectableData))
            {
                return;
            }

            collectableCollider.enabled = false;
            interactZone.PickedUpInteractable();
            return;
        }

        // DropOffChest and any future non-collectable interactables handle
        // their own behaviour in Interact().
        interactable.Interact();
    }*/

    public void TriggerInteract() {
        if (interactZone == null) { return; }

        Interactable interactable = interactZone.GetLastInteractable();

        if (interactable == null) { return; }

        if (interactable is InteractableCollectable collectable) {
            Collider collectableCollider = collectable.GetComponent<Collider>();

            // Prevent the same collectable being added twice before it is destroyed.
            if (collectableCollider == null || !collectableCollider.enabled) {
                return;
            }

            if (Inventory.Instance == null) {
                Debug.LogWarning("No Inventory instance exists in the scene.", this);
                return;
            }

            if (collectable.Interact() is not SO_InteractableCollectableData collectableData) {
                return;
            }

            if (!Inventory.Instance.AddCollectable(collectableData)) {
                return;
            }

            collectableCollider.enabled = false;
            interactZone.PickedUpInteractable();
            return;
        }

        // DropOffChest and any future non-collectable interactables handle
        // their own behaviour in Interact().
        interactable.Interact();
    }
}