using UnityEngine;
using UnityEngine.InputSystem;


public class Ladder : MonoBehaviour
{
    // How fast the player climbs.
    [SerializeField] private float climbSpeed = 3f;

   

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        // Only affect the Player.
        if (rb == null || !rb.CompareTag("Player"))
            return;
        Movement movement = rb.GetComponent<Movement>();
       // Tell the Movement script that the player is climbing.
       if (movement != null)
        {
            movement.SetClimbing(true);
        }

        // The direction the player climbs.
        Vector3 climbDirection = transform.up;

        // W = climb UP
        float input = 0f;

        if (Keyboard.current != null && Keyboard.current.wKey.isPressed)
        {
            input = 1f;
        }

        // Keep any movement that is not along the ladder.
        Vector3 sidewaysVelocity =
            Vector3.ProjectOnPlane(rb.linearVelocity, climbDirection);

        // Move the player up the ladder.
        rb.linearVelocity =
            sidewaysVelocity + climbDirection * (input * climbSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null || !rb.CompareTag("Player"))
            return;
        Movement movement = rb.GetComponent<Movement>();
       // Tell the Movement script that the player stopped climbing.
       if (movement != null)
        {
            movement.SetClimbing(false);
        }
    }
}