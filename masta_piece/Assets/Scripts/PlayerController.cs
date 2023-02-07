using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [Header("Movement flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    
    [Header("Movement Speeds")]
    [SerializeField]public float walkingSpeed = 2.5f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField]public float sprintingSpeed = 7f;

    [Header("Falling")] 
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public LayerMask groundLayer;
    public float rayCastHeightOffset = 0.5f;

    [Header("Jumping")] 
    public float jumpingHeight = 3f;
    public float gravityIntensity = -15f;

    private InputManager inputManager;
    private PlayerManager playerManager;
    private AnimatorManager animatorManager;
    private Vector3 moveDirection;
    private Transform cameraObject;
    private float rotationSpeed = 15f;

    private void Awake() {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    public void HandleAllMovement() {
        HandleFallingAndLanding();
        if (playerManager.isInteracting)
            return;
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() {
        if(isJumping)
            return;
        moveDirection = new Vector3(cameraObject.forward.x, 0f, cameraObject.forward.z) * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting) {
            moveDirection = moveDirection * sprintingSpeed;
        }
        else {
            if (inputManager.moveAmount >= 0.5f) {
                moveDirection = moveDirection * runningSpeed;
            }
            else {
                moveDirection = moveDirection * walkingSpeed;
            }
        }

        // moveDirection = moveDirection * runningSpeed;

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation() {
        if(isJumping)
            return;
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

    private void HandleFallingAndLanding() {
        RaycastHit hitInfo;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y = raycastOrigin.y + rayCastHeightOffset;

        if (!isGrounded && !isJumping) {
            if (!playerManager.isInteracting) {
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hitInfo, 0.5f, groundLayer)) {
            if (!isGrounded && !playerManager.isInteracting) {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            inAirTimer = 0;
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }
    }

    public void HandleJump() {
        if (isGrounded) {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpingHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;
        }
    }

    public void HandleEmotes(String emoteName) {
        if (isGrounded) {
            if (emoteName == "dab") {
                //animatorManager.animator.SetBool("isDabbing", true);
                animatorManager.PlayTargetAnimation("Dab", false);
            }

            if (emoteName == "flossing") {
                animatorManager.PlayTargetAnimation("Floss", false);
            }
        }
    }
}
