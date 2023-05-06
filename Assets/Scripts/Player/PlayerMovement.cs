using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Components")]
    public CharacterController controller;

    [Header("Movement")]
    public float baseSpeed = 2f;
    public float runSpeed = 4f;
    public bool isRunning = false;

    [Header("Gravity")]
    public Vector3 gravityDirection = new Vector3(0f, -1f, 0f);
    public float gravity = 10f;

    [Header("State")]
    public float currentSpeed;
    public Vector3 move;
    public Vector3 velocity;
    public bool isGrounded;
    public float inputX;
    public float inputZ;

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) controller.Move(new Vector3(0, 2, 0) - transform.position);

        // Apply downwords velocity of player is not on the ground
        if (controller.isGrounded && velocity.y < 0) velocity = gravityDirection * 2;

        currentSpeed = baseSpeed;
        if (move.magnitude == 0f) currentSpeed = 0f;

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
}
