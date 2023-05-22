using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class DisableIfNotOwner : NetworkBehaviour {
    public override void OnStartClient() {
        base.OnStartClient();

        if (!base.IsOffline && base.IsOwner) return;

        gameObject.SetActive(false);
    }
}
