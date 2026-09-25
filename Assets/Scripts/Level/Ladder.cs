using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class Ladder : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float bottomExitTolerance = 0.15f;
    [SerializeField] private float detachSpeed = 3f;
    [SerializeField] private float detachUpwardSpeed = 2f;

    private BoxCollider climbTrigger;

    // A detached player is ignored until they have completely left the trigger.
    private readonly HashSet<Rigidbody> detachedPlayers = new();

    private void Awake()
    {
        climbTrigger = GetComponent<BoxCollider>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        // Ignore the player's interaction trigger and use only their solid collider.
        if (other.isTrigger)
            return;

        Rigidbody rb = other.attachedRigidbody;

        if (rb == null || !rb.CompareTag("Player"))
            return;

        if (detachedPlayers.Contains(rb))
            return;

        PlayerMovement movement = rb.GetComponent<PlayerMovement>();
        float climbInput = 0f;
        bool cancelClimb = false;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                climbInput = 1f;
            else if (Keyboard.current.sKey.isPressed)
                climbInput = -1f;

            // Space lets the player jump away from any point on the ladder.
            cancelClimb = Keyboard.current.spaceKey.isPressed;
        }

        if (cancelClimb)
        {
            DetachPlayer(rb, movement, true);
            return;
        }

        // Holding S at the bottom changes from climbing to normal movement.
        bool atBottom = other.bounds.min.y <= climbTrigger.bounds.min.y + bottomExitTolerance;

        if (atBottom && climbInput < 0f)
        {
            DetachPlayer(rb, movement, false);
            return;
        }

        movement?.SetClimbing(true);

        rb.linearVelocity = Vector3.up * (climbInput * climbSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        Rigidbody rb = other.attachedRigidbody;

        if (rb == null || !rb.CompareTag("Player"))
            return;

        detachedPlayers.Remove(rb);

        PlayerMovement movement = rb.GetComponent<PlayerMovement>();
        movement?.SetClimbing(false);
    }

    private void DetachPlayer(Rigidbody rb, PlayerMovement movement, bool jumpAway)
    {
        detachedPlayers.Add(rb);
        movement?.SetClimbing(false);

        // Push away from the trigger so detaching does not depend on camera direction.
        Vector3 awayFromLadder = Vector3.ProjectOnPlane(rb.worldCenterOfMass - climbTrigger.bounds.center, Vector3.up);

        if (awayFromLadder.sqrMagnitude < 0.001f)
        {
            awayFromLadder = -transform.forward;
        }

        awayFromLadder.Normalize();

        float upwardSpeed = jumpAway ? detachUpwardSpeed : 0f;
        rb.linearVelocity = awayFromLadder * detachSpeed + Vector3.up * upwardSpeed;
    }
}
