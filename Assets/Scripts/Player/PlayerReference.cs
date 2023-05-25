using UnityEngine;

public class PlayerReference : MonoBehaviour {
    public PlayerState playerState;

    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public MatchLooperObjects matchLooperObjects;

    void Start() {
        GameObject playerObject = playerState.gameObject;

        playerMovement = playerObject.GetComponent<PlayerMovement>();
        playerStats = playerObject.GetComponent<PlayerStats>();
        matchLooperObjects = playerObject.GetComponent<MatchLooperObjects>();
    }
}
