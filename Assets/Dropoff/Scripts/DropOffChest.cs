using System;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class DropOffChest : MonoBehaviour
{
    
    [Header("Interaction")]
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private GameObject interactionPrompt;

    [Header("Chest")]
    [SerializeField] private DropOffChestAnimator chestAnimator;

    private IDropOffProvider dropOffProvider;
    private bool playerInRange;

    // Score/game systems can subscribe to this.
    // Example: dropOffChest.ItemDroppedOff += OnItemDroppedOff;
    public event Action<GameObject> ItemDroppedOff;
    
    private bool CanDropOff() => playerInRange && dropOffProvider != null && dropOffProvider.HasItem();
    
    // Update is called once per frame
    private void Update()
    {

    }

    private static IDropOffProvider FindDropOffProvider(Collider playerCollider)
    {
        MonoBehaviour[] behaviours = playerCollider.GetComponentsInParent<MonoBehaviour>();

        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IDropOffProvider provider)
                return provider;
        }

        return null;
    }
    
    private void SetPromptVisible(bool visible)
    {
        if (interactionPrompt != null &&
            interactionPrompt.activeSelf != visible)
        {
            interactionPrompt.SetActive(visible);
        }
    }
}
