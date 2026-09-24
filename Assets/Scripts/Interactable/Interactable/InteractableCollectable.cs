using UnityEngine;

public class InteractableCollectable : Interactable
{

    [Header("Interaction")]
    [SerializeField] private GameObject interactionPrompt;

    private Transform pickupParent;
    private Vector3 pickupLocalPosition;
    private Quaternion pickupLocalRotation;
    private Vector3 pickupWorldPosition;
    private Quaternion pickupWorldRotation;
    private bool hasPickupPosition;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetPromptVisible(false);
    }

    protected override void Start()
    {
        base.Start();
		//Debug.Log($"ID {GetID()}");
    }

    public override SO_InteractableData Interact() { return this.data; }

    /// <summary>Assigns the collectable data used by a spawned instance.</summary>
    public void Initialize(SO_InteractableCollectableData collectableData) { this.data = collectableData; }

    /// <summary>Records this instance's placement and hides it while carried.</summary>
    public void PickUp()
    {
        pickupParent = transform.parent;
        pickupLocalPosition = transform.localPosition;
        pickupLocalRotation = transform.localRotation;
        pickupWorldPosition = transform.position;
        pickupWorldRotation = transform.rotation;
        hasPickupPosition = true;
        transform.SetParent(null, true);
        gameObject.SetActive(false);
    }

    /// <summary>Returns this instance to the placement recorded when it was picked up.</summary>
    public void Restore()
    {
        if (!hasPickupPosition) { return; }

        if (pickupParent != null)
        {
            transform.SetParent(pickupParent, false);
            transform.SetLocalPositionAndRotation(pickupLocalPosition, pickupLocalRotation);
        }
        else
        {
            transform.SetParent(null);
            transform.SetPositionAndRotation(pickupWorldPosition, pickupWorldRotation);
        }

        gameObject.SetActive(true);
    }

    public override bool ChangeState(InteractionState interactionState) {
        this.state = interactionState;
        return true;
    }

    public override string ToString() {
        return ("[InteractableCollectable, ID=" + this.data.GetID() + 
            ", InteractionType=" + this.data.GetInteractionType() +
            ", InteractionState=" + this.state + 
            ", CollectableName=" + ((SO_InteractableCollectableData)this.data).GetCollectableName() +
            ", CollectableValue=" + ((SO_InteractableCollectableData)this.data).GetCollectableValue() + 
            "]");
    }
    
    private bool IsPlayer(Collider other) => other.CompareTag("Player") || other.transform.root.CompareTag("Player");
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            SetPromptVisible(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            SetPromptVisible(false);
        }
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
