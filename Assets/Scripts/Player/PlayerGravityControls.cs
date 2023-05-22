using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using FishNet.Object;

public class PlayerGravityControls : NetworkBehaviour {
    [Header("Config")]
    public Transform playerTransform;
    public Transform cameraTransform;
    public float rotationSpeed = 5f;

    private PlayerMovement playerMovement;

    private Quaternion targetRotation;
    private bool isRotating = false;

    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update() {
        if (!base.IsOffline && !base.IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Z)) SetGravityDirection();
        if (!isRotating) return;

        RotatePlayer();
    }

    private void SetGravityDirection() {
        Vector3 currentDirection = playerMovement.gravityDirection;
        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.Normalize();

        if (lookDirection.y > 0.85f) playerMovement.gravityDirection = Vector3.up;
        else if (lookDirection.y < -0.85f) playerMovement.gravityDirection = Vector3.down;
        else if (lookDirection.x < -0.75f) playerMovement.gravityDirection = Vector3.left;
        else if (lookDirection.x > 0.75f) playerMovement.gravityDirection = Vector3.right;
        else if (lookDirection.z < -0.75f) playerMovement.gravityDirection = Vector3.back;
        else if (lookDirection.z > 0.75f) playerMovement.gravityDirection = Vector3.forward;

        Debug.Log(playerMovement.gravityDirection + " " + currentDirection);

        if (lookDirection == currentDirection) return;

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
