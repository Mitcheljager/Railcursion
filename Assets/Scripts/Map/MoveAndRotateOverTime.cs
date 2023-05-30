using UnityEngine;

public class MoveAndRotateOverTime : MonoBehaviour {
    public float speed = 1.0f;
    public float rotationSpeed = 1.0f;

    private void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
