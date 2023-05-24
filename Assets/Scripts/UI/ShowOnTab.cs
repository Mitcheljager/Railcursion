using UnityEngine;

public class ShowOnTab : MonoBehaviour {
    public GameObject objectToShow;

    private void Update() {
        objectToShow.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
