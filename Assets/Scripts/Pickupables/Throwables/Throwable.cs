using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Throwable : MonoBehaviour {
    public float forwardsMultiplier = 50f;
    public float upMultiplier = 20f;
    public float bounceForce = 10f;
    public bool useGravity = true;
    public GameObject thrownObject;
    public Rigidbody body;

    private bool thrown = false;
    private float gravity = 10f;
    private Vector3 gravityDirection;

    void Start() {
        body.useGravity = false;
    }

    void FixedUpdate() {
        if (!thrown) return;
        if (!useGravity) return;

        Vector3 gravityForce = gravityDirection.normalized * gravity;

        // Apply custom gravity force to the Rigidbody
        body.AddForce(gravityForce, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision) {
        Vector3 normal = collision.contacts[0].normal;
        Vector3 bounceForceVector = -normal * bounceForce;
        body.AddForce(bounceForceVector, ForceMode.Impulse);
    }

    public void Throw(PlayerInventory playerInventory) {
        PlayerMovement playerMovement = playerInventory.playerReference.playerMovement;
        PlayerCamera playerCamera = playerMovement.playerCamera;

        gravityDirection = playerMovement.gravityDirection;
        gravity = playerMovement.gravity;

        thrownObject.SetActive(true);
        thrownObject.transform.position = playerInventory.thrownFromTransform.position;
        body.AddForce(playerCamera.gameObject.transform.forward * forwardsMultiplier + playerInventory.gameObject.transform.up * upMultiplier);

        thrown = true;
        playerInventory.holdingThrowable = null;
    }

    public List<PlayerReference> GetPlayersInRadius(Transform transform, float radius = 10f) {
        List<PlayerReference> playersInRadius = new List<PlayerReference>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders) {
            if (collider.tag != "Player" && collider.tag != "DuplicatedPlayer") continue;

            PlayerReference playerReference = collider.GetComponent<PlayerReference>();
            if (playerReference == null) continue;

            Debug.Log(playerReference);

            playersInRadius.Add(playerReference);
        }

        return playersInRadius;
    }
}
