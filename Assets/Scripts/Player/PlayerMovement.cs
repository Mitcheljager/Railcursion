using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;

public class PlayerMovement : NetworkBehaviour {
    [Header("Components")]
    public CharacterController controller;
    public PlayerState playerState;

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

    [Header("State")]
    [SyncVar(Channel = Channel.Unreliable)] public float currentSpeed;
    [SyncVar(Channel = Channel.Unreliable)] public Vector3 move;
    [SyncVar(Channel = Channel.Unreliable)] public Vector3 velocity;
    [SyncVar(Channel = Channel.Unreliable)] public float inputX;
    [SyncVar(Channel = Channel.Unreliable)] public float inputZ;
    [SyncVar(Channel = Channel.Unreliable)] public bool isGrounded = false;

    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Apply downwords velocity of player is not on the ground
        // if (controller.isGrounded && velocity.y < 0) velocity = gravityDirection * 2;
        if (isGrounded && IsGravityPositive()) velocity = gravityDirection * 2;

        currentSpeed = baseSpeed;
        if (move.magnitude == 0f) currentSpeed = 0f;

        if (playerState.isDead) return;
        if (!base.IsOffline && !base.IsOwner) return;

        if (Input.GetKeyDown(KeyCode.R)) {
            controller.enabled = false; // Disable collision
            transform.position = new Vector3(0, 2, 0);
            controller.enabled = true; // Enable collision
            playerState.isDead = false;
        }

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
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        move = transform.right * inputX + transform.forward * inputZ;

        if (move.magnitude > 1) move /= move.magnitude;

        controller.Move(move * currentSpeed * Time.deltaTime);

        velocity += gravityDirection * gravity * Time.deltaTime;
        float velocityMagnitude = velocity.magnitude;
        if (velocityMagnitude > maxVelocity) velocity = velocity.normalized * maxVelocity;

        controller.Move(velocity * Time.deltaTime);

    }

    public bool IsGravityPositive() {
        return velocity.y * (gravityDirection.x + gravityDirection.y + gravityDirection.z) > 0;
    }
}
