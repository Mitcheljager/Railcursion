using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreboardItem : MonoBehaviour {
    [Header("Components")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;
    public Image image;
    public Color activeColor;

    public void SetText(PlayerStats playerStats, bool isCurrentPlayer = false) {
        nameText.text = playerStats.playerState.playerName;
        killsText.text = playerStats.kills.ToString();
        deathsText.text = playerStats.deaths.ToString();

        if (!isCurrentPlayer) return;

        image.gameObject.SetActive(true);
        image.color = activeColor;
    }
}
