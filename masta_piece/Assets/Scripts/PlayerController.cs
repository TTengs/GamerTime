using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private ThirdPersonPlayerActions playerActions;
    private InputAction move;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider beanCollider;
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f; 
    
    private Vector3 forceDirection = Vector3.zero;

    private void Awake() {
        rb = this.GetComponent<Rigidbody>();
        playerActions = new ThirdPersonPlayerActions();
    }

    private void OnEnable() {
        playerActions.Player.Jump.started += DoJump;
        move = playerActions.Player.Move;
        playerActions.Player.Enable();
    }

    private void OnDisable() {
        playerActions.Player.Jump.started -= DoJump;
        playerActions.Player.Disable();
    }

    private void FixedUpdate() {
        //forceDirection.x += move.ReadValue<Vector2>().x * movementForce;
        //forceDirection.z += move.ReadValue<Vector2>().y * movementForce;

        forceDirection = orientation.forward * move.ReadValue<Vector2>().y +
                         orientation.right * move.ReadValue<Vector2>().x;
        
        rb.AddForce(forceDirection * movementForce, ForceMode.Impulse);

        if (rb.velocity.y < 0f) {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed) {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }
    }

    private void DoJump(InputAction.CallbackContext obj) {
        print("Is try jump " + IsGrounded());
        if (IsGrounded()) {
            print("Try jump and ground");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(beanCollider.bounds.center,
            new Vector3(beanCollider.bounds.center.x, beanCollider.bounds.min.y, beanCollider.bounds.center.z), 0.5f,
            groundLayer);
    }
}
