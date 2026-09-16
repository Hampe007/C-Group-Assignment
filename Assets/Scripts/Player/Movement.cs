using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    CharacterController characterController;
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

    [SerializeField] InputActionReference Move;
    [SerializeField] InputActionReference JumpAction;
    [SerializeField] InputActionReference Look;
    [SerializeField] InputActionReference Run;

    [Header("GroundCheck")]

    [SerializeField] LayerMask groundLayerMask;

    

    RaycastHit slopeHit;

    float moveLockTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        Move.action.Enable();
        Look.action.Enable();
        JumpAction.action.Enable();
        JumpAction.action.performed += Jump;
        Run.action.Enable();
  
    }
    private void OnDisable()
    {
        Move.action.Disable();
        JumpAction.action.Disable();
        Look.action.Disable();
        JumpAction.action.performed -= Jump;
        Run.action.Disable();
     
    }
    bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down,out slopeHit, 1.3f, groundLayerMask))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
       
            return angle <= maxSlopeAngle&&angle!=0;
        }

        return false;
    }

    // Update is called once per frame
    Vector2 moveInput;
    Vector3 moveDirection;

    RaycastHit rightWallHit;
    RaycastHit leftWallHit;

    RaycastHit finalHit;
    bool IsOnWall()
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
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position,Vector3.down,1.3f,groundLayerMask);
        moveInput = Move.action.ReadValue<Vector2>();
        Vector3 lookDirection = transform.eulerAngles;
        lookDirection.y += Look.action.ReadValue<Vector2>().x;
        transform.eulerAngles= lookDirection;

        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }


        
      

        MovePlayer();
        SpeedControl();
       

        //Vector3 finalMove = move * walkSpeed + Vector3.up * velocity.y;
        //characterController.Move(finalMove * Time.deltaTime);
    }
    Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }
    void SpeedControl()
    {
        float currentMaxSpeed;
        if (Run.action.phase == InputActionPhase.Performed)
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
    void MovePlayer()
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
    }
    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            if (IsOnWall())
                rb.AddForce(Vector3.down * gravityScale / 2);
            else
                rb.AddForce(Vector3.down * gravityScale);
        } 
        
    }
    void Jump(InputAction.CallbackContext context)
    {
       
        if (isGrounded) 
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
        else if (IsOnWall())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce((finalHit.normal+(Vector3.up*0.6f)).normalized*jumpStrength*1.5f,ForceMode.Impulse);
            moveLockTime = 0.4f;
        }
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + Vector3.down, transform.position + Vector3.down + (Vector3.down * 0.3f));
    }


}
