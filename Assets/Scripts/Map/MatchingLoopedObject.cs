using System.Collections.Generic;
using UnityEngine;

public class MatchingLoopedObject : MonoBehaviour {
    [Header("Child")]
    public Transform childMatchingTransform;
    [Header("State")]
    public bool initialized = false;
    public Vector3 offset;
}
