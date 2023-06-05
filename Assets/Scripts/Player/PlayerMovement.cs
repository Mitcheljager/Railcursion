using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerMovement : NetworkBehaviour {
    [Header("Components")]
    public CharacterController controller;
    public PlayerState playerState;
    public PlayerCamera playerCamera;

    [Header("Movement")]
    public float baseSpeed = 2f;
    public float runSpeed = 4f;
    public bool isRunning = false;

    [Header("Gravity")]
    public Vector3 gravityDirection = new Vector3(0f, -1f, 0f);
    public float gravity = 10f;
    public float maxVelocity = 120f;
    public Transform groundCheck;
    public float groundDistance = 0.25f;
    public LayerMask groundMask;

    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner), Header("State")]
    public float currentSpeed { get; [ServerRpc(RunLocally = true)] set; } = 0f;
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
    public Vector3 move { get; [ServerRpc(RunLocally = true)] set; } = Vector3.zero;
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
    public Vector3 velocity { get; [ServerRpc(RunLocally = true)] set; } = Vector3.zero;
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
    public float inputX { get; [ServerRpc(RunLocally = true)] set; } = 0f;
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
    public float inputZ { get; [ServerRpc(RunLocally = true)] set; } = 0f;
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
    public bool isGrounded { get; [ServerRpc(RunLocally = true)] set; } = false;

    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Apply downwords velocity of player is not on the ground
        // if (controller.isGrounded && velocity.y < 0) velocity = gravityDirection * 2;
        if (isGrounded && IsGravityPositive()) velocity = gravityDirection * 2;

        currentSpeed = baseSpeed;
        if (move.magnitude == 0f) currentSpeed = 0f;

        if (!base.IsOffline && !base.IsOwner) return;

        if (Input.GetKeyDown(KeyCode.R)) {
            controller.enabled = false; // Disable collision
            transform.position = new Vector3(5, 2, 5);
            controller.enabled = true; // Enable collision
            gravityDirection = new Vector3(0f, -1f, 0f);

            playerState.Respawn();
        }

        if (playerState.isDead) return;

        SetRunning();
        SetMovement();
    }

    private void SetRunning() {
        if (!Input.GetKey(KeyCode.LeftShift)) {
            isRunning = false;
            return;
        }

        isRunning = move.magnitude > 0;
        currentSpeed = runSpeed;
    }

    private void SetMovement() {
        SetMovementValues();
        controller.Move(move * currentSpeed * Time.deltaTime);

        SetVelocityValues();
        controller.Move(velocity * Time.deltaTime);
    }

    private void SetMovementValues() {
        inputX = isGrounded ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        inputZ = isGrounded ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        move = transform.right * inputX + transform.forward * inputZ;

        if (move.magnitude > 1) move.Normalize();
    }

    private void SetVelocityValues() {
        velocity += gravityDirection * gravity * Time.deltaTime;
        float velocityMagnitude = velocity.magnitude;
        if (velocityMagnitude > maxVelocity) velocity = velocity.normalized * maxVelocity;
    }

    public bool IsGravityPositive() {
        return (velocity.x + velocity.y + velocity.z) * (gravityDirection.x + gravityDirection.y + gravityDirection.z) > 0;
    }
}
