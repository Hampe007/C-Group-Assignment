using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    Vector3 velocity;
    bool isGrounded;

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


    [Header("InputActions")]

    [SerializeField] private InputActionReference interact;
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference look;
    [SerializeField] InputActionReference run;

    [Header("GroundCheck")]

    [SerializeField] LayerMask groundLayerMask;

    PlayerInteractZone interactZone;

    RaycastHit slopeHit;

    float moveLockTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        interactZone = GetComponentInChildren<PlayerInteractZone>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        move.action.Enable();
        look.action.Enable();
        jump.action.Enable();
        jump.action.performed += OnJump;
        run.action.Enable();
  
        interact.action.Enable();
        interact.action.performed += OnInteract;
    }
    private void OnDisable()
    {
        move.action.Disable();
        jump.action.Disable();
        look.action.Disable();
        jump.action.performed -= OnJump;
        run.action.Disable();
     
        interact.action.Disable();
        interact.action.performed -= OnInteract;
    }
    /*bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down,out slopeHit, 1.3f, groundLayerMask))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
       
            return angle <= maxSlopeAngle&&angle!=0;
        }

        return false;
    }*/

    // Update is called once per frame
    Vector2 moveInput;
    Vector3 moveDirection;

    RaycastHit rightWallHit;
    RaycastHit leftWallHit;

    RaycastHit finalHit;
    /*bool IsOnWall()
    {
        
       
        if (Physics.Raycast(transform.position, transform.right, out rightWallHit, 1, groundLayerMask))
        {
            if (moveInput.x > 0)
            {
                finalHit = rightWallHit;
                return true;
            }
        }
        if (Physics.Raycast(transform.position, -transform.right, out leftWallHit, 1, groundLayerMask))
        {
            if (moveInput.x < 0)
            {
                finalHit = leftWallHit;
                return true;
            }
           
        }
        return false;
    }*/

    void Update()
    {
        //isGrounded = Physics.Raycast(transform.position,Vector3.down,1.3f,groundLayerMask);
        moveInput = move.action.ReadValue<Vector2>();
        Vector3 lookDirection = transform.eulerAngles;
        lookDirection.y += look.action.ReadValue<Vector2>().x;
        transform.eulerAngles= lookDirection;

        /*if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }*/

        MovePlayer();
        SpeedControl();
    }
    
    Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }
    void SpeedControl()
    {
        float currentMaxSpeed;
        if (run.action.phase == InputActionPhase.Performed)
            currentMaxSpeed = runMaxSpeed;
        else  
            currentMaxSpeed = walkMaxSpeed;

        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVelocity.magnitude > currentMaxSpeed)
        {
            Vector3 limitedVelocity=flatVelocity.normalized*currentMaxSpeed;
            rb.linearVelocity=new Vector3(limitedVelocity.x,rb.linearVelocity.y,limitedVelocity.z);
        }
    }
    /*void MovePlayer()
    {
        if (moveLockTime > 0)
        {
            moveLockTime -= Time.deltaTime;
            return;
        }
        var forward = transform.forward * moveInput.y;
        var right = transform.right * moveInput.x;
        moveDirection = (forward + right).normalized;

        
        

        if (IsOnSlope())
        {
            rb.AddForce(GetSlopeDirection() * acceleration);
        }
        else if (isGrounded)
            rb.AddForce(moveDirection * acceleration);
        else
            rb.AddForce(moveDirection * acceleration* airMultiplier);

    }*/
    void MovePlayer()
    {
        if (moveLockTime > 0)
        {
            moveLockTime -= Time.fixedDeltaTime;
            return;
        }

        Vector3 forward = transform.forward * moveInput.y;
        Vector3 right = transform.right * moveInput.x;
        moveDirection = (forward + right).normalized;

        float maxSpeed =
            run.action.phase == InputActionPhase.Performed
                ? runMaxSpeed
                : walkMaxSpeed;

        Vector3 targetVelocity = moveDirection * maxSpeed;

        // Keep current vertical velocity for gravity/jumping.
        rb.linearVelocity = new Vector3(
            targetVelocity.x,
            rb.linearVelocity.y,
            targetVelocity.z
        );
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        /*
        if (!isGrounded)
        {
            if (IsOnWall())
                rb.AddForce(Vector3.down * gravityScale / 2);
            else
                rb.AddForce(Vector3.down * gravityScale);
        
        }*/
        /*if (!isGrounded)
        {
            if (IsOnWall())
                rb.AddForce(Vector3.down * gravityScale / 2);
            else
                rb.AddForce(Vector3.down * gravityScale);
        }*/ 
    }
    void OnJump(InputAction.CallbackContext context)
    {
       
        /*if (isGrounded) 
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
        else if (IsOnWall())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce((finalHit.normal+(Vector3.up*0.6f)).normalized*jumpStrength*1.5f,ForceMode.Impulse);
            moveLockTime = 0.4f;
        }*/
       
    }
    
    private void OnInteract(InputAction.CallbackContext context) {
        if (interactZone == null) return;

        Interactable interactable = interactZone.GetLastInteractable();

        if (interactable != null) {
            Debug.Log($"Interacted with: {interactable.gameObject.name}");

            SO_InteractableData result = interactable.Interact();

            if (result != null && Inventory.Instance != null) {
                
                switch (interactable) {
                    case InteractableCollectable collectable:

                        if (Inventory.Instance.AddCollectable((SO_InteractableCollectableData)result)) {
                            collectable.GetGameObject().GetComponent<Collider>().enabled = false;
                            this.interactZone.PickedUpInteractable();
                            Debug.Log($"Collectable {((SO_InteractableCollectableData)result).GetCollectableName()} added to Inventory.");
                        } else {
                            Debug.Log("No collectable was added to Inventory.");
                        }
                        break;
                    default:
                        //Debug.Log(interactable.GetType().Name);
                        break;
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.down, transform.position + Vector3.down + (Vector3.down * 0.3f));
    }
}
