using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour {
    [Header("AudioHelper Config")]
    public float minPitch = 1f;
    public float maxPitch = 1f;
    [Header("AudioHelper Optional")]
    public AudioClip[] audioClips;

    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip() {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }

    public void PlayWithRandomPitch() {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }
}
