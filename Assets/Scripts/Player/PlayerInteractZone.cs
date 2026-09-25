using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInteractZone : MonoBehaviour {
    [SerializeField] private List<GameObject> interactablesInRange = new List<GameObject>();
    private void Update() {
        if (this.interactablesInRange.Count > 0) {
            GameObject latestGameObject = this.interactablesInRange[^1]; //maybe needs to be replaced???
            bool activeGameObject = true;

            if (latestGameObject == null || !latestGameObject.activeInHierarchy)
            {
                activeGameObject = false;
            }

            if (!activeGameObject)
            {
                this.interactablesInRange.RemoveAt(this.interactablesInRange.Count - 1);
                if (this.interactablesInRange.Count != 0)
                {
                    latestGameObject = this.interactablesInRange[^1];

                    if (latestGameObject != null)
                    {
                        Interactable latestInteractable = this.GetLastInteractable();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Interactable interactable))
        {
            // Several colliders can enter the zone for one object; track each interactable only once.
            if (!this.interactablesInRange.Contains(other.gameObject)) {
                this.interactablesInRange.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Interactable interactable))
        {
            this.interactablesInRange.Remove(other.gameObject);
        }
    }

    /// <summary>Immediately removes the picked-up object from the interaction targets.</summary>
    public void PickedUpInteractable(GameObject pickedUp) { this.interactablesInRange.Remove(pickedUp); }

    /// <summary>Clears interaction targets after the player is repositioned.</summary>
    public void ClearInteractables() { this.interactablesInRange.Clear(); }

    private Interactable GetLastInteractableBySkippingType(Type skippedType)
    {
        for (int i = 1; i <= this.interactablesInRange.Count; i++)
        {
            Interactable currentInteractable = this.interactablesInRange[^i].GetComponent<Interactable>();

            if (skippedType != currentInteractable.GetType())
            {
                return currentInteractable;
            }
        }
        return null;
    }

    //TODO: Code needs to be added to check if the buttons are activated.
    public Interactable GetFirstInteractable()
    {
        this.RemoveUnavailableInteractables();
        if (interactablesInRange.Count > 0)
            return this.interactablesInRange[0].GetComponent<Interactable>();
        return null;
    }

    public Interactable GetLastInteractable()
    {
        this.RemoveUnavailableInteractables();
        if (interactablesInRange.Count > 0)
            return this.interactablesInRange[^1].GetComponent<Interactable>();
        return null;
    }

    public SO_InteractableData GetFirstInteractableData()
    {
        return this.GetFirstInteractable().GetInteractableData();
    }

    public SO_InteractableData GetLastInteractableData()
    {
        return this.GetLastInteractable().GetInteractableData();
    }

    private void RemoveUnavailableInteractables() {
        // Picked-up collectables are deactivated before their trigger-exit callback can run.
        this.interactablesInRange.RemoveAll(interactable =>
            interactable == null || !interactable.activeInHierarchy);
    }
}

