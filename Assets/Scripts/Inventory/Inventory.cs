using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    /*private static Inventory _instance;
    private static Inventory Instance {
        get {
            if (_instance == null) { _instance = FindFirstObjectByType<Inventory>(); }

            return _instance;
        }
    }*/

    public static Inventory Instance { get; private set; }

    [SerializeField] private CarriedCollectableList collectables = new();

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public SO_InteractableCollectableData GetFirstCollectable() {
        if (this.collectables.Count > 0)
            return this.collectables[0];
        return null;
    }

    public SO_InteractableCollectableData GetLastCollectable() {
        if (this.collectables.Count > 0)
            return this.collectables[^1];
        return null;
    }

    public SO_InteractableCollectableData GetCollectableByIndex(int index) {
        if (this.collectables.Count == 0) { return null; }
        if (index < 0 || index >= collectables.Count) { return null; }
        return this.collectables[index];
    }

    public SO_InteractableCollectableData GetCollectableByName(string collectableName) {
        if (this.collectables.Count == 0) { return null; }
        if (string.IsNullOrEmpty(collectableName)) { return null; }
        foreach (SO_InteractableCollectableData data in collectables) {
            if (data.GetCollectableName() == collectableName) {
                return data;
            }
        }
        return null;
    }

    public SO_InteractableCollectableData GetCollectableByID(int collectableID) {
        if (this.collectables.Count == 0) { return null; }
        if (collectableID < 0) { return null; }
        foreach (SO_InteractableCollectableData data in collectables) {
            if (data.GetID() == collectableID) {
                return data;
            }
        }
        return null;
    }

    /// <summary>
    /// Adds the world instance to the inventory and hides it so it can be restored if the player dies.
    /// </summary>
    public bool AddCollectable(InteractableCollectable collectable) {
        SO_InteractableCollectableData collectableData = collectable == null ? null : collectable.GetInteractableData() as SO_InteractableCollectableData;
        if (collectableData == null || !collectable.gameObject.activeInHierarchy)
        {
            return false;
        }
        if (collectableData.GetID() < 0)
        {
            return false;
        }
        if (this.collectables.Contains(collectable))
        {
            return false;
        }
        this.collectables.Add(collectable);
        collectable.PickUp();
        SoundFXManager.Instance.PlaySoundFXClip(AudioUtils.SoundEffects.pickupSound.audioClip, transform, AudioUtils.SoundEffects.pickupSound.volume);
        return true;
    }

    public bool RemoveFirstCollectable() {
        if (this.collectables.Count > 0) {
            this.collectables.RemoveAt(0);
            return true;
        }
        return false;
    }

    public bool RemoveLastCollectable() {
        if (this.collectables.Count > 0) {
            this.collectables.RemoveAt(this.collectables.Count - 1);
            return true;
        }
        return false;
    }

    public bool RemoveCollectableByIndex(int index) {
        if (this.collectables.Count == 0) { return false; }
        if (index < 0 || index >= collectables.Count) { return false; }
        this.collectables.RemoveAt(index);
        return true;
    }

    public bool RemoveCollectableByName(string collectableName) {
        if (this.collectables.Count == 0) { return false; }
        if (string.IsNullOrEmpty(collectableName)) { return false; }
        foreach (SO_InteractableCollectableData data in collectables) {
            if (data.GetCollectableName() == collectableName) {
                this.collectables.Remove(data);
                return true;
            }
        }
        return false;
    }

    public bool RemoveCollectableByID(int collectableID) {
        if (this.collectables.Count == 0) { return false; }
        if (collectableID < 0) { return false; }
        foreach (SO_InteractableCollectableData data in collectables)
        {
            if (data.GetID() == collectableID) {
                this.collectables.Remove(data);
                return true;
            }
        }
        return false;
    }

    public SO_InteractableCollectableData RemoveAndGetFirstCollectable()
    {
        if (this.collectables.Count > 0) {
            SO_InteractableCollectableData removedData = this.collectables[0];
            this.collectables.RemoveAt(0);
            return removedData;
        }
        return null;
    }

    public SO_InteractableCollectableData RemoveAndGetLastCollectable()
    {
        if (this.collectables.Count > 0) {
            SO_InteractableCollectableData removedData = this.collectables[^1];
            this.collectables.RemoveAt(this.collectables.Count - 1);
            return removedData;
        }
        return null;
    }

    public SO_InteractableCollectableData RemoveAndGetCollectableByIndex(int index)
    {
        if (this.collectables.Count == 0) { return null; }
        if (index < 0 || index >= collectables.Count) { return null; }
        SO_InteractableCollectableData removedData = this.collectables[index];
        this.collectables.RemoveAt(index);
        return removedData;
    }

    public SO_InteractableCollectableData RemoveAndGetCollectableByName(string collectableName) {
        if (this.collectables.Count == 0) { return null; }
        if (string.IsNullOrEmpty(collectableName)) { return null; }
        foreach (SO_InteractableCollectableData data in collectables) {
            if (data.GetCollectableName() == collectableName) {
                SO_InteractableCollectableData removedData = data;
                this.collectables.Remove(data);
                return removedData;
            }
        }
        return null;
    }

    public SO_InteractableCollectableData RemoveAndGetCollectableByID(int collectableID) {
        if (this.collectables.Count == 0) { return null; }
        if (collectableID < 0) { return null; }
        foreach (SO_InteractableCollectableData data in collectables) {
            if (data.GetID() == collectableID) {
                SO_InteractableCollectableData removedData = data;
                this.collectables.Remove(data);
                return removedData;
            }
        }
        return null;
    }
    
    public CarriedCollectableList RemoveAllAndGetCollectables() {
        if (this.collectables.Count > 0) {
            CarriedCollectableList removedCollectables = collectables;
            collectables.RemoveAll();
            return removedCollectables;
        }
        
        return null;
    }
    
    public int RemoveAllAndGetAllValues() {
        if (this.collectables.Count > 0) {
            int sum = 0;
            foreach (SO_InteractableCollectableData data in collectables) {
                sum += (int)data.GetCollectableValue();
            }
            collectables.RemoveAll();
            return sum;
        }
        
        return -1;
    }

    /// <summary>Restores every carried instance to its pickup position and empties the inventory.</summary>
    public void RestoreAllCollectables() { this.collectables.RestoreAll(); }
}
