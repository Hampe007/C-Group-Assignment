using UnityEngine;

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

    public void TriggerInteract() {
        if (interactZone == null) { return; }

        Interactable interactable = interactZone.GetLastInteractable();

        if (interactable == null) { return; }

        if (interactable is InteractableCollectable collectable) {
            Collider collectableCollider = collectable.GetComponent<Collider>();

            // Disabling the collider blocks another input event from collecting the same object.
            if (collectableCollider == null || !collectableCollider.enabled) {
                return;
            }

            if (Inventory.Instance == null) {
                Debug.LogWarning("No Inventory instance exists in the scene.", this);
                return;
            }

            if (!Inventory.Instance.AddCollectable(collectable)) {
                return;
            }

            interactZone.PickedUpInteractable(collectable.gameObject);
            return;
        }

        // DropOffChest and any future non-collectable interactables handle
        // their own behaviour in Interact().
        interactable.Interact();
    }
}
