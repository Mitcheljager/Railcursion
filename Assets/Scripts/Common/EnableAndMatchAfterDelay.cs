using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnableAndMatchAfterDelay : MonoBehaviour {
    public GameObject objectToEnable;
    public float timer;

    void OnEnable() {
        StartCoroutine(Activate());
    }

    private IEnumerator Activate() {
        yield return new WaitForSeconds(timer);

        objectToEnable.transform.position = gameObject.transform.position;
        objectToEnable.SetActive(true);
    }
}
