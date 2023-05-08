using UnityEngine;

public class PlayerJump : MonoBehaviour {
    public float jumpForce = 5f;
    [Header("State")]
    public bool isJumping = false;

    private CharacterController characterController;
    private PlayerMovement playerMovement;

    void Start() {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) isJumping = true;
        if (Input.GetKeyUp(KeyCode.Space)) isJumping = false;

        if (playerMovement.isGrounded && isJumping) Jump();
    }

    private void Jump() {
        Debug.Log("Jump");
        float jumpVelocity = Mathf.Sqrt(jumpForce * 2f * playerMovement.gravity);
        playerMovement.velocity = playerMovement.gravityDirection * jumpVelocity * -1f;
    }
}
