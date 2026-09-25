using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private PlayerMovement movement;
    private CameraLook cameraLook;
    private PlayerInteraction playerInteraction;

    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference run;
    [SerializeField] InputActionReference slide;
    [SerializeField] InputActionReference look;
    [SerializeField] InputActionReference interact;

    Vector2 moveInput;
    Vector2 lookInput;
    bool isRunning;
    bool isSliding;

    private void Awake() {
        movement = GetComponent<PlayerMovement>();
        cameraLook = GetComponentInChildren<CameraLook>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void OnEnable() {
        move.action.Enable();
        jump.action.Enable();
        run.action.Enable();
        slide.action.Enable();
        look.action.Enable();
        interact.action.Enable();

        jump.action.performed += OnJump;
        interact.action.performed += OnInteract;
    }

    private void OnDisable() {
        jump.action.performed -= OnJump;
        interact.action.performed -= OnInteract;

        move.action.Disable();
        jump.action.Disable();
        run.action.Disable();
        slide.action.Disable();
        look.action.Disable();
        interact.action.Disable();
    }

    private void Update() {
        if (movement == null || cameraLook == null || playerInteraction == null) { return; }

        if (GameState.Instance != null && !GameState.Instance.IsPlayerInControl()) {
            movement.SetInputValues(Vector2.zero, false, false);
            cameraLook.SetLookInput(Vector2.zero);
            return;
        }

        // Read continuous inputs every frame
        moveInput = move.action.ReadValue<Vector2>();
        lookInput = look.action.ReadValue<Vector2>();
        isRunning = run.action.phase == InputActionPhase.Performed;
        isSliding = slide.action.phase == InputActionPhase.Performed;

        // Pass continuous state down
        movement.SetInputValues(moveInput, isRunning, isSliding);
        cameraLook.SetLookInput(lookInput);
    }

    private void OnJump(InputAction.CallbackContext context) {
        if (movement == null)
            return;

        if (GameState.Instance != null && !GameState.Instance.IsPlayerInControl())
            return;

        movement.TriggerJump();
    }

    private void OnInteract(InputAction.CallbackContext context) {
        if (playerInteraction == null)
            return;

        if (GameState.Instance != null && !GameState.Instance.IsPlayerInControl())
            return;

        playerInteraction.TriggerInteract();
    }
}
