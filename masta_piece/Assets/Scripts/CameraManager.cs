using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour {
    private InputManager inputManager;
    public Transform targetTransform; //The object the camera follows
    public Transform cameraPivot; //Object to pivot camera
    public Transform cameraTransform;
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public LayerMask collisionLayers; //Camera collision layers
    private Vector3 cameraVectorPosition;

    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 0.5f;
    public float cameraPivotSpeed = 0.5f;
    public float cameraCollisionOffset = 0.2f; // how much camera jumps when colliding
    public float minimumCollisionOffset = 0.2f;
    
    public float minimumPivotAngle = -35f;
    public float maximumPivotAngle = 35f;
    public float cameraCollisionRadius = 0.2f;
    public float lookAngle;
    public float pivotAngle;

    private void Awake() {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement() {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget() {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera() {
        Vector3 rotation;
        Quaternion targetRotation;
        
        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);
        
        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions() {
        float targetPosition = defaultPosition;
        RaycastHit hitInfo;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hitInfo,
                Mathf.Abs(targetPosition), collisionLayers)) {
            float distance = Vector3.Distance(cameraPivot.position, hitInfo.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset) {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
