using UnityEngine;

public class FadeInOnEnable : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public float targetAlpha = 1f;
    public float transitionDuration = 1f;

    private float currentTransitionTime = 0f;

    void OnEnable() {
        canvasGroup.alpha = 0;
        currentTransitionTime = 0f;
    }

    void Update() {
        if (canvasGroup.alpha >= targetAlpha) return;

        currentTransitionTime += Time.deltaTime;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, currentTransitionTime / transitionDuration);
    }
}
