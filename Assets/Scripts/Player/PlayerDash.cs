using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour {
    [Header("Dash")]
    public float dashForce = 10f;
    public float upwardForce = 1f;

    private PlayerMovement playerMovement;
    private PlayerState playerState;

    private bool canDash = true;

    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        playerState = GetComponent<PlayerState>();
    }

    void Update() {
        if (playerState.isDead) return;

        if (playerMovement.isGrounded) canDash = true;

        if (!canDash) return;
        if (playerMovement.isGrounded) return;

        if (Input.GetKeyDown(KeyCode.Space)) Dash();
    }

    private void Dash() {
        Vector3 dashDirection = playerMovement.move.normalized;

        // Get the up vector relative to the player's orientation
        Vector3 upwardVector = transform.up;
        // Add upward force relative to player's orientation
        dashDirection += upwardVector * upwardForce;

        // Cancel out original momentum
        playerMovement.velocity = Vector3.zero;

        playerMovement.velocity += dashDirection * dashForce;
        canDash = false;
    }
}