using System;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    GROUNDED = 0,
    AIRBORNE = 1,
    SLIDING = 2,
    WALL_RUNNING=3
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    CapsuleCollider capsuleCollider;

    [Header("Movement Settings")]
    [SerializeField] private float acceleration;
    [SerializeField] private float walkMaxSpeed;
    [SerializeField] private float runMaxSpeed;
    [SerializeField] private float slideMaxSpeed;
    [SerializeField] private float gravityScale;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float slideDrag;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float slopeAcceleration;

    [SerializeField] private int maxAirJumps;

    private float maxSpeedDecreaseGround=0.3f;
    private float maxSpeedDecreaseAir=0.1f;
    private int airJumps;
    private float maxSpeed;

    //[Header("Input Actions")]
    //[SerializeField] InputActionReference move;
    //[SerializeField] InputActionReference jump;
    //[SerializeField] InputActionReference run;
    //[SerializeField] InputActionReference slide;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundCheckDistance;

    // State passed from PlayerController
    private Vector2 moveInput;
    private bool isRunPressed;
    private bool isSlidePressed;

    PlayerState currentState;
    private Vector3 moveDirection;
    private bool isClimbing;

    RaycastHit slopeHit;
    float slopeAngle;

    float inputLockTime;

    RaycastHit wallHit;
    float wallGravityMultiplier=0.25f;
    int wallDir;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        isRunPressed = true;
    }

    private void Start()
    {
        airJumps = maxAirJumps;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Called continuously by PlayerController to update movement inputs.
    public void SetInputValues(Vector2 inputVector, bool running, bool sliding) {
        moveInput = inputVector;
        isRunPressed = running;
        isSlidePressed = sliding;

        //Debug.Log($"moveInput: {moveInput}, isRunning: {isRunPressed}, isSliding: {isSlidePressed}");
    }
 
    public void TriggerJump()
    {
        if (airJumps <= 0 && (currentState == PlayerState.AIRBORNE))
            return;

        if (currentState == PlayerState.AIRBORNE )
            airJumps--;

        if (currentState == PlayerState.WALL_RUNNING) //wall jump
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x + (wallHit.normal.x * jumpStrength), jumpStrength, rb.linearVelocity.z + (wallHit.normal.z * jumpStrength));
            inputLockTime = 0.3f;
        }
        else  rb.linearVelocity = new Vector3 (rb.linearVelocity.x, jumpStrength, rb.linearVelocity.z); //regular jump
    }
    private void FixedUpdate()
    {
        
        if (GameState.Instance != null && !GameState.Instance.IsPlayerInControl())
            return;

        if (isClimbing)
            return;
        
        MovePlayer();
        SpeedControl();
    }

    bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, groundCheckDistance, groundLayerMask))
        {
            slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);

            if (slopeAngle < maxSlopeAngle && slopeAngle != 0)
                return true;
            
        }
        return false;
    }
  
    void StartWallRun()
    {
        if (rb.linearVelocity.y < 0) rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    }
    bool CanWallRun()
    {
        if (moveInput.x == 0 || currentState == PlayerState.GROUNDED || currentState == PlayerState.SLIDING) return false;
        RaycastHit tryHit;
        if (Physics.Raycast(transform.position, transform.right * Mathf.Sign(moveInput.x), out tryHit, 1.3f, groundLayerMask))
        {
            wallDir = Math.Sign(moveInput.x);
            wallHit = tryHit;
            return true;
        }
        return false;
    }
    Vector3 WallForward()
    {
        Vector3 forward = Vector3.Cross(transform.up, wallHit.normal);
        if ((transform.forward - forward).magnitude > (transform.forward - -forward).magnitude) forward = -forward;
        return forward;
    }

    bool CanExitSlide()
    {
        return !Physics.
            Raycast(transform.position, Vector3.up, groundCheckDistance, groundLayerMask);
    }

    void SetPlayerState() {
        Vector3 flatVel = rb.linearVelocity;
        flatVel.y = 0;
        float flatMagnitude=flatVel.magnitude;

        bool isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayerMask);

        if (CanWallRun())
        {
            if (currentState != PlayerState.WALL_RUNNING) StartWallRun();
            currentState = PlayerState.WALL_RUNNING;
            return;
        }
        if (isGrounded)
        {
            //Debug.Log("GROUNDED");
            if (isSlidePressed)
            {
                currentState = PlayerState.SLIDING;
            }
            else
            {
                if (currentState == PlayerState.SLIDING && flatMagnitude > runMaxSpeed) 
                    maxSpeed = flatMagnitude; 
                currentState = PlayerState.GROUNDED;
            }
        }
       
        else
        {
            if (currentState != PlayerState.AIRBORNE && flatMagnitude < maxSpeed)
            {
                maxSpeed = flatMagnitude;
            }
            currentState = PlayerState.AIRBORNE;
        }
    }

    void SetColliderSize()
    {
        Transform cameraPos = GetComponentInChildren<Camera>().transform;

        if (currentState == PlayerState.SLIDING)
        {
            capsuleCollider.height = 1;
            capsuleCollider.center = new Vector3(0, -0.5f, 0);
            cameraPos.localPosition = new Vector3(0, 0, 0);
        }
        else if (CanExitSlide())
        {
            capsuleCollider.height = 2;
            capsuleCollider.center = new Vector3(0, 0, 0);
            cameraPos.localPosition = new Vector3(0, 1, 0);
        }
    }
    void SetMaxSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float flatMagmitude = flatVel.magnitude;
        
        if (flatMagmitude < runMaxSpeed)
            maxSpeed = isRunPressed ? runMaxSpeed : walkMaxSpeed;
       
        else if (currentState == PlayerState.GROUNDED) 
            maxSpeed -=maxSpeedDecreaseGround;
        
        else if (currentState == PlayerState.AIRBORNE&&maxSpeed>runMaxSpeed)
            maxSpeed -= maxSpeedDecreaseAir;
       
        if (isRunPressed && maxSpeed < runMaxSpeed)
            maxSpeed = runMaxSpeed;
      
        if (currentState == PlayerState.SLIDING&&(maxSpeed<=runMaxSpeed||IsOnSlope())) 
            maxSpeed = slideMaxSpeed;
        
        
    }
    private void MovePlayer()
    {
        SetPlayerState();
        SetColliderSize();
        SetMaxSpeed();

        switch (currentState)
        {
            case PlayerState.GROUNDED:
                rb.linearDamping = groundDrag;
                airJumps = maxAirJumps;
                break;

            case PlayerState.AIRBORNE:
                rb.linearDamping = airDrag;
                rb.linearVelocity += Vector3.down * gravityScale;
                break;

            case PlayerState.SLIDING:
                rb.linearDamping = slideDrag;
                airJumps = maxAirJumps;
                break;

            case PlayerState.WALL_RUNNING:
                rb.linearDamping = airDrag;
                airJumps = maxAirJumps;
                rb.linearVelocity += WallForward() * 2;
                rb.linearVelocity += Vector3.down * (gravityScale * wallGravityMultiplier);
                if (wallDir == Math.Sign(moveInput.x)&&inputLockTime<=0) rb.linearVelocity += -wallHit.normal*10;
                moveInput.y = 0;
                break;

            default:
                break;
        }

        Vector3 forward = transform.forward * moveInput.y;
        Vector3 right = transform.right * moveInput.x;
        moveDirection = (forward + right).normalized;
        
        if (inputLockTime > 0)
        {
            inputLockTime -= Time.fixedDeltaTime;
            moveDirection = Vector3.zero;
        }

        if (currentState == PlayerState.SLIDING)
            moveDirection = Vector3.zero;

        float multiplier = currentState != PlayerState.AIRBORNE ? 1 : airMultiplier;

        Vector3 flatVel = rb.linearVelocity;
        flatVel.y = 0;

        Vector3 targetVelocity = moveDirection * acceleration;

        if (IsOnSlope())
        {
            if (currentState == PlayerState.SLIDING)
            {
                targetVelocity += SlopeAcceleration();
            }
            else
            {
            targetVelocity = Vector3.ProjectOnPlane(moveDirection,
                slopeHit.normal).normalized * acceleration;
            }
        }

        rb.linearVelocity += targetVelocity * multiplier;
    }

    Vector3 SlopeAcceleration() {
        float angleMultiplier = Mathf.Sin(slopeAngle * Mathf.Deg2Rad);

        return Vector3.ProjectOnPlane(Vector3.down, slopeHit.normal).normalized
               * slopeAcceleration
               * angleMultiplier;
    }

    void SpeedControl()
    {

        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (flatVelocity.magnitude > maxSpeed)
        {
            flatVelocity = flatVelocity.normalized * maxSpeed;

            rb.linearVelocity = new Vector3(
                flatVelocity.x,
                rb.linearVelocity.y,
                flatVelocity.z);
        }
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