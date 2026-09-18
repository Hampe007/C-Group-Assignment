using UnityEngine;

public class InteractableCollectable : Interactable
{

    [Header("Interaction")]
    [SerializeField] private GameObject interactionPrompt;

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
