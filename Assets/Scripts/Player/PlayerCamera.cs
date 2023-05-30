using UnityEngine;
using FishNet.Object;

public class PlayerCamera : NetworkBehaviour {
    public float mouseSensitivity = 25f;
    public Transform playerBody;
    public PlayerState playerState;
    public SkinnedMeshRenderer[] meshRenderers;
    public GameObject[] objectsToDisable;

    private float xRotation = 0f;

    public override void OnStartClient() {
        base.OnStartClient();
        if (!base.IsOffline && !base.IsOwner) return;

        Camera cam = GetComponent<Camera>();
        AudioListener audioListener = GetComponent<AudioListener>();
        cam.enabled = true;
        audioListener.enabled = true;

        foreach(SkinnedMeshRenderer meshRenderer in meshRenderers) {
            meshRenderer.enabled = false;
        }

        foreach(GameObject objectToDisable in objectsToDisable) {
            objectToDisable.SetActive(false);
        }
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update() {
        if (!base.IsOffline && !base.IsOwner) return;

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
