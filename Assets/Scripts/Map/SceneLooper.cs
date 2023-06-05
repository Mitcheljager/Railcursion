using System.Collections.Generic;
using UnityEngine;

public class SceneLooper : MonoBehaviour {
    public List<GameObject> objectsToDuplicate;
    public Vector3 duplicationCount = new Vector3(3, 3, 3);
    public Vector3 duplicationOffset = new Vector3(100f, 100f, 100f);

    private void Start() {
        DuplicateSceneObjects(objectsToDuplicate);
    }

    public List<GameObject> DuplicateSceneObjects(List<GameObject> objects, bool applyOffsetComponent = false, float countMultiplier = 1, ObjectPool objectPool = null) {
        // Calculate the total number of duplicates in each direction
        Vector3 counts = duplicationCount * countMultiplier;
        List<GameObject> duplicatedObjects = new List<GameObject>();

        // Iterate over the objects to duplicate
        foreach (GameObject gameObject in objects) {
            // Duplicate the object and position the duplicates in a grid pattern
            for (int i = Mathf.RoundToInt(-counts.x); i <= Mathf.RoundToInt(counts.x); i++) {
                for (int j = Mathf.RoundToInt(-counts.y); j <= Mathf.RoundToInt(counts.y); j++) {
                    for (int k = Mathf.RoundToInt(-counts.z); k <= Mathf.RoundToInt(counts.z); k++) {
                        Vector3 currentOffset = new Vector3(i * duplicationOffset.x, j * duplicationOffset.y, k * duplicationOffset.z);
                        if (currentOffset.magnitude == 0) continue;

                        // Get from pool instead of Instantiate

                        GameObject duplicate = objectPool == null ? Instantiate(gameObject) : objectPool.GetObject();
                        duplicate.transform.position += currentOffset;
                        if (applyOffsetComponent) {
                            MatchingLoopedObject matchingLoopedObject = duplicate.GetComponent<MatchingLoopedObject>();
                            if (matchingLoopedObject == null) matchingLoopedObject = duplicate.AddComponent<MatchingLoopedObject>();
                            matchingLoopedObject.offset = duplicate.transform.position;
                        }

                        duplicatedObjects.Add(duplicate);
                    }
                }
            }
        }

        duplicatedObjects.Sort((a, b) => a.transform.position.magnitude.CompareTo(b.transform.position.magnitude));

        return duplicatedObjects;
    }
}
