using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // private ThirdPersonPlayerActions playerActions;
    // private InputAction move;
    // [SerializeField] private LayerMask groundLayer;
    // [SerializeField] private Collider beanCollider;
    // [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementSpeed = 7f;
    // [SerializeField] private float jumpForce = 5f;
    // [SerializeField] private float maxSpeed = 5f; 
    //
    // private Vector3 forceDirection = Vector3.zero;
    // private Vector3 cameraRelativeMovement;
    //
    // // private void Awake() {
    // //     rb = this.GetComponent<Rigidbody>();
    // //     playerActions = new ThirdPersonPlayerActions();
    // // }
    //
    // private void OnEnable() {
    //     playerActions.Player.Jump.started += DoJump;
    //     move = playerActions.Player.Move;
    //     playerActions.Player.Enable();
    // }
    //
    // private void OnDisable() {
    //     playerActions.Player.Jump.started -= DoJump;
    //     playerActions.Player.Disable();
    // }
    //
    // private void FixedUpdate() {
    //     //forceDirection.x += move.ReadValue<Vector2>().x * movementForce;
    //     //forceDirection.z += move.ReadValue<Vector2>().y * movementForce;
    //
    //     forceDirection = orientation.forward * move.ReadValue<Vector2>().y +
    //                      orientation.right * move.ReadValue<Vector2>().x;
    //     
    //     rb.AddForce(forceDirection * movementForce, ForceMode.VelocityChange);
    //
    //     if (rb.velocity.y < 0f) {
    //         rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
    //     }
    //
    //     Vector3 horizontalVelocity = rb.velocity;
    //     horizontalVelocity.y = 0;
    //     if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed) {
    //         rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
    //     }
    // }
    //
    // private void DoJump(InputAction.CallbackContext obj) {
    //     print("Is try jump " + IsGrounded());
    //     if (IsGrounded()) {
    //         print("Try jump and ground");
    //         rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //     }
    // }
    //
    // private bool IsGrounded() {
    //     RaycastHit hitInfo;
    //     bool result = beanCollider.Raycast(new Ray(beanCollider.transform.position, Vector3.down), out hitInfo, Mathf.Infinity);
    //     print(hitInfo.transform.tag);
    //     return result;
    // }
    //
    // void HandleRotation()
    // {
    //     Vector3 targetDirection;
    //
    //     targetDirection.x = cameraRelativeMovement.x;
    //     targetDirection.z = cameraRelativeMovement.z;
    //     targetDirection.y = 0.0f;
    //     targetDirection.Normalize();
    //
    //     Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
    //     //Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
    //
    //     //transform.rotation = playerRotation;
    // }

    private InputManager inputManager;
    private Vector3 moveDirection;
    private Transform cameraObject;
    private float rotationSpeed = 15f;

    private void Awake() {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement() {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() {
        moveDirection = new Vector3(cameraObject.forward.x, 0f, cameraObject.forward.z) * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation() {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
}
