using System.Collections.Generic;
using UnityEngine;

public class MatchLooperObjects : MonoBehaviour {
    public GameObject prefab;
    public Transform childMatcherTransform;
    [Header("Config")]
    public bool keepMatchingTransform = true;
    public float countMultiplier = 1f;
    public bool useObjectPool = false;
    public ObjectPool objectPool;
    public bool cull = false;
    [Header("State")]
    public List<GameObject> matchingObjects;

    [HideInInspector] public SceneLooper sceneLooper;

    private PlayerReference playerReference;

    void Start() {
        sceneLooper = FindObjectOfType<SceneLooper>();
        playerReference = GetComponent<PlayerReference>();

        DuplicateObjects();
    }

    void Update() {
        if (!keepMatchingTransform) return;

        // Match the transform of the duplicated objects to the transform of the original object
        foreach (GameObject matchingObject in matchingObjects) {
            MatchingLoopedObject matchingLoopedObject = matchingObject.GetComponent<MatchingLoopedObject>();
            Vector3 offset = matchingLoopedObject.offset;

            matchingObject.transform.position = transform.position + offset;
            matchingObject.transform.rotation = transform.rotation;

            if (childMatcherTransform) {
                matchingLoopedObject.childMatchingTransform.localPosition = childMatcherTransform.localPosition;
                matchingLoopedObject.childMatchingTransform.localRotation = childMatcherTransform.localRotation;
            }
        }
    }

    private void DuplicateObjects() {
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.Add(prefab);

        matchingObjects = sceneLooper.DuplicateSceneObjects(gameObjects, true, countMultiplier, objectPool, cull);
        if (playerReference != null) {
            foreach (GameObject gameObject in matchingObjects) {
                PlayerReference gameObjectPlayerReference = gameObject.GetComponent<PlayerReference>();
                gameObjectPlayerReference.playerMovement = playerReference.playerMovement;
                gameObjectPlayerReference.playerState = playerReference.playerState;
            }
        }
    }
}
