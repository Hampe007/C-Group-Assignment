using System;
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

    [SerializeField] Rigidbody playerRB;

    Camera cameraComponent;
    void Start()
    {
        cameraComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
       
        lookX-=Look.action.ReadValue<Vector2>().y;

        lookX = Mathf.Clamp(lookX,-90, 90);

        transform.localEulerAngles = new Vector3(lookX, 0, 0);
        Vector3 flatVelocity = new Vector3(playerRB.linearVelocity.x, 0, playerRB.linearVelocity.z);
        cameraComponent.fieldOfView = 60 + flatVelocity.magnitude;
        

        
    }
}
