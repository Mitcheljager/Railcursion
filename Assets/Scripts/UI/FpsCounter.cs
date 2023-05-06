using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FpsCounter : MonoBehaviour {
    public TextMeshProUGUI text;

    private float fpsMeasureDelay = 0.5f;
    private float fpsNextPeriod = 0f;

    void Start() {
        fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasureDelay;
        Application.targetFrameRate = 300; // Max framerate
    }

    void Update() {
        if (Time.realtimeSinceStartup > fpsNextPeriod) {
            int fps = Mathf.RoundToInt(1 / Time.unscaledDeltaTime);
            text.text = "" + fps + " FPS";

            fpsNextPeriod += fpsMeasureDelay;
        }
    }
}
