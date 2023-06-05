using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackGrenade : MonoBehaviour {
    public Throwable throwable;
    public AudioHelper audioHelper;
    public MatchLooperObjects matchLooperObjects;
    public GameObject effectsWrapper;
    public float timer = 2f;
    public float destroyAfterExplodeTimer = 10f;
    public float radius = 20f;
    public float force = 20f;
    public float upForce = 5f;

    void OnEnable() {
        StartCoroutine(Activate());
    }

    private IEnumerator Activate() {
        yield return new WaitForSeconds(timer);

        audioHelper.transform.position = gameObject.transform.position;
        effectsWrapper.transform.position = gameObject.transform.position;
        audioHelper.PlayRandomClip(true, matchLooperObjects.matchingObjects, 100);
        effectsWrapper.SetActive(true);

        throwable.body.useGravity = false;
        throwable.body.isKinematic = true;

        List<PlayerReference> players = throwable.GetPlayersInRadius(transform, radius);
        PushPlayersAway(players);

        yield return new WaitForSeconds(destroyAfterExplodeTimer);

        DestroyAllRelevantComponents();

        yield return null;
    }

    private void PushPlayersAway(List<PlayerReference> players) {
        foreach(PlayerReference player in players) {
            PlayerMovement playerMovement = player.playerMovement;
            Vector3 direction = ((player.gameObject.transform.position + Vector3.up) - transform.position).normalized;
            playerMovement.velocity = direction * force + playerMovement.gravityDirection * upForce;
        }
    }

    private void DestroyAllRelevantComponents() {
        foreach(GameObject matchingObject in matchLooperObjects.matchingObjects) {
            matchingObject.SetActive(false);
        }

        Destroy(throwable.gameObject);
    }
}
