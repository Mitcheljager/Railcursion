using UnityEngine;

public class SceneLooper : MonoBehaviour {
    public GameObject[] objectsToDuplicate;
    public int duplicationCount = 3;
    public Vector3 duplicationOffset = new Vector3(100f, 100f, 100f);

    private void Start() {
        DuplicateSceneObjects();
    }

    private void DuplicateSceneObjects() {
        // Calculate the total number of duplicates in each direction
        int totalDuplicationCount = (2 * duplicationCount + 1) * (2 * duplicationCount + 1) * (2 * duplicationCount + 1);

        // Iterate over the objects to duplicate
        foreach (GameObject obj in objectsToDuplicate) {
            // Duplicate the object and position the duplicates in a grid pattern
            for (int i = -duplicationCount; i <= duplicationCount; i++) {
                for (int j = -duplicationCount; j <= duplicationCount; j++) {
                    for (int k = -duplicationCount; k <= duplicationCount; k++) {
                        GameObject duplicate = Instantiate(obj);
                        duplicate.transform.position += new Vector3(i * duplicationOffset.x, j * duplicationOffset.y, k * duplicationOffset.z);
                    }
                }
            }
        }
    }
}
