using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Railgun : MonoBehaviour {
    [Header("Config")]
    public Transform cameraTransform;
    public int maxDistance = 1000;
    public float cooldown = 2f;
    [Header("Animation")]
    public Transform gunTransform;
    public float recoilDistance = -0.25f;
    public float recoilDuration = 0.1f;
    public float recoilRecoveryDuration = 0.5f;
    [Header("Effect")]
    public Transform effectOrigin;
    public MatchLooperObjects matchLooperObjects;
    public float destroyDelay = 2f;
    public float maxEffectDistance = 100;
    [Header("Audio")]
    public AudioHelper[] audioHelpers;
    [Header("Mask")]
    public LayerMask layerMask;
    [Header("State")]
    public float currentCooldown = 0f;
    public float currentRecoilTimer = 0f;

    private ObjectPool objectPool;
    private Vector3 gunOriginalPosition;
    private bool isRecoiling = false;

    void Start() {
        gunOriginalPosition = gunTransform.localPosition;
        objectPool = GetComponent<ObjectPool>();
    }

    void Update() {
        if (currentRecoilTimer > 0f) {
            currentRecoilTimer -= Time.deltaTime;
            MoveRecoil();
        }

        if (currentCooldown > 0f) {
            currentCooldown -= Time.deltaTime;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        currentCooldown = cooldown;
        currentRecoilTimer = recoilDuration + recoilRecoveryDuration;

        foreach (AudioHelper audioHelper in audioHelpers){
            audioHelper.PlayRandomClip();
        }

        GameObject target = GetTarget();

        if (!target) return;
        if (target.tag != "Player" && target.tag != "DuplicatedPlayer") return;

        PlayerReference playerReference = target.GetComponent<PlayerReference>();

        if (playerReference.playerState.isDead) return;
        playerReference.playerState.isDead = true;
    }

    public GameObject GetTarget() {
        Debug.Log("Fire!");

		RaycastHit raycastHit;
		Vector3 position = cameraTransform.position;
		Vector3 target = position + cameraTransform.forward * maxDistance;
        Vector3 direction = (target - position).normalized;

        Debug.DrawLine(position, target, Color.red);

        if (!Physics.Linecast(position, target, out raycastHit, layerMask)) {
            ShowEffects(direction, maxEffectDistance);
            return null;
        }

        float distance = Vector3.Distance(effectOrigin.position, raycastHit.point);
        direction = (raycastHit.point - effectOrigin.position).normalized;
        ShowEffects(direction, distance);

        Debug.DrawLine(position, raycastHit.transform.position, Color.yellow);

        // if (raycastHit.collider == null) Physics.SphereCast(position, 0.1f, direction, out raycastHit, maxDistance, layerMask);
		// if (raycastHit.collider == null) return null;

        return raycastHit.collider.gameObject;
	}

    private void ShowEffects(Vector3 direction, float length) {
        Debug.Log("Create effects");
        ShowObjectFromObjectPool(effectOrigin.position, direction, length);

        foreach(GameObject matchingObject in matchLooperObjects.matchingObjects) {
            Vector3 offset = matchingObject.GetComponent<MatchingLoopedObject>().offset;

            ShowObjectFromObjectPool(effectOrigin.position + offset, direction, length);
        }
    }

    private void ShowObjectFromObjectPool(Vector3 position, Vector3 direction, float length) {
        GameObject effect = objectPool.GetObject();

        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(direction);

        VisualEffect visualEffect = effect.GetComponent<VisualEffect>();
        visualEffect.SetFloat("Length", length);

        StartCoroutine(objectPool.DelaySetInactive(effect, destroyDelay));
    }

    private void MoveRecoil() {
        if (isRecoiling) return;

        Vector3 targetPosition = gunOriginalPosition;
        float duration = recoilRecoveryDuration;

        Vector3 position = gunTransform.localPosition;
        if (currentRecoilTimer > recoilRecoveryDuration) targetPosition = new Vector3(position.x, position.y, position.z + recoilDistance);

        StartCoroutine(PerformSlerp(gunTransform.localPosition, targetPosition, duration, Mathf.SmoothStep));
    }

    private IEnumerator PerformSlerp(Vector3 startPosition, Vector3 targetPosition, float duration, Func<float, float, float, float> interpolation) {
        isRecoiling = true;

        float distance = Vector3.Distance(startPosition, targetPosition);
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            t = interpolation(0f, 1f, t);
            gunTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        isRecoiling = false;
    }
}
