using System;
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
    [SerializeField] float groundCheckDistance;

    bool isGrounded;
    bool isClimbing;


    Vector2 moveInput;
    Vector3 moveDirection;

    RaycastHit slopeHit;


    bool IsOnSlope()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out slopeHit, groundCheckDistance, groundLayerMask))
        {
            float slopeAngle=Vector3.Angle(Vector3.up,slopeHit.normal);
            if (slopeAngle < maxSlopeAngle && slopeAngle != 0)
                return true;
        }
        return false;
    }

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
        print("enable");
        move.action.Enable();
        look.action.Enable();
        jump.action.Enable();
        run.action.Enable();

        jump.action.performed += OnJump;
    }

    private void OnDisable()
    {
        print("disable");
        jump.action.performed -= OnJump;

        move.action.Disable();
        look.action.Disable();
        jump.action.Disable();
        run.action.Disable();
    }


    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayerMask);
        moveInput = move.action.ReadValue<Vector2>();

        Vector3 lookDirection = transform.eulerAngles;
        lookDirection.y += look.action.ReadValue<Vector2>().x;
        transform.eulerAngles = lookDirection;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }
    private void MovePlayer()
    {
        if (!isGrounded && !isClimbing)
        {
            rb.linearVelocity += Vector3.down * gravityScale;
            rb.linearDamping = airDrag;
        }
        else 
            rb.linearDamping = groundDrag;
        
        Vector3 forward = transform.forward * moveInput.y;
        Vector3 right = transform.right * moveInput.x;
        moveDirection = (forward + right).normalized;


      
        
        float multiplier = isGrounded
            ? 1
            : airMultiplier;

        Vector3 targetVelocity = moveDirection * acceleration;
       
        if (IsOnSlope())
            targetVelocity = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal) * acceleration;
        
            

        rb.linearVelocity += new Vector3(
            targetVelocity.x,
            0,
            targetVelocity.z)*multiplier;
    }
    void SpeedControl()
    {

        float maxSpeed = run.action.phase == InputActionPhase.Performed
            ? runMaxSpeed
            : walkMaxSpeed;
        Vector3 flatVelocity = new Vector3(
            rb.linearVelocity.x,
            0,
            rb.linearVelocity.z);

        if (flatVelocity.magnitude > maxSpeed) 
        {
            flatVelocity = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(flatVelocity.x, rb.linearVelocity.y, flatVelocity.z);
        }
            
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!isGrounded) return;
       
        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x, 
            jumpStrength, 
            rb.linearVelocity.z);
    }

    public void SetClimbing(bool climbing)
    {
        isClimbing = climbing;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            transform.position,
            transform.position + (Vector3.down * groundCheckDistance));
    }
}