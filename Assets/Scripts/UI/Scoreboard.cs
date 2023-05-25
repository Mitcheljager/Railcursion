using UnityEngine;

public class Scoreboard : MonoBehaviour{
    public PlayersManager playersManager;
    public Transform wrapperTransform;
    public GameObject scoreboardItemPrefab;

    private void OnEnable() {
        UpdateScoreboardEvent.OnUpdateScoreboardEvent.AddListener(HandleUpdateScoreboard);
        UpdateScoreboard();
    }

    private void OnDisable() {
        UpdateScoreboardEvent.OnUpdateScoreboardEvent.RemoveListener(HandleUpdateScoreboard);
    }

    private void HandleUpdateScoreboard(UpdateScoreboardEvent.UpdateScoreboardData eventData) {
        UpdateScoreboard();
    }

    private void UpdateScoreboard() {
        DestroyCurrentItems();

        foreach(PlayerReference player in playersManager.players) {
            GameObject item = Instantiate(scoreboardItemPrefab);
            item.transform.parent = wrapperTransform;

            ScoreboardItem scoreboardItem = item.GetComponent<ScoreboardItem>();
            scoreboardItem.SetText(player.playerStats, playersManager.currentPlayer == player);
        }
    }

    private void DestroyCurrentItems() {
        while (wrapperTransform.childCount > 0) {
            DestroyImmediate(wrapperTransform.GetChild(0).gameObject);
        }
    }
}
