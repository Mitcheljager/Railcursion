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
        if (holdingThrowable != null && Input.GetKey(KeyCode.G)) holdingThrowable.Throw(this);
    }

    void OnDestroy() {
        if (holdingThrowable != null) Destroy(holdingThrowable.gameObject);
    }

    public bool IsPickingUp() {
        if (!base.IsOffline && !base.IsOwner) return false;
        return Input.GetKeyDown(KeyCode.F);
    }

    public void PickUp(Pickupable pickupable) {
        if (pickupable.itemType == PickupableType.Throwable) PickUpThrowable(pickupable);
    }

    private void PickUpThrowable(Pickupable pickupable) {
        GameObject gameObject = Instantiate(pickupable.prefab);
        holdingThrowable = gameObject.GetComponent<Throwable>();
    }
}
