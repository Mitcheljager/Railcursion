using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBox : MonoBehaviour {
    private BoxCollider boxCollider;
    private Vector3 size;

    void Start() {
        boxCollider = GetComponent<BoxCollider>();
        size = boxCollider.bounds.size;
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag != "Player") return;

        Teleport(collider);
    }

    private void Teleport(Collider player) {
        CharacterController characterController = player.gameObject.GetComponent<CharacterController>();
        Transform playerTransform = player.gameObject.transform;
        Vector3 currentVelocity = characterController.velocity;

        Vector3 playerPosition = player.transform.position;
        Vector3 boxPosition = transform.position;
        Vector3 difference = Vector3.zero;

        if (playerPosition.y < boxPosition.y - size.y / 2) difference.y = size.y;
        else if (playerPosition.y > boxPosition.y + size.y / 2) difference.y = -size.y;

        if (playerPosition.x < boxPosition.x - size.x / 2) difference.x = size.x;
        else if (playerPosition.x > boxPosition.x + size.x / 2) difference.x = -size.x;

        if (playerPosition.z < boxPosition.z - size.z / 2) difference.z = size.z;
        else if (playerPosition.z > boxPosition.z + size.z / 2) difference.z = -size.z;

        characterController.enabled = false; // Disable collision
        playerTransform.position = playerPosition + difference;
        characterController.enabled = true; // Enable collision
        characterController.Move(currentVelocity * Time.deltaTime); // Apply momentum

        MoveOneShotAudio(difference);
    }

    private void MoveOneShotAudio(Vector3 difference) {
        AudioSource[] allAudioSources = GameObject.FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources) {
            if (audioSource.gameObject.name != "One shot audio") continue;

            audioSource.transform.position += difference;
        }
    }
}
