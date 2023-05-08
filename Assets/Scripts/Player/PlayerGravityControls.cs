using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerGravityControls : MonoBehaviour {
    [Header("Config")]
    public Transform playerTransform;
    public Transform cameraTransform;
    public float rotationSpeed = 5f;
    [Header("Post Processing")]
    public Volume volume;

    private PlayerMovement playerMovement;
    private CharacterController characterController;

    private Quaternion targetRotation;
    private bool isRotating = false;

    private ColorAdjustments colorAdjustments;
    private float hueShift;

    void Start() {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();

        volume.profile.TryGet(out colorAdjustments);

        HueShift();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) SetGravityDirection();
        if (!isRotating) return;

        RotatePlayer();
    }

    private void SetGravityDirection() {
        Vector3 currentDirection = playerMovement.gravityDirection;
        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.Normalize();


        if (lookDirection.y > 0.85f) playerMovement.gravityDirection = Vector3.up;
        else if (lookDirection.y < -0.85f) playerMovement.gravityDirection = Vector3.down;
        else if (lookDirection.x < -0.75f) playerMovement.gravityDirection = Vector3.left;
        else if (lookDirection.x > 0.75f) playerMovement.gravityDirection = Vector3.right;
        else if (lookDirection.z < -0.75f) playerMovement.gravityDirection = Vector3.back;
        else if (lookDirection.z > 0.75f) playerMovement.gravityDirection = Vector3.forward;

        Debug.Log(playerMovement.gravityDirection + " " + currentDirection);

        if (lookDirection == currentDirection) return;

        // Calculate the rotation based on the gravity direction
        targetRotation = Quaternion.FromToRotation(Vector3.down, playerMovement.gravityDirection.normalized);
        isRotating = true;

        playerMovement.velocity = Vector3.zero;

        HueShift();
    }

    private void RotatePlayer() {
        float rotationStep = rotationSpeed * Time.deltaTime;

        // Rotate the player gradually towards the target rotation
        playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotationStep);

        // Check if the rotation has reached the target rotation
        if (Quaternion.Angle(playerTransform.rotation, targetRotation) < 0.1f) isRotating = false;
    }

    private void HueShift() {
        Vector3 direction = playerMovement.gravityDirection;
        Color color = new Color(direction.x, direction.y * -1, direction.z);

        // Convert the color to HSV
        Color.RGBToHSV(color, out float h, out float s, out float v);

        // Map the hue value from [0, 1] to [-180, 180]
        float targetHueShift = Mathf.Lerp(-160f, 160f, h);

        // Start the coroutine to interpolate the hueShift value over time
        StartCoroutine(LerpHueShift(hueShift, targetHueShift));
    }

    private IEnumerator LerpHueShift(float from, float to) {
        float elapsed = 0f;

        while (elapsed < 1f) {
            hueShift = Mathf.Lerp(from, to, elapsed);
            colorAdjustments.hueShift.value = hueShift;

            elapsed += Time.deltaTime * rotationSpeed / 100;
            yield return null;
        }

        hueShift = to;
        colorAdjustments.hueShift.value = hueShift;
    }
}
