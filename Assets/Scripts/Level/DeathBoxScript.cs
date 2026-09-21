using UnityEngine;

public class DeathBoxScript : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private bool ResetGame = true;
    private Vector3 StartPosition;

    void Start()
    {
        StartPosition = Player.transform.position;

        Debug.Log(StartPosition);
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Bang! You're dead.");
        if (ResetGame == true)
        {
            GameState.Instance.GameOver();
            //Application.LoadLevel(1);
        }
        else
        {
            Player.transform.position = StartPosition;
        }



    }


}
