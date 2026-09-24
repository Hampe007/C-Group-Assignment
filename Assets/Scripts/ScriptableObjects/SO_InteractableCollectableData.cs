using UnityEngine;

[CreateAssetMenu(fileName = "InteractableCollectableData", menuName = "Scriptable Objects/Interactables/Collectable Data")]
public class SO_InteractableCollectableData : SO_InteractableData {
    [SerializeField] protected string collectableName;
    [SerializeField] protected uint collectableValue = 0;
    [SerializeField] private GameObject collectablePrefab;

    protected override void OnEnable() {
        base.OnEnable();
        this.type = InteractionType.ITEM;
        if (string.IsNullOrEmpty(this.collectableName)) { this.collectableName = "default_collectable_name"; }
    }

    public override void SetInteractionType(InteractionType type) { this.type = InteractionType.ITEM; }

    public override bool ChangeData(int interactableID, InteractionType type) {
        if (interactableID < 0) { return false; }
        this.interactableID = interactableID;
        this.type = InteractionType.ITEM;
        return true;
    }

    public bool ChangeData(int interactableID, string collectableName) {
        if (interactableID < 0) { return false; }
        this.interactableID = interactableID;
        if (string.IsNullOrEmpty(collectableName)) { 
            this.collectableName = "default_collectable_name"; 
        } else { this.collectableName = collectableName; }
        return true;
    }

    public bool ChangeData(int interactableID, uint collectableValue) {
        if (interactableID < 0) { return false; }
        this.interactableID = interactableID;
        this.collectableValue = collectableValue;
        return true;
    }

    public bool ChangeData(string collectableName, uint collectableValue) {
        if (string.IsNullOrEmpty(collectableName)) { 
            this.collectableName = "default_collectable_name"; 
        } else { this.collectableName = collectableName; }
        this.collectableValue = collectableValue;
        return true;
    }

    public bool ChangeData(int interactableID, string collectableName, uint collectableValue) {
        if (interactableID < 0) { return false; }
        this.interactableID = interactableID;
        if (string.IsNullOrEmpty(collectableName)) { 
            this.collectableName = "default_collectable_name"; 
        } else { this.collectableName = collectableName; }
        this.collectableValue = collectableValue;
        return true;
    }

    public string GetCollectableName() { return this.collectableName; }

    public uint GetCollectableValue() { return this.collectableValue; }

    public GameObject GetCollectablePrefab() { return this.collectablePrefab; }

    public void SetCollectableName(string collectableName) {
        if (string.IsNullOrEmpty(collectableName)) { 
            this.collectableName = "default_collectable_name"; 
        } else { this.collectableName = collectableName; } 
    }

    public void SetCollectableValue(uint collectableValue) { 
        this.collectableValue = collectableValue;
    }
}
