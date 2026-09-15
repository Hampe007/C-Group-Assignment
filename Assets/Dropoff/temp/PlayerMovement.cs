using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference look;
    [SerializeField] private InputActionReference interact;
    [SerializeField] private Transform cameraTransform;

    PlayerInteractZone interactZone;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSpeed = 0.1f;

    private CharacterController controller;
    private float pitch;

    private void Awake()
    {
        //interactZone = GetComponentInChildren<PlayerInteractZone>(true);
        interactZone = GetComponentInChildren<PlayerInteractZone>();
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        move.action.Enable();
        look.action.Enable();

        interact.action.Enable();
        interact.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        move.action.Disable();
        look.action.Disable();

        interact.action.Disable();
        interact.action.performed -= OnInteract;
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

    private void OnInteract(InputAction.CallbackContext context) {
        Debug.Log("Pressed E.");

        if (interactZone == null) return;

        Interactable interactable = interactZone.GetLastInteractable();

        if (interactable != null) {
            Debug.Log($"Interacted with: {interactable.gameObject.name}");

            SO_InteractableData result = interactable.Interact();

            if (result != null && Inventory.Instance != null) {
                
                switch (interactable) {
                    case InteractableCollectable collectable:
                        //Inventory.Instance.AddCollectable(result);

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
}