using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class AudioHelper : NetworkBehaviour {
    [Header("AudioHelper Config")]
    public float minPitch = 1f;
    public float maxPitch = 1f;
    [Header("AudioHelper Optional")]
    public AudioClip[] audioClips;

    public AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip(bool leaveAtPosition = true, List<GameObject> matchingObjects = null, int loopMax = 8) {
        audioSource.pitch = Random.Range(minPitch, maxPitch);

        AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)];

        if (leaveAtPosition && !base.IsOffline && !base.IsOwner) {
            Vector3 position = GetNearestObjectPosition(matchingObjects, loopMax);
            AudioSource.PlayClipAtPoint(randomClip, position, audioSource.volume);
        } else audioSource.PlayOneShot(randomClip);
    }

    public void PlayWithRandomPitch() {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }

    public Vector3 GetNearestObjectPosition(List<GameObject> matchingObjects, int loopMax) {
        if (matchingObjects == null || matchingObjects.Count == 0) return transform.position;

        Vector3 cameraPosition = Camera.main.transform.position;
        Transform originalTransform = transform;
        Vector3 originalOffset = originalTransform.localPosition;

        GameObject closestObject = gameObject;
        float closestDistance = Vector3.Distance(cameraPosition, originalTransform.position);

        // Only loop through the first 8, as any other will always be further away
        for (int i = 0; i < Mathf.Min(loopMax, matchingObjects.Count); i++) {
            GameObject matchingObject = matchingObjects[i];
            if (matchingObject == null) continue;

            float distance = Vector3.Distance(cameraPosition, matchingObject.transform.position);

            if (distance > closestDistance) continue;

            closestObject = matchingObject;
            closestDistance = distance;
        }

        return closestObject.transform.position;
    }
}
