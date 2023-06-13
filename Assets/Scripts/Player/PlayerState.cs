using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerState : NetworkBehaviour {
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
    public string playerName { get; [ServerRpc(RunLocally = true)] set; } = "";
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner)]
    public bool isDead { get; [ServerRpc(RunLocally = true)] set; } = false;

    public override void OnStartClient() {
        base.OnStartClient();

        AddPlayerToPlayersManager();

        if (!base.IsOffline && !base.IsOwner) return;

        string[] possibleNames = { "Reinhardt", "Cassidy", "Tracer", "Roadhog", "Brigitte", "Mei", "Mercy", "Baptiste", "Moira", "Junkrat", "Pharah" };
        playerName = possibleNames[Random.Range(0, possibleNames.Length)];
    }

    [ServerRpc(RequireOwnership = false)]
    public void Kill(PlayerState killer) {
        Debug.Log("Player State: Kill");

        isDead = true;

        KillEvent.Dispatch(killer, this);
    }

    [ServerRpc]
    public void Respawn() {
        isDead = false;
    }

    private void AddPlayerToPlayersManager() {
        PlayersManager playersManager = GameObject.FindObjectOfType<PlayersManager>();

        if (playersManager == null) return;

        PlayerReference playerReference = GetComponent<PlayerReference>();

        playersManager.players.Add(playerReference);
        if (!base.IsOffline && base.IsOwner) playersManager.currentPlayer = GetComponent<PlayerReference>();
    }
}
