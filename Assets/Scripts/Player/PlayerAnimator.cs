using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    public PlayerReference playerReference;

    private PlayerMovement playerMovement;
    private PlayerState playerState;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        playerMovement = playerReference.playerMovement;
        playerState = playerReference.playerState;
    }

    void Update() {
        animator.SetFloat("Speed", playerMovement.move.magnitude * (playerMovement.isRunning ? playerMovement.runSpeed : playerMovement.baseSpeed));
        animator.SetFloat("Input X", playerMovement.inputX);
        animator.SetFloat("Input Z", playerMovement.inputZ);
        animator.SetBool("Is Grounded", playerMovement.isGrounded);
        animator.SetBool("Is Dead", playerState.isDead);
    }
}
