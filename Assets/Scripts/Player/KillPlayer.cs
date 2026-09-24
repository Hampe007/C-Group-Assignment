using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameObject Player;
    public Transform RespawnPoint;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance?.RestoreAllCollectables();
            other.GetComponentInChildren<PlayerInteractZone>(true)?.ClearInteractables();
            if (other.attachedRigidbody != null) { other.attachedRigidbody.linearVelocity = Vector3.zero; }
            other.transform.position = RespawnPoint.position;
        }
    }
}
