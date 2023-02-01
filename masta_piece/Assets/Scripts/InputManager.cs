using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    private ThirdPersonPlayerActions playerActions;
    private AnimatorManager animatorManager;
    private PlayerController playerController;
    
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;
    
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    public bool sprint_button;
    public bool jump_button;

    private void Awake() {
        animatorManager = GetComponent<AnimatorManager>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable() {
        if (playerActions == null) {
            playerActions = new ThirdPersonPlayerActions();
            
            playerActions.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerActions.Player.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerActions.Player.Sprinting.performed += i => sprint_button = true;
            playerActions.Player.Sprinting.canceled += i => sprint_button = false;
            playerActions.Player.Jump.performed += i => jump_button = true;
        }
        
        playerActions.Enable();
    }

    private void OnDisable() {
        playerActions.Disable();
    }

    public void HandleAllInputs() {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
    }

    private void HandleMovementInput() {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;
        
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerController.isSprinting);
    }

    private void HandleSprintingInput() {
        if (sprint_button && moveAmount > 0.5f) {
            playerController.isSprinting = true;
        }
        else {
            playerController.isSprinting = false;
        }
    }

    private void HandleJumpingInput() {
        if (jump_button) {
            jump_button = false;
            playerController.HandleJump();
        }
    }
}
