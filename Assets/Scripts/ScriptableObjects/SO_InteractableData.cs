using UnityEngine;

public enum InteractionType : byte {
    /*STATIC_OBSTACLE = 0,
    MOVING_OBSTACLE = 1,
    TRANSITION = 2,
    TRANSITION_TO_AREA = 3,
    CUTSCENE = 4,
    VERBAL = 5,
    ORTHOGRAPHIC = 6,
    ITEM = 7,
    SPELL_ATTRIBUTE = 8,
    BUTTON = 9*/

    STATIC_OBSTACLE = 0,
    MOVING_OBSTACLE = 1,
    ORTHOGRAPHIC = 2,
    ITEM = 3
}

//[CreateAssetMenu(menuName = "Interactable Data")]
public class SO_InteractableData : ScriptableObject {
    [SerializeField] protected int interactableID = -1;
    protected InteractionType type = InteractionType.STATIC_OBSTACLE;

    protected virtual void OnEnable() {}

    public int GetID() { return this.interactableID; }

    public InteractionType GetInteractionType() { return this.type; }

    public bool SetID(int interactableID) {
        if (interactableID < 0) { return false; }
        this.interactableID = interactableID;
        return true;
    }

    public virtual void SetInteractionType(InteractionType type) { this.type = type; }

    public virtual bool ChangeData(int interactableID, InteractionType type) {
        if (interactableID < 0) { return false; }
        this.interactableID = interactableID;
        this.type = type;
        return true;
    }
}
