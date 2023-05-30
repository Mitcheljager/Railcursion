using UnityEngine;
using HeathenEngineering.SteamworksIntegration;
using TMPro;

public class LobbyDisplay : MonoBehaviour {
    [Header("Setup")]
    public MatchmakingLobby matchmakingLobby;
    [Header("UI")]
    public GameObject lobbyMemberItem;
    public Transform lobbyMembersTransform;
    public TextMeshProUGUI numberOfPlayerText;

    public void Update() {
        RefreshLobbyMembers();
    }

    public void RefreshLobbyMembers() {
        if (matchmakingLobby.lobby == null || matchmakingLobby.lobby.Members == null) return;

        foreach (Transform transform in lobbyMembersTransform) {
            Destroy(transform.gameObject);
        }

        foreach(LobbyMemberData member in matchmakingLobby.lobby.Members) {
            GameObject gameObject = Instantiate(lobbyMemberItem, lobbyMembersTransform);
            LobbyMemberItem item = gameObject.GetComponent<LobbyMemberItem>();

            item.member = member;
            item.isHost = member.IsOwner;
        }

        numberOfPlayerText.text = matchmakingLobby.lobby.MemberCount + "/" + matchmakingLobby.lobby.MaxMembers;
    }

    public void ShowSteamInviteOverlay() {
        Steamworks.SteamFriends.ActivateGameOverlayInviteDialog(matchmakingLobby.lobby.SteamId);
    }
}
