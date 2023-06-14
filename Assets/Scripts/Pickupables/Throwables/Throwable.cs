using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FishNet.Object;

public class Throwable : NetworkBehaviour {
    public float forwardsMultiplier = 50f;
    public float upMultiplier = 20f;
    public float bounceForce = 10f;
    public bool useGravity = true;
    public GameObject thrownObject;
    public Rigidbody body;
    public UnityEvent onThrowEvent;

    public ObjectPool objectPool;

    private bool thrown = false;
    private float gravity = 10f;
    private Vector3 gravityDirection;
    private PlayersManager playersManager;

    void Start() {
        body.useGravity = false;
        playersManager = FindObjectOfType<PlayersManager>();
    }

    void FixedUpdate() {
        if (!thrown) return;
        if (!useGravity) return;

        Vector3 gravityForce = gravityDirection.normalized * gravity;

        // Apply custom gravity force to the Rigidbody
        body.AddForce(gravityForce, ForceMode.Acceleration);

        ObserverSyncRigidBody(body.velocity, body.position);
    }

    void OnCollisionEnter(Collision collision) {
        Vector3 normal = collision.contacts[0].normal;
        Vector3 bounceForceVector = -normal * bounceForce;
        body.AddForce(bounceForceVector, ForceMode.Impulse);
    }

    [ServerRpc(RequireOwnership = false)]
    public void Throw(PlayerInventory playerInventory) {
        PlayerMovement playerMovement = playerInventory.playerReference.playerMovement;
        PlayerCamera playerCamera = playerMovement.playerCamera;

        Vector3 force = playerCamera.gameObject.transform.forward * forwardsMultiplier + playerMovement.gameObject.transform.up * upMultiplier;
        ApplyForce(playerInventory, playerMovement.gravityDirection, playerMovement.gravity, playerInventory.thrownFromTransform.position, force);

        if (onThrowEvent != null) onThrowEvent.Invoke();
    }

    private void ApplyForce(PlayerInventory playerInventory, Vector3 direction, float gravityStrength, Vector3 startPosition, Vector3 force) {
        gravityDirection = direction;
        gravity = gravityStrength;

        ActivateObject(playerInventory, startPosition);
        ObserversActivateObject(playerInventory, startPosition);

        body.AddForce(force);
        thrown = true;
    }

    private void ActivateObject(PlayerInventory playerInventory, Vector3 startPosition) {
        thrownObject.transform.position = startPosition;
        playerInventory.holdingThrowable = null;
        thrownObject.SetActive(true);
    }

    [ObserversRpc]
    private void ObserversActivateObject(PlayerInventory playerInventory, Vector3 startPosition) {
        ActivateObject(playerInventory, startPosition);
    }

    [ObserversRpc]
    private void ObserverSyncRigidBody(Vector3 velocity, Vector3 position) {
        if (base.IsServer) return;

        float threshold = 1f;
        float distance = Vector3.Distance(body.position, position);
        if (distance > threshold) body.position = position;

        body.velocity = velocity;
    }

    public List<PlayerReference> GetPlayersInRadius(Transform transform, float radius = 10f) {
        List<PlayerReference> playersInRadius = new List<PlayerReference>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (PlayerReference player in playersManager.players) {
            if (player.playerState.isDead) continue;

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > radius) continue;

            Debug.Log("Player in radius: " + player);

            playersInRadius.Add(player);
        }

        return playersInRadius;
    }
}
