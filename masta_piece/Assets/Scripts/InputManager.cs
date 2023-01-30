using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    private ThirdPersonPlayerActions playerActions;

    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;

    private void OnEnable() {
        if (playerActions == null) {
            playerActions = new ThirdPersonPlayerActions();
            
            playerActions.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
        }
        
        playerActions.Enable();
    }

    private void OnDisable() {
        playerActions.Disable();
    }

    public void HandleAllInputs() {
        HandleMovementInput();
    }

    private void HandleMovementInput() {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }
}
