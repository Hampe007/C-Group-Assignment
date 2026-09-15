using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInteractZone : MonoBehaviour {
    [SerializeField] private List<GameObject> interactablesInRange = new List<GameObject>();

    //used only temporarily as flags
    //private bool hasActivatedOrderedButtons = false;
    //private bool hasDeactivatedOrderedButtons = false;
    //private bool hasActivatedOneOrderedButton = false;
    //private bool hasActivatedClickButton = false; //probably can't handle all the click buttons at the same time
    private bool hasPickedUp = false;
    private bool toBeDestroyed = false;

    private void Update() {
        if (this.interactablesInRange.Count > 0) {
            //Debug.Log("Interactable in reach");
            GameObject latestGameObject = this.interactablesInRange[^1]; //maybe needs to be replaced???
            bool activeGameObject = true;

            /*if (this.hasActivatedOrderedButtons)
            {
                this.ShowInteractPrompt(InteractionType.BUTTON);
                this.hasActivatedOrderedButtons = false;
            }

            if (this.hasDeactivatedOrderedButtons)
            {
                Interactable latestAvailableInteractable = this.GetLastInteractableBySkippingType(
                    typeof(Puzzle_ActivateButton));
                if (latestAvailableInteractable != null)
                {
                    this.ShowInteractPrompt(latestAvailableInteractable);
                }
                else
                {
                    //this.interactablesInRange.Clear();
                    InteractPrompt.Instance.HideInteractPrompt();
                }
                this.hasDeactivatedOrderedButtons = false;
            }

            if (this.hasActivatedOneOrderedButton)
            {
                activeGameObject = false;
                this.hasActivatedOneOrderedButton = false;
            }

            if (this.hasActivatedClickButton)
            {
                this.ShowInteractPrompt(InteractionType.BUTTON);
                this.hasActivatedClickButton = false;
                activeGameObject = false;
            }*/

            if (this.hasPickedUp)
            {
                this.hasPickedUp = false;
                activeGameObject = false;
                this.toBeDestroyed = true;
            }

            if (latestGameObject == null)
            {
                activeGameObject = false;
                this.toBeDestroyed = true;
            }

            if (!activeGameObject)
            {
                this.interactablesInRange.RemoveAt(this.interactablesInRange.Count - 1);
                if (this.toBeDestroyed)
                {
                    //Debug.Log(this.interactablesInRange.Count);
                    Destroy(latestGameObject);
                }

                if (this.interactablesInRange.Count != 0)
                {
                    latestGameObject = this.interactablesInRange[^1];

                    if (latestGameObject != null)
                    {
                        Interactable latestInteractable = this.GetLastInteractable();
                        /*if (latestInteractable.GetType() == typeof(Puzzle_ActivateButton) &&
                                ((Puzzle_ActivateButton)latestInteractable).
                                GetButtonInteractionType() == InteractionType.BUTTON &&
                                ((Puzzle_ActivateButton)latestInteractable).IsButtonActive())
                        {
                            this.ShowInteractPrompt(InteractionType.BUTTON);
                        }
                        else if (latestInteractable.GetType() == typeof(Puzzle_ClickSendPuzzleEvent) &&
                                ((Puzzle_ClickSendPuzzleEvent)latestInteractable).
                                GetButtonInteractionType() == InteractionType.BUTTON &&
                                ((Puzzle_ClickSendPuzzleEvent)latestInteractable).IsButtonActive())
                        {
                            this.ShowInteractPrompt(InteractionType.BUTTON);
                        }
                        else if (latestInteractable.GetInteractionType() is InteractionType type && (
                                type == InteractionType.VERBAL ||
                                type == InteractionType.ITEM ||
                                type == InteractionType.SPELL_ATTRIBUTE))
                        {
                            this.ShowInteractPrompt(type);
                        }*/
                    }
                }
                //else { InteractPrompt.Instance.HideInteractPrompt(); }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Interactable interactable))
        {
            /*bool activeInteractable = true;

            if (interactable.GetType() == typeof(Puzzle_ClickSendPuzzleEvent))
            {
                if (((Puzzle_ClickSendPuzzleEvent)interactable).IsButtonActive())
                {
                    this.interactablesInRange.Add(other.gameObject);
                }
                else
                {
                    activeInteractable = false;
                }
            } /*else if (interactable.GetType() == typeof(Puzzle_ActivateButton)) {
                if (((Puzzle_ActivateButton)interactable).IsButtonActive()) {
                    this.interactablesInRange.Add(other.gameObject);
                } else {
                    activeInteractable = false;
                }
            } */
            /*else
            {
                this.interactablesInRange.Add(other.gameObject);
            }

            if (activeInteractable)
                this.ShowInteractPrompt(interactable);
            */

            this.interactablesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Interactable interactable))
        {
            this.interactablesInRange.Remove(other.gameObject);

            /*if (this.interactablesInRange.Count != 0)
            {
                Interactable newInteractableInRange = this.GetLastInteractable();
                this.ShowInteractPrompt(newInteractableInRange);
            }
            else { InteractPrompt.Instance.HideInteractPrompt(); }*/
        }
    }

    /*private void ShowInteractPrompt(Interactable interactable)
    {
        if (interactable.GetType() == typeof(Puzzle_ActivateButton))
        {
            if (((Puzzle_ActivateButton)interactable).IsButtonActive())
            {
                this.ShowInteractPrompt(InteractionType.BUTTON);
            }
        }
        else if (interactable.GetType() == typeof(Puzzle_ClickSendPuzzleEvent))
        {
            if (((Puzzle_ClickSendPuzzleEvent)interactable).IsButtonActive())
            {
                this.ShowInteractPrompt(InteractionType.BUTTON);
            }
        }
        else { this.ShowInteractPrompt(interactable.GetInteractionType()); }
    }*/

    /*private void ShowInteractPrompt(InteractionType type)
    {
        switch (type)
        {
            case InteractionType.ITEM:
                {
                    InteractPrompt.Instance.ShowInteractPrompt(InteractPrompt.Instance.artifactSprite);
                    break;
                }
            case InteractionType.SPELL_ATTRIBUTE:
                {
                    InteractPrompt.Instance.ShowInteractPrompt(InteractPrompt.Instance.spellsSprite);
                    break;
                }
            case InteractionType.VERBAL:
                {
                    InteractPrompt.Instance.ShowInteractPrompt(InteractPrompt.Instance.talkSprite);
                    break;
                }
            case InteractionType.BUTTON:
                {
                    InteractPrompt.Instance.ShowInteractPrompt(InteractPrompt.Instance.pushSprite);
                    break;
                }
            default:
                {
                    Debug.Log("This interactable can't be displayed.");
                    break;
                }
        }
    }*/

    //public void EnableOrderedButtonInteraction() { this.hasActivatedOrderedButtons = true; }

    //public void DisableOrderedButtonInteraction() { this.hasDeactivatedOrderedButtons = true; }

    //public void ActivatedOneOrderedButton() { this.hasActivatedOneOrderedButton = true; }

    //public void ActivatedClickButton() { this.hasActivatedClickButton = true; }

    public void PickedUpInteractable() { this.hasPickedUp = true; }

    //public void DestroyLatestInteractable() { this.hasActivatedOrderedButtons = true; }


    //private GameObject GetLastGameObjectBySkippingType(Type skippedType) {
    private Interactable GetLastInteractableBySkippingType(Type skippedType)
    {
        for (int i = 1; i <= this.interactablesInRange.Count; i++)
        {
            Interactable currentInteractable = this.interactablesInRange[^i].GetComponent<Interactable>();
            //if (skippedType.GetType() != this.interactablesInRange[^i].GetType()) {
            if (skippedType != currentInteractable.GetType())
            {
                return currentInteractable;
            }
        }
        return null;
    }

    //code needs to be added to check if the buttons are activated!!!!
    public Interactable GetFirstInteractable()
    {
        if (interactablesInRange.Count > 0)
            return this.interactablesInRange[0].GetComponent<Interactable>();
        return null;
    }

    //code needs to be added to check if the buttons are activated!!!!
    public Interactable GetLastInteractable()
    {
        if (interactablesInRange.Count > 0)
            return this.interactablesInRange[^1].GetComponent<Interactable>();
        return null;
    }

    //code needs to be added to check if the buttons are activated!!!!
    public SO_InteractableData GetFirstInteractableData()
    {
        return this.GetFirstInteractable().GetInteractableData();
    }

    //code needs to be added to check if the buttons are activated!!!!
    public SO_InteractableData GetLastInteractableData()
    {
        return this.GetLastInteractable().GetInteractableData();
    }
}

