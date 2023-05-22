using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientGravityToCurvature : MonoBehaviour {
    [Header("Config")]
    private PlayerMovement playerMovement;

    void OnTriggerEnter(Collider collider) {
        Debug.Log("Enter");
        if (collider.tag != "Player") return;

        PlayerReference playerReference = collider.GetComponent<PlayerReference>();
        playerMovement = playerReference.playerMovement;
    }

    void OnTriggerStay(Collider collider) {
        if (collider.tag != "Player") return;

        Vector3 start = playerMovement.groundCheck.position;
        Vector3 end = start + Vector3.down * 3;
        RaycastHit hit;

        Debug.Log(end);

        Debug.DrawLine(start, end, Color.red);

        if (Physics.Linecast(start, end, out hit, playerMovement.groundMask)) {
            // Get the surface normal from the raycast hit
            Vector3 surfaceNormal = hit.normal;

            // Calculate the angle between the surface normal and the upward direction (assuming world up)
            float angle = Vector3.Angle(surfaceNormal, Vector3.up);
            Vector3 direction = (playerMovement.transform.position - hit.point).normalized;
            Quaternion rotation = Quaternion.FromToRotation(playerMovement.transform.up, surfaceNormal) * playerMovement.transform.rotation;

            playerMovement.gameObject.transform.rotation = rotation;
            playerMovement.gravityDirection = direction * -1;

            // Do something with the angle
            Debug.Log("Surface normal" + surfaceNormal);
            Debug.Log("Surface angle: " + angle);
            Debug.Log("Directionm: " + direction);
        }
    }
}
