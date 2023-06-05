using UnityEngine;
using System;
using System.Collections;

public class BobAndRotate : MonoBehaviour {
    public float floatStrength = 1f;
    public float rotationSpeed = 30f;

    private float originalY;
    private Coroutine rotationCoroutine;

    void Start() {
        originalY = transform.position.y;
    }

    void OnEnable() {
        rotationCoroutine = StartCoroutine(RotateObject());
    }

    private void OnDisable() {
        if (rotationCoroutine == null) return;

        StopCoroutine(rotationCoroutine);
        rotationCoroutine = null;
    }

    void Update() {
        transform.position = new Vector3(
            transform.position.x,
            originalY + ((float)Math.Sin(Time.time) * floatStrength),
            transform.position.z);
    }

    private IEnumerator RotateObject() {
        while (true) {
            transform.Rotate(new Vector3(rotationSpeed, rotationSpeed, rotationSpeed) * Time.deltaTime);
            yield return null;
        }
    }
}
