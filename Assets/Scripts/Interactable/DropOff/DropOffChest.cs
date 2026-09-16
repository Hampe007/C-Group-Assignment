using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropOffChest : Interactable
{
    [Header("Interaction")]
    [SerializeField] private GameObject interactionPrompt;

    [Header("Chest")]
    [SerializeField] private DropOffChestAnimator chestAnimator;

    private bool playerInRange;

    // Score/game systems can subscribe to this.
    // Example: score can use droppedItem.GetCollectableValue().
    public event Action<SO_InteractableCollectableData> ItemDroppedOff;

    private bool HasItemToDropOff() => Inventory.Instance != null && Inventory.Instance.GetLastCollectable() != null;

    private bool ShouldShowPrompt() =>
        playerInRange &&
        HasItemToDropOff();

    protected override void OnEnable()
    {
        base.OnEnable();
        SetPromptVisible(false);
    }

    private void Update() => SetPromptVisible(ShouldShowPrompt());

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other))
        {
            return;
        }

        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other))
        {
            return;
        }

        playerInRange = false;
        SetPromptVisible(false);
    }

    public override SO_InteractableData Interact()
    {
        if (!HasItemToDropOff())
        {
            return data;
        }

        // Removes the most recently collected item from the inventory.
        SO_InteractableCollectableData droppedItem =
            Inventory.Instance.RemoveAndGetLastCollectable();

        if (droppedItem == null)
        {
            return data;
        }

        chestAnimator?.PlayDepositAnimation();

        // Example: score system can subscribe here.
        ItemDroppedOff?.Invoke(droppedItem);

        SetPromptVisible(false);

        return data;
    }

    private bool IsPlayer(Collider other) => other.CompareTag("Player") || other.transform.root.CompareTag("Player");

    private void SetPromptVisible(bool visible)
    {
        if (interactionPrompt != null &&
            interactionPrompt.activeSelf != visible)
        {
            interactionPrompt.SetActive(visible);
        }
    }
}
