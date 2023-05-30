using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatMessage : MonoBehaviour {
    [Header("Components")]
    public AnimationHelper animationHelper;
    public TextMeshProUGUI messageText;

    void Start() {
        animationHelper.FadeIn(0.25f, true);
    }
}
