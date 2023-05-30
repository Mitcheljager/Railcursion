using UnityEngine;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;

public class SpawnPlayer : NetworkBehaviour {
    public GameObject playerPrefab;
    public NetworkManager networkManager;

    public void SpawnOnServer(NetworkConnection connection, bool asServer) {
        Debug.Log("=======================Spawn====================");
        Debug.Log(connection.ClientId);

        if (!asServer) return;

        networkManager = FindObjectOfType<NetworkManager>();

        NetworkObject gameObject = networkManager.GetPooledInstantiated(playerPrefab, true);
        gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        networkManager.ServerManager.Spawn(gameObject, connection);
    }
}
