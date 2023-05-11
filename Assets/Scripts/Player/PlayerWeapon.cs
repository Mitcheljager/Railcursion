using UnityEngine;

public class PlayerWeapon : MonoBehaviour {
    [Header("Config")]
    public Transform cameraTransform;
    public int maxDistance = 1000;
    public float cooldown = 2f;
    [Header("Audio")]
    public AudioHelper[] audioHelpers;
    [Header("Mask")]
    public LayerMask layerMask;
    [Header("State")]
    public float currentCooldown = 0f;

    void Start() {

    }

    void Update() {
        if (currentCooldown > 0f) {
            currentCooldown -= Time.deltaTime;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        currentCooldown = cooldown;

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

        if (!Physics.Linecast(position, target, out raycastHit, layerMask)) return null;

        Debug.DrawLine(position, raycastHit.transform.position, Color.yellow);

        if (raycastHit.collider == null) Physics.SphereCast(position, 0.1f, direction, out raycastHit, maxDistance, layerMask);
		if (raycastHit.collider == null) return null;

        return raycastHit.collider.gameObject;
	}
}
