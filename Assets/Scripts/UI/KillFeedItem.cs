using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillFeedItem : MonoBehaviour {
    [Header("Components")]
    public TextMeshProUGUI killerText;
    public TextMeshProUGUI victimText;
    public Image image;
    [Header("Config")]
    public float targetHeight = 40f;
    public float slideDuration = 0.5f;
    [Header("State")]
    public string killerName;
    public string victimName;
    public Sprite icon;

    void Start() {
        killerText.text = killerName;
        victimText.text = victimName;

        SetWidth();
        StartCoroutine(SlideIn());

        Destroy(gameObject, 5);
    }

    void SetWidth() {
        Vector2 killerTextSize = killerText.GetPreferredValues();
        Vector2 victimTextSize = victimText.GetPreferredValues();
        RectTransform rectTransform = GetComponent<RectTransform>();

        // Set width of item
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.x = killerTextSize.x + victimTextSize.x + 50;
        rectTransform.sizeDelta = sizeDelta;

        Debug.Log(killerTextSize);

        // Set x position of icon based on killer name
        RectTransform iconRectTransform = image.gameObject.GetComponent<RectTransform>();
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.x = killerTextSize.x + 25;
        anchoredPosition.y = 0;
        iconRectTransform.anchoredPosition = anchoredPosition;
    }

    private IEnumerator SlideIn() {
        float timer = 0f;

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = 0;
        rectTransform.sizeDelta = sizeDelta;

        while (timer < slideDuration) {
            float currentAlpha = Mathf.Lerp(0f, 1f, timer / slideDuration);
            float currentHeight = Mathf.Lerp(0f, targetHeight, timer / slideDuration);

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
    }
}
