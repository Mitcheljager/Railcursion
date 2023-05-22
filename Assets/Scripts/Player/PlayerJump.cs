using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerJump : NetworkBehaviour {
    [Header("Config")]
    public AudioHelper[] audioHelpers;
    [Header("Config")]
    public float jumpForce = 5f;
    [field: SyncVar(ReadPermissions = ReadPermission.ExcludeOwner), Header("State")]
    public bool isJumping { get; [ServerRpc(RunLocally = true)] set; } = false;
    public bool hasJumped = false;

    private MatchLooperObjects matchLooperObjects;
    private CharacterController characterController;
    private PlayerMovement playerMovement;

    void Start() {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        matchLooperObjects = GetComponent<MatchLooperObjects>();
    }

    void Update() {
        if (!playerMovement.isGrounded) hasJumped = false;
        if (playerMovement.isGrounded && isJumping) Jump();

        if (!base.IsOffline && !base.IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space)) isJumping = true;
        if (Input.GetKeyUp(KeyCode.Space)) isJumping = false;
    }

    private void Jump() {
        if (hasJumped) return;

        Debug.Log("Jump");

        hasJumped = true;

        foreach(AudioHelper audioHelper in audioHelpers) {
            audioHelper.PlayRandomClip(true, matchLooperObjects.matchingObjects);
        }

        if (!base.IsOffline && !base.IsOwner) return;

        float jumpVelocity = Mathf.Sqrt(jumpForce * 2f * playerMovement.gravity);
        playerMovement.velocity = playerMovement.gravityDirection * jumpVelocity * -1f;
    }
}
