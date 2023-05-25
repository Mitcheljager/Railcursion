using System.Collections.Generic;
using UnityEngine;

public class AttachToBone : MonoBehaviour {
    public Transform boneTransform;

    private void Start() {
        gameObject.transform.parent = boneTransform;
    }
}
