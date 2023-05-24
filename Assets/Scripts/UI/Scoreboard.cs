using UnityEngine;

public class Scoreboard : MonoBehaviour{
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
        Debug.Log("Updating scoreboard");

        // TODO: Refactor to not be horrible

        DestroyCurrentItems();

        PlayerStats[] players = FindObjectsOfType<PlayerStats>();

        foreach(PlayerStats player in players) {
            GameObject item = Instantiate(scoreboardItemPrefab);
            item.transform.parent = wrapperTransform;

            ScoreboardItem scoreboardItem = item.GetComponent<ScoreboardItem>();
            scoreboardItem.SetText(player);
        }
    }

    private void DestroyCurrentItems() {
        while (wrapperTransform.childCount > 0) {
            DestroyImmediate(wrapperTransform.GetChild(0).gameObject);
        }
    }
}
