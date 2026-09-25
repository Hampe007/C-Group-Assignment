using UnityEngine;

public abstract class Interactable : MonoBehaviour, IInteractable {
    [SerializeField] protected SO_InteractableData data;
    [SerializeField] protected InteractionState state = InteractionState.ON;

    protected virtual void OnEnable() {}

    protected virtual void Start() {
        // Initialize the ID from the database - Must be called in Start() to ensure the database is loaded
        // this.InitializeIDFromDatabase();
    }

    public virtual SO_InteractableData Interact() { return this.data; }

    public SO_InteractableData GetInteractableData() { return this.data; }

    public GameObject GetGameObject() { return this.gameObject; }

    public void Activate() { this.state = InteractionState.ON; }

    public void Deactivate() { this.state = InteractionState.OFF; }

    public InteractionType GetInteractionType() { return this.data.GetInteractionType(); }

    public InteractionState GetInteractionState() { return this.state; }

    public bool SetID(int interactableID) { return this.data.SetID(interactableID); }

    public int GetID() { return this.data.GetID(); }

    public void SetInteractionType(InteractionType type) { this.data.SetInteractionType(type); }

    public virtual bool ChangeState(InteractionState interactionState) {
        this.state = interactionState;
        return true;
    }

    public override string ToString() {
        return ("[Interactable, ID=" + this.data.GetID() + 
            ", InteractionType=" + this.data.GetInteractionType() +
            ", InteractionState=" + this.state + 
            "]");
    }
}
