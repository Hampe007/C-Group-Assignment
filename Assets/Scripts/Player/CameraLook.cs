using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] InputActionReference look;
    public Vector3 lookDirection;
    public Vector3 localAngles;
    private float lookX;
    private float lookY;

    // Update is called once per frame
    void Update()
    {
        // Do not rotate camera if the game is paused or over
        if (GameState.Instance != null && playerRB != null && !GameState.Instance.IsPlayerInControl())
            return;

        // Read the 2D vector input from the new Input System
        Vector2 lookInput = look.action.ReadValue<Vector2>();

        // Vertical Look (Pitch): Tilt the camera up and down
        lookX -= lookInput.y * verticalSensitivity;
        lookX = Mathf.Clamp(lookX, -90f, 90f);
        transform.localEulerAngles = new Vector3(lookX, 0, 0);
     

        // Horizontal Look (Yaw): Rotate the player body left and right
        lookY = lookInput.x * horizontalSensitivity;
        Vector3 rotationDelta = new Vector3(0, lookY, 0);
        Quaternion newRotation = playerRB.rotation * Quaternion.Euler(rotationDelta);
        playerRB.MoveRotation(newRotation);

        // Camera Field of View based on velocity
        /*if (playerRB != null) {
            Vector3 flatVelocity = new Vector3(playerRB.linearVelocity.x, 0, playerRB.linearVelocity.z);
            cameraComponent.fieldOfView = 60 + flatVelocity.magnitude;
        }*/
    }
}
