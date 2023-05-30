using UnityEngine;
using Steamworks;
using TMPro;
using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using FishNet.Managing;

public class CreateLobby : MonoBehaviour {
    public UserData userData;
    public MatchmakingLobby matchmakingLobby;
    public TMP_InputField lobbyNameInput;
    public TMP_InputField maxPlayersInput;
    public string lobbyType = "public";
    public string lobbyName = "Lobby name";
    public int maxPlayers = 8;

    private NetworkManager networkManager;

    void Start() {
        SetInitialValues();

        networkManager = matchmakingLobby.networkManager;
    }

    void OnEnable() {
        SetInitialValues();
    }

    public void SetInitialValues() {
        lobbyNameInput.text = userData.Name + "'s " + " lobby";
    }

    public void SetLobbyType(string type) {
        lobbyType = type;
    }

    public void Create() {
        int maxPlayers = Mathf.Clamp(int.Parse(maxPlayersInput.text), 0, 8);

        Matchmaking.Client.CreateLobby(lobbyType == "public" ? ELobbyType.k_ELobbyTypePublic : ELobbyType.k_ELobbyTypePrivate, maxPlayers, (result, lobby, error) => {
            if (error) {
                Debug.Log("Error creating lobby");
                Debug.Log(error);
                return;
            }

            Debug.Log("Created " + lobbyType + " lobby");

            matchmakingLobby.lobby = lobby;

            matchmakingLobby.lobby["name"] = lobbyNameInput.text;
            matchmakingLobby.lobby["game"] = "Railcursion";
            matchmakingLobby.lobby["map"] = "Frontier";

            matchmakingLobby.ToggleLobbyDisplay(true);

            gameObject.SetActive(false);

            matchmakingLobby.ConnectServerAndClient();
        });
    }
}
