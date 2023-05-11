using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFalling : MonoBehaviour {
    [Header("Audio")]
    public AudioSource audioSource;
    public float maximumVolume = 0.2f;

    [Header("Velocity")]
    public float minimumVelocity = 60f;
    public float maximumVelocity = 120f;

    [Header("Animation")]
    public Transform shoulderTransform;
    public float maxDistanceWhileFalling = 0.1f;
    public float animationSpeed = 5f;

    private PlayerMovement playerMovement;
    private PlayerState playerState;

    public bool hasLanded = false;
    private Vector3 shoulderOriginalPosition;

    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        playerState = GetComponent<PlayerState>();

        shoulderOriginalPosition = shoulderTransform.localPosition;
    }

    void Update() {
        MoveWhileFalling();

        if (!playerMovement.isGrounded) MoveWhileFalling();

        if (playerMovement.velocity.magnitude < minimumVelocity) {
            if (audioSource.isPlaying) audioSource.Stop();
            return;
        }

        if (!playerMovement.isGrounded && hasLanded) hasLanded = false;

        SetVolume();
    }

    private void SetVolume() {
        float normalizedVelocity = Mathf.InverseLerp(minimumVelocity, maximumVelocity, playerMovement.velocity.magnitude);
        audioSource.volume = Mathf.Lerp(0f, maximumVolume, normalizedVelocity);

        if (!audioSource.isPlaying) audioSource.Play();
    }

    private void MoveWhileFalling() {
        float distance = playerMovement.velocity.magnitude == 0f ? 0f : maxDistanceWhileFalling * (1f / maximumVelocity * playerMovement.velocity.magnitude);
        if (!playerMovement.IsGravityPositive()) distance *= -5f;
        Vector3 targetPosition = new Vector3(0, distance, 0);

        shoulderTransform.localPosition = Vector3.Slerp(shoulderTransform.localPosition, targetPosition, animationSpeed * Time.deltaTime);
    }
}
