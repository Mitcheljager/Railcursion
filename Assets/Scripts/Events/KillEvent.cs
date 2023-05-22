using UnityEngine;
using UnityEngine.Events;

public class KillEvent : MonoBehaviour {
    public class KillEventData {
        public string killerName;
        public string victimName;

        public KillEventData(string killerName, string victimName) {
            this.killerName = killerName;
            this.victimName = victimName;
        }
    }

    public static UnityEvent<KillEventData> OnKillEvent = new UnityEvent<KillEventData>();

    public static void DispatchKillEvent(string killerName, string victimName) {
        KillEventData eventData = new KillEventData(killerName, victimName);
        OnKillEvent.Invoke(eventData);
    }
}
