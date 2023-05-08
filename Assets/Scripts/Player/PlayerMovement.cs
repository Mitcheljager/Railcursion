using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
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
    public Transform groundCheck;
    public float groundDistance = 0.25f;
    public LayerMask groundMask;

    [Header("State")]
    public float currentSpeed;
    public Vector3 move;
    public Vector3 velocity;
    public float inputX;
    public float inputZ;
    public bool isGrounded = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            controller.Move(new Vector3(0, 2, 0) - transform.position);
            playerState.isDead = false;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Apply downwords velocity of player is not on the ground
        // if (controller.isGrounded && velocity.y < 0) velocity = gravityDirection * 2;
        if (isGrounded && velocity.y * GravityDirectionPositiveOrNegative() > 0) velocity = gravityDirection * 2;

        currentSpeed = baseSpeed;
        if (move.magnitude == 0f) currentSpeed = 0f;

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
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        move = transform.right * inputX + transform.forward * inputZ;

        if (move.magnitude > 1) move /= move.magnitude;

        controller.Move(move * currentSpeed * Time.deltaTime);
        velocity += gravityDirection * gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Limit velocity
        float velocityMagnitude = velocity.magnitude;
        if (velocityMagnitude > gravity) velocity = velocity.normalized * gravity;
    }

    private float GravityDirectionPositiveOrNegative() {
        return gravityDirection.x + gravityDirection.y + gravityDirection.z;
    }
}
