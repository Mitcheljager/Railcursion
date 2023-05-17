using System.Collections.Generic;
using UnityEngine;

public class MatchLooperObjects : MonoBehaviour {
    public GameObject prefab;
    public SceneLooper sceneLooper;
    [Header("Config")]
    public float countMultiplier = 1f;
    public bool requiresOwner = true;
    [Header("State")]
    public List<GameObject> matchingObjects;

    private PlayerReference playerReference;

    void Start() {
        sceneLooper = FindObjectOfType<SceneLooper>();
        playerReference = GetComponent<PlayerReference>();

        DuplicateObjects();
    }

    void Update() {
        // Match the transform of the duplicated objects to the transform of the original object
        foreach (GameObject matchingObject in matchingObjects) {
            Vector3 offset = matchingObject.GetComponent<MatchingLoopedObject>().offset;

            matchingObject.transform.position = transform.position + offset;
            matchingObject.transform.rotation = transform.rotation;
        }
    }

    private void DuplicateObjects() {
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.Add(prefab);

        matchingObjects = sceneLooper.DuplicateSceneObjects(gameObjects, true, countMultiplier);
        if (playerReference != null) {
            foreach (GameObject gameObject in matchingObjects) {
                PlayerReference gameObjectPlayerReference = gameObject.GetComponent<PlayerReference>();
                gameObjectPlayerReference.playerMovement = playerReference.playerMovement;
                gameObjectPlayerReference.playerState = playerReference.playerState;
            }
        }
    }
}
