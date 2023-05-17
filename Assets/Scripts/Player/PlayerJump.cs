using UnityEngine;
using FishNet.Object;

public class PlayerJump : NetworkBehaviour {
    [Header("Config")]
    public AudioHelper[] audioHelpers;
    [Header("Config")]
    public float jumpForce = 5f;
    [Header("State")]
    public bool isJumping = false;
    public bool hasJumped = false;

    private CharacterController characterController;
    private PlayerMovement playerMovement;

    void Start() {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update() {
        if (!playerMovement.isGrounded) hasJumped = false;

        if (!base.IsOffline && !base.IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Space)) isJumping = true;
        if (Input.GetKeyUp(KeyCode.Space)) isJumping = false;

        if (playerMovement.isGrounded && isJumping) Jump();
    }

    [ServerRpc(RunLocally = true)]
    private void Jump() {
        if (hasJumped) return;

        Debug.Log("Jump");

        hasJumped = true;

        float jumpVelocity = Mathf.Sqrt(jumpForce * 2f * playerMovement.gravity);
        playerMovement.velocity = playerMovement.gravityDirection * jumpVelocity * -1f;

        PlayAudio();
    }

    [ObserversRpc(RunLocally = true)]
    private void PlayAudio() {
        foreach(AudioHelper audioHelper in audioHelpers) {
            audioHelper.PlayRandomClip();
        }
    }
}
