using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    public PlayerReference playerReference;
    public Renderer skinnedRenderer;

    public PlayerMovement playerMovement;
    public PlayerState playerState;
    public Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        playerMovement = playerReference.playerMovement;
        playerState = playerReference.playerState;
    }

    void Update() {
        if (!skinnedRenderer.enabled) return;

        animator.SetFloat("Speed", playerMovement.move.magnitude * (playerMovement.isRunning ? playerMovement.runSpeed : playerMovement.baseSpeed));
        animator.SetFloat("Input X", playerMovement.inputX);
        animator.SetFloat("Input Z", playerMovement.inputZ);
        animator.SetBool("Is Grounded", playerMovement.isGrounded);
        animator.SetBool("Is Dead", playerState.isDead);
    }
}
