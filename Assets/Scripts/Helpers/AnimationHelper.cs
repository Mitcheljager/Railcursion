using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour {
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private bool isAnimating = false;

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SlideIn(float targetHeight = 40f, float duration = 0.5f) {
        if (isAnimating) return;
        StartCoroutine(SlideInCoroutine(targetHeight, duration));
    }

    public void FadeOut(float duration = 0.5f, bool force = false) {
        if (isAnimating) return;
        if (!force && canvasGroup.alpha == 0f) return;
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(duration));
    }

    public void FadeIn(float duration = 0.5f, bool force = false) {
        if (isAnimating) return;
        if (!force && canvasGroup.alpha == 1f) return;
        StartCoroutine(FadeInCoroutine(duration));
    }

    private IEnumerator SlideInCoroutine(float targetHeight, float duration) {
        isAnimating = true;
        float timer = 0f;

        canvasGroup.alpha = 0;

        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = 0;
        rectTransform.sizeDelta = sizeDelta;

        while (timer < duration) {
            float currentAlpha = Mathf.Lerp(0f, 1f, timer / duration);
            float currentHeight = Mathf.Lerp(0f, targetHeight, timer / duration);

            canvasGroup.alpha = currentAlpha;

            sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = currentHeight;
            rectTransform.sizeDelta = sizeDelta;

            timer += Time.deltaTime;

            yield return null;
        }

        // Ensure the final height matches the target height exactly
        canvasGroup.alpha = 1f;
        Vector2 finalSizeDelta = rectTransform.sizeDelta;
        finalSizeDelta.y = targetHeight;
        rectTransform.sizeDelta = finalSizeDelta;
        isAnimating = false;
    }

    private IEnumerator FadeOutCoroutine(float duration) {
        Debug.Log("Fade out");
        isAnimating = true;
        float timer = 0f;

        canvasGroup.alpha = 1f;

        while (timer < duration) {
            float currentAlpha = Mathf.Lerp(1f, 0f, timer / duration);
            canvasGroup.alpha = currentAlpha;
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        isAnimating = false;
    }

    private IEnumerator FadeInCoroutine(float duration) {
        Debug.Log("Fade in");
        isAnimating = true;
        float timer = 0f;

        canvasGroup.alpha = 0;

        while (timer < duration) {
            float currentAlpha = Mathf.Lerp(0f, 1f, timer / duration);
            canvasGroup.alpha = currentAlpha;
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        isAnimating = false;
    }
}
