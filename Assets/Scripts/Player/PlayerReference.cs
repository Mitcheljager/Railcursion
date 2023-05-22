using UnityEngine;

public class PlayerReference : MonoBehaviour {
    public PlayerState playerState;

    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public MatchLooperObjects matchLooperObjects;

    void Start() {
        playerMovement = playerState.gameObject.GetComponent<PlayerMovement>();
        matchLooperObjects = playerState.gameObject.GetComponent<MatchLooperObjects>();
    }
}
