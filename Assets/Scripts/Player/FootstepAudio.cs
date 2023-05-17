using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour {
    [Header("Config")]
    public PlayerMovement playerMovement;
    [Header("Audio")]
    public AudioHelper audioHelperWalking;
    public AudioHelper audioHelperRunning;

    private float timer = 0f;

    void Update() {
        float delay = 3f / playerMovement.currentSpeed * playerMovement.move.magnitude;

        if (timer < delay) {
            timer += Time.deltaTime;
            return;
        }

        if (playerMovement.move.magnitude < 0.2f) return;
        if (!playerMovement.isGrounded) return;

        if (playerMovement.currentSpeed > playerMovement.baseSpeed) audioHelperRunning.PlayRandomClip();
        else audioHelperWalking.PlayRandomClip();

        timer = 0f;
    }
}
