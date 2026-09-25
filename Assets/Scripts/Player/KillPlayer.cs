using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameObject Player;
    public Transform RespawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Return carried items and discard stale interaction targets before moving the player to the respawn point.
            Inventory.Instance?.RestoreAllCollectables();
            other.GetComponentInChildren<PlayerInteractZone>(true)?.ClearInteractables();
            if (other.attachedRigidbody != null)
            {
                other.attachedRigidbody.linearVelocity = Vector3.zero;
            }
            
            other.transform.position = RespawnPoint.position;
        }
    }
}

