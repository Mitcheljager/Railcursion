using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillFeedItem : MonoBehaviour {
    [Header("Components")]
    public AnimationHelper animationHelper;
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
        animationHelper.SlideIn(targetHeight, slideDuration);

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
}
