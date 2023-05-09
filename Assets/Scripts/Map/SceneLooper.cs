using System.Collections.Generic;
using UnityEngine;

public class SceneLooper : MonoBehaviour {
    public List<GameObject> objectsToDuplicate;
    public int duplicationCount = 3;
    public Vector3 duplicationOffset = new Vector3(100f, 100f, 100f);

    private void Start() {
        DuplicateSceneObjects(objectsToDuplicate);
    }

    public List<GameObject> DuplicateSceneObjects(List<GameObject> objects, bool applyOffsetComponent = false, int countOverwrite = 0) {
        // Calculate the total number of duplicates in each direction
        int count = countOverwrite > 0 ? countOverwrite : duplicationCount;
        int totalDuplicationCount = (2 * count + 1) * (2 * count + 1) * (2 * count + 1);

        List<GameObject> duplicatedObjects = new List<GameObject>();

        // Iterate over the objects to duplicate
        foreach (GameObject gameObject in objects) {
            // Duplicate the object and position the duplicates in a grid pattern
            for (int i = -count; i <= count; i++) {
                for (int j = -count; j <= count; j++) {
                    for (int k = -count; k <= count; k++) {
                        Vector3 currentOffset = new Vector3(i * duplicationOffset.x, j * duplicationOffset.y, k * duplicationOffset.z);
                        if (currentOffset.magnitude == 0) continue;

                        GameObject duplicate = Instantiate(gameObject);
                        duplicate.transform.position += currentOffset;
                        if (applyOffsetComponent) {
                            MatchingLoopedObject matchingLoopedObject = duplicate.AddComponent<MatchingLoopedObject>();
                            matchingLoopedObject.offset = duplicate.transform.position;
                        }

                        duplicatedObjects.Add(duplicate);
                    }
                }
            }
        }

        return duplicatedObjects;
    }
}
