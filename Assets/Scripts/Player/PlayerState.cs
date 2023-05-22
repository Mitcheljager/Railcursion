using UnityEngine;
using FishNet.Object;

public class PlayerState : NetworkBehaviour {
    public string playerName = "";
    public bool isDead = false;

    void Start() {
        string[] possibleNames = { "Reinhardt", "Cassidy", "Tracer", "Roadhog", "Brigitte", "Mei", "Mercy", "Baptiste", "Moira", "Junkrat", "Pharah" };
        playerName = possibleNames[Random.Range(0, possibleNames.Length)];
    }

    [ServerRpc]
    public void Kill(string killer) {
        Debug.Log("Player State: Kill");

        isDead = true;

        KillEvent.DispatchKillEvent(killer, playerName);
    }

    [ServerRpc(RunLocally = true)]
    public void Respawn() {
        isDead = false;
    }
}
