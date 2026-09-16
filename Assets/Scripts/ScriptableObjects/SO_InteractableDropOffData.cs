using UnityEngine;

[CreateAssetMenu(
    fileName = "InteractableDropOffData",
    menuName = "Scriptable Objects/Interactables/Drop Off Data")]
public class SO_InteractableDropOffData : SO_InteractableData
{
    protected override void OnEnable()
    {
        base.OnEnable();

        // A drop-off data asset should always be DROPOFF.
        type = InteractionType.DROPOFF;
    }

    public override void SetInteractionType(InteractionType type) =>
        this.type = InteractionType.DROPOFF;

    public override bool ChangeData(int interactableID, InteractionType type)
    {
        if (interactableID < 0)
        {
            return false;
        }

        this.interactableID = interactableID;
        this.type = InteractionType.DROPOFF;

        return true;
    }
}
