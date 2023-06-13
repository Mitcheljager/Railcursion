using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleInOnEnable : MonoBehaviour {
    public float ratePerSecond = 1f;
	public bool x = true;
	public bool y = true;
	public bool z = true;

	private Vector3 startScale;
	private float currentScale = 0f;

    void OnEnable() {
		startScale = transform.localScale;
        currentScale = 0f;
    }

    void Update() {
        if (currentScale >= 1f) return;

        currentScale += ratePerSecond * Time.deltaTime;
        if (currentScale > 1f) currentScale = 1f;

        transform.localScale = new Vector3(
            x ? currentScale * startScale.x : startScale.x,
            y ? currentScale * startScale.y : startScale.y,
            z ? currentScale * startScale.z : startScale.z
        );
    }
}
