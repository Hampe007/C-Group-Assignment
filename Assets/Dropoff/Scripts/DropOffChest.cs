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
    
    private void Awake()
    {
        SetPromptVisible(false);

        Debug.Log("Initialized and hid the interaction prompt.");

        if (interactAction == null)
        {
            Debug.LogWarning($"[{nameof(DropOffChest)}] No interact action is assigned.", this);
        }

        if (chestAnimator == null)
        {
            Debug.LogWarning($"[{nameof(DropOffChest)}] No chest animator is assigned.", this);
        }
    } 
    
    private void OnEnable()
    {
        if (interactAction != null && !interactAction.action.enabled)
        {
            interactAction.action.Enable();
            Debug.Log($"Enabled input action '{interactAction.action.name}'.");
        }
    }
    
    private void Update()
    {
        SetPromptVisible(CanDropOff());

        if (CanDropOff() && interactAction != null && interactAction.action.WasPressedThisFrame())
        {
            Debug.Log("Interact input received while an item can be dropped off.");
            DropOff();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        dropOffProvider = FindDropOffProvider(other);

        if (dropOffProvider != null)
        {
            playerInRange = true;
            Debug.Log($"Player entered range. Found provider '{dropOffProvider.GetType().Name}'. Has item: {dropOffProvider.HasItem()}.");
            return;
        }

        Debug.LogWarning($"[{nameof(DropOffChest)}] Player entered range, but no {nameof(IDropOffProvider)} was found.", other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerInRange = false;
        dropOffProvider = null;

        SetPromptVisible(false);
        Debug.Log("Player left range; cleared the drop-off provider.");
    }
    
    private void DropOff()
    {
        if (!CanDropOff())
        {
            Debug.Log("Drop-off was requested, but its requirements were no longer met.");
            return;
        }
        
        GameObject droppedItem = dropOffProvider.DropOffItem();

        if (droppedItem == null)
        {
            Debug.LogWarning($"[{nameof(DropOffChest)}] The provider returned no item to drop off.", this);
            return;
        }

        Debug.Log($"Dropping off '{droppedItem.name}'.");
        chestAnimator?.PlayDepositAnimation();

        // Score/game system is notified here.
        ItemDroppedOff?.Invoke(droppedItem);
        Debug.Log($"Notified subscribers and destroyed '{droppedItem.name}'.");

        Destroy(droppedItem);

        SetPromptVisible(false);
    }
    
    private static IDropOffProvider FindDropOffProvider(Collider playerCollider)
    {
        MonoBehaviour[] behaviours = playerCollider.GetComponentsInParent<MonoBehaviour>();

        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IDropOffProvider provider)
            {
                return provider;
            }
        }
        
        return null;
    }
    
    private void SetPromptVisible(bool visible)
    {
        if (interactionPrompt != null && interactionPrompt.activeSelf != visible)
        {
            interactionPrompt.SetActive(visible);
            Debug.Log($"Interaction prompt is now {(visible ? "visible" : "hidden")}.");
        }
    }
    
}
