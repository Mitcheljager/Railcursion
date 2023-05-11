using UnityEngine;

public class PlayerJump : MonoBehaviour {
    [Header("Config")]
    public AudioHelper audioHelper;
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

        if (Input.GetKeyDown(KeyCode.Space)) isJumping = true;
        if (Input.GetKeyUp(KeyCode.Space)) isJumping = false;

        if (playerMovement.isGrounded && isJumping && !hasJumped) Jump();
    }

    private void Jump() {
        Debug.Log("Jump");

        hasJumped = true;

        float jumpVelocity = Mathf.Sqrt(jumpForce * 2f * playerMovement.gravity);
        playerMovement.velocity = playerMovement.gravityDirection * jumpVelocity * -1f;

        audioHelper.PlayRandomClip();
    }
}
