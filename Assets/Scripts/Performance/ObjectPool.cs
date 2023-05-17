using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public GameObject prefab;
    public int poolCount = 50;

    private List<GameObject> pooledObjects = new List<GameObject>();

    void Start() {
        for (int i = 0; i < poolCount; i++) {
            CreateObject();
        }
    }

    public GameObject GetObject() {
        // Check if there is an inactive object in the pool
        foreach (GameObject gameObject in pooledObjects) {
            if (!gameObject.activeInHierarchy) {
                gameObject.SetActive(true);
                return gameObject;
            }
        }

        // If no inactive object found, create a new one
        GameObject newObject = CreateObject(true);
        poolCount++;
        return newObject;
    }

    public IEnumerator DelaySetInactive(GameObject gameObject, float delay) {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    private GameObject CreateObject(bool state = false) {
        GameObject newObject = Instantiate(prefab);
        newObject.SetActive(state);
        pooledObjects.Add(newObject);
        return newObject;
    }
}
