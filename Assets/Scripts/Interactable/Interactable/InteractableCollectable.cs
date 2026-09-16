using UnityEngine;

public class InteractableCollectable : Interactable {

    protected override void OnEnable() { base.OnEnable(); }

    protected override void Start() {
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
}
