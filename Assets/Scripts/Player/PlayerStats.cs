using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerStats : NetworkBehaviour {
    [SyncVar(OnChange = nameof(CallUpdateScoreboard))] public int kills = 0;
    [SyncVar(OnChange = nameof(CallUpdateScoreboard))] public int deaths = 0;

    [Header("State")]
    public PlayerState playerState;

    void Start() {
        playerState = GetComponent<PlayerState>();
    }

    public override void OnStartClient() {
        base.OnStartClient();
    }

    private void OnEnable() {
        KillEvent.OnKillEvent.AddListener(HandleKillEvent);
    }

    private void OnDisable() {
        KillEvent.OnKillEvent.RemoveListener(HandleKillEvent);
    }

    private void HandleKillEvent(KillEvent.KillEventData eventData) {
        AddKill(eventData.killer, eventData.victim);
        AddDeath(eventData.killer, eventData.victim);
    }

    private void AddKill(PlayerState killer, PlayerState victim) {
        Debug.Log("Add kill for playerState: " + playerState + " " + playerState.playerName);
        Debug.Log("Add kill: " + killer.playerName + " " + victim.playerName);

        if (killer != playerState) return;

        Debug.Log("Add kill final");

        kills++;
    }

    private void AddDeath(PlayerState killer, PlayerState victim) {
        Debug.Log("Add death");

        if (victim != playerState) return;

        deaths++;
    }

    private void CallUpdateScoreboard(int prev, int next, bool asServer) {
        if (asServer) return;
        if (prev == next) return;

        UpdateScoreboardEvent.Dispatch();
    }
}
