using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHelper : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public RectTransform rectTransform;

    private bool isAnimating = false;

    void Start() {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) rectTransform = GetComponent<RectTransform>();
    }

    public void FlyIn(float distance = 40f, float duration = 0.5f) {
        if (isAnimating) return;
        StartCoroutine(FlyInCoroutine(distance, duration));
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

    private IEnumerator FlyInCoroutine(float distance, float duration) {
        isAnimating = true;
        float timer = 0f;

        canvasGroup.alpha = 0;

        Vector2 startPosition = rectTransform.anchoredPosition;
        float startTop = startPosition.y;
        float targetTop = startTop - distance;
        rectTransform.anchoredPosition = startPosition;

        while (timer < duration) {
            float currentAlpha = Mathf.Lerp(0f, 1f, timer / duration);
            float currentTop = Mathf.Lerp(startTop, targetTop, timer / duration);

            canvasGroup.alpha = currentAlpha;

            Vector2 currentPosition = rectTransform.anchoredPosition;
            currentPosition.y = startTop - currentTop;
            rectTransform.anchoredPosition = currentPosition;

            timer += Time.deltaTime;

            yield return null;
        }

        // Ensure the final position matches the target position exactly
        canvasGroup.alpha = 1f;
        Vector2 finalPosition = rectTransform.anchoredPosition;
        finalPosition.y = startPosition.y - targetTop;
        rectTransform.anchoredPosition = finalPosition;

        isAnimating = false;
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
