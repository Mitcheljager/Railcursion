using System.Collections.Generic;
using UnityEngine;

public class SceneLooper : MonoBehaviour {
    public List<GameObject> objectsToDuplicate;
    public int duplicationCount = 3;
    public Vector3 duplicationOffset = new Vector3(100f, 100f, 100f);

    private void Start() {
        DuplicateSceneObjects(objectsToDuplicate);
    }

    public List<GameObject> DuplicateSceneObjects(List<GameObject> objects, bool applyOffsetComponent = false) {
        // Calculate the total number of duplicates in each direction
        int totalDuplicationCount = (2 * duplicationCount + 1) * (2 * duplicationCount + 1) * (2 * duplicationCount + 1);

        List<GameObject> duplicatedObjects = new List<GameObject>();

        // Iterate over the objects to duplicate
        foreach (GameObject gameObject in objects) {
            // Duplicate the object and position the duplicates in a grid pattern
            for (int i = -duplicationCount; i <= duplicationCount; i++) {
                for (int j = -duplicationCount; j <= duplicationCount; j++) {
                    for (int k = -duplicationCount; k <= duplicationCount; k++) {
                        Vector3 currentOffset = new Vector3(i * duplicationOffset.x, j * duplicationOffset.y, k * duplicationOffset.z);
                        if (currentOffset.magnitude == 0) continue;

                        GameObject duplicate = Instantiate(gameObject);
                        duplicate.transform.position += currentOffset;
                        if (applyOffsetComponent) {
                            MatchingLoopedObject matchingLoopedObject = duplicate.AddComponent<MatchingLoopedObject>();
                            matchingLoopedObject.offset = duplicate.transform.position;
                            Debug.Log(matchingLoopedObject.offset);
                        }

                        duplicatedObjects.Add(duplicate);
                    }
                }
            }
        }

        return duplicatedObjects;
    }
}
