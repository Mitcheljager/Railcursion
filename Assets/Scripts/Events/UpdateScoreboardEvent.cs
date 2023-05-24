using UnityEngine;
using UnityEngine.Events;

public class UpdateScoreboardEvent : MonoBehaviour {
    public class UpdateScoreboardData {

    }

    public static UnityEvent<UpdateScoreboardData> OnUpdateScoreboardEvent = new UnityEvent<UpdateScoreboardData>();

    public static void Dispatch() {
        Debug.Log("Dispatching update scoreboard event");

        UpdateScoreboardData eventData = new UpdateScoreboardData();
        OnUpdateScoreboardEvent.Invoke(eventData);
    }
}
