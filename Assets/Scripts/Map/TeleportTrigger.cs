using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour {
    public TeleportTrigger matchingTrigger;

    [Header("State")]
    public bool readyToBeTeleported = false;

    void OnTriggerEnter(Collider player) {
        if (player.tag != "Player") return;

        readyToBeTeleported = true;
    }

    void OnTriggerExit(Collider player) {
        if (player.tag != "Player") return;
        if (!readyToBeTeleported) return;

        Teleport(player);
        readyToBeTeleported = false;
    }

    private void Teleport(Collider player) {
        CharacterController characterController = player.gameObject.GetComponent<CharacterController>();
        Transform playerTransform = player.gameObject.transform;
        Vector3 currentVelocity = characterController.velocity;

        Vector3 newPosition = matchingTrigger.transform.position - transform.position;

        characterController.enabled = false; // Disable collision
        playerTransform.position += newPosition;
        characterController.enabled = true; // Enable collision
        characterController.Move(currentVelocity * Time.deltaTime); // Apply momentum
    }
}
