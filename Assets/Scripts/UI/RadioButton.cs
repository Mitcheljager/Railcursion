using UnityEngine;

public class RadioButton : MonoBehaviour {
    public string value = "";
    public GameObject[] activeObjects;
    public bool active = false;

    void Start() {
        SetState(active);
    }

    public void SetState(bool state) {
        active = state;

        foreach(GameObject activeObject in activeObjects) {
            activeObject.SetActive(active);
        }
    }
}
