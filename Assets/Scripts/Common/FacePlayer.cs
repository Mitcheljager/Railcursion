using UnityEngine;

public class FacePlayer : MonoBehaviour {
    private void LateUpdate() {
        transform.LookAt(Camera.main.transform);
    }
}
