using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using FishNet.Object;
using FishNet.Connection;

public class KnockbackGrenade : NetworkBehaviour {
    public Throwable throwable;
    public AudioHelper audioHelper;
    public MatchLooperObjects matchLooperObjects;
    public VisualEffect visualEffect;
    public float timer = 2f;
    public float destroyAfterExplodeTimer = 10f;
    public float radius = 20f;
    public float force = 20f;
    public float upForce = 5f;

    void Start() {
        matchLooperObjects.objectPool = throwable.objectPool;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerActivate() {
        StartCoroutine(Activate());
    }

    private IEnumerator Activate() {
        yield return new WaitForSeconds(timer);

        ServerExplode();
        ObserversExplode();

        List<PlayerReference> players = throwable.GetPlayersInRadius(transform, radius);
        PushPlayersAway(players);

        yield return new WaitForSeconds(destroyAfterExplodeTimer);

        ObserversDestroy();
        DespawnComponent();

        yield return null;
    }

    private void Explode() {
        audioHelper.transform.position = gameObject.transform.position;
        visualEffect.transform.position = gameObject.transform.position;
        audioHelper.PlayRandomClip(true, matchLooperObjects.matchingObjects, 100);
        visualEffect.Play();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerExplode() {
        Explode();

        throwable.body.useGravity = false;
        throwable.body.isKinematic = true;
    }

    [ObserversRpc]
    private void ObserversExplode() {
        Explode();
    }

    private void PushPlayersAway(List<PlayerReference> players) {
        foreach(PlayerReference player in players) {
            PlayerMovement playerMovement = player.playerMovement;
            Vector3 direction = ((player.gameObject.transform.position + Vector3.up) - transform.position).normalized;
            TargetPushAwayPlayer(playerMovement.Owner, playerMovement, direction * force + playerMovement.gravityDirection * upForce);
        }
    }

    [TargetRpc]
    private void TargetPushAwayPlayer(NetworkConnection target, PlayerMovement playerMovement, Vector3 velocity) {
        playerMovement.velocity = velocity;
    }

    private void DestroyAllRelevantComponents() {
        foreach(GameObject matchingObject in matchLooperObjects.matchingObjects) {
            matchingObject.SetActive(false);
        }
    }

    [ObserversRpc]
    private void ObserversDestroy() {
        if (base.IsServer) return;
        DestroyAllRelevantComponents();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnComponent() {
        DestroyAllRelevantComponents();
        base.Despawn(throwable.gameObject);
    }
}
