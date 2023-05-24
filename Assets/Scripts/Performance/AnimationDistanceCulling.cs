using UnityEngine;

public class AnimationDistanceCulling : MonoBehaviour {
    public float distanceThreshold = 10f; // Define your distance threshold here

    private void Start() {
        if (Camera.main == null) return;

        Animator animator = GetComponent<Animator>();
        Transform cameraTransform = Camera.main.transform; // Assuming you're using the main camera, modify as needed

        float distance = Vector3.Distance(transform.position, cameraTransform.position);

        if (distance > distanceThreshold) animator.enabled = false;
    }
}
