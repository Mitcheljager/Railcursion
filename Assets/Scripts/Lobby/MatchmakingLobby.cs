using UnityEngine;
using UnityEngine.Events;
using Steamworks;
using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Connection;

public class MatchmakingLobby : MonoBehaviour {
    public SpawnPlayer spawnPlayer;
    public NetworkManager networkManager;
    public GameObject chatManagerObject;
    public UnityEvent eventOnJoin;
    public LobbyDisplay lobbyDisplay;
    public LobbyData lobby;

    void Start() {
        Matchmaking.Client.EventLobbyLeave.AddListener(StopConnection);
    }

    void Destroy() {
        Matchmaking.Client.EventLobbyLeave.RemoveListener(StopConnection);
    }

    public void JoinLobby(CSteamID lobbyId) {
        Matchmaking.Client.LeaveAllLobbies();

        Matchmaking.Client.JoinLobby(lobbyId, (joinedLobby, error) => {
            if (error) {
                Debug.Log("Error joining lobby");
            } else {
                Debug.Log("Joined lobby");
                lobby = joinedLobby.Lobby;

                ToggleLobbyDisplay(true);
                ConnectClient();

                eventOnJoin.Invoke();
            }

            if (lobbyDisplay != null) lobbyDisplay.RefreshLobbyMembers();
        });

        Debug.Log("Joining lobby: " + lobbyId);
    }

    public void LeaveLobby() {
        if (lobby == null) return;

        lobby.Leave();

        ToggleLobbyDisplay(false);
    }

    public void ToggleLobbyDisplay(bool state) {
        if (lobbyDisplay == null) return;

        lobbyDisplay.gameObject.SetActive(state);
    }

    public void ShowSteamInviteOverlay() {
        global::Steamworks.SteamFriends.ActivateGameOverlay("friends");
    }

    public void ConnectServerAndClient() {
        chatManagerObject.SetActive(true);

        networkManager.ServerManager.StartConnection();
        networkManager.ClientManager.StartConnection();
    }

    public void ConnectClient() {
        chatManagerObject.SetActive(true);

        networkManager.ClientManager.StartConnection();
    }

    public void StopConnection(LobbyData lobbyData) {
        chatManagerObject.SetActive(false);

        if (networkManager.IsClient) networkManager.ClientManager.StopConnection();
        if (networkManager.IsServer) networkManager.ServerManager.StopConnection(false);
    }

    public void StartGame() {
        networkManager.SceneManager.OnClientLoadedStartScenes += DoSpawn;

        SceneLoadData sceneLoadData = new SceneLoadData("SampleScene");
        sceneLoadData.ReplaceScenes = ReplaceOption.All;

        networkManager.SceneManager.LoadGlobalScenes(sceneLoadData);

        lobby.SetGameServer(lobby.Owner.user.id);
    }

    private void DoSpawn(NetworkConnection connection, bool asServer) {
        Debug.Log("!!!!!!!!!!!Do Spawn!!!!!!!!!!!!!");

        spawnPlayer.SpawnOnServer(connection, asServer);
    }
}
