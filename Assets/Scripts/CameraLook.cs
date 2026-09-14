using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] InputActionReference Look;
    public Vector3 lookDirection;
    public Vector3 localAngles;
    float lookX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        lookX-=Look.action.ReadValue<Vector2>().y;

        lookX = Mathf.Clamp(lookX,-90, 90);

        transform.localEulerAngles = new Vector3(lookX, 0, 0);
    }
}
