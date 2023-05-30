using UnityEngine;
using TMPro;

public class LobbySettings : MonoBehaviour {
    [Header("Setup")]
    public MatchmakingLobby matchmakingLobby;
    [Header("UI")]
    public TextMeshProUGUI lobbyNameText;

    void OnEnable() {
        lobbyNameText.text = matchmakingLobby.lobby.Name;
    }
}
