using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] float acceleration;
    [SerializeField] float walkMaxSpeed;
    [SerializeField] float runMaxSpeed;
    [SerializeField] float gravityScale;
    [SerializeField] float jumpStrength;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float airMultiplier;
    [SerializeField] float maxSlopeAngle;

    [Header("Input Actions")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference look;
    [SerializeField] InputActionReference run;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayerMask;

    Vector2 moveInput;
    Vector3 moveDirection;
    float moveLockTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        move.action.Enable();
        look.action.Enable();
        jump.action.Enable();
        run.action.Enable();

        jump.action.performed += OnJump;
    }

    private void OnDisable()
    {
        jump.action.performed -= OnJump;

        move.action.Disable();
        look.action.Disable();
        jump.action.Disable();
        run.action.Disable();
    }

    private void Update()
    {
        moveInput = move.action.ReadValue<Vector2>();

        Vector3 lookDirection = transform.eulerAngles;
        lookDirection.y += look.action.ReadValue<Vector2>().x;
        transform.eulerAngles = lookDirection;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (moveLockTime > 0f)
        {
            moveLockTime -= Time.fixedDeltaTime;
            return;
        }

        Vector3 forward = transform.forward * moveInput.y;
        Vector3 right = transform.right * moveInput.x;
        moveDirection = (forward + right).normalized;

        float maxSpeed = run.action.phase == InputActionPhase.Performed
            ? runMaxSpeed
            : walkMaxSpeed;

        Vector3 targetVelocity = moveDirection * maxSpeed;

        rb.linearVelocity = new Vector3(
            targetVelocity.x,
            rb.linearVelocity.y,
            targetVelocity.z);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        // Preserve your existing jump hookup; add jump behaviour here later.
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            transform.position + Vector3.down,
            transform.position + Vector3.down + (Vector3.down * 0.3f));
    }
}