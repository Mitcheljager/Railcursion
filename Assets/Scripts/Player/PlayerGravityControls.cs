using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityControls : MonoBehaviour {
    public Transform playerTransform;
    public Transform cameraTransform;
    public float rotationSpeed = 5f;

    private PlayerMovement playerMovement;
    private CharacterController characterController;

    private Quaternion targetRotation;
    private bool isRotating = false;

    void Start() {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) SetGravityDirection();
        if (isRotating) RotatePlayer();
    }

    private void SetGravityDirection() {
        // if (isRotating) return;

        playerMovement.gravityDirection = -playerMovement.gravityDirection;

        // Calculate the rotation based on the gravity direction
        targetRotation = Quaternion.FromToRotation(Vector3.down, playerMovement.gravityDirection.normalized);
        isRotating = true;

        playerMovement.velocity = Vector3.zero;
    }

    private void RotatePlayer() {
        float rotationStep = rotationSpeed * Time.deltaTime;

        // Rotate the player gradually towards the target rotation
        playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotationStep);

        // Check if the rotation has reached the target rotation
        if (Quaternion.Angle(playerTransform.rotation, targetRotation) < 0.1f) isRotating = false;
    }
}
