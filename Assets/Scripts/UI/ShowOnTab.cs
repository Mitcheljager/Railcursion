using UnityEngine;

public class ShowOnTab : MonoBehaviour {
    public GameObject objectToShow;

    void Update() {
        objectToShow.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
