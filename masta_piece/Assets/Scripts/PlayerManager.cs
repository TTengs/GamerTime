using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    private InputManager inputManager;
    private PlayerController playerController;
    private CameraManager cameraManager;
    private Animator animator;

    public bool isInteracting;
    private void Awake() {
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate() {
        playerController.HandleAllMovement();
    }

    private void LateUpdate() {
        cameraManager.HandleAllCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
        playerController.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerController.isGrounded);
    }
}
