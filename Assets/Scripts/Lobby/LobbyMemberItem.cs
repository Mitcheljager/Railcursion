using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HeathenEngineering.SteamworksIntegration;

public class LobbyMemberItem : MonoBehaviour {
    public LobbyMemberData member;
    public TextMeshProUGUI nameText;
    public RawImage avatarImage;
    public GameObject isHostObject;
    public bool isHost = false;
    public GameObject isReadyObject;

    void Start() {
        if (member.user == null) return;

        SetDetails();
    }

    public void SetDetails() {
        nameText.text = member.user.Name;
        if(member.user.Avatar != null) avatarImage.texture = member.user.Avatar;
        else member.user.LoadAvatar((result) => { avatarImage.texture = result; });

        isHostObject.SetActive(isHost);
        isReadyObject.SetActive(member.IsReady);
    }
}
