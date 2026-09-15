using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    CharacterController characterController;
    Vector3 velocity;
    bool isGrounded;

    [SerializeField] float walkSpeed;
    [SerializeField] float gravityScale;
    [SerializeField] float jumpStrength;

    [SerializeField] InputActionReference Move;
    [SerializeField] InputActionReference JumpAction;
    [SerializeField] InputActionReference Look;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {
        Move.action.Enable();
        Look.action.Enable();
        JumpAction.action.Enable();
        JumpAction.action.performed += Jump;
  
    }
    private void OnDisable()
    {
        Move.action.Disable();
        JumpAction.action.Disable();
        Look.action.Disable();
        JumpAction.action.performed -= Jump;
     
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
        Vector2 moveInput = Move.action.ReadValue<Vector2>();
        Vector3 lookDirection = transform.eulerAngles;
        lookDirection.y += Look.action.ReadValue<Vector2>().x;
        transform.eulerAngles= lookDirection;
        
        var forward = transform.forward*moveInput.y;
        var right = transform.right * moveInput.x;
        Vector3 move = (forward + right).normalized;
        move = Vector3.ClampMagnitude(move, 1f);

        if(!isGrounded) 
            velocity.y -= gravityScale * Time.deltaTime;

        Vector3 finalMove = move * walkSpeed + Vector3.up * velocity.y;
        characterController.Move(finalMove * Time.deltaTime);
    }
    void Jump(InputAction.CallbackContext context)
    {
        if(isGrounded) velocity.y = jumpStrength;
    }
   
}
