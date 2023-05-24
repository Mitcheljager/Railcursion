using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreboardItem : MonoBehaviour {
    [Header("Components")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI deathsText;

    public void SetText(PlayerStats playerStats) {
        nameText.text = playerStats.playerState.playerName;
        killsText.text = playerStats.kills.ToString();
        deathsText.text = playerStats.deaths.ToString();
    }
}
