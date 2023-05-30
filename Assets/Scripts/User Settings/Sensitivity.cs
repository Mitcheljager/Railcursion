using UnityEngine;

public class Sensitivity : MonoBehaviour {
    public PlayerCamera playerCamera;

    private float baseSensitivity;

    void Start() {
        playerCamera = Camera.main.GetComponent<PlayerCamera>();
        baseSensitivity = playerCamera.mouseSensitivity;

        SetSensitivity();
    }

    public void SetSensitivity() {
        playerCamera.mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 25f) * 10f;
    }
}
