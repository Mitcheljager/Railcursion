using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class KillFeed : NetworkBehaviour {
    public Transform wrapperTransform;
    public GameObject killFeedPrefab;

    public override void OnStartClient() {
        base.OnStartClient();
    }

    private void OnEnable() {
        KillEvent.OnKillEvent.AddListener(HandleKillEvent);
    }

    private void OnDisable() {
        KillEvent.OnKillEvent.RemoveListener(HandleKillEvent);
    }

    private void HandleKillEvent(KillEvent.KillEventData eventData) {
        CreateKillFeedItem(eventData.killerName, eventData.victimName);
    }

    [ObserversRpc]
    private void CreateKillFeedItem(string killerName, string victimName) {
        GameObject item = Instantiate(killFeedPrefab);

        KillFeedItem component = item.GetComponent<KillFeedItem>();
        component.killerName = killerName;
        component.victimName = victimName;

        item.transform.parent = wrapperTransform;
        item.transform.SetAsFirstSibling();
    }
}
