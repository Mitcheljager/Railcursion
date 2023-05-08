using UnityEngine;

public class PlayerWeapon : MonoBehaviour {
    [Header("Config")]
    public int maxDistance = 1000;
    public Transform cameraTransform;

    [Header("Mask")]
    public LayerMask layerMask;


    void Start() {

    }

    void Update() {
        if (!Input.GetMouseButton(0)) return;

        GameObject target = GetTarget();

        Debug.Log("Target: " + target);

        if (!target) return;
        if (target.tag != "Player" && target.tag != "DuplicatedPlayer") return;

        PlayerReference playerReference = target.GetComponent<PlayerReference>();

        if (playerReference.playerState.isDead) return;
        playerReference.playerState.isDead = true;
    }

    public GameObject GetTarget() {
        Debug.Log("Fire!");

		//Check for Collider with Raycast
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
