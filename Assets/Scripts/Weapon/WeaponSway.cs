using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour {
    [Header("Config")]
    public float smoothnessMultiplier = 10f;
    public float distanceMultiplier = 1f;

    private void Update() {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * distanceMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * distanceMultiplier;

        // calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // rotate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothnessMultiplier * Time.deltaTime);
    }
}
