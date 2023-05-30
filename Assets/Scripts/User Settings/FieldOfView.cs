using UnityEngine;

public class FieldOfView : MonoBehaviour {
    private float baseFoV;

    void Awake() {
        baseFoV = Camera.main.fieldOfView;
    }

    void Start() {
        SetFieldOfView();
    }

    public void SetFieldOfView() {
        Camera.main.fieldOfView = PlayerPrefs.GetFloat("FieldOfView", baseFoV);
    }
}
