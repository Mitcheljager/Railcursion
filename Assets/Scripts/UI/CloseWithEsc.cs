using UnityEngine;

public class CloseWithEsc : MonoBehaviour {
    public bool destroy = false;

    void Update() {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (destroy) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}
