using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowObjectOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject objectToToggle;

    void OnDisable() {
        objectToToggle.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        objectToToggle.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!eventData.fullyExited) return;
        objectToToggle.SetActive(false);
    }
}
