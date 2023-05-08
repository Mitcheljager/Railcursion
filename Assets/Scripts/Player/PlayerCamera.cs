using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    public float mouseSensitivity = 25f;
    public Transform playerBody;
    public PlayerState playerState;

    private float xRotation = 0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update() {
        if(Cursor.lockState != CursorLockMode.Locked) return;
        if (playerState.isDead) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
