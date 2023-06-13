using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PlayerInventory : NetworkBehaviour {
    public PlayerReference playerReference;
    public Transform thrownFromTransform;
    [Header("State")]
    public Throwable holdingThrowable;

    void Update() {
        if (!base.IsOffline && !base.IsOwner) return;
        if (Input.GetKeyDown(KeyCode.G)) Throw();
    }

    void OnDestroy() {
        if (holdingThrowable != null) Destroy(holdingThrowable.gameObject);
    }

    public bool IsPickingUp() {
        if (!base.IsOffline && !base.IsOwner) return false;
        return Input.GetKeyDown(KeyCode.F);
    }

    public void PickUp(Pickupable pickupable, ObjectPool objectPool) {
        if (pickupable.itemType == PickupableType.Throwable) PickUpThrowable(pickupable, objectPool);
    }

    private void PickUpThrowable(Pickupable pickupable, ObjectPool objectPool) {
        GameObject gameObject = Instantiate(pickupable.prefab);
        base.Spawn(gameObject);
        holdingThrowable = gameObject.GetComponent<Throwable>();
        if (objectPool != null) holdingThrowable.objectPool = objectPool;
    }

    [ServerRpc(RequireOwnership = false)]
    private void Throw() {
        if (holdingThrowable == null) return;

        holdingThrowable.Throw(this);
    }
}
