using UnityEngine;
using HeathenEngineering.SteamworksIntegration;

public class LobbiesList : MonoBehaviour {
    public LobbyManager lobbyManager;
    public GameObject lobbyItemPrefab;
    public Transform wrapper;

    void OnEnable() {
        lobbyManager.Search(100);
    }

    public void LobbyResults(LobbyData[] results) {
        Debug.Log("Search");

        foreach (Transform child in wrapper) {
            Destroy(child.gameObject);
        }

        foreach(LobbyData lobby in results) {
            GameObject gameObject = Instantiate(lobbyItemPrefab, wrapper);
            BrowseLobbyRecord lobbyRecord = gameObject.GetComponent<BrowseLobbyRecord>();
            lobbyRecord.SetLobby(lobby);
        }
    }
}
