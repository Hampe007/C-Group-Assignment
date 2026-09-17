using System;
using UnityEngine;
using UnityEngine.InputSystem;




public enum PlayerState
{
    ground=0,
    air=1,
    sliding=2,
    slideJump=3
}

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private Rigidbody rb;

    CapsuleCollider capsuleCollider;

    [Header("Movement")]
    [SerializeField] float acceleration;
    [SerializeField] float walkMaxSpeed;
    [SerializeField] float runMaxSpeed;
    [SerializeField] float gravityScale;
    [SerializeField] float jumpStrength;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float slopeDrag;
    [SerializeField] float airMultiplier;
    [SerializeField] float maxSlopeAngle;

    [SerializeField] float slopeAcceleration;

    [SerializeField] int maxAirJumps;
    int airJumps;

    float maxSpeed;

    [Header("Input Actions")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference look;
    [SerializeField] InputActionReference run;
    [SerializeField] InputActionReference slide;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float groundCheckDistance;


   
    PlayerState currentState;

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
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        airJumps = maxAirJumps;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        move.action.Enable();
        look.action.Enable();
        jump.action.Enable();
        run.action.Enable();
        slide.action.Enable();

        jump.action.performed += OnJump;
    }

    private void OnDisable()
    {
        jump.action.performed -= OnJump;

        move.action.Disable();
        look.action.Disable();
        jump.action.Disable();
        run.action.Disable();
        slide.action.Disable();
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
        SpeedControl();
    }
    bool CanExitSlide()
    {
        return !Physics.Raycast(transform.position, Vector3.up, groundCheckDistance, groundLayerMask);
    }
    void SetPlayerState()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayerMask);
        bool slidePressed = slide.action.phase == InputActionPhase.Performed;
        if (isGrounded)
        {
            if (slidePressed)
            {
                currentState = PlayerState.sliding;
            }
            else
            {
                currentState = PlayerState.ground;
            }
        }
        else if ((currentState == PlayerState.sliding||currentState==PlayerState.slideJump)&&rb.linearVelocity.magnitude>runMaxSpeed) 
        {
            currentState = PlayerState.slideJump;
        }
        else currentState = PlayerState.air;
    }
    void SetColliderSize()
    {
        Transform cameraPos = GetComponentInChildren<Camera>().transform;
        if (currentState == PlayerState.sliding)
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
    private void MovePlayer()
    {
      
        SetPlayerState();
        SetColliderSize();

        maxSpeed = run.action.phase == InputActionPhase.Performed
          ? runMaxSpeed
          : walkMaxSpeed;

        switch (currentState)
        {
            case PlayerState.ground:
                rb.linearDamping = groundDrag;
                airJumps = maxAirJumps;
                break;
            case PlayerState.air:
            case PlayerState.slideJump:
                rb.linearDamping = airDrag;
                rb.linearVelocity += Vector3.down * gravityScale; //gravity
                break;
            case PlayerState.sliding:
                rb.linearDamping = slopeDrag;
                airJumps = maxAirJumps;
                break;
          
        }
    
           
        
        Vector3 forward = transform.forward * moveInput.y;
        Vector3 right = transform.right * moveInput.x;
        moveDirection = (forward + right).normalized;

        if (currentState == PlayerState.sliding)         
            moveDirection = Vector3.zero;
        

        float multiplier = currentState != PlayerState.air
            ? 1
            : airMultiplier;
        
        
        Vector3 flatVel=rb.linearVelocity;
        flatVel.y = 0;
        if (flatVel.magnitude > maxSpeed&&currentState==PlayerState.slideJump ) 
            multiplier *= 0f;
        

        Vector3 targetVelocity = moveDirection * acceleration;
       
        if (IsOnSlope())
            if (currentState == PlayerState.sliding)
                targetVelocity += Vector3.ProjectOnPlane(Vector3.down, slopeHit.normal).normalized * slopeAcceleration;
            
            else 
                targetVelocity = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized * acceleration;



        rb.linearVelocity += targetVelocity * multiplier;
    }
    void SpeedControl()
    {
        if (currentState == PlayerState.sliding || currentState == PlayerState.slideJump) return;
        
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
        if (airJumps<=0&& (currentState == PlayerState.air||currentState==PlayerState.slideJump)) return;
        if( currentState==PlayerState.air||currentState==PlayerState.slideJump) airJumps--;
        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x, 
            jumpStrength, 
            rb.linearVelocity.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(
            transform.position,
            transform.position + (Vector3.down * groundCheckDistance));
    }
}