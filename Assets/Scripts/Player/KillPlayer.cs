using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameObject Player;
    public Transform RespawnPoint;
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = RespawnPoint.position;
        }
    }
}
