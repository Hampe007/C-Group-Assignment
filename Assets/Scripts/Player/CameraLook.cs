using UnityEngine;

public class CameraLook : MonoBehaviour {
    Camera cameraComponent;
    private Rigidbody playerRB;
    private Vector2 lookInput;

    [SerializeField] private float verticalSensitivity = 0.2f;
    [SerializeField] private float horizontalSensitivity = 0.35f;

    public Vector3 lookDirection;
    public Vector3 localAngles;
    float lookX;
    float lookY;

    void Awake() {
        playerRB = GetComponentInParent<Rigidbody>();
    }

    void Start() {
        cameraComponent = GetComponent<Camera>();
    }

    public void SetLookInput(Vector2 input) { lookInput = input; }

    void Update() {
        if (cameraComponent == null || playerRB == null) { return; }

        // Do not rotate camera if the game is paused or over
        if (GameState.Instance != null && playerRB != null && !GameState.Instance.IsPlayerInControl())
            return;

        // Vertical Look (Pitch): Tilt the camera up and down
        lookX -= lookInput.y * verticalSensitivity;
        lookX = Mathf.Clamp(lookX, -90f, 90f);
        transform.localEulerAngles = new Vector3(lookX, 0, 0);

        // Horizontal Look (Yaw): Rotate the player body left and right
        lookY = lookInput.x * horizontalSensitivity;
        Vector3 rotationDelta = new Vector3(0, lookY, 0);
        Quaternion newRotation = playerRB.rotation * Quaternion.Euler(rotationDelta);
        playerRB.MoveRotation(newRotation);
    }
}
