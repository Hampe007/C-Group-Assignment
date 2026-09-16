using UnityEngine;
using UnityEngine.InputSystem;

// TEMP SCRIPT!
// REUSED SCRIPT FROM HCS DATABASE!

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference look;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSpeed = 0.5f;

    private CharacterController controller;
    private float pitch;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        move.action.Enable();
        look.action.Enable();
    }

    private void OnDisable()
    {
        move.action.Disable();
        look.action.Disable();
    }

    private void Update()
    {
        Vector2 movement = move.action.ReadValue<Vector2>();
        Vector2 mouse = look.action.ReadValue<Vector2>();

        Vector3 direction =
            transform.right * movement.x +
            transform.forward * movement.y;

        controller.Move(direction * (moveSpeed * Time.deltaTime));

        transform.Rotate(Vector3.up * mouse.x * lookSpeed);

        pitch -= mouse.y * lookSpeed;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}

// © 2025 hcstech.se