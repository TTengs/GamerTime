using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private ThirdPersonPlayerActions playerActions;
    private InputAction move;
    
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
        forceDirection.x += move.ReadValue<Vector2>().x * movementForce;
        forceDirection.z += move.ReadValue<Vector2>().y * movementForce;
        
        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

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
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded() {
        //Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);
        return Physics.Raycast(transform.position, Vector3.down, 1 * 0.5f + 0.3f);
    }
}
