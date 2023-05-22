using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour {
    [Header("Config")]
    public PlayerReference playerReference;
    [Header("Audio")]
    public AudioHelper audioHelperWalking;
    public AudioHelper audioHelperRunning;

    private PlayerMovement playerMovement;
    private float timer = 0f;

    void Start() {
        playerMovement = playerReference.playerMovement;
    }

    void Update() {
        float delay = 3f / playerMovement.currentSpeed * playerMovement.move.magnitude;

        if (timer < delay) {
            timer += Time.deltaTime;
            return;
        }

        if (playerMovement.move.magnitude < 0.2f) return;
        if (!playerMovement.isGrounded) return;

        PlayAudio();

        timer = 0f;
    }

    private void PlayAudio() {
        List<GameObject> matchingObjects = playerReference.matchLooperObjects.matchingObjects;
        if (playerMovement.currentSpeed > playerMovement.baseSpeed) audioHelperRunning.PlayRandomClip(true, matchingObjects);
        else audioHelperWalking.PlayRandomClip(true, matchingObjects);
    }
}
