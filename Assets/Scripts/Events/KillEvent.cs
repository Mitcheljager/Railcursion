using UnityEngine;
using UnityEngine.Events;

public class KillEvent : MonoBehaviour {
    public class KillEventData {
        public PlayerState killer;
        public PlayerState victim;

        public KillEventData(PlayerState killer, PlayerState victim) {
            this.killer = killer;
            this.victim = victim;
        }
    }

    public static UnityEvent<KillEventData> OnKillEvent = new UnityEvent<KillEventData>();

    public static void Dispatch(PlayerState killer, PlayerState victim) {
        Debug.Log("Dispatching kill event");

        KillEventData eventData = new KillEventData(killer, victim);
        OnKillEvent.Invoke(eventData);
    }
}
