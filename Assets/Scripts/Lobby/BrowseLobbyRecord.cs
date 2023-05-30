using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HeathenEngineering.SteamworksIntegration;

public class BrowseLobbyRecord : MonoBehaviour {
    public MatchmakingLobby matchmakingLobby;
    public TextMeshProUGUI lobbyId;
    public TextMeshProUGUI lobbySize;
    public TextMeshProUGUI lobbyMap;
    public Button connectButton;
    public TextMeshProUGUI buttonLabel;

    [Header("List Record")]
    public LobbyData lobby;

    public void SetLobby(LobbyData lobby) {
        Debug.Log("Setting lobby data for " + lobby.SteamId);

        gameObject.SetActive(true);

        this.lobby = lobby;
        string lobbyName = lobby["name"];
        string mapName = lobby["map"];

        lobbyId.text = string.IsNullOrEmpty(lobbyName) ? "<empty>" : lobbyName;
        lobbyMap.text = string.IsNullOrEmpty(mapName) ? "<unknown>" : mapName;

        lobbySize.text = lobby.MemberCount + "/" + lobby.MaxMembers.ToString();
    }

    public void Selected() {
        matchmakingLobby.JoinLobby(lobby.SteamId);
    }

    private void Update() {
        if( lobby.SteamId != CSteamID.Nil && lobby.IsAMember(UserData.Me)) {
            connectButton.interactable = false;
            buttonLabel.text = "You are here!";
        } else {
            connectButton.interactable = true;
            buttonLabel.text = "Join";
        }
    }
}
